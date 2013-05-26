using System;
using System.Collections.Generic;
using System.Text;

namespace DatabasePicExporter.DPE.Entity
{
    public class Column
    {
        private int _columnId;
        public int ColumnId
        {
            get { return _columnId; }
            set { _columnId = value; }
        }

        private string _columnName;
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private bool _isPrimaryKey;
        public bool IsPrimaryKey
        {
            get { return _isPrimaryKey; }
            set { _isPrimaryKey = value; }
        }

        private bool _isIdentity;
        public bool IsIdentity
        {
            get { return _isIdentity; }
            set { _isIdentity = value; }
        }

        private string _indexName;
        public string IndexName
        {
            get { return _indexName; }
            set { _indexName = value; }
        }

        private string _indexSort;
        public string IndexSort
        {
            get { return _indexSort; }
            set { _indexSort = value; }
        }
    }
}
