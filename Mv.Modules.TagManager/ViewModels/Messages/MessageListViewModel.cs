using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Mv.Ui.Mvvm;
using Prism.Events;
using Prism.Mvvm;
using Unity;

namespace Mv.Modules.TagManager.ViewModels.Messages
{
    public class UserMessage
    {
        public string Source { get; set; }
        public int Level { get; set; }
        public string Content { get; set; }
    }

    public class UserMessageEvent:PubSubEvent<UserMessage>
    { 
    
    }

    public class MessageListViewModel:ViewModelBase
    {

        public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>() {"Welcome！" };
 

        public MessageListViewModel(IUnityContainer container) : base(container)
        {
  
            EventAggregator.GetEvent<UserMessageEvent>().Subscribe(x =>
            {

             Invoke(() =>
                {

                    var Msg = DateTime.Now.ToString() + "\t" + $"{x.Source.ToUpper().PadRight(25,' ')}{ x.Content}";
                    Messages.Add(Msg);
                    if (Messages.Count > 1000)
                    {
                        Messages.RemoveAt(0);
                    }
                });
            });
        }
    }
}
