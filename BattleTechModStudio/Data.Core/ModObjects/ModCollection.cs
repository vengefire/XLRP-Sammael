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

        public IEnumerable<Mod> ValidMods => Mods.Where(mod => mod.IsValid);
        public IEnumerable<Mod> InvalidMods => Mods.Where(mod => !mod.IsValid);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddMod(ModBase modBase)
        {
            Mods.Add(new Mod(modBase));
        }

        public void ProcessModDependencies()
        {
            // Phase 1 - Set object inter-dependencies...
            Mods
                .ToList().ForEach(
                    mod =>
                    {
                        if (mod.ConflictsWithModNames != null && mod.ConflictsWithModNames.Any())
                        {
                            mod.ConflictsWith = Mods.Where(mod1 => mod.ConflictsWithModNames.Contains(mod1.Name)).ToList();
                            var conflictingMods = mod.ConflictsWith.Select(mod1 => mod1.Name);
                            conflictingMods.ToList().ForEach(s => mod.InvalidReasonList.Add($"Conflicting dependency [{s}]"));
                        }

                        if (mod.OptionallyDependsOnModNames != null && mod.OptionallyDependsOnModNames.Any())
                        {
                            mod.OptionallyDependsOn = Mods.Where(mod1 => mod.OptionallyDependsOnModNames.Contains(mod1.Name)).ToList();
                        }

                        if (mod.DependsOnModNames != null && mod.DependsOnModNames.Any())
                        {
                            mod.DependsOn = Mods.Where(mod1 => mod.DependsOnModNames.Contains(mod1.Name)).ToList();
                            var missingDependencies = mod.DependsOnModNames.Except(mod.DependsOn.Select(mod1 => mod1.Name));
                            missingDependencies.ToList().ForEach(s => mod.InvalidReasonList.Add($"Missing dependency [{s}]"));
                        }

                        mod.IsValid = !mod.InvalidReasonList.Any();
                    });

            // Phase 2 - Check each mods dependency validity. We can only do this after setting mod inter-dependency in phase 1
            Mods
                .Where(mod => mod.DependsOn.Any())
                .ToList().ForEach(
                    mod =>
                    {
                        var dependencyTree = GetModDependenciesWithLevel(mod, 1, null);
                        dependencyTree.Sort((o, o1) => o1.level.CompareTo(o.level));

                        var invalidDependencies = dependencyTree.Where(o => !o.mod.IsValid).Select(o => o.mod.Name);
                        invalidDependencies.ToList().ForEach(s => mod.InvalidReasonList.Add($"Dependency [{s}] is invalid."));
                        mod.IsValid = !mod.InvalidReasonList.Any();
                    });
        }

        private List<dynamic> GetModDependenciesWithLevel(Mod mod, int level, List<dynamic> dependencies)
        {
            dependencies = dependencies ?? new List<dynamic>();
            mod.DependsOn.ForEach(mod1 =>
            {
                dependencies.Add(new {mod = mod1, level});
                GetModDependenciesWithLevel(mod1, level + 1, dependencies);
            });
            return dependencies;
        }

        public void ProcessModLoadOrder()
        {
            var remainingModsToLoad = Mods.Where(mod => mod.IsValid).ToList();
            var modsLoaded = new List<Mod>(remainingModsToLoad.Count());
            var loadOrder = 0;
            var loadCycle = 0;
            while (remainingModsToLoad.Any())
            {
                loadCycle += 1;
                var modsToLoad = remainingModsToLoad
                    .Where(mod => mod.DependsOn.All(mod1 => modsLoaded.Contains(mod1) && mod.OptionallyDependsOn.All(mod2 => modsLoaded.Contains(mod2))))
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
    }
}