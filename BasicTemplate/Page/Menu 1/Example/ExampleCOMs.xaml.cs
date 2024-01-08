using BasicTemplate.Base;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
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

namespace BasicTemplate.Page.Menu_1.Example
{
    /// <summary>
    /// ExampleCOMs.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExampleCOMs : UserControl
    {
        public ExampleCOMs()
        {
            InitializeComponent();
        }
    }

    class vmExampleCOMs : ObservableObject
    {
        List



        private ICommand _FindCOMs;
        public ICommand FindCOMs
        {
            get
            {
                if (_FindCOMs == null)
                    _FindCOMs = new BaseCommand(p => {

                        string ComName = "USB Serial Port";

                        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
                        {
                            var ComNames = SerialPort.GetPortNames();
                            var Searcher = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());
                            var ComList = ComNames.Select(n => Searcher.FirstOrDefault(s => s.Contains(n))).ToList();

                            for (int i = 0; i < ComList.Count(); i++)
                            {
                                if (ComList[i] == null) continue;
                                else if (ComList[i].Contains(ComName))
                                {
                                    RlyBd = new SerialPort(COMS[i], 115200);
                                    // Should add some lines that it is genuine.
                                }

                            }
                        }
                    });
                return _FindCOMs;
            }
        }
    }

}
