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
        public static readonly DependencyProperty TodoItemDropCommandProperty =
    DependencyProperty.Register("TodoItemDropCommand", typeof(ICommand), typeof(ListviewDraggable),
        new PropertyMetadata(null));

        public static readonly DependencyProperty TargetTodoItemProperty =
    DependencyProperty.Register("TargetTodoItem", typeof(object), typeof(ListviewDraggable),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object TargetTodoItem
        {
            get { return (object)GetValue(TargetTodoItemProperty); }
            set { SetValue(TargetTodoItemProperty, value); }
        }

        public ICommand TodoItemDropCommand
        {
            get { return (ICommand)GetValue(TodoItemDropCommandProperty); }
            set { SetValue(TodoItemDropCommandProperty, value); }
        }

        public ListviewDraggable()
        {
            InitializeComponent();
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


    }

    class vmListviewDraggable : ObservableObject
    {
        public ObservableCollection<vmSlotCamera> ListCamera { get; set; }

        private vmSlotCamera _TargetCamera;
        public vmSlotCamera TargetCamera

        {
            get => _TargetCamera;
            set
            {
                _TargetCamera = value;
                OnPropertyChanged("TargetCamera");
            }
        }


        public vmListviewDraggable(ObservableCollection<vmSlotCamera> _ListCamera)
        {
            ListCamera = _ListCamera;


        }


    }
}
