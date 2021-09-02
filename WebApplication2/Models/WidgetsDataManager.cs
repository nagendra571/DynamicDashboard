
using Dynamic_User_Defined_Dashboards.Models;
using System;
using System.Text.Json;

namespace WebApplication2.Models
{
    public static class WidgetsDataManager
    {
        public static object PrepareData(WidgetStructure obj) 
        {
            Type ObjInstance = GetTypeByElementId(obj.ElementID);

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

        private static Type GetTypeByElementId(int ID)
        {
            Type T = null;
            switch (ID)
            {
                case 7:
                    T = typeof(TileCard1);
                    break;
                case 6:
                    T = typeof(TileCard2);
                    break;
                case 5:
                case 4:
                case 3:
                    T = typeof(PieChart);
                    break;

            }
            return T;
        }
    }
}
