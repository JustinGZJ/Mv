﻿using HslCommunication.ModBus;
using System;
using System.Reactive.Linq;

using System.Reactive.Concurrency;
namespace Mv.Modules.Schneider.Service
{
    public class TesionMonitor
    {
        ModbusTcpNet modbus;
        public event Action<ushort> OnRecieve;
        public TesionMonitor(string address, int node,int interval)
        {
            modbus = new ModbusTcpNet(address);

            Observable.Interval(TimeSpan.FromMilliseconds(interval), NewThreadScheduler.Default).Subscribe(x =>
                {
                    var result = modbus.ReadUInt16($"s:{node};162");
                    if (result.IsSuccess)
                    {
                        OnRecieve.Invoke(result.Content);
                    }
                });
        }
        public IObservable<ushort> GetObservable()
        {
          return  Observable.FromEvent<ushort>(x => OnRecieve += x, x => OnRecieve -= x);
        }
    }
}