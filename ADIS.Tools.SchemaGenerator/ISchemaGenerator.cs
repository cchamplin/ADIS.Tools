using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemaGenerator
{
    public interface ISchemaGenerator
    {
        string Generate(Type t);
    }
}
