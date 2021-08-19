using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public interface IContextHelper
    {
        void PopulateValues();
        string GetColumnName<T>(string propertyName);
        List<string> GetColumnNames<T>();
        string GetTableName<T>();
        Dictionary<Type, Dictionary<string, string>> PopulateColumnNames();
        Dictionary<Type, string> PopulateTableNames();
    }
}