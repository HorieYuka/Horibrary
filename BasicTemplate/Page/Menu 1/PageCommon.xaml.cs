using BasicTemplate.Base;
using MaterialDesignThemes.Wpf;
using ScottPlot;
using System.IO.Ports;
using System.Management;
using System.Windows.Controls;
using System.Windows.Input;

namespace BasicTemplate.Page
{
    /// <summary>
    /// PageCommon.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PageCommon : UserControl
    {
        public PageCommon()
        {
            InitializeComponent();
        }
    }

    class vmPageCommon : ObservableObject, IPage
    {
        public PackIconKind PageIcon => PackIconKind.Apps;
        public string PageName => "공통";
        public short PageNum => 0;

        public vmPageCommon()
        {


        }


        private ICommand _FindGPIBs;
        public ICommand FindGPIBs
        {
            get
            {
                if (_FindGPIBs == null)
                    _FindGPIBs = new BaseCommand(p => {


                    });
                return _FindGPIBs;
            }
        }


    }
}
