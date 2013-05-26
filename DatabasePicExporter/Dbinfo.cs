using DatabasePicExporter.DPE.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace DatabasePicExporter
{
    public static class Dbinfo
    {
        public static string ConnString = "Data Source=(local);Initial Catalog=Northwind;Integrated Security=True";

        /// <summary>
        /// get mssql server version
        /// </summary>
        public static string Version
        {
            get
            {
                string sql = "exec master.dbo.xp_msver 'ProductVersion'";
                string version = "9"; //default value equals 9
                using (SqlDataReader reader = SqlHelper.ExecuteReader(ConnString, CommandType.Text, sql))
                {
                    while (reader.Read())
                        version = reader["character_value"].ToString().Substring(0, 1);
                }

                return version;
            }
        }

        /// <summary>
        /// get tables
        /// </summary>
        /// <returns></returns>
        public static List<Table> GetTables()
        {
            List<Table> tables = new List<Table>();
            string sql = string.Empty;

            switch (Version)
            {
                case "9":
                    sql = sqlGetTables;
                    break;
                case "1":
                    sql = sqlGetTables;
                    break;
                default:
                    sql = sqlGetTables2K;
                    break;
            }

            using (SqlDataReader reader = SqlHelper.ExecuteReader(ConnString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    Table table = new Table();
                    table.TableId = Convert.ToInt32(reader["TableId"].ToString());
                    table.TableName = reader["TableName"].ToString();
                    table.Description = reader["TableDesc"].ToString();
                    table.Columns = GetColumns(table.TableName);

                    tables.Add(table);
                }
            }

            return tables;
        }

        /// <summary>
        /// get columns by tablename
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<Column> GetColumns(string tableName)
        {
            List<Column> columns = new List<Column>();
            string sql = string.Empty;

            switch (Version)
            {
                case "9"://mssql 2005
                    sql = string.Format(sqlGetColumns2005, tableName);
                    break;
                case "1"://10 mssql 2008
                    sql = string.Format(sqlGetColumns2008, tableName);
                    break;
                default://mssql 2000
                    sql = string.Format(sqlGetColumns2k, tableName);
                    break;
            }

            using (SqlDataReader reader = SqlHelper.ExecuteReader(ConnString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    Column column = new Column();
                    column.ColumnId = Convert.ToInt32(reader["ColumnId"].ToString());
                    column.ColumnName = reader["ColumnName"].ToString();
                    column.Type = reader["Type"].ToString() + "(" + reader["Length"].ToString() + ")";
                    column.Description = reader["ColumnDesc"].ToString();
                    column.IndexName = reader["IndexName"].ToString().Trim();
                    column.IndexSort = reader["IndexSort"].ToString();
                    if (reader["PrimaryKey"].ToString() == "√")
                    {
                        column.IsPrimaryKey = true;
                    }
                    if (reader["Identity"].ToString() == "√")
                    {
                        column.IsIdentity = true;
                    }
                    
                    columns.Add(column);
                }
            }

            return columns;
        }

        #region sqlGetTables for sql server 2008 and later
        private static string sqlGetTables = @"SELECT 
	O.object_id AS TableId,
    TableName=O.name  ,
    TableDesc= PTB.[value]  
FROM sys.columns C
    INNER JOIN sys.objects O
        ON C.[object_id]=O.[object_id]
            AND O.type='U'
            AND O.is_ms_shipped=0
    INNER JOIN sys.types T
        ON C.user_type_id=T.user_type_id
    LEFT JOIN sys.extended_properties PTB
        ON PTB.class=1 
            AND PTB.minor_id=0 
            AND C.[object_id]=PTB.major_id
WHERE C.column_id=1 
ORDER BY TableName";
        #endregion

        #region sqlGetTables2K
        private static string sqlGetTables2K = @"SELECT 
a.id AS TableId,
 d.name   AS TableName, 
  isnull(f.value, '')  AS TableDesc    
FROM dbo.syscolumns a  
       INNER JOIN
      dbo.sysobjects d ON a.id = d.id AND d.xtype = 'U' AND 
      d.name <> 'dtproperties'  
      LEFT OUTER JOIN
      dbo.sysproperties f ON d.id = f.id AND f.smallid = 0
 where a.colorder = 1 
ORDER BY d.name, a.id, a.colorder";
        #endregion

        #region sqlGetColumns2005
        private static string sqlGetColumns2005 = @"SELECT 
    TableName=CASE WHEN C.column_id=1 THEN O.name ELSE N'' END,
    TableDesc=ISNULL(CASE WHEN C.column_id=1 THEN PTB.[value] END,N''),
    Column_id=C.column_id,
    ColumnName=C.name,
    PrimaryKey=ISNULL(IDX.PrimaryKey,N''),
    [IDENTITY]=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END,
    Computed=CASE WHEN C.is_computed=1 THEN N'√'ELSE N'' END,
    Type=T.name,
    Length=C.max_length,
    Precision=C.precision,
    Scale=C.scale,
    NullAble=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END,
    [Default]=ISNULL(D.definition,N''),
    ColumnDesc=ISNULL(PFD.[value],N''),
    IndexName=ISNULL(IDX.IndexName,N''),
    IndexSort=ISNULL(IDX.Sort,N''),
    Create_Date=O.Create_Date,
    Modify_Date=O.Modify_date
FROM sys.columns C
    INNER JOIN sys.objects O
        ON C.[object_id]=O.[object_id]
            AND O.type='U'
            AND O.is_ms_shipped=0
    INNER JOIN sys.types T
        ON C.user_type_id=T.user_type_id
    LEFT JOIN sys.default_constraints D
        ON C.[object_id]=D.parent_object_id
            AND C.column_id=D.parent_column_id
            AND C.default_object_id=D.[object_id]
    LEFT JOIN sys.extended_properties PFD
        ON PFD.class=1 
            AND C.[object_id]=PFD.major_id 
            AND C.column_id=PFD.minor_id
            --AND PFD.name='MS_Description'  -- 字段说明对应的描述名称(一个字段可以添加多个不同name的描述)
    LEFT JOIN sys.extended_properties PTB
        ON PTB.class=1 
            AND PTB.minor_id=0 
            AND C.[object_id]=PTB.major_id
            --AND PTB.name='MS_Description'  -- 表说明对应的描述名称(一个表可以添加多个不同name的描述) 

    LEFT JOIN                       -- 索引及主键信息
    (
        SELECT 
            IDXC.[object_id],
            IDXC.column_id,
            Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending')
                WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END,
            PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END,
            IndexName=IDX.Name
        FROM sys.indexes IDX
        INNER JOIN sys.index_columns IDXC
            ON IDX.[object_id]=IDXC.[object_id]
                AND IDX.index_id=IDXC.index_id
        LEFT JOIN sys.key_constraints KC
            ON IDX.[object_id]=KC.[parent_object_id]
                AND IDX.index_id=KC.unique_index_id
        INNER JOIN  -- 对于一个列包含多个索引的情况,只显示第1个索引信息
        (
            SELECT [object_id], Column_id, index_id=MIN(index_id)
            FROM sys.index_columns
            GROUP BY [object_id], Column_id
        ) IDXCUQ
            ON IDXC.[object_id]=IDXCUQ.[object_id]
                AND IDXC.Column_id=IDXCUQ.Column_id
                AND IDXC.index_id=IDXCUQ.index_id
    ) IDX
        ON C.[object_id]=IDX.[object_id]
            AND C.column_id=IDX.column_id 

WHERE O.name=N'{0}'       -- 如果只查询指定表,加上此条件
ORDER BY O.name,C.column_id";
        #endregion

        #region sqlgGetColumns2008
        private static string sqlGetColumns2008 = @"SELECT     
  TableName=case   when   a.colorder=1   then   d.name   else   ''   end,   
  TableDesc=case   when   a.colorder=1   then   isnull(f.value,'')   else   ''   end,   
    ColumnId=a.colorder,   
  ColumnName=a.name,   
  [Identity]=case   when   COLUMNPROPERTY(   a.id,a.name,'IsIdentity')=1   then   '√'else   ''   end,   
  PrimaryKey=case   when   exists(SELECT   1   FROM   sysobjects   where   xtype='PK'   and   name   in   (   
  SELECT   name   FROM   sysindexes   WHERE   indid   in(   
  SELECT   indid   FROM   sysindexkeys   WHERE   id   =   a.id   AND   colid=a.colid   
  )))   then   '√'   else   ''   end,   
  Type=b.name,   
    [Precision]=a.length,   
   Length=COLUMNPROPERTY(a.id,a.name,'PRECISION'),   
  Scale=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),   
   NullAble=case   when   a.isnullable=1   then   '√'else   ''   end,   
  [Default]=isnull(e.text,''),   
  ColumnDesc=isnull(g.[value],'')   ,
IndexName=case when isnull(h.索引名称,'') like 'IX_%' then '√' else '' end,   
  IndexSort=case when isnull(h.索引名称,'') like 'IX_%' then isnull(h.排序,'') else '' end  
  FROM   syscolumns   a   
  left   join   systypes   b   on   a.xtype=b.xusertype   
  inner   join   sysobjects   d   on   a.id=d.id     and   d.xtype='U'   and     d.name<>'dtproperties'   
  left   join   syscomments   e   on   a.cdefault=e.id   
  left   join   sys.extended_properties g   on   a.id=g.major_id   and   a.colid=g.minor_id          
  left   join   sys.extended_properties f   on   d.id=f.major_id   and   f.minor_id   =0   
  left   join(   
  select   索引名称=a.name,c.id,d.colid   
  ,排序=case   indexkey_property(c.id,b.indid,b.keyno,'isdescending')   
  when   1   then   '降序'   when   0   then   '升序'   end   
  from   sysindexes   a   
  join   sysindexkeys   b   on   a.id=b.id   and   a.indid=b.indid   
  join   (--这里的作用是有多个索引时,取索引号最小的那个   
  select   id,colid,indid=min(indid)   from   sysindexkeys   
  group   by   id,colid)   b1   on   b.id=b1.id   and   b.colid=b1.colid   and   b.indid=b1.indid   
  join   sysobjects   c   on   b.id=c.id   and   c.xtype='U'   and     c.name<>'dtproperties'   
  join   syscolumns   d   on   b.id=d.id   and   b.colid=d.colid   
  where   a.indid   not   in(0,255)   
  )   h   on   a.id=h.id   and   a.colid=h.colid  

 where   d.name=N'{0}'         
  order   by   a.id,a.colorder";
        #endregion

        #region sqlGetColumns2k
        private static string sqlGetColumns2k = @"SELECT     
  TableName=case   when   a.colorder=1   then   d.name   else   ''   end,   
  TableDesc=case   when   a.colorder=1   then   isnull(f.value,'')   else   ''   end,   
  Column_id=a.colorder,   
  ColumnName=a.name,   
  [Identity]=case   when   COLUMNPROPERTY(   a.id,a.name,'IsIdentity')=1   then   '√'else   ''   end,   
  PrimaryKey=case   when   exists(SELECT   1   FROM   sysobjects   where   xtype='PK'   and   name   in   (   
  SELECT   name   FROM   sysindexes   WHERE   indid   in(   
  SELECT   indid   FROM   sysindexkeys   WHERE   id   =   a.id   AND   colid=a.colid   
  )))   then   '√'   else   ''   end,   
  Type=b.name,   
  [Precision]=a.length,   
  Length=COLUMNPROPERTY(a.id,a.name,'PRECISION'),   
  Scale=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),   
  NullAble=case   when   a.isnullable=1   then   '√'else   ''   end,   
  [Default]=isnull(e.text,''),   
  ColumnDesc=isnull(g.[value],''),   
  IndexName=case when isnull(h.索引名称,'') like 'IX_%' then '√' else '' end,   
  IndexSort=case when isnull(h.索引名称,'') like 'IX_%' then isnull(h.排序,'') else '' end   
  FROM   syscolumns   a   
  left   join   systypes   b   on   a.xtype=b.xusertype   
  inner   join   sysobjects   d   on   a.id=d.id     and   d.xtype='U'   and     d.name<>'dtproperties'   
  left   join   syscomments   e   on   a.cdefault=e.id   
  left   join   sysproperties   g   on   a.id=g.id   and   a.colid=g.smallid       
  left   join   sysproperties   f   on   d.id=f.id   and   f.smallid=0   
  left   join(   
  select   索引名称=a.name,c.id,d.colid   
  ,排序=case   indexkey_property(c.id,b.indid,b.keyno,'isdescending')   
  when   1   then   '降序'   when   0   then   '升序'   end   
  from   sysindexes   a   
  join   sysindexkeys   b   on   a.id=b.id   and   a.indid=b.indid   
  join   (--这里的作用是有多个索引时,取索引号最小的那个   
  select   id,colid,indid=min(indid)   from   sysindexkeys   
  group   by   id,colid)   b1   on   b.id=b1.id   and   b.colid=b1.colid   and   b.indid=b1.indid   
  join   sysobjects   c   on   b.id=c.id   and   c.xtype='U'   and     c.name<>'dtproperties'   
  join   syscolumns   d   on   b.id=d.id   and   b.colid=d.colid   
  where   a.indid   not   in(0,255)   
  )   h   on   a.id=h.id   and   a.colid=h.colid   
  where   d.name='{0}'         --如果只查询指定表,加上此条件   
  order   by   a.id,a.colorder  ";
        #endregion

    }
}
