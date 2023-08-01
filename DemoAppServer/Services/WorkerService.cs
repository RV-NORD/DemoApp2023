using Azure.Core;
using Azure;
using DemoApp.DAL;
using DemoApp.DAL.Context;
using DemoApp.DAL.Entityes;
using DemoAppServer.Extensions;
using DemoAppServer.Services.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace DemoAppServer.Services
{
    public class WorkerService : WorkerCRUD.WorkerCRUDBase
    {
        private IManageWorkers _Service;

        public WorkerService(IManageWorkers service)
        {
            _Service = service;
        }


        public override async Task<CheckReply> CheckDB(Empty request, ServerCallContext context)
        {
            return await _Service.CheckConnectAsync() ? new CheckReply { Mes = "OK" } : new CheckReply { Mes = "NEOK" };
        }

        public override async Task<CheckReply> ReCreateDB(Empty request, ServerCallContext context)
        {
            await _Service.ReCreateDB();
            return new CheckReply { Mes = "OK" };
        }

        public async override Task<ListReply> ListWorkers(Empty request, ServerCallContext context)
        {
            var workers = await _Service.GetAllWorkersAsync();
            ListReply reply = new ListReply();
            if (workers.Count > 0)
            {
                foreach (var worker in workers)
                {
                    reply.Workers.Add(worker.ToWorkerReply());
                }
            }
            return reply;
        }
        public async override Task<WorkerReply> CreateWorker(WorkerReply request, ServerCallContext context)
        {
            var result = await _Service.AddWorkerAsync(request.FromWorkerReply());
            return result.ToWorkerReply();
        }

        public async override Task<BoolReply> DeleteWorker(DeleteWorkerRequest request, ServerCallContext context)
        {
            return new BoolReply { Result = await _Service.DeleteWorkerAsync(request.Id)};
        }

        public async override Task<WorkerReply> UpdateWorker(WorkerReply request, ServerCallContext context)
        {
            var result = await _Service.EditWorkerAsync(request.FromWorkerReply());
            return result.ToWorkerReply();
        }

        public override async Task<WorkerReply> GetWorker(GetWorkerRequest request, ServerCallContext context)
        {
            var result = await _Service.GetWorkerAsync(request.Id).ConfigureAwait(false);
            return result.ToWorkerReply();
        }
        public async override Task<ChildReply> CreateChild(ChildReply request, ServerCallContext context)
        {
            var result = await _Service.AddChildAsync(request.FromChildReply());
            return result.ToChildReply();
        }

        public async override Task<BoolReply> DeleteChild(DeleteChildRequest request, ServerCallContext context)
        {
            return new BoolReply { Result = await _Service.DeleteChildAsync(request.Id) };
        }

        public async override Task<ChildReply> UpdateChild(ChildReply request, ServerCallContext context)
        {
            var result = await _Service.EditChildAsync(request.FromChildReply());
            return result.ToChildReply();
        }
        public async override Task<ListStatReply> ListStat(Empty request, ServerCallContext context)
        {
            var stats = await _Service.GetAllStatsAsync();
            ListStatReply reply = new ListStatReply();
            if (stats.Count > 0)
            {
                foreach (var stat in stats)
                {
                    reply.Stats.Add(stat.ToStatReply());
                }
            }
            return reply;
        }

        public async override Task<CheckReply> ClientDataStream(IAsyncStreamReader<WorkerReply> requestStream, ServerCallContext context)
        {
            await foreach (WorkerReply request in requestStream.ReadAllAsync())
            {
                await _Service.AddWorkerAsync(request.FromWorkerReply());
            }
            Console.WriteLine("Все данные получены...");
            return new CheckReply { Mes = "все данные получены" };
        }
    }
}
