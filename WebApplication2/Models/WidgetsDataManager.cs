
using Dynamic_User_Defined_Dashboards.Models;
using System;
using System.Text.Json;

namespace WebApplication2.Models
{
    public static class WidgetsDataManager
    {
        public static object PrepareData(WidgetStructure obj) 
        {
            Type ObjInstance = GetType(obj.ClassType);

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
