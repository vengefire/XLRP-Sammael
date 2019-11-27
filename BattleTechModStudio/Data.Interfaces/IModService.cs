using Data.Core.ModObjects;

namespace Data.Interfaces
{
    public interface IModService
    {
        ModCollection LoadModCollectionFromDirectory(string sourceDirectory);
    }
}