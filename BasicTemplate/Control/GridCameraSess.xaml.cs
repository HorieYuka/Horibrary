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
        private void ItemMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && sender is FrameworkElement frameworkElement)
            {
                object todoItem = frameworkElement.DataContext;

            }
        }


        public GridCameraSess()
        {
            InitializeComponent();
        }

    }

    class vmGridCameraSess : ObservableObject
    {
        public static readonly DependencyProperty AddItemProperty =
                DependencyProperty.Register("AddItem", typeof(object), typeof(ListviewDraggable),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public vmGridCameraSess() 
        {


        }
    }
}
