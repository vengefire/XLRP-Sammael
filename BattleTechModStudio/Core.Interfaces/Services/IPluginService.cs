using System.Collections.Generic;
using Core.Interfaces.Data;

namespace Core.Interfaces.Services
{
    public interface IPluginService
    {
        List<IPlugin> GetPlugins(string path);
    }
}