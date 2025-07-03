using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using QRToolbox.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;

namespace QRToolbox.Revit
{
    internal class ImportImageEvnt : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            try
            {
                // Получаем документ Revit
                UIDocument uidoc = app.ActiveUIDocument;
                Autodesk.Revit.DB.Document doc = app.ActiveUIDocument.Document;

                // Configure image type options
                ImageTypeOptions options = new ImageTypeOptions(DataBank.ImagePath, false, ImageTypeSource.Import);

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> allSheets = collector.OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType().ToElements();
                List<Autodesk.Revit.DB.ViewSheet> allSSheets = new List<Autodesk.Revit.DB.ViewSheet>();
                List<string> sheetNames = new List<string>();

                foreach (Element sheetElement in allSheets)
                {
                    Autodesk.Revit.DB.ViewSheet sheet = sheetElement as Autodesk.Revit.DB.ViewSheet;
                    if (sheet != null && !sheetNames.Contains(sheet.Name))
                    {
                        sheetNames.Add(sheet.Name);
                        allSSheets.Add(sheet);
                    }
                }

                // Запускаем транзакцию для внесения изменений в документ
                using (Transaction transaction = new Transaction(doc)) {
                    transaction.Start("Импорт изображения");
                    FilteredElementCollector viewCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets);
                    int countPage = 0;
                    foreach (Element viewElement in viewCollector) {
                        View view = viewElement as View;
                        if (view != null) {
                            List<FamilyInstance> instances = new FilteredElementCollector(doc)
                                .OfClass(typeof(FamilyInstance))
                                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                                .Cast<FamilyInstance>()
                                .ToList();

                            for (int i = countPage; i < instances.Count; i++) {
                                FamilyInstance instance = instances[i];
                                XYZ point = new XYZ();
                                LocationPoint location = instance.Location as LocationPoint;
                                if (location != null) {
                                    point = location.Point;
                                    point += new XYZ(-0.715, 0.1065, 0);
                                    ImageType imageType = ImageType.Create(doc, options);
                                    ImageInstance.Create(doc, view, imageType.Id, new ImagePlacementOptions(point, DataBank.Placement));
                                }
                                countPage++;
                                break;
                            }
                        }
                    }
                    transaction.Commit();

                }

            } catch (Exception ex)
            {
                TaskDialog.Show(ex.InnerException.ToString(), ex.Message);
            }
        }

        public string GetName()
        {
            return "Import Image";
        }
    }
}
