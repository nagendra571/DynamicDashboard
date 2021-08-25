using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using WebApplication2.Models;

namespace Dynamic_User_Defined_Dashboards.Models
{
    public interface IDashboard_TutorialContext
    {
        DbSet<DashboardLinkedElements> DashboardLinkedElements { get; set; }
        DbSet<DashboardsInfo> DashboardsInfo { get; set; }
        DbSet<Elements> Elements { get; set; }
        DbSet<Templates> Templates { get; set; }
    }

    public static class DbContextExtensions
    {
        public static IEnumerable<string> GetTableNames(this DbContext @this)
        {
            var tableNames = new List<string>();


            using (var command = @this.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT Distinct TABLE_NAME FROM information_schema.TABLES";
                command.CommandType = CommandType.Text;

                @this.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        tableNames.Add(result.GetString(0));
                    }
                }
            }

            //using (var connection = @this.Database.GetDbConnection())
            //{
            //    connection.Open();
            //    using (var command = connection.CreateCommand())
            //    {
            //        command.CommandText = "SELECT Distinct TABLE_NAME FROM information_schema.TABLES";
            //        using (var reader = command.ExecuteReader())
            //        {
            //            if (reader.HasRows)
            //            {
            //                while (reader.Read())
            //                {
            //                    tableNames.Add(reader.GetString(0));
            //                }
            //            }
            //            reader.Close();
            //        }
            //    }
            //    connection.Close();
            //}


            return tableNames;
        }

        public static IEnumerable<string> GetColumnNames(this DbContext @this, string TableName)
        {
            var ColumnsNames = new List<string>();

            using (var command = @this.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + TableName + "'";
                command.CommandType = CommandType.Text;

                @this.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        ColumnsNames.Add(result.GetString(0));
                    }
                }
            }

            //using (var connection = @this.Database.GetDbConnection())
            //{
            //    connection.Open();
            //    using (var command = connection.CreateCommand())
            //    {
            //        command.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + TableName + "'";
            //        using (var reader = command.ExecuteReader())
            //        {
            //            if (reader.HasRows)
            //            {
            //                while (reader.Read())
            //                {
            //                    ColumnsNames.Add(reader.GetString(0));
            //                }
            //            }
            //            reader.Close();
            //        }
            //    }
            //    connection.Close();
            //}

            return ColumnsNames;
        }

        public static Dictionary<string, IEnumerable<string>> GetTableAndColumns(this DbContext @this)
        {
            var tableNames = new List<string>();
            var tableAndColumns = new Dictionary<string, IEnumerable<string>>();

            using (var command = @this.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT Distinct TABLE_NAME FROM information_schema.VIEWS";
                command.CommandType = CommandType.Text;

                @this.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        tableNames.Add(result.GetString(0));
                    }
                }
            }

            foreach (string table in tableNames)
            {
                var columnNames = new List<string>();
                using (var command = @this.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + table + "'";
                    command.CommandType = CommandType.Text;

                    @this.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            columnNames.Add(result.GetString(0));
                        }
                    }
                }
                tableAndColumns.Add(table, columnNames);
            }

            //using (var connection = @this.Database.GetDbConnection())
            //{
            //    connection.Open();
            //    using (var command = connection.CreateCommand())
            //    {
            //        command.CommandText = "SELECT Distinct TABLE_NAME FROM information_schema.TABLES";
            //        using (var reader = command.ExecuteReader())
            //        {
            //            if (reader.HasRows)
            //            {
            //                while (reader.Read())
            //                {
            //                    tableNames.Add(reader.GetString(0));
            //                }
            //            }
            //            reader.Close();
            //        }
            //    }


            //    foreach (string table in tableNames)
            //    {
            //        var columnNames = new List<string>();
            //        using (var command = connection.CreateCommand())
            //        {
            //            command.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + table + "'";
            //            using (var reader = command.ExecuteReader())
            //            {
            //                if (reader.HasRows)
            //                {
            //                    while (reader.Read())
            //                    {
            //                        columnNames.Add(reader.GetString(0));
            //                    }
            //                }
            //                reader.Close();
            //            }
            //        }
            //        tableAndColumns.Add(table, columnNames);
            //    }

            //    connection.Close();
            //}

            return tableAndColumns;
        }

        public static List<T> RawSqlQuery<T>(this DbContext @this, string query, Func<DbDataReader, T> map)
        {
            var entities = new List<T>();

            //using (var connection = @this.Database.GetDbConnection())
            //{
            //    using (var command = connection.CreateCommand())
            //    {
            //        command.CommandText = query;
            //        command.CommandType = CommandType.Text;

            //        //connection.Open();
            //        @this.Database.OpenConnection();

            //        using (var result = command.ExecuteReader())
            //        {
            //            while (result.Read())
            //            {
            //                entities.Add(map(result));
            //            }                        
            //        }
            //        //connection.Close();
            //    }
            //}

            using (var command = @this.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                @this.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }
                }
            }


            return entities;
        }
    }


}


