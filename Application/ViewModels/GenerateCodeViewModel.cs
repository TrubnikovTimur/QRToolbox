using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QRToolbox.Application.Models;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Data;
using System.Drawing;
using QRToolbox.Application.Views;
using System.Windows.Controls;

namespace QRToolbox.Application.ViewModels {
    internal class GenerateCodeViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        private BitmapImage _qrCodeImage;
        private string _txtQrCode;
        public string txtQrCode
        {
            get => _txtQrCode;
            set
            {
                _txtQrCode = value;
                OnPropertyChanged(nameof(QrCodeImage));
            }
        }
        public BitmapImage QrCodeImage
        {
            get => _qrCodeImage;
            set
            {
                _qrCodeImage = value;
                OnPropertyChanged(nameof(QrCodeImage));
            }
        }

        private System.Windows.Media.Color _selectedColorFromPicker;
        public System.Windows.Media.Color SelectedColorFromPicker {
            get => _selectedColorFromPicker;
            set {
                _selectedColorFromPicker = value;
                OnPropertyChanged(nameof(SelectedColorFromPicker));
            }
        }


        public List<ButtonModeModel> ButtonModels { get; private set; }

        public GenerateCodeViewModel()
        {
            txtQrCode = "Input text here...";

            ButtonModels = new List<ButtonModeModel>()
            {
                new ButtonModeModel("Создание QR-кода", new RelayCommand(CreateQRCode), null),
                new ButtonModeModel("Cохранение QR-кода", new RelayCommand(SaveQRCode), null),
                new ButtonModeModel("Добавление логотипа", new RelayCommand(LogoQRCode), null),
                new ButtonModeModel("Импортировать в Revit", new RelayCommand(PutImageInRevit), null)
            };

        }
        public class PlaceholderTextConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return "Input text here...";
                }
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
        #region Действия кнопок
        private void ClearText(object obj)
        {
            txtQrCode = string.Empty;
        }
        private void CreateQRCode(object obj)
        {
            try
            {
                System.Drawing.Color drawingColor = System.Drawing.Color.FromArgb(SelectedColorFromPicker.A, SelectedColorFromPicker.R, SelectedColorFromPicker.G, SelectedColorFromPicker.B);
                QrCodeImage = QRWithLogoGenerate.BitmapToBitmapImage(QRWithLogoGenerate.ChangeColor(QRWithLogoGenerate.CreateQRCode(_txtQrCode), drawingColor));
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private void SaveQRCode(object obj)
        {
            BitmapSource bitmapSource = _qrCodeImage;
            QRWithLogoGenerate.SaveImage(bitmapSource);
        }
        private void LogoQRCode(object obj)
        {
            Bitmap Logotip = QRWithLogoGenerate.OpenImage();
            CreateQRCode(_txtQrCode);
            Bitmap QrCodeImageB = QRWithLogoGenerate.BitmapImageToBitmap(QrCodeImage);
            Bitmap ResizeLogotip = QRWithLogoGenerate.ResizeLogoForQr(QrCodeImageB, Logotip);
            System.Drawing.Image RLW = QRWithLogoGenerate.AddWhiteBackground(ResizeLogotip);
            System.Drawing.Image QrWLogo = QRWithLogoGenerate.ConnectQrWithLogo(QrCodeImageB, RLW);
            QrCodeImage = QRWithLogoGenerate.ImageToBitmapImage(QrWLogo);
        }
        private void PutImageInRevit(object obj) {
            if (obj is Page page)
                page.NavigationService.Navigate(new SelectionSheetsPage());
        }

        #endregion
    }
}