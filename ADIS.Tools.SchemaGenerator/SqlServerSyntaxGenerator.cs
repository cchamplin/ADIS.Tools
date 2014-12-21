using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaGenerator
{
    public class SqlServerSyntaxGenerator : ISQLSyntaxGenerator
    {
        protected string tableName;
        protected string schema;
        protected List<SqlServerColumn> columns;

        public SqlServerSyntaxGenerator()
        {
            columns = new List<SqlServerColumn>();
        }

        protected class SqlServerColumn
        {
            public string name;
            public string type;
            public int length;
            public bool primary;
            public bool nullable;
            public SqlServerColumn(string name, string type, bool nullable, bool primary,int length = -1) {
                this.name = name;
                this.type = type;
                this.primary = primary;
                this.length = length;
                this.nullable = nullable;
            }
        }
        protected string TypeToString(Type t) {
            switch (t.Name.ToLower())
            {
                case "int":
                case "int32":
                case "integer":
                    return "int";
                case "long":
                case "int64":
                    return "bigint";
                case "double":
                case "float":
                case "decimal":
                    return "float";
                case "string":
                    return "nvarchar";
                case "char":
                    return "char";
                case "DateTime":
                    return "datetime";
                case "byte[]":
                    return "varbinary";
                case "bool":
                case "boolean":
                    return "bit";
                case "int16":
                case "short":
                    return "smallint";
                case "timespan":
                    return "time";
                case "byte":
                    return "tinyint";
                case "guid":
                    return "uniqueidentifier";
                default:
                    throw new Exception("Unrecognized data type");

            }
        }

        protected string TypeToString(System.Data.SqlDbType t)
        {
            switch (t)
            {
                case System.Data.SqlDbType.Bit:
                    return "bit";
                case System.Data.SqlDbType.BigInt:
                    return "bigint";
                case System.Data.SqlDbType.Char:
                    return "char";
                case System.Data.SqlDbType.DateTime:
                    return "datetime";
                case System.Data.SqlDbType.Binary:
                    return "binary";
                case System.Data.SqlDbType.DateTime2:
                    return "datetime2";
                case System.Data.SqlDbType.DateTimeOffset:
                    return "datetimeoffset";
                case System.Data.SqlDbType.Decimal:
                    return "decimal";
                case System.Data.SqlDbType.Float:
                    return "float";
                case System.Data.SqlDbType.Image:
                    return "image";
                case System.Data.SqlDbType.Int:
                    return "int";
                case System.Data.SqlDbType.Money:
                    return "money";
                case System.Data.SqlDbType.NChar:
                    return "nchar";
                case System.Data.SqlDbType.NText:
                    return "ntext";
                case System.Data.SqlDbType.NVarChar:
                    return "nvarchar";
                case System.Data.SqlDbType.Real:
                    return "real";
                case System.Data.SqlDbType.SmallDateTime:
                    return "smalldatetime";
                case System.Data.SqlDbType.SmallInt:
                    return "smallint";
                case System.Data.SqlDbType.SmallMoney:
                    return "smallmoney";
                case System.Data.SqlDbType.Text:
                    return "text";
                case System.Data.SqlDbType.Time:
                    return "time";
                case System.Data.SqlDbType.Timestamp:
                    return "timestamp";
                case System.Data.SqlDbType.TinyInt:
                    return "tinyint";
                case System.Data.SqlDbType.UniqueIdentifier:
                    return "uniqueidentifier";
                case System.Data.SqlDbType.VarBinary:
                    return "varbinary";
                case System.Data.SqlDbType.VarChar:
                    return "varchar";
                case System.Data.SqlDbType.Xml:
                    return "xml";
                default:
                    throw new Exception("Unrecognized data type");

            }
        }


        public void SetTable(string name)
        {
            this.schema = "ADIS";
            this.tableName = name;
        }

        public void SetTable(string schema, string name)
        {
            this.schema = schema;
            this.tableName = name;
        }

        public void AddColumn(string name, Type dataType, bool nullable)
        {
            this.columns.Add(new SqlServerColumn(name, TypeToString(dataType), nullable, false));
        }

        public void AddColumn(string name, Type dataType, bool nullable, int length)
        {
            this.columns.Add(new SqlServerColumn(name, TypeToString(dataType), nullable, false, length));
        }

        public void AddColumn(string name, Type dataType, bool nullable, bool primary)
        {
            this.columns.Add(new SqlServerColumn(name,TypeToString(dataType),nullable,primary));
        }

        public void AddColumn(string name, Type dataType, bool nullable, bool primary, int length)
        {
            this.columns.Add(new SqlServerColumn(name,TypeToString(dataType), nullable,primary,length));
        }

        public void AddColumn(string name, System.Data.SqlDbType dataType,bool nullable)
        {
            this.columns.Add(new SqlServerColumn(name, TypeToString(dataType), nullable, false));
        }

        public void AddColumn(string name, System.Data.SqlDbType dataType, bool nullable, int length)
        {
            this.columns.Add(new SqlServerColumn(name, TypeToString(dataType), nullable, false, length));
        }

        public void AddColumn(string name, System.Data.SqlDbType dataType, bool nullable, bool primary)
        {
            this.columns.Add(new SqlServerColumn(name, TypeToString(dataType), nullable, primary));
        }

        public void AddColumn(string name, System.Data.SqlDbType dataType,bool nullable, bool primary, int length)
        {
            this.columns.Add(new SqlServerColumn(name, TypeToString(dataType), nullable, primary, length));
        }

        public string Generate()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CREATE TABLE ");
            sb.Append(schema);
            sb.Append(".");
            sb.Append(tableName);
            sb.Append("(");
            sb.AppendLine();
            bool first = true;
            foreach (var column in columns)
            {
                if (!first)
                {
                    sb.Append(",");
                    sb.AppendLine();
                    
                }
                first = false;
                sb.Append("\t");
                sb.Append(column.name);
                sb.Append(" ");
                sb.Append(column.type);
                if (column.length != -1)
                {
                    sb.Append("(");
                    sb.Append(column.length);
                    sb.Append(")");
                }
                if (column.nullable && !column.primary)
                {
                    sb.Append(" NULL");
                }
                else if (!column.primary)
                {
                    sb.Append(" NOT NULL");
                }
                if (column.primary)
                {
                    sb.Append(" NOT NULL");
                    sb.Append(" ");
                    sb.Append("PRIMARY KEY");
                }

            }
            sb.Append(");");
            sb.AppendLine();
            return sb.ToString();
        }


       
    }
}
