using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QRToolbox.Application.Models
{
    public class ButtonModeModel 
    {
        public string Header { get; private set; }
        public string ImageFileName { get; private set; }
        public ICommand NavigateCommand { get; private set; }

        /// <summary>
        /// Конструктор класса ButtonModeModel
        /// </summary>
        /// <param name="header">Заголовок</param>
        /// <param name="navPage">Страница, на которую будет происходить навигация</param>
        /// <param name="imagePath">Название изображения из каталога ресурсов!!! По умолчанию 'union-qr.png'</param>
        public ButtonModeModel(string header, ICommand command, string imagePath = null)
        {
            Header = header;
            NavigateCommand = command;

            // Если путь не задан, то устанавливается картинка по умолчанию
            if (imagePath != null)
                ImageFileName = Path.Combine("/QRToolbox;component/Resources/", imagePath);
            else
                ImageFileName = Path.Combine("/QRToolbox;component/Resources/", "union-qr.png");
        }
    }
}
