using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.WPF
{
    public static class AppSettings
    {
        public static string DBType { get; private set; }
        public static string DBCS { get; private set; }
        public static string Redis { get; private set; }

        public static void InitConfig()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            DBType = config.GetSection("DBType").Value;
            DBCS = config.GetSection("DBType").Value;
            Redis = config.GetSection("DBType").Value;
        }
    }
}
