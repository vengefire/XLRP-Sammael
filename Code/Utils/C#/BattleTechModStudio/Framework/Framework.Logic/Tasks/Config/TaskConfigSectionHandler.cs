using System.Configuration;
using Framework.Logic.Tasks.Config.Background.Task;
using Framework.Logic.Tasks.Config.Background.Trigger;
using Framework.Logic.Tasks.Config.Scheduled;

namespace Framework.Logic.Tasks.Config
{
    public class TaskConfigSectionHandler : ConfigurationSection
    {
        public static bool ConfigurationPresent => ConfigurationManager.GetSection("tasks") != null;

        [ConfigurationProperty("scheduledTasks", IsRequired = false, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ScheduledTaskCollection))]
        public ScheduledTaskCollection ScheduledTasks => (ScheduledTaskCollection) this["scheduledTasks"];

        [ConfigurationProperty("triggers", IsRequired = false, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(TaskTriggerCollection))]
        public TaskTriggerCollection TaskTriggers => (TaskTriggerCollection) this["triggers"];

        [ConfigurationProperty("backgroundTasks", IsRequired = false, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(BackgroundTaskCollection))]
        public BackgroundTaskCollection BackgroundTasks => (BackgroundTaskCollection) this["backgroundTasks"];
    }
}