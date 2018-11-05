using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Interactivity;

namespace DragDropMVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xamlw
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
    class MainViewModel
    {
        public MainViewModel()
        {
            SomeData = new ObservableCollection<Data>();

            SomeData.Add(new Data()
            {
                Close = 1,
                Label = "Item 0",
                Open = 2,
                Value = 34,
            });

            SomeData.Add(new Data()
            {
                Close = 2,
                Label = "Item 1",
                Open = 5,
                Value = 66,
            });

            SomeData.Add(new Data()
            {
                Close = 3,
                Label = "Item 2",
                Open = 514,
                Value = 11,
            });

            SomeData.Add(new Data()
            {
                Close = 4,
                Label = "Item 3",
                Open = 54,
                Value = 11,
            });

            SomeData.Add(new Data()
            {
                Close = 5,
                Label = "Item 4",
                Open = 72,
                Value = 394,
            });

        }

        public ICommand MessageCommandPreviewMouseDown
        {
            get
            {
                return new RelayCommand<object>(
                    new Action<object>(
                        (obj) =>
                        {
                            MessageBox.Show("Cool Behavior!");
                        }));
            }
        }

        public ObservableCollection<Data> SomeData
        {
            get;
            set;
        }

        

    }

    public class Data : INotifyPropertyChanged
    {
        private int open;
        private int close;
        private int value;
        private string label;

        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                PropChanged("Label");
            }
        }

        public int Open
        {
            get { return open; }
            set
            {
                open = value;
                PropChanged("Open");
            }
        }

        public int Close
        {
            get { return close; }
            set
            {
                close = value;
                PropChanged("Close");
            }
        }


        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                PropChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void PropChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }


    public class XamDataGridBehaviorEx : Behavior<XamDataGrid>
      {

        #region Overrides

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
        }
        

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PreviewMouseDown -= AssociatedObject_PreviewMouseDown;
        }

        #endregion

        #region Events

        void AssociatedObject_PreviewMouseDown(object sender, EventArgs e)
        {
            if (Command != null)
                Command.Execute(e);

        }

        #endregion

        #region Props

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables 
          //animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand),
            typeof(XamDataGridBehaviorEx), new PropertyMetadata(null));



        #endregion

    }

    public class RelayCommand<T> : ICommand
    {

        #region Declarations

        readonly Predicate<T> _canExecute;
        readonly Action<T> _execute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class and 
        /// the command can always be executed.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {

            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public virtual Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public virtual void Execute(Object parameter)
        {
            _execute((T)parameter);
        }

        #endregion
    }

  

 
}
