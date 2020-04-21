using System;
using Polly;
using Polly.NoOp;
using Polly.Retry;

namespace Mv.TransferService
{
    public class DownloadSettings
    {
        internal DownloadSettings() { }

        public Policy BuildPolicy { get; set; }

        public BlockDownloadItemPolicy DownloadPolicy { get; internal set; }

        public int MaxConcurrent { get; set; }

        //public static implicit operator DownloadSettings(DownloadSettings v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
