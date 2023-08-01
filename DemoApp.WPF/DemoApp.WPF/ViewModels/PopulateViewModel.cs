using DemoApp.DAL.Context;
using DemoApp.DAL.Entityes;
using DemoApp.WPF.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Grpc.Net.Client;
using DemoApp.DAL;
using DemoApp.WPF.Extensions;

namespace DemoApp.WPF.ViewModels
{
    class PopulateViewModel : BindableBase
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        const int NUM_TASKS = 1;

        private bool _IsRun = false;
        public bool IsRun
        {
            get { return _IsRun; }
            set { SetProperty(ref _IsRun, value); }
        }
        private string GRPC;


        string[] F = new string[10] { "Иванов", "Петров", "Сидоров", "Фёдоров", "Бабаев", "Алибабаев", "Абдурахманов", "Алексеев", "Денисов", "Паровозов" };
        string[] I = new string[10] { "Пётр", "Олег", "Роман", "Александр", "Денис", "Сидор", "Никифор", "Артём", "Владимир", "Руслан" };

        private DelegateCommand _StartCommand;
        private readonly WorkerContext _DB;
        private readonly IManageWorkers _WorkerService;

        public DelegateCommand StartCommand =>
            _StartCommand ?? (_StartCommand = new DelegateCommand(ExecuteStartCommand, CanExecuteStartCommand).ObservesProperty(() => IsRun));

        private bool CanExecuteStartCommand() => !IsRun;


        async void ExecuteStartCommand()
        {
            var ct = cts.Token;

            IsRun = true;
            await PopulateWorker(ct).ConfigureAwait(false);
        }

        private DelegateCommand _StopCommand;
        public DelegateCommand StopCommand =>
            _StopCommand ?? (_StopCommand = new DelegateCommand(ExecuteStopCommand, CanExecuteStopCommand).ObservesProperty(() => IsRun));

        private bool CanExecuteStopCommand() => IsRun;


        void ExecuteStopCommand()
        {
            cts.Cancel();
        }

        private async Task PopulateWorker(CancellationToken ct)
        {
            var rnd = new Random();
            using var channel = GrpcChannel.ForAddress(GRPC);
            var client = new WorkerCRUD.WorkerCRUDClient(channel);
            var call = client.ClientDataStream();
            while (true)
            {
                try
                {
                    await Task.Delay(rnd.Next(1000, 3000), ct);
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    IsRun = false;
                    // завершаем отправку сообшений в потоке
                    await call.RequestStream.CompleteAsync();
                    // получаем ответ сервера
                    CheckReply response = await call.ResponseAsync;
                    MessageBox.Show(response.Mes,"Сообщ. сервера:");
                    return;
                }
                catch (Exception ex)
                {
                    //
                }

                var child_cnt = rnd.Next(1, 10);
                Child[] childs = new Child[child_cnt];
                string name = rnd.NextItem(I);
                string fam = rnd.NextItem(F);
                string ot = name + "ович";
                Worker worker = new Worker { SurName = fam, FirstName = name, LastName = rnd.NextItem(I) + "ович", BirthDay = rnd.NextDate(new DateTime(1950, 10, 10), new DateTime(2000, 10, 10)), Pol = true };
                for (int i = 0; i < child_cnt; i++)
                    childs[i] = new Child { FullName = $"{fam} {rnd.NextItem(I)} {ot}", BirthDay = rnd.NextDate(new DateTime(2010, 10, 10), DateTime.Today) };
                worker.Childs.AddRange(childs);
                await call.RequestStream.WriteAsync(worker.ToWorkerReply());

               
            }
        }




        public PopulateViewModel()
        {
            GRPC = App.Configuration.GetSection("AppSettings")["GRPC"];
        }

    }
}
