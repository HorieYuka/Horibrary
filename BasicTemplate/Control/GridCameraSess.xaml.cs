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
    /// GridCameraSess.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GridCameraSess : UserControl
    {


        public GridCameraSess()
        {
            InitializeComponent();
        }


        private void Grid_Drop(object sender, DragEventArgs e)
        {
            HitTestResult result = VisualTreeHelper.HitTest(DisplayGrid, e.GetPosition(DisplayGrid));

            object data = e.Data.GetData(DataFormats.Serializable);

            int a = 1;
        }


    }

    class vmGridCameraSess : ObservableObject
    {
        public static readonly DependencyProperty AddItemProperty =
                DependencyProperty.Register("AddItem", typeof(object), typeof(ListviewDraggable),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ICommand AddCameraSessCmd { get; }

        public vmGridCameraSess() 
        {

            AddCameraSessCmd = new BaseCommand(aaa);
        }



        public void aaa(object sender)
        {
            var TargetCameraSess = (vmSlotCamera)sender;

        }
    }
}
