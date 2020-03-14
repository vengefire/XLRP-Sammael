using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Data.Core.Annotations;

namespace Data.Core.ModObjects
{
    public class ModCollection : INotifyPropertyChanged
    {
        public List<Mod> Mods { get; } = new List<Mod>();

        public IEnumerable<Mod> EnabledMods => Mods.Where(mod => mod.Enabled);
        public IEnumerable<Mod> DisabledMods => Mods.Where(mod => !mod.Enabled);
        public IEnumerable<Mod> ValidMods => EnabledMods.Where(mod => mod.IsValid);
        public IEnumerable<Mod> InvalidMods => Mods.Where(mod => !mod.IsValid);

        public List<Mod> ValidModsLoadOrder => ValidMods.OrderBy(mod => mod.LoadOrder).ToList();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddMod(Mod mod)
        {
            Mods.Add(mod);
        }

        public void ProcessModDependencies()
        {
            // Phase 1 - Set object inter-dependencies...
            Mods
                .ToList().ForEach(
                    mod =>
                    {
                        mod.ConflictsWithMods = EnabledMods.Where(mod1 => mod.ConflictsWithModNames.Contains(mod1.Name))
                            .ToList();
                        var conflictingMods = mod.ConflictsWithMods.Select(mod1 => mod1.Name);
                        conflictingMods.ToList()
                            .ForEach(s => mod.InvalidReasonList.Add($"Conflicting dependency [{s}]"));

                        mod.OptionallyDependsOnMods = EnabledMods
                            .Where(mod1 => mod.OptionallyDependsOnModNames.Contains(mod1.Name)).ToList();

                        mod.DependsOnMods = EnabledMods.Where(mod1 => mod.DependsOnModNames.Contains(mod1.Name))
                            .ToList();
                        var missingDependencies =
                            mod.DependsOnModNames.Except(mod.DependsOnMods.Select(mod1 => mod1.Name));
                        missingDependencies.ToList()
                            .ForEach(s => mod.InvalidReasonList.Add($"Missing dependency [{s}]"));

                        mod.IsValid = !mod.InvalidReasonList.Any();
                    });

            // Phase 2 - Check each mods dependency validity. We can only do this after setting Mod inter-dependency in phase 1
            Mods
                .Where(mod => mod.DependsOnMods.Any())
                .ToList().ForEach(
                    mod =>
                    {
                        var dependencyTree = GetModDependenciesWithLevel(mod, 1, null);
                        dependencyTree.Sort((o, o1) => o1.level.CompareTo(o.level));

                        var invalidDependencies = dependencyTree.Where(o => !o.mod.IsValid).Select(o => o.mod.Name);
                        invalidDependencies.ToList()
                            .ForEach(s => mod.InvalidReasonList.Add($"Dependency [{s}] is invalid."));

                        var disabledDependencies = dependencyTree.Where(o => !o.mod.Enabled).Select(o => o.mod.Name);
                        disabledDependencies.ToList()
                            .ForEach(s => mod.InvalidReasonList.Add($"Dependency [{s}] is disabled."));

                        mod.IsValid = !mod.InvalidReasonList.Any();
                    });
        }

        private List<dynamic> GetModDependenciesWithLevel(Mod mod, int level, List<dynamic> dependencies)
        {
            dependencies = dependencies ?? new List<dynamic>();
            mod.DependsOnMods.ForEach(mod1 =>
            {
                dependencies.Add(new {mod = mod1, level});
                GetModDependenciesWithLevel(mod1, level + 1, dependencies);
            });
            return dependencies;
        }

        public void ProcessModLoadOrder()
        {
            var remainingModsToLoad = EnabledMods.Where(mod => mod.IsValid).ToList();
            var modsLoaded = new List<Mod>(remainingModsToLoad.Count());
            var loadOrder = 0;
            var loadCycle = 0;
            while (remainingModsToLoad.Any())
            {
                loadCycle += 1;
                var modsToLoad = remainingModsToLoad
                    .Where(mod => mod.DependsOnMods.All(mod1 => modsLoaded.Contains(mod1)) && mod
                        .OptionallyDependsOnMods.Where(mod2 => mod2.IsValid)
                        .All(mod2 => modsLoaded.Contains(mod2)))
                    .OrderBy(mod => mod.Name).ToList();
                modsToLoad.ForEach(mod =>
                {
                    mod.LoadOrder = ++loadOrder;
                    mod.LoadCycle = loadCycle;
                    modsLoaded.Add(mod);
                    remainingModsToLoad.Remove(mod);
                });
            }
        }

        public void ExpandManifestGroups()
        {
            ValidMods.ToList().ForEach(mod => mod.ExpandManifestEntries());
        }
    }
}