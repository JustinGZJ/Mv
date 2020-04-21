using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mv.TransferService
{
    internal class DownloadSerializedData
    {
        [JsonProperty]
        public DownloadContext Context { get; internal set; }

        [JsonProperty]
        public List<BlockTransferContext> BlockContexts { get; internal set; }
    }
}
