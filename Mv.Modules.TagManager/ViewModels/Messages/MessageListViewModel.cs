using Mv.Ui.Mvvm;
using MV.Core.Events;
using Prism.Logging;
using System.Collections.ObjectModel;
using Unity;

namespace Mv.Modules.TagManager.ViewModels.Messages
{


    public class MessageListViewModel : ViewModelBase
    {

        public ObservableCollection<UserMessage> Messages { get; set; } = new ObservableCollection<UserMessage>() { new UserMessage { Source = "System", Level = Category.Info, Content = "Welcome!" } };



        public MessageListViewModel(IUnityContainer container) : base(container)
        {
            EventAggregator.GetEvent<UserMessageEvent>().Subscribe(x =>
          {

              Invoke(() =>
                   {
                       Logger.Log(x.Content, x.Level, Priority.None);
                       Messages.Add(x);
                       if (Messages.Count > 1000)
                       {
                           Messages.RemoveAt(0);
                       }
                   });
          });
        }
    }
}
