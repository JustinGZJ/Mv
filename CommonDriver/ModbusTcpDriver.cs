using DataService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using HslCommunication.ModBus;

namespace CommonDriver
{
    [Description("Modbus TCP协议")]
    public class ModbusTcpDriver : DriverInitBase, IPLCDriver, IMultiReadWrite
    {
        public sealed class Modbus
        {
            public const byte fctReadCoil = 1;
            public const byte fctReadDiscreteInputs = 2;
            public const byte fctReadHoldingRegister = 3;
            public const byte fctReadInputRegister = 4;
            public const byte fctWriteSingleCoil = 5;
            public const byte fctWriteSingleRegister = 6;
            public const byte fctWriteMultipleCoils = 15;
            public const byte fctWriteMultipleRegister = 16;
            public const byte fctReadWriteMultipleRegister = 23;

            /// <summary>Constant for exception illegal function.</summary>
            public const byte excIllegalFunction = 1;
            /// <summary>Constant for exception illegal data address.</summary>
            public const byte excIllegalDataAdr = 2;
            /// <summary>Constant for exception illegal data value.</summary>
            public const byte excIllegalDataVal = 3;
            /// <summary>Constant for exception slave device failure.</summary>
            public const byte excSlaveDeviceFailure = 4;
            /// <summary>Constant for exception acknowledge.</summary>
            public const byte excAck = 5;
            /// <summary>Constant for exception slave is busy/booting up.</summary>
            public const byte excSlaveIsBusy = 6;
            /// <summary>Constant for exception gate path unavailable.</summary>
            public const byte excGatePathUnavailable = 10;
            /// <summary>Constant for exception not connected.</summary>
            public const byte excExceptionNotConnected = 253;
            /// <summary>Constant for exception connection lost.</summary>
            public const byte excExceptionConnectionLost = 254;
            /// <summary>Constant for exception response timeout.</summary>
            public const byte excExceptionTimeout = 255;
            /// <summary>Constant for exception wrong offset.</summary>
            public const byte excExceptionOffset = 128;
            /// <summary>Constant for exception send failt.</summary>
            public const byte excSendFailt = 100;
        }
        protected ModbusTcpNet con = new ModbusTcpNet() { AddressStartWithZero=false,DataFormat=HslCommunication.Core.DataFormat.CDAB};
        public void Dispose()
        {
            con.ConnectClose();
            //  throw new System.NotImplementedException();
        }
        public ModbusTcpDriver()
        {

        }
        public ModbusTcpDriver(IDataServer server, short id, string name, string serverName, int timeOut = 500, IDictionary<string, string> paras = null) : base(server, id, name, serverName, timeOut, paras)
        {
            ID = id;
            Name = name;
            Parent = server;
            _ip = serverName;
      

        }

        event EventHandler<Exception> IDriver.OnError
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        string _ip;//服务ip
        int _port = 5000; //服务端口
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public string ServerName
        {
            get { return _ip; }
            set { _ip = value; }
        }
        List<IGroup> _grps = new List<IGroup>(20);
        public short ID { get; }
        public string Name { get; }
        private bool _IsClosed = true;
        public bool IsClosed => _IsClosed;
        public int TimeOut { get; set; }
        public IEnumerable<IGroup> Groups => _grps;
        public IDataServer Parent { get; }

        public bool Connect()
        {
            con.Port = Port;
            con.IpAddress = ServerName;

            _IsClosed = !con.ConnectServer().IsSuccess;
            return con.ConnectServer().IsSuccess;
        }

        public IGroup AddGroup(string name, short id, int updateRate, float deadBand = 0, bool active = false)
        {
            NetShortGroup grp = new NetShortGroup(id, name, updateRate, active, this);
            _grps.Add(grp);
            return grp;
        }

        public bool RemoveGroup(IGroup grp)
        {
            grp.IsActive = false;
            return _grps.Remove(grp);
        }

        public event ErrorEventHandler OnError;
        public event ShutdownRequestEventHandler OnClose;

        public byte[] ReadBytes(DeviceAddress address, ushort size)
        {
            var r = con.Read(GetAddress(address), size);
            _IsClosed = !r.IsSuccess;
            return r.Content;
        }
        public ItemData<uint> ReadUInt32(DeviceAddress address)
        {
            var r = con.ReadUInt32(GetAddress(address));
            _IsClosed = !r.IsSuccess;
            return r.IsSuccess ? new ItemData<uint>(r.Content, 0, QUALITIES.QUALITY_GOOD)
                : new ItemData<uint>(0, 0, QUALITIES.QUALITY_BAD);
        }

        public ItemData<int> ReadInt32(DeviceAddress address)
        {
            var r = con.ReadInt32(GetAddress(address));
            _IsClosed = !r.IsSuccess;
            return r.IsSuccess ? new ItemData<int>(r.Content, 0, QUALITIES.QUALITY_GOOD)
                : new ItemData<int>(0, 0, QUALITIES.QUALITY_BAD);
        }

        public ItemData<ushort> ReadUInt16(DeviceAddress address)
        {
            var r = con.ReadUInt16(GetAddress(address));
            _IsClosed = !r.IsSuccess;
            return r.IsSuccess ? new ItemData<ushort>(r.Content, 0, QUALITIES.QUALITY_GOOD)
                : new ItemData<ushort>(0, 0, QUALITIES.QUALITY_BAD);
        }

        public ItemData<short> ReadInt16(DeviceAddress address)
        {
            var r = con.ReadInt16(GetAddress(address));
            _IsClosed = !r.IsSuccess;
            return r.IsSuccess ? new ItemData<short>(r.Content, 0, QUALITIES.QUALITY_GOOD)
                : new ItemData<short>(0, 0, QUALITIES.QUALITY_BAD);
        }

        public ItemData<byte> ReadByte(DeviceAddress address)
        {
            var r = con.Read(GetAddress(address), 1);
            _IsClosed = !r.IsSuccess;
            return !r.IsSuccess ? new ItemData<byte>(0, 0, QUALITIES.QUALITY_BAD)
                : new ItemData<byte>(r.Content[0], 0, QUALITIES.QUALITY_GOOD);
        }

        public ItemData<string> ReadString(DeviceAddress address, ushort size)
        {
            var r = con.ReadString(GetAddress(address), size);
            return r.IsSuccess ? new ItemData<string>(r.Content, 0, QUALITIES.QUALITY_GOOD)
                : new ItemData<string>("", 0, QUALITIES.QUALITY_BAD);

        }

        public ItemData<float> ReadFloat(DeviceAddress address)
        {
            var r = con.ReadFloat(GetAddress(address));
            _IsClosed = !r.IsSuccess;
            return r.IsSuccess ? new ItemData<float>(r.Content, 0, QUALITIES.QUALITY_GOOD)
                : new ItemData<float>(0, 0, QUALITIES.QUALITY_BAD);
        }

        public ItemData<bool> ReadBit(DeviceAddress address)
        {
            var r = con.ReadBool(GetAddress(address));
            _IsClosed = !r.IsSuccess;
            return r.IsSuccess ? new ItemData<bool>(r.Content, 0, QUALITIES.QUALITY_GOOD)
                : new ItemData<bool>(false, 0, QUALITIES.QUALITY_BAD);
        }

        public ItemData<object> ReadValue(DeviceAddress address)
        {
            return this.ReadValueEx(address);
        }

        public int WriteBytes(DeviceAddress address, byte[] bit)
        {
            return con.Write(GetAddress(address), bit).IsSuccess ? 0 : -1;
        }

        public int WriteBit(DeviceAddress address, bool bit)
        {
            return con.Write(GetAddress(address), bit).IsSuccess ? 0 : -1;
        }

        public int WriteBits(DeviceAddress address, byte bits)
        {
            var r = con.ReadInt16(GetAddress(address));
            _IsClosed = !r.IsSuccess;
            if (r.IsSuccess)
            {
                var m = r.Content & 0xff00 | bits;
                return con.Write(GetAddress(address), m).IsSuccess ? 0 : -1;
            }

            return -1;
        }

        public int WriteInt16(DeviceAddress address, short value)
        {
            return con.Write(GetAddress(address), value).IsSuccess ? 0 : -1;
        }

        public int WriteUInt16(DeviceAddress address, ushort value)
        {
            return con.Write(GetAddress(address), value).IsSuccess ? 0 : -1;
        }

        public int WriteInt32(DeviceAddress address, int value)
        {
            return con.Write(GetAddress(address), value).IsSuccess ? 0 : -1;
        }

        public int WriteUInt32(DeviceAddress address, uint value)
        {
            return con.Write(GetAddress(address), value).IsSuccess ? 0 : -1;
        }

        public int WriteFloat(DeviceAddress address, float value)
        {
            return con.Write(GetAddress(address), value).IsSuccess ? 0 : -1;
        }

        public int WriteString(DeviceAddress address, string str)
        {
            return con.Write(GetAddress(address), str).IsSuccess ? 0 : -1;
        }

        public int WriteValue(DeviceAddress address, object value)
        {
            return this.WriteValueEx(address, value);
        }

        public int PDU { get; } = 100;
        public virtual DeviceAddress GetDeviceAddress(string address)
        {
            DeviceAddress dv = DeviceAddress.Empty;
            if (string.IsNullOrEmpty(address))
                return dv;
            var sindex = address.IndexOf(':');
            if (sindex > 0)
            {
                dv.Area = int.Parse(address.Substring(0, sindex));
                address = address.Substring(sindex + 1);
            }
            else
            {
                dv.Area = 1;
            }
            switch (address[0])
            {
                case '0':
                    {
                        dv.DBNumber = Modbus.fctReadCoil;
                        int st;
                        int.TryParse(address, out st);
                        //dv.Start = (st / 16) * 16;//???????????????????
                        dv.Bit = (byte)(st % 16);
                        st /= 16;
                        dv.Start = st;
                    }
                    break;
                case '1':
                    {
                        dv.DBNumber = Modbus.fctReadDiscreteInputs;
                        int st;
                        int.TryParse(address.Substring(1), out st);
                        //dv.Start = (st / 16) * 16;//???????????????????
                        dv.Bit = (byte)(st % 16);
                        st /= 16;
                        dv.Start = st;
                    }
                    break;
                case '4':
                    {
                        int index = address.IndexOf('.');
                        dv.DBNumber = Modbus.fctReadHoldingRegister;
                        if (index > 0)
                        {
                            dv.Start = int.Parse(address.Substring(1, index - 1));
                            dv.Bit = byte.Parse(address.Substring(index + 1));
                        }
                        else
                            dv.Start = int.Parse(address.Substring(1));
      
                    }
                    break;
                case '3':
                    {
                        int index = address.IndexOf('.');
                        dv.DBNumber = Modbus.fctReadInputRegister;
                        if (index > 0)
                        {
                            dv.Start = int.Parse(address.Substring(1, index - 1));
                            dv.Bit = byte.Parse(address.Substring(index + 1));
                        }
                        else
                            dv.Start = int.Parse(address.Substring(1));
          
                    }
                    break;
            }
            return dv;
        }

        public string GetAddress(DeviceAddress address)
        {
            string idx="1";
            if (address.DBNumber == Modbus.fctReadCoil)
            {
                idx = (address.Bit + address.Start * 16).ToString();
            }
            else if (address.DBNumber == Modbus.fctReadDiscreteInputs)
            {
                idx = (address.Bit + address.Start * 16).ToString();
            }
            else if (address.DBNumber == Modbus.fctReadHoldingRegister)
            {
                idx = $"{address.Start}";
            }
            else if (address.DBNumber == Modbus.fctReadInputRegister) {
                idx = $"{address.Start}";
            }

            return $"s:{address.Area};x:{address.DBNumber};{idx}";
        }

        public int Limit { get; } = 960;
        public ItemData<Storage>[] ReadMultiple(DeviceAddress[] addrsArr)
        {
            return this.PLCReadMultiple(new ShortCacheReader(), addrsArr);
        }

        public int WriteMultiple(DeviceAddress[] addrArr, object[] buffer)
        {
            return this.PLCWriteMultiple(new ShortCacheReader(), addrArr, buffer, Limit);
        }
    }
}
