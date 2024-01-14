using BasicTemplate.Base;
using BasicTemplate.Control;
using ScottPlot;
using ScottPlot.Plottable;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace BasicTemplate.Example
{
    /// <summary>
    /// ExampleLiveChairPlot.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExampleLiveChairPlot : UserControl
    {
        public ExampleLiveChairPlot()
        {
            InitializeComponent();
        }
    }

    class vmExampleLiveChairPlot : ObservableObject, IExample
    {
        public string ExampleName => "실시간 그래프, 크로스 헤어 생성하기";
        public short ExampleNum => 2;

        public WpfPlot PlotBase { get; set; }


        private Crosshair Chair;
        private SignalPlot Sigplot;
        private BackgroundWorker bWorker;
        private double[] PlotDataBuffer;

        private vmTextboxWithClearBtn _TextPresetTime;
        public vmTextboxWithClearBtn TextPresetTime
        {
            get => _TextPresetTime;
            set
            {
                _TextPresetTime = value;
                OnPropertyChanged("TextPresetTime");
            }
        }

        private vmDataTipInfo _DataTip;
        public vmDataTipInfo DataTip
        {
            get => _DataTip;
            set
            {
                _DataTip = value;
                OnPropertyChanged("DataTip");
            }
        }


        private bool _bTrigger;
        public bool bTrigger
        {
            get => _bTrigger;
            set
            {
                _bTrigger = value;
                OnPropertyChanged("bTrigger");
            }
        }


        private ICommand _CreatePlotCmd;
        public ICommand CreatePlotCmd
        {
            get
            {
                if (_CreatePlotCmd == null)
                    _CreatePlotCmd = new BaseCommand(p =>
                    {

                        if (bTrigger == false)
                        {
                            // Check values
                            if (!ValidateValue())
                            {
                                Helper.UpdateBelowStatus(null, "입력값에 오류가 있습니다.", null);
                                return;
                            }

                            bTrigger = true;
                            PlotBase.Refresh();

                            bWorker.RunWorkerAsync();
                        }
                        else
                            bTrigger = false;

                    });
                return _CreatePlotCmd;
            }
        }


        private ICommand _DataTipActionCmd;
        public ICommand DataTipActionCmd
        {
            get
            {
                if (_DataTipActionCmd == null)
                    _DataTipActionCmd = new BaseCommand(p =>
                    {
                        string Instruction = (string)p;

                        if (!DataTip.bIsDataTipShown)
                        {
                            Chair.IsVisible = true;
                            DataTip.VisibleToggler(true);
                        }

                        if (p == null)
                        {
                            (double mouseCoordX, double mouseCoordY) = PlotBase.GetMouseCoordinates();
                            double xyRatio = PlotBase.Plot.XAxis.Dims.PxPerUnit / PlotBase.Plot.YAxis.Dims.PxPerUnit;

                            (double Px, double Py, int PIdx)
                            = Sigplot.GetPointNearestX(mouseCoordX);

                            Chair.X = PIdx;
                            Chair.Y = PlotDataBuffer[PIdx];
                            DataTip.Xvalue = PIdx.ToString();
                            DataTip.Yvalue = PlotDataBuffer[PIdx].ToString();
                        }
                    });
                return _DataTipActionCmd;
            }
        }

        private void RunLivePlot(object? sender, DoWorkEventArgs e)
        {
            List<double> Stack = new List<double>();

            Stopwatch Sw = new Stopwatch();
            RandomDataGenerator Ran = new RandomDataGenerator();

            int SampleValue = (int)ModelConstChart3.MaxPlotBuffLength;
            int TimeValue = int.Parse(TextPresetTime.Text);

            Sw.Start();
            while (bTrigger && Sw.Elapsed.TotalMinutes < TimeValue)
            {
                double[] DataInput = Ran.RandomSample(SampleValue);

                for (int i = 0; i < DataInput.Length; i++)
                    PlotDataBuffer[i] += DataInput[i];

                PlotBase.Plot.SetAxisLimitsY(0, PlotDataBuffer.Max()  * 1.2);

                if(DataTip.bIsDataTipShown)
                {
                    Chair.Y = PlotDataBuffer[(int)Chair.X];
                    DataTip.Yvalue = PlotDataBuffer[(int)Chair.X].ToString();
                }

                UiInvoke(delegate { PlotBase.Refresh(); });
                Thread.Sleep(50);
            }

        }

        public bool ValidateValue()
        {
            int Dummy = 0;

            if (int.TryParse(TextPresetTime.Text, out Dummy))
                if (Dummy < 0 && Dummy > ModelConstChart3.MeasureTimeLimit)
                    return false;
            return true;
        }

        private void ClearDataTip(object? sender, EventArgs e)
        {
            DataTip.VisibleToggler(false);
            Chair.IsVisible = false;

            if(!bTrigger)
                PlotBase.Refresh();
        }


        public vmExampleLiveChairPlot()
        {
            // Create plot
            PlotBase = new WpfPlot();
            PlotBase.Configuration.DoubleClickBenchmark = false;
            PlotBase.Configuration.LockVerticalAxis = true;
            PlotBase.Configuration.LockHorizontalAxis = true;
            PlotBase.Plot.SetAxisLimitsX(0, (int)ModelConstChart3.MaxPlotBuffLength);
            PlotBase.Plot.SetAxisLimitsY(0, 10);

            // InitializePlot
            PlotDataBuffer = new double[(int)ModelConstChart3.MaxPlotBuffLength];
            Sigplot = PlotBase.Plot.AddSignal(PlotDataBuffer);
            Sigplot.MinRenderIndex = 0;
            Sigplot.MaxRenderIndex = (int)ModelConstChart3.MaxPlotBuffLength - 1;
            Chair = PlotBase.Plot.AddCrosshair(0, 0);
            Chair.IsVisible = false;
            PlotBase.Refresh();

            // Create Controls
            DataTip = new vmDataTipInfo();
            DataTip.ClearDataTipEvt += ClearDataTip;

            TextPresetTime = new vmTextboxWithClearBtn(
                _MaxLimit: ModelConstChart3.MeasureTimeLimit.ToString(),
                _MinLimit: "1",
                _Txt: "10");

            // Set Background thread
            bWorker = new BackgroundWorker();
            bWorker.DoWork += RunLivePlot;
        }


    }



}
