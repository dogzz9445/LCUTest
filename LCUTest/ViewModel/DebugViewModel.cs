using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotAPI;
using LCUTest.Model;
using Common;
using System.Diagnostics;
using System.IO;

namespace LCUTest.ViewModel
{
    public class DebugViewModel : BindableBase
    {
        private string _commandApiUrl;
        private string _commandApiMethod;
        public string CommandApiUrl { get => _commandApiUrl; set => SetProperty(ref _commandApiUrl, value); }
        public string CommandApiMethod { get => _commandApiMethod; set => SetProperty(ref _commandApiMethod, value); }

        public ObservableCollection<Log> Logs;

        private LCUClient _LCUClient;

        public DebugViewModel()
        {
            Logs = new ObservableCollection<Log>();
            _LCUClient = new LCUClient();
        }

        private DelegateCommand _connectCommand;
        public DelegateCommand ConnectCommand
        {
            get => _connectCommand ??= new DelegateCommand(() =>
            {
                try
                {
                    Process[] processes = Process.GetProcessesByName(LCUData.CLIENT_NAME);
                    LCUData.clientProcess = processes[0];

                    LCUData.LeaguePath = Path.GetDirectoryName(LCUData.clientProcess.MainModule.FileName);
                    var lockFilePath = Path.Combine(LCUData.LeaguePath, "lockfile");

                    using (var fileStream = new FileStream(lockFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(fileStream))
                    {
                        var text = reader.ReadToEnd();
                        string[] items = text.Split(':');
                        LCUData.ToKen = items[3];
                        LCUData.Port = ushort.Parse(items[2]);
                        LCUData.ApiUrl = "https://127.0.0.1:" + LCUData.Port.ToString() + "/";

                        Console.WriteLine($"Token : {LCUData.ToKen}");
                        Console.WriteLine($"Port : {LCUData.Port}");
                        Console.WriteLine($"ApiUri : {LCUData.ApiUrl}");
                    }

                    _LCUClient.Connect();

                    Logs.Add(new Log() { Description = "클라이언트 찾기 완료" });
                }
                catch
                {
                    Logs.Add(new Log() { Description = "클라이언트 찾기 실패" });
                }
            });
        }

        private DelegateCommand _testCommand;
        public DelegateCommand TestCommand
        {
            get => _testCommand ??= new DelegateCommand(async () =>
            {
                try
                {
                    var message = await _LCUClient.UsingApiEventHttpMessage(CommandApiMethod, CommandApiUrl);
                    Logs.Add(new Log() { Description = message.Content.ReadAsStringAsync().Result });
                }
                catch
                {
                    Logs.Add(new Log() { Description = "API 호출 실패" });
                }
            });
        }
    }
}
