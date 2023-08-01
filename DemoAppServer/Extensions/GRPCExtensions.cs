using DemoApp.DAL;
using DemoApp.DAL.Entityes;
using Google.Protobuf.WellKnownTypes;

namespace DemoAppServer.Extensions
{
    internal static class GRPCExtensions
    {
        static TimeOnly timeZero = new TimeOnly(12, 0, 0, 0, 0);
        internal static WorkerReply ToWorkerReply(this Worker worker)
        {
            var workerReply = new WorkerReply();
            workerReply.Id = worker.Id;
            workerReply.SurName = worker.SurName;
            workerReply.FirstName = worker.FirstName;
            workerReply.LastName = worker.LastName;
            workerReply.BirthDay = Timestamp.FromDateTimeOffset(worker.BirthDay.ToDateTime(timeZero));
            workerReply.Pol = worker.Pol;
            if (worker.Childs.Count > 0)
            {
                foreach (var child in worker.Childs)
                {
                    workerReply.Childs.Add(child.ToChildReply());
                }
            }
            return workerReply;
        }

        internal static Worker FromWorkerReply(this WorkerReply workerReply)
        {
            var worker = new Worker();
            worker.Id = workerReply.Id;
            worker.SurName = workerReply.SurName;
            worker.FirstName = workerReply.FirstName;
            worker.LastName = workerReply.LastName;
            worker.BirthDay = DateOnly.FromDateTime(workerReply.BirthDay.ToDateTime());
            worker.Pol = workerReply.Pol;
            if (workerReply.Childs.Count > 0)
            {
                foreach (var child in workerReply.Childs)
                {
                    worker.Childs.Add(child.FromChildReply());
                }
            }
            return worker;
        }

        internal static ChildReply ToChildReply(this Child child)
        {
            var childReply = new ChildReply();
            childReply.Id = child.Id;
            childReply.FullName = child.FullName;
            childReply.BirthDay = Timestamp.FromDateTimeOffset(child.BirthDay.ToDateTime(timeZero));
            childReply.Workerid = child.WorkerId;
            return childReply;
        }

        internal static Child FromChildReply(this ChildReply childReply)
        {
            var child = new Child();
            child.Id = childReply.Id;
            child.FullName = childReply.FullName;
            child.BirthDay = DateOnly.FromDateTime(childReply.BirthDay.ToDateTime());
            child.WorkerId = childReply.Workerid;
            return child;
        }
        internal static StatReply ToStatReply(this Stat001 stat)
        {
            var statReply = new StatReply();
            statReply.FullName = stat.FIO;
            statReply.BirthDay = Timestamp.FromDateTimeOffset(stat.DR.ToDateTime(timeZero));
            statReply.Childcnt = stat.ChildCnt;
            return statReply;
        }

        internal static Stat001 FromStatReply(this StatReply statReply)
        {
            var stat = new Stat001();
            stat.FIO = statReply.FullName;
            stat.DR = DateOnly.FromDateTime(statReply.BirthDay.ToDateTime());
            stat.ChildCnt = statReply.Childcnt;
            return stat;
        }
    }
}
