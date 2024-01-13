using BasicTemplate.Base;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace BasicTemplate.Control
{
    /// <summary>
    /// DataTipInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataTipInfo : UserControl
    {
        public DataTipInfo()
        {
            InitializeComponent();
        }
    }

    class vmDataTipInfo : ObservableObject
    {
        public event EventHandler ClearDataTipEvt;
        public bool bIsDataTipShown;

        private Visibility _VisibleTip;
        public Visibility VisibleTip
        {
            get => _VisibleTip;
            set
            {
                _VisibleTip = value;
                OnPropertyChanged("VisibleTip");
            }
        }

        private string _Xvalue;
        public string Xvalue
        {
            get => _Xvalue;
            set
            {
                _Xvalue = value;
                OnPropertyChanged("Xvalue");
            }
        }

        private string _Yvalue;
        public string Yvalue
        {
            get => _Yvalue;
            set
            {
                _Yvalue = value;
                OnPropertyChanged("Yvalue");
            }
        }

        private ICommand _ClearDataTipCmd;
        public ICommand ClearDataTipCmd
        {
            get
            {
                if (_ClearDataTipCmd == null)
                    _ClearDataTipCmd = new BaseCommand(p =>
                    {
                        ClearDataTipEvt.Invoke(null, new EventArgs());

                    });
                return _ClearDataTipCmd;
            }
        }

        public void VisibleToggler(bool En)
        {
            if (En)
                VisibleTip = Visibility.Visible;
            else
                VisibleTip = Visibility.Collapsed;

            bIsDataTipShown = En;
        }

        public vmDataTipInfo() 
        {
            Xvalue = "-";
            Yvalue = "-";
            VisibleTip = Visibility.Collapsed;
        }
    }
}
