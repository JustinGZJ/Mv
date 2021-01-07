using System;
using System.Collections.Generic;
using System.Text;
using Prism.Events;
using Mv.Core;
using MV.Core.Events;
using Prism.Logging;

namespace Mv.Modules.Axis.Controller
{

    public class MessageWraper : IMessageWraper
    {
        private readonly IEventAggregator @event;
        UserMessageEvent userMessageEvent;
        public MessageWraper(IEventAggregator @event)
        {
            this.@event = @event;
            userMessageEvent= @event.GetEvent<UserMessageEvent>();
        }
        public void Error(string msg, string source = "")
        {
            userMessageEvent.Publish(new UserMessage { Content = msg, Level = Category.Exception, Source = source });
        }
        public void Msg(string msg, string source = "")
        {
            userMessageEvent.Publish(new UserMessage { Content = msg, Level = Category.Info, Source = source });
        }

        public void Warn(string msg, string source = "")
        {
            userMessageEvent.Publish(new UserMessage { Content = msg, Level = Category.Warn, Source = source });
        }
    }
}
