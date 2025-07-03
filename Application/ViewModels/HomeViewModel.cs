using QRToolbox.Application.Models;
using QRToolbox.Application.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;

namespace QRToolbox.Application.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        public List<ButtonModeModel> ButtonModels { get; private set; }


        public HomeViewModel()
        {
            ButtonModels = new List<ButtonModeModel>()
            {
                new ButtonModeModel("Создать QR-код", new RelayCommand(NavigateToCreateQRCode), null),
                new ButtonModeModel("Загрузить QR-код", new RelayCommand(NavigateToLoadQRCode), "simply-scanner.png")
            };
        }

        #region Действия кнопок

        // Метод для навгиации на страницу создания QR-кода. ОБЯЗАТЕЛЬНО ЗАМЕНИТЕ ПАРАМЕТР В ЭТОЙ СТРОЧКЕ 'page.NavigationService.Navigate(new TestPage())'
        private void NavigateToCreateQRCode(object obj)
        {
                if (obj is Page page)
                    page.NavigationService.Navigate(new GenerateCodePage());
        }

        // Метод для навгиации на страницу загрузки QR-кода. ОБЯЗАТЕЛЬНО ЗАМЕНИТЕ ПАРАМЕТР В ЭТОЙ СТРОЧКЕ 'page.NavigationService.Navigate(new TestPage())'
        private void NavigateToLoadQRCode(object obj)
        {
            if (obj is Page page)
                page.NavigationService.Navigate(new LoadCodePage());
        }
        #endregion

    }
}
