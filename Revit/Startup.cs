using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using QRToolbox.Application.Views;
using System;

namespace QRToolbox.Revit
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class Startup : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Регестрируем внешнее событие в Revit
                IExternalEventHandler externalEvent = new ImportImageEvnt();
                DataBank.ImportImageEvent = ExternalEvent.Create(externalEvent);

                // Создаём экземпляр окна главного приложения и отображаем его
                MainWindow window = new MainWindow();
                window.Show();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("QRToolbox: Startup", ex.Message);
                return Result.Failed;
            }

        }
    }
}
