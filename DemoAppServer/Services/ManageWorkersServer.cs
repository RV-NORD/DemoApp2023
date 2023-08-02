using Azure.Core;
using DemoApp.DAL.Context;
using DemoApp.DAL.Entityes;
using DemoAppServer.Extensions;
using DemoAppServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace DemoAppServer.Services
{
    public class ManageWorkersServer : IManageWorkers
    {
        private WorkerContext _DB;

        public async Task<Worker> AddWorkerAsync(Worker worker)
        {
            var result = await _DB.Worker.AddAsync(worker);
            await _DB.SaveChangesAsync().ConfigureAwait(false);
            return result.Entity;
        }
        public async Task<Child> AddChildAsync(Child child)
        {
            var result = await _DB.Childs.AddAsync(child);
            await _DB.SaveChangesAsync().ConfigureAwait(false);
            return result.Entity;
        }
        public async Task<bool> AddChildsRangeAsync(Child[] childs)
        {
            await _DB.Childs.AddRangeAsync(childs);
            return (await _DB.SaveChangesAsync().ConfigureAwait(false) > 0) ? true : false;
        }

        public async Task<bool> DeleteWorkerAsync(int id)
        {
            _DB.Worker.Remove(new Worker { Id = id });
            return (await _DB.SaveChangesAsync().ConfigureAwait(false) > 0) ? true : false;
        }
        public async Task<bool> DeleteChildAsync(int id)
        {
            _DB.Childs.Remove(new Child { Id = id });
            return (await _DB.SaveChangesAsync().ConfigureAwait(false) > 0) ? true : false;
        }

        public async Task<Worker> EditWorkerAsync(Worker worker)
        {
            var old_worker =await GetWorkerAsync(worker.Id);

            var type = worker.GetType();

            foreach (PropertyInfo prop in type.GetProperties().Where(p => p.CanWrite))
            {
                
                if (prop is null || !prop.CanWrite || prop.GetValue(worker)?.ToString() == prop.GetValue(old_worker)?.ToString()) continue;
                prop.SetValue(old_worker, prop.GetValue(worker));
            }
            await _DB.SaveChangesAsync().ConfigureAwait(false);


            return old_worker;
        }
        public async Task<Child> EditChildAsync(Child child)
        {
            var old_child = await GetChildAsync(child.Id);

            var type = child.GetType();

            foreach (PropertyInfo prop in type.GetProperties().Where(p => p.CanWrite))
            {

                if (prop is null || !prop.CanWrite || prop.GetValue(child)?.ToString() == prop.GetValue(old_child)?.ToString()) continue;
                prop.SetValue(old_child, prop.GetValue(child));
            }
            await _DB.SaveChangesAsync().ConfigureAwait(false);


            return old_child;
        }

        public async Task<List<Worker>> GetAllWorkersAsync()
        {
            return await _DB.Worker.Include(x => x.Childs).AsSplitQuery().ToListAsync().ConfigureAwait(false);
        }
        public async Task<List<WorkerChildCountStatistic>> GetAllStatsAsync()
        {
            return await _DB.Worker.Select(w => new WorkerChildCountStatistic
            {
                FullName = w.Name,
                BirthDay = w.BirthDay,
                ChildCount = w.Childs.Count
            }
                ).ToListAsync();

        }

        public async Task<Worker> GetWorkerAsync(int id)
        {
            return await _DB.Worker.FirstOrDefaultAsync(item => item.Id == id).ConfigureAwait(false);
        }
        public async Task<Child> GetChildAsync(int id)
        {
            return await _DB.Childs.FirstOrDefaultAsync(item => item.Id == id).ConfigureAwait(false);
        }


        public Task<bool> IsDBActiveAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task ReCreateDB()
        {
            await _DB.Database.EnsureDeletedAsync();
            await _DB.Database.EnsureCreatedAsync();
        }
        public async Task<bool> CheckConnectAsync() => await _DB.Database.CanConnectAsync();

        public ManageWorkersServer(WorkerContext db)
        {
            _DB = db;
        }
    }
}
