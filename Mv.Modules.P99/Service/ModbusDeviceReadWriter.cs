
using HslCommunication.Core;
using HslCommunication.ModBus;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Mv.Modules.P99.Service
{
    public class ModbusDeviceReadWriter : IDeviceReadWriter
    {
        byte[] _rbs = new byte[100 * 2];
        byte[] _wbs = new byte[40*2];
        public bool IsConnected { get ; set; }
        ModbusTcpNet modbus;
        public ModbusDeviceReadWriter()
        {
            modbus = new ModbusTcpNet("192.168.1.1", 8000) {
                IsStringReverse = true
              //  ByteTransform = new ReverseWordTransform(DataFormat.ABCD)
            };
            modbus.AddressStartWithZero = true;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var rr = modbus.Read("0", 100);
                    if (rr.IsSuccess)
                    {                    
                        Buffer.BlockCopy(rr.Content,0,_rbs, 0, rr.Content.Length);
                    }
                    IsConnected = rr.IsSuccess;
                    var wt=modbus.Write("400",_wbs);
                }
            }, TaskCreationOptions.LongRunning);
        }
        public ushort GetWord(int index)
        {
           
            return modbus.ByteTransform.TransUInt16(_rbs, index * 2);
        }
        public bool GetBit(int index, int bit)
        {
            var m = GetWord(index);
            var r = (m & (1 << bit)) > 0;
            return r;
        }

        public short GetShort(int index)
        {
            return modbus.ByteTransform.TransInt16(_rbs, index * 2);
        }

        public void SetShort(int index, short value)
        {
            Buffer.BlockCopy(modbus.ByteTransform.TransByte(value), 0, _wbs, index * 2, 2);
    
        }

        public void SetBit(int index, int bit, bool value)
        {
            if (value)
            {
                var mInt16 = (ushort)(modbus.ByteTransform.TransUInt16(_wbs, index * 2) | (1 << bit));
                SetShort(index, (short)mInt16);
            }
            else
            {
                var mInt16 = (ushort)(modbus.ByteTransform.TransUInt16(_wbs, index * 2) & (~(1 << bit)));
                SetShort(index, (short)mInt16);
            }
        }

        public void SetString(int index, string value)
        {
            var bs = modbus.ByteTransform.TransByte(value,Encoding.ASCII);
            Buffer.BlockCopy(bs, 0, _wbs, index * 2, bs.Length);
        }

        /// <summary>
        /// 从读取缓冲区读取
        /// </summary>
        /// <param name="index">字的索引</param>
        /// <param name="len">字符串长度</param>
        /// <returns></returns>
        public string GetString(int index, int len)
        {
           return modbus.ByteTransform.TransString(_rbs, index, len, Encoding.ASCII);
          
        }

        public bool GetSetBit(int index, int bit)
        {
            return (modbus.ByteTransform.TransUInt16(_wbs, index * 2) & (1 << bit)) > 0;
        }

        public void PlcConnect()
        {
          //  throw new NotImplementedException();
        }

    
    }
}
