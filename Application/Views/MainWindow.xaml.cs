using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Input;
using ZXing.QrCode;
using Autodesk.Revit.UI;
using Xceed.Wpf.Toolkit;

namespace QRToolbox.Application.Views
{

    public partial class MainWindow : Window
    {
  
        public MainWindow()
        {
            QRCodeReader reader = new QRCodeReader();
            CheckComboBox checkComboBox = new CheckComboBox();
            InitializeComponent();
        }
    }
}
