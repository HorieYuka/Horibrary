using BasicTemplate.Base;
using BasicTemplate.Page;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasicTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new WindowNavigation();
        }
    }

    class WindowNavigation : ObservableObject
    {
        public BelowStatusModel BelowStatus { get; set; }

        public List<IPage> PageList { get; set; }

        private IPage _CurrentPage;
        public IPage CurrentPage
        {
            get => _CurrentPage;
            set
            {
                _CurrentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        private ICommand _ChangePageCmd;
        public ICommand ChangePageCmd
        {
            get
            {
                if (_ChangePageCmd == null)
                    _ChangePageCmd = new BaseCommand(p => {
                        int Idx = int.Parse((string)p);
                        CurrentPage = PageList[Idx];
                        CurrentPage.RetrieveState();
                    });
                return _ChangePageCmd;
            }
        }


        public WindowNavigation()
        {
            // Define common event
            BelowStatus = new BelowStatusModel();
            Helper.EvtBelowStatus += UpdateBelowStatus;

            // Define pages
            PageList = [
                new vmPageCommon(), 
                new vmPageGraphControl(),
                new vmPageDeviceControl(),
                ];
        }

        private void UpdateBelowStatus(object sender, EventArgs e)
        {
            BelowStatusModel Md = (BelowStatusModel) sender;

            if(Md.BelowBar != null) BelowStatus.BelowBar = Md.BelowBar;
            if (Md.BelowText != null) BelowStatus.BelowText = Md.BelowText;
            if (Md.BelowCircle != null) BelowStatus.BelowCircle = Md.BelowCircle;
        }
    }
}