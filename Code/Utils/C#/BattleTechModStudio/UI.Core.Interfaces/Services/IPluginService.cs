using System.Collections.Generic;
using UI.Core.Interfaces.Data;

namespace UI.Core.Interfaces.Services
{
    public interface IPluginService
    {
        List<IPlugin> GetPlugins(string path);
    }
}