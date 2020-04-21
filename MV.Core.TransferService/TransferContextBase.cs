using System;
using Newtonsoft.Json;

namespace Mv.TransferService
{
    public class TransferContextBase
    {
        [JsonProperty]
        public Guid Id { get; protected set; }

        [JsonProperty]
        public long TotalSize { get; internal set; }

        [JsonProperty]
        public virtual string LocalPath { get; internal set; }
    }
}
