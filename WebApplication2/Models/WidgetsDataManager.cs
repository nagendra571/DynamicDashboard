
using Dynamic_User_Defined_Dashboards.Models;
using System;
using System.Text.Json;

namespace WebApplication2.Models
{
    public static class WidgetsDataManager
    {
        public static object PrepareData(WidgetStructure obj) 
        {
            Type ObjInstance = obj.ClassType.Contains("Tilecard1", StringComparison.OrdinalIgnoreCase) ? typeof(TileCard1) : typeof(TileCard2);

            var WidgetObject = JsonSerializer.Deserialize(obj.Formation, ObjInstance);            

            return WidgetObject;
        }

        private static object GetInstance(string strFullyQualifiedName)
        {
            Type t = Type.GetType(strFullyQualifiedName);
            return Activator.CreateInstance(t);
        }

        private static Type GetType(string strFullyQualifiedName)
        {
            Type t = Type.GetType(strFullyQualifiedName);
            return t;
        }
    }
}
