using Autodesk.Revit.DB;
using QRToolbox.Application.Models;
using QRToolbox.Application.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;
using QRToolbox.Revit;
using System.Windows.Input;
namespace QRToolbox.Application.ViewModels {
    internal class LoadCodeViewModel : INotifyPropertyChanged {

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public List<ButtonModeModel> ButtonModelsColumn1 { get; private set; }
        public List<ButtonModeModel> ButtonModelsColumn2 { get; private set; }

        public ICommand CopyInBufCommand { get; private set; }

        private BitmapImage _qrCodeImage;
        public BitmapImage QrCodeImage
        {
            get => _qrCodeImage;
            set
            {
                _qrCodeImage = value;
                OnPropertyChanged(nameof(QrCodeImage));
            }
        }
        private string _qrScan;
        public string QrScan
        {

            get => _qrScan;
            set
            {
                _qrScan = value;
                OnPropertyChanged(nameof(QrScan));
            }
        }

        public LoadCodeViewModel()
        {
            ButtonModelsColumn1 = new List<ButtonModeModel>()
            {
                new ButtonModeModel("Загрузка QR-кода", new RelayCommand(LoadQRCode), null),
                new ButtonModeModel("Загрузка из буффера", new RelayCommand(LoadFromBuf), null)
            };
            ButtonModelsColumn2 = new List<ButtonModeModel>()
            {
                new ButtonModeModel("Импортировать в Revit", new RelayCommand(PutImageInRevit), null),
                new ButtonModeModel("Сканировать", new RelayCommand(ScanQR_Click), null)
            };
            CopyInBufCommand = new RelayCommand(CopyInBuf);

        }
        #region Действия кнопок
        private void LoadQRCode(object obj) {
            try
            {
                QrCodeImage = QRWithLogoGenerate.BitmapToBitmapImage(QRWithLogoGenerate.OpenImage());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadFromBuf(object obj)
        {
            if (Clipboard.ContainsImage())
            {
                var image = Clipboard.GetImage();
                using (var stream = new System.IO.MemoryStream())
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    QrCodeImage = bitmap;
                }
            }
            else
            {
                MessageBox.Show("No image found in clipboard.");
            }
        }
        private void ScanQR_Click(object obj)
        {
            try { 
            if (QrCodeImage != null)
            {
                    QrScan = QRWithLogoGenerate.ScanQrCode(QrCodeImage);
            }
            else
            {
                MessageBox.Show("Загрузите QR-код.");
            }
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }

        }
        private void PutImageInRevit(object obj) {
            //try {
            //    if (QrCodeImage != null) {
            //        DataBank.ImagePath = QRWithLogoGenerate.SaveBitmapImageToTempFile(QrCodeImage);
            //        DataBank.Placement = BoxPlacement.Center;
            //        DataBank.ImportImageEvent.Raise();     
            //    } 
            //    else 
            //    {
            //        MessageBox.Show("Загрузите QR-код.");
            //    }
            //}
            //catch (Exception ex) 
            //    { MessageBox.Show(ex.Message); }
            if (obj is Page page)
                page.NavigationService.Navigate(new SelectionSheetsPage());
        }


        private void CopyInBuf(object obj) 
        {
            if (QrScan != null) {
                Clipboard.SetText(QrScan);
            } else {
                MessageBox.Show("Отсканируйте QR-код.");
            }
        }
        #endregion
    }
}
