using System.Collections.Generic;
using Data.Core.Enums;
using Data.Core.GameObjects;

namespace Data.Console.Utils
{
    internal static class ContentExtractors
    {
        internal static void GenerateStoreContentList(List<ManifestEntry> manifestEntries, string outputDirectory, string filename)
        {
            var typesQualifyingForStores = new List<GameObjectTypeEnum>()
            {
                GameObjectTypeEnum.UpgradeDef,
                GameObjectTypeEnum.HeatSinkDef,
                GameObjectTypeEnum.WeaponDef,
                GameObjectTypeEnum.JumpJetDef,
                GameObjectTypeEnum.AmmunitionBoxDef,
                GameObjectTypeEnum.MechDef
            };

            ExtractionUtils.ExtractItemsByTypeToExcel(manifestEntries, outputDirectory, filename, typesQualifyingForStores);
        }

        internal static void GenerateTacticalContentList(List<ManifestEntry> manifestEntries, string outputDirectory, string filename)
        {
            var typesQualifyingForStores = new List<GameObjectTypeEnum>()
            {
                GameObjectTypeEnum.UpgradeDef,
                GameObjectTypeEnum.HeatSinkDef,
                GameObjectTypeEnum.WeaponDef,
                GameObjectTypeEnum.JumpJetDef,
                GameObjectTypeEnum.AmmunitionBoxDef,
                GameObjectTypeEnum.MechDef,
                GameObjectTypeEnum.VehicleDef
            };

            ExtractionUtils.ExtractItemsByTypeToExcel(manifestEntries, outputDirectory, filename, typesQualifyingForStores);
        }
    }
}