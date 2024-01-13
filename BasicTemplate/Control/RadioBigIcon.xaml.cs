using BasicTemplate.Base;
using System;
using System.Collections.Generic;
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
    /// RadioBigIcon.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RadioBigIcon : UserControl
    {
        public RadioBigIcon()
        {
            InitializeComponent();
        }
    }

    class vmRadioBigIcon : ObservableObject
    {
        private bool _bIsChekced;
        public bool bIsChekced

        {
            get => _bIsChekced;
            set
            {
                _bIsChekced = value;
                OnPropertyChanged("bIsChekced");
            }
        }

        public vmRadioBigIcon(bool _bIsChecked = false)
            => bIsChekced = _bIsChecked;
    }
}
