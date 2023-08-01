using DemoApp.DAL.Entityes;

namespace DemoAppServer.Services.Interfaces
{
    public interface IManageWorkers
    {
        Task<Child> AddChildAsync(Child child);

        Task<bool> AddChildsRangeAsync(Child[] childs);
        Task<Worker> AddWorkerAsync(Worker worker);
        Task<bool> CheckConnectAsync();
        Task<bool> DeleteChildAsync(int id);

        Task<bool> DeleteWorkerAsync(int id);

        Task<Worker> EditWorkerAsync(Worker worker);
        Task<List<Worker>> GetAllWorkersAsync();
        Task<List<Stat001>> GetAllStatsAsync();
        Task<Worker> GetWorkerAsync(int id);
        Task<Child> GetChildAsync(int id);
        Task<bool> IsDBActiveAsync(CancellationToken ct);
        Task ReCreateDB();
        Task<Child> EditChildAsync(Child child);
    }
}
