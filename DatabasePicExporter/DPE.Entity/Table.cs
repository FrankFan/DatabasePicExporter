using System.Collections.Generic;

namespace DatabasePicExporter.DPE.Entity
{
    public class Table
    {
        private int _tableId;
        public int TableId
        {
            get { return _tableId; }
            set { _tableId = value; }
        }

        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private List<Column> _column;
        public List<Column> Columns
        {
            get { return _column; }
            set { _column = value; }
        }
    }
}
