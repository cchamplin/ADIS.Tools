using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ADIS.Core.ComponentServices;
namespace SchemaGenerator
{
    public interface ISQLSyntaxGenerator
    {
        void SetTable(string name);
        void SetTable(string schema, string name);
        void AddColumn(string name, Type dataType, bool nullable);
        void AddColumn(string name, Type dataType, bool nullable, int length);
        void AddColumn(string name, Type dataType, bool nullable, bool primary);
        void AddColumn(string name, Type dataType, bool nullable, bool primary, int length);
        void AddColumn(string name, DbDataType dataType, bool nullable);
        void AddColumn(string name, DbDataType dataType, bool nullable, int length);
        void AddColumn(string name, DbDataType dataType, bool nullable, bool primary);
        void AddColumn(string name, DbDataType dataType, bool nullable, bool primary, int length);
        string Generate();
    }
}
