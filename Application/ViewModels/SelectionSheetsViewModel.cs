using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QRToolbox.Application.Models;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB;
using System.Linq;

namespace QRToolbox.Application.ViewModels
{
    public class SelectionSheetsViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Autodesk.Revit.DB.ViewSheet ViewSheet { get; set; }
        public List<ButtonModeModel> ButtonModels { get; private set; }

        #endregion

        private BitmapImage _qrCodeImage;
        private bool _isSelected;
        private List<Autodesk.Revit.DB.ViewSheet> _allSSheets;
        public interface ISelectionSheetsViewModel
        {
            void Initialize(List<ViewSheet> allSSheets);
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

        public List<ButtonModeModel> ButtonModels1 { get; private set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public SelectionSheetsViewModel()
        {

            ButtonModels1 = new List<ButtonModeModel>()
            {
                new ButtonModeModel("Создать QR-код", new RelayCommand(PInR), null)
            };
            AllSheets = new ObservableCollection<Sheet>();
            SelectedSheet = AllSheets.FirstOrDefault();
            ViewModel();
        }
        public void Initialize(List<ViewSheet> allSSheets)
        {
            AllSheets = new ObservableCollection<Sheet>();

            foreach (var sheet in allSSheets)
            {
                AllSheets.Add(new Sheet { Name = sheet.Name });
            }

            SelectedSheet = AllSheets.FirstOrDefault();
        }
        public class Sheet
        {
            public string Name { get; set; }
        }

        private ObservableCollection<Sheet> _allSheets;
        public ObservableCollection<Sheet> AllSheets
        {
            get { return _allSheets; }
            set
            {
                _allSheets = value;
                OnPropertyChanged(nameof(AllSheets));
            }
        }

        private Sheet _selectedSheet;
        public Sheet SelectedSheet
        {
            get { return _selectedSheet; }
            set
            {
                _selectedSheet = value;
                OnPropertyChanged(nameof(SelectedSheet));
            }
        }

        public void ViewModel()
        {
            AllSheets = new ObservableCollection<Sheet>();

            foreach (var sheet in _allSSheets)
            {
                AllSheets.Add(new Sheet { Name = sheet.Name });
            }

            SelectedSheet = AllSheets.FirstOrDefault();
        }


        #region Button Actions

        private void PInR(object obj)
        {
            try
            {
                if (QrCodeImage != null)
                {
                    DataBank.ImagePath = QRWithLogoGenerate.SaveBitmapImageToTempFile(QrCodeImage);
                    DataBank.Placement = BoxPlacement.BottomLeft;
                    DataBank.ImportImageEvent.Raise();
                }
                else
                {
                    MessageBox.Show("Загрузите QR-код.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}