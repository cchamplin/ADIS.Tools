using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.ComponentServices;

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

        protected string TypeToString(DbDataType t)
        {
            switch (t)
            {
                case DbDataType.BIT:
                    return "bit";
                case DbDataType.BIGINT:
                    return "bigint";
                case DbDataType.CHAR:
                    return "char";
                case DbDataType.DATETIME:
                    return "datetime";
                case DbDataType.DECIMAL:
                    return "decimal";
                case DbDataType.FLOAT:
                    return "float";
                case DbDataType.IMAGE:
                    return "image";
                case DbDataType.INT:
                    return "int";
                case DbDataType.MONEY:
                    return "money";

                case DbDataType.VARCHAR:
                    return "nvarchar";
                case DbDataType.REAL:
                    return "real";
                case DbDataType.SHORTDATETIME:
                    return "smalldatetime";
                case DbDataType.SMALLINT:
                    return "smallint";
                case DbDataType.TEXT:
                    return "text";
                case DbDataType.TIME:
                    return "time";
                case DbDataType.TIMESTAMP:
                    return "timestamp";
                case DbDataType.TINYINT:
                    return "tinyint";
                case DbDataType.UNIQUEIDENTIFIER:
                case DbDataType.UUID:
                case DbDataType.GUID:
                    return "uniqueidentifier";
                case DbDataType.VARBINARY:
                case DbDataType.BINARYVAR:
                case DbDataType.VARBIN:
                    return "varbinary";
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

        public void AddColumn(string name, DbDataType dataType,bool nullable)
        {
            this.columns.Add(new SqlServerColumn(name, TypeToString(dataType), nullable, false));
        }

        public void AddColumn(string name, DbDataType dataType, bool nullable, int length)
        {
            this.columns.Add(new SqlServerColumn(name, TypeToString(dataType), nullable, false, length));
        }

        public void AddColumn(string name, DbDataType dataType, bool nullable, bool primary)
        {
            this.columns.Add(new SqlServerColumn(name, TypeToString(dataType), nullable, primary));
        }

        public void AddColumn(string name, DbDataType dataType, bool nullable, bool primary, int length)
        {
            this.columns.Add(new SqlServerColumn(name, TypeToString(dataType), nullable, primary, length));
        }

        public string Generate()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CREATE TABLE [");
            sb.Append(schema);
            sb.Append("].[");
            sb.Append(tableName);
            sb.Append("](");
            sb.AppendLine();
            bool first = true;

            // Place primary key first
            foreach (var column in columns.OrderBy(x => x.primary).Reverse())
            {
                if (!first)
                {
                    sb.Append(",");
                    sb.AppendLine();
                    
                }
                first = false;
                sb.Append("\t[");
                sb.Append(column.name);
                sb.Append("] [");
                sb.Append(column.type);
                sb.Append("]");
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
