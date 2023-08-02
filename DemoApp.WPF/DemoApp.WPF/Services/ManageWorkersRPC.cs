using DemoApp.DAL;
using DemoApp.DAL.Context;
using DemoApp.DAL.Entityes;
using DemoApp.WPF.Extensions;


using DemoApp.WPF.Services.Interfaces;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoApp.WPF.Services
{
    internal class ManageWorkersRPC : IManageWorkers
    {
        private WorkerContext _DB;
        private string GRPC;


        public async Task<Child> AddChildAsync(Child child, Worker worker)
        {
            child.WorkerId = worker.Id;
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            ChildReply childReply = await client.CreateChildAsync(child.ToChildReply());
            return childReply.FromChildReply();
        }

        public Task<bool> AddChildsRangeAsync(Child[] childs, Worker worker)
        {
            throw new NotImplementedException();
        }


        public async Task<Worker> AddWorkerAsync(Worker worker)
        {
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            WorkerReply workerReply = await client.CreateWorkerAsync(worker.ToWorkerReply());
            return workerReply.FromWorkerReply();
        }


        public async Task<bool> DeleteChildAsync(Child child)
        {
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            BoolReply result = await client.DeleteChildAsync(new DeleteChildRequest { Id = child.Id });

            return result.Result;
        }


        public async Task<bool> DeleteWorkerAsync(Worker worker)
        {
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            BoolReply result = await client.DeleteWorkerAsync(new DeleteWorkerRequest { Id = worker.Id } );

            return result.Result;
        }


        public async Task<Child> EditChildAsync(Child child)
        {
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            ChildReply result = await client.UpdateChildAsync(child.ToChildReply());

            return result.FromChildReply();
        }


        public async Task<Worker> EditWorkerAsync(Worker worker)
        {
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            WorkerReply result = await client.UpdateWorkerAsync(worker.ToWorkerReply());

            return result.FromWorkerReply();
        }


        public async Task<List<Worker>> GetAllWorkersAsync()
        {
            List<Worker> workerList = new();
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            ListReply workers = await client.ListWorkersAsync(new Google.Protobuf.WellKnownTypes.Empty());
            foreach(WorkerReply worker in workers.Workers)
            {
                workerList.Add(worker.FromWorkerReply());
            }
            return workerList;
        }

        public async Task<List<WorkerChildCountStatistic>> GetAllStatAsync()
        {
            List<WorkerChildCountStatistic> statList = new();
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            ListStatReply statReply = await client.ListStatAsync(new Google.Protobuf.WellKnownTypes.Empty());
            foreach (StatReply stat in statReply.Stats)
            {
                statList.Add(stat.FromStatReply());
            }
            return statList;
        }

        public bool IsDBActive()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsDBActiveAsync(CancellationToken ct)
        {
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            try
            {
                CheckReply checkDB = await client.CheckDBAsync(new Google.Protobuf.WellKnownTypes.Empty());
                return (checkDB.Mes == "OK") ? true : false;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public async Task ReCreateDB()
        {
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            CheckReply checkDB = await client.ReCreateDBAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return;
        }



        public ManageWorkersRPC(WorkerContext db)
        {
            _DB = db;
            GRPC = App.Configuration.GetSection("AppSettings")["GRPC"];
        }
    }
}
