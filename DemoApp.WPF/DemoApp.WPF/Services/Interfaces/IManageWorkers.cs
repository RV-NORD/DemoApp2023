using DemoApp.DAL.Entityes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DemoApp.WPF.Services.Interfaces
{
    public interface IManageWorkers
    {
        Task<Child> AddChildAsync(Child child, Worker worker);
        Task<bool> AddChildsRangeAsync(Child[] childs, Worker worker);
        Task<Worker> AddWorkerAsync(Worker worker);
        Task<bool> DeleteChildAsync(Child child);
        Task<bool> DeleteWorkerAsync(Worker worker);
        Task<Child> EditChildAsync(Child child);
        Task<Worker> EditWorkerAsync(Worker worker);
        Task<List<Stat001>> GetAllStatAsync();

        Task<List<Worker>> GetAllWorkersAsync();
        bool IsDBActive();
        Task<bool> IsDBActiveAsync(CancellationToken ct);
        Task ReCreateDB();
    }
}
