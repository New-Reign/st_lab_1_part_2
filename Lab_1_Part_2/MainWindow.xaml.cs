using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Timer);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        public class ProcessInfo
        {
            public string ProcessName { get; set; }
            public DateTime StartTime { get; set; }
            public int Id { get; set; }
            public long Memory { get; set; }
            public int Threads { get; set; }
            public TimeSpan TotalTime { get; set; }
        }

        public List<ProcessInfo> LoadCollectionData()
        {
            List<ProcessInfo> Processes = new List<ProcessInfo>();
            foreach (Process p in Process.GetProcesses())
            {
                using (p)
                {
                    try
                    {
                        Processes.Add(new ProcessInfo()
                        {
                            ProcessName = p.ProcessName,
                            Id = p.Id,
                            Memory = p.WorkingSet64,
                            Threads = p.Threads.Count,
                            StartTime = p.StartTime,
                            TotalTime = p.TotalProcessorTime
                        });
                    }
                    catch
                    {
                        Processes.Add(new ProcessInfo()
                        {
                            ProcessName = p.ProcessName,
                            Id = p.Id
                        });
                    }
                }
            }
            return Processes;
        }

        public void Timer(object sender, EventArgs e)
        {
            InitializeComponent();
            LoadCollectionData();
            dataGrid1.ItemsSource = LoadCollectionData();
            this.Title = "Currently Active Processes = " + LoadCollectionData().Count;

            CommandManager.InvalidateRequerySuggested();
        }
    }
}