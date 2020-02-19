using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Core.Enums;
using Data.Core.GameObjects;
using OfficeOpenXml;

namespace Data.Console.Utils
{
    internal static class StoreContentGenerator
    {
        internal static void GenerateStoreContentList(List<ManifestEntry> manifestEntries, string outputDirectory)
        {
            var typesQualifyingForStores = new List<GameObjectTypeEnum>()
            {
                GameObjectTypeEnum.UpgradeDef,
                GameObjectTypeEnum.HeatSinkDef,
                GameObjectTypeEnum.WeaponDef,
                GameObjectTypeEnum.JumpJetDef,
                GameObjectTypeEnum.AmmunitionBoxDef
            };

            var shopItemsByCategory = manifestEntries.Where(entry => typesQualifyingForStores.Contains(entry.GameObjectType))
                .GroupBy(entry => entry.GameObjectType, (key, entries) => new { ItemType = key, items = entries.Select(entry => entry) })
                .ToList();

            var outputFileName = Path.Combine(outputDirectory, "xlrp-store-content.xlsx");
            if (File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }

            var excelPackage = new ExcelPackage(new FileInfo(outputFileName));
            var workBook = excelPackage.Workbook;
            var index = 0;
            ExcelWorksheet currentSheet = null;

            foreach (var shopItemCategory in shopItemsByCategory)
            {
                System.Console.WriteLine($"{shopItemCategory.ItemType}");
                System.Console.WriteLine($"{string.Join("\r\n", shopItemCategory.items.Select(entry => entry.Id))}");

                currentSheet = workBook.Worksheets.Add(shopItemCategory.ItemType.ToString());
                int rowIndex = 1;
                int colindex = 1;

                shopItemCategory.items.ToList().ForEach(entry => currentSheet.Cells[rowIndex++, colindex].Value = entry.Id);
            }

            excelPackage.Save();
        }
    }
}