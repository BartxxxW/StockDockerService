using Microsoft.EntityFrameworkCore.Storage;
using System.Configuration;

namespace StockAPI
{
    public static class ConfigData
    {
        public static string Database="";
        public static string ConnectionStringSettings = $"Server=BARTEK;Database={Database};Trusted_Connection=True;TrustServerCertificate=True;";
        public static string ChangeDb(string dbName)
        {
            return string.Empty;
        }
    }
}
