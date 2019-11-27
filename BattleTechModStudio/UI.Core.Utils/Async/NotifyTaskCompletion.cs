using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace UI.Core.Utils.Async
{
    public sealed class NotifyTaskCompletion : INotifyPropertyChanged
    {
        public NotifyTaskCompletion(Task task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                TaskCompletion = WatchTaskAsync(task);
            }
        }

        public string ErrorMessage => InnerException == null ? null : InnerException.Message;

        public AggregateException Exception => Task.Exception;

        public Exception InnerException => Exception == null ? null : Exception.InnerException;

        public bool IsCanceled => Task.IsCanceled;

        public bool IsCompleted => Task.IsCompleted;

        public bool IsFaulted => Task.IsFaulted;

        public bool IsNotCompleted => !Task.IsCompleted;

        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;

        // public TResult Result => this.Task.Status == TaskStatus.RanToCompletion ? this.Task.Result : default(TResult);
        public TaskStatus Status => Task.Status;

        public Task Task { get; }

        public Task TaskCompletion { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }

            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
            {
                return;
            }

            propertyChanged(this, new PropertyChangedEventArgs("Status"));
            propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));

            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }
    }
}