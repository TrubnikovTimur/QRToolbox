using QRToolbox.Application.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QRToolbox.Application.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        private RelayCommand _mainWindowLoaded; // Команда, срабатывающая при событии загрузки Window
        private RelayCommand _mainFrameLoaded;  // Команда, срабатывающая при событии загрузки Frame
        private RelayCommand _dragWindow;       // Команда перетаскивания окна, срабатывающая при нажатии по шапке окна
        private RelayCommand _minimizeWindow;   // Команда для сворачивания окна
        private RelayCommand _maximizeWindow;   // Команда для перехода между полноэкранным и нормальным режим окна
        private RelayCommand _closeWindow;      // Команда для закрытия окна
        private RelayCommand _backAction;       // Команда вернуться назад
        private MainWindow _window { get; set; }


        public RelayCommand MainWindowLoaded
        {
            get
            {
                return _mainWindowLoaded ??
                    new RelayCommand(obj =>
                    {
                        // Действие, которое будет выполнять команда
                        if (obj is MainWindow window)
                            _window = window;
                    },
                    obj =>
                    {
                        // Проверка, которая будет выполнена перед выполнением действия выше
                        return obj != null;
                    });
            }
        }

        public RelayCommand MainFrameLoaded
        {
            get
            {
                return _mainFrameLoaded ??
                    new RelayCommand(obj =>
                    {
                        if (obj is Frame frame)
                            frame.Navigate(new HomePage());
                    }, 
                    obj => obj != null);
            }
        }

        public RelayCommand DragAction
        {
            get
            {
                return _dragWindow ??
                    new RelayCommand(obj =>
                    {
                        _window.DragMove();
                    }, 
                    obj => Mouse.LeftButton == MouseButtonState.Pressed);
            }
        }

        public RelayCommand MinimizeAction
        {
            get
            {
                return _minimizeWindow ??
                    new RelayCommand(obj =>
                    {
                        _window.WindowState = WindowState.Minimized;
                    },
                    obj => _window != null);
            }
        }

        public RelayCommand MaximizeAction
        {
            get
            {
                return _maximizeWindow ??
                    new RelayCommand(obj =>
                    {
                        if (_window.WindowState == WindowState.Maximized)
                        {
                            _window.WindowState = WindowState.Normal;
                        }
                        else
                        {
                            _window.WindowState = WindowState.Maximized;
                        }
                    },
                    obj => _window != null);
            }
        }

        public RelayCommand CloseAction
        {
            get
            {
                return _closeWindow ??
                    new RelayCommand(obj =>
                    {
                        _window.Close();
                    },
                    obj => _window != null);
            }
        }

        public RelayCommand BackAction {
            get {
                return _backAction ??
                new RelayCommand(obj => 
                {
                    if (!(_window.MainFrame.Content is HomePage)) {
                        _window.MainFrame.Content = new HomePage();
                    }
                },
                obj => _window != null);
            }
        }
    }
}
