﻿using System;
using System.Windows;
using System.Windows.Input;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;
namespace QRToolbox.Application.ViewModels
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            try { 
            execute(parameter);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
