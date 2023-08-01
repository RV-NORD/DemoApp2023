using DemoApp.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace DemoAppServer.Data
{
    public static class DBRegistrator
    {

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config) => services
             .AddDbContext<WorkerContext>(opt =>
             {
                 var type = config.GetSection("AppSettings")["DBType"];
                 switch (type)
                 {
                     case null: throw new InvalidOperationException("Не определён тип БД");
                     default: throw new InvalidOperationException($"Тип подключения {type} не поддерживается");
                     case "PGSQL":
                         opt.UseNpgsql(config.GetSection("AppSettings")[type]);
                         break;
                     case "MSSQL":
                         opt.UseSqlServer(config.GetSection("AppSettings")[type]);
                         break;
                     case "SQLite":
                         opt.UseSqlite(config.GetSection("AppSettings")[type]);
                         break;
                     case "InMemory":
                         opt.UseInMemoryDatabase("Worker.db");
                         break;
                 }
             }
             )
         ;

    }
}
