using Dynamic_User_Defined_Dashboards.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class ContextHelper : IContextHelper
    {
        private readonly ILogger<ContextHelper> logger;
        private readonly Dashboard_TutorialContext context;

        private static Dictionary<Type, string> tableNames = new Dictionary<Type, string>(30);

        private Dictionary<Type, Dictionary<string, string>> columnNames = new Dictionary<Type, Dictionary<string, string>>(30);

        public ContextHelper(ILogger<ContextHelper> logger, Dashboard_TutorialContext context)
        {
            this.logger = logger;
            this.context = context;            
        }


        public void PopulateValues()
        {
            PopulateTableNames();
            PopulateColumnNames();
        }

        public Dictionary<Type, string> PopulateTableNames()
        {
            logger.LogInformation("Populating table names in context helper");

            //foreach (var entityType in context.Model.GetEntityTypes())
            //{
            //    tableNames.Add(entityType.ClrType, entityType.Name);
            //}

            return tableNames;
        }

        public Dictionary<Type, Dictionary<string, string>> PopulateColumnNames()
        {
            logger.LogInformation("Populating column names in context helper");

            //foreach (var entityType in context.Model.GetEntityTypes())
            //{
            //    var clrType = entityType.ClrType;

            //    if (!columnNames.ContainsKey(clrType))
            //    {
            //        columnNames.Add(clrType, new Dictionary<string, string>(30));
            //    }

            //    foreach (var property in entityType.GetProperties())
            //    {
            //        columnNames[clrType].Add(property.Name, property.Name);
            //    }
            //}
            return columnNames;
        }

        public string GetTableName<T>()
        {
            return context.Model.FindEntityType(typeof(T).Name).Name;
        }

        public string GetColumnName<T>(string propertyName)
        {
            return columnNames[typeof(T)][propertyName];
        }

        public List<string> GetColumnNames<T>()
        {
            return columnNames[typeof(T)].Select(x => x.Value).ToList();
        }
    }
}
