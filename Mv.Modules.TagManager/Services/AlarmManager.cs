using DataService;
using Mv.Modules.TagManager.ViewModels.Messages;
using Prism.Events;
using Prism.Logging;
using PropertyTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Reactive.Linq;
using MV.Core.Events;

namespace Mv.Modules.TagManager.Services
{
    public interface IAlarmManager
    { }
    public class AlarmManager : IAlarmManager, IDisposable
    {
        private readonly IDataServer server;
        private readonly IEventAggregator @event;
        private readonly ILoggerFacade logger;


        public AlarmManager(IDataServer server, IEventAggregator @event, ILoggerFacade logger)
        {
            var alarmGroup = server.GetGroupByName("alarms");
            if (alarmGroup != null)
            {
                foreach (var item in alarmGroup.Items)
                {
                    item.ValueChanged += Item_ValueChanged;
                }
            }

            var PARASGroup = server.GetGroupByName("PARAS");
            if (PARASGroup != null)
            {
                foreach (var item in PARASGroup.Items)
                {
                    item.ValueChanged += PARA_ValueChanged;
                }
            }
            System.Reactive.Linq.Observable.Interval(TimeSpan.FromMilliseconds(1000)).Subscribe(x =>
            {
                var TAG = server["HANDSHAKE"];
                TAG?.Write((short)x % 10);
            });
            this.server = server;
            this.@event = @event;
            this.logger = logger;

        }
        private void PARA_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (sender is ITag tag)
            {
                var value = tag.GetValue(e.Value).ToString();
                var meta = tag.GetMetaData();
                var msg = $"{meta.Name}\t{meta.Address}\t{meta.Description}\t{value}";
                this.@event.GetEvent<UserMessageEvent>().Publish(new UserMessage { Content = msg, Level = 0, Source = "PARAS" });
                logger.Log(msg, Category.Info, Priority.None);
            }
        }


        private void Item_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (sender is BoolTag tag)
            {
                var value = (bool)tag.GetValue(e.Value) ? "ON" : "OFF";
                var meta = tag.GetMetaData();
                var msg = $"{meta.Name}\t{meta.Address}\t{meta.Description}\t{value}";
                this.@event.GetEvent<UserMessageEvent>().Publish(new UserMessage { Content = msg, Level = 0, Source = "alarms" });
                logger.Log(msg, Category.Info, Priority.None);
            }

        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    var alarmGroup = server?.GetGroupByName("alarms");
                    if (alarmGroup != null)
                    {
                        foreach (var item in alarmGroup.Items)
                        {
                            item.ValueChanged -= Item_ValueChanged;
                        }
                    }
                    var PARASGroup = server.GetGroupByName("PARAS");
                    if (PARASGroup != null)
                    {
                        foreach (var item in PARASGroup.Items)
                        {
                            item.ValueChanged -= PARA_ValueChanged;
                        }
                    }
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~AlarmManager()
        // {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
