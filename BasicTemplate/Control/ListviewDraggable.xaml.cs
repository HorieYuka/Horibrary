using BasicTemplate.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// ListviewDraggable.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ListviewDraggable : UserControl
    {

        public ListviewDraggable()
        {
            InitializeComponent();
        }


        private void ListDragOver(object sender, DragEventArgs e)
        {

        }

        private void ItemMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                     sender is FrameworkElement frameworkElement)
            {
                object todoItem = frameworkElement.DataContext;

                DragDropEffects dragDropResult = DragDrop.DoDragDrop(frameworkElement,
                    new DataObject(DataFormats.Serializable, todoItem),
                    DragDropEffects.Move);


            }
        }

        private void ItemDragOver(object sender, DragEventArgs e)
        {

        }

    }

    class vmListviewDraggable
    {
        public ObservableCollection<vmSlotCamera> ListCamera { get; set; }

        public vmListviewDraggable(ObservableCollection<vmSlotCamera> _ListCamera)
        {
            ListCamera = _ListCamera;
        }
    }
}
