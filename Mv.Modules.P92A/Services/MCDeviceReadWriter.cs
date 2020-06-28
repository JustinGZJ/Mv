using HslCommunication.Profinet.Melsec;
using Prism.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Mv.Modules.P92A.Service
{
    public class MCDeviceReadWriter : IDeviceReadWriter
    {
        public bool IsConnected { get; set; }

        byte[] _rbs = new byte[675 * 2];

        byte[] _wbs = new byte[10 * 2];



        MelsecMcNet melsec = new MelsecMcNet("192.168.0.10", 6000);
        private readonly ILoggerFacade logger;

        public MCDeviceReadWriter(ILoggerFacade logger)
        {
            Task.Factory.StartNew(() =>
            {
                melsec.ConnectServer();
                while (true)
                {
                   
                    for (int ia=0;ia<2;ia++)
                    {
                        
                        var rr = melsec.ReadBool($"M{1+5000*ia}", (ushort)(625*8));
                        if (rr.IsSuccess)
                        {
                            for (int i = 0; i < 625; i++)
                            {
                                var m = 0;
                                for (int j = 0; j < 8; j++)
                                {
                                    m += (rr.Content[i * 8 + j] ? 1 : 0) << j;
                                }
                                _rbs[i+625*ia] = (byte)m;
                            }
                        }
                        else
                        {
                            logger.Log(rr.Message, Category.Warn, Priority.None);
                        }
                        IsConnected = rr.IsSuccess;
                    }
                    var rd = melsec.Read($"D950", (ushort)(50));
                    if(rd.IsSuccess)
                    {
                        Buffer.BlockCopy(rd.Content, 0, _rbs, 625 * 2, 100);
                    }              
                    var bs = new byte[_wbs.Length * 8];
                    for (int i = 0; i < _wbs.Length; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            var k = (int)_wbs[i] & (1 << j);
                            bs[(i * 8) + j] = (byte)((k > 0) ? 1 : 0);
                        }
                    }
                    var wt = melsec.Write("M13000",bs);
                }
            }, TaskCreationOptions.LongRunning);
            this.logger = logger;
        }
        bool local;
        public ushort GetWord(int index)
        {
            return BitConverter.ToUInt16(_rbs, index * 2);
        }
        public bool GetBit(int index, int bit)
        {
            var m = GetWord(index);
            var r = (m & (1 << bit)) > 0;
            return r;
        }

        public short GetShort(int index)
        {
            return BitConverter.ToInt16(_rbs, index * 2);
        }

        public void SetShort(int index, short value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _wbs, index * 2, 2);
        }

        public void SetBit(int index, int bit, bool value)
        {
            if (value)
            {
                var mInt16 = (ushort)(BitConverter.ToUInt16(_wbs, index * 2) | (1 << bit));
                SetShort(index, (short)mInt16);
            }
            else
            {
                var mInt16 = (ushort)(BitConverter.ToUInt16(_wbs, index * 2) & (~(1 << bit)));
                SetShort(index, (short)mInt16);
            }
        }

        public void SetString(int index, string value)
        {
            var bs = Encoding.ASCII.GetBytes(value);
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
            return Encoding.ASCII.GetString(_rbs, index, len);
        }

        public bool GetSetBit(int index, int bit)
        {
            return (BitConverter.ToUInt16(_wbs, index * 2) & (1 << bit)) > 0;
        }

        public void PlcConnect()
        {
            //    throw new NotImplementedException();
        }

        public int GetInt(int index)
        {
            return BitConverter.ToInt32(_rbs, index * 2);
        }

        public void SetInt(int index, int value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _wbs, index * 2, 4);
        }

        public uint GetUInt(int index)
        {
            return BitConverter.ToUInt32(_rbs, index * 2);
        }

        public void SetUInt(int index, uint value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _wbs, index * 2, 4);
        }
    }
}