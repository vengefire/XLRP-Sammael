using System;
using System.Collections.Generic;
using UI.Core.Interfaces.Data;

namespace HomePlugin
{
    public class HomePlugin : IPlugin
    {
        public string Description => @"The Home plugin for BattleTech - Mod Studio";
        public List<IPluginControl> Modules { get; }
        public string Name => @"BattleTech Mod Studio - Home";
        public Type PageType => typeof(HomePluginControl);
    }
}