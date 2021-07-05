using Prism.Modularity;
using System.Collections.Generic;

namespace Mv.Ui.Core.Modularity
{
    public class RemoteModuleInfo : ModuleInfo
    {
        public List<RemoteRef> RemoteRefs { get; set; }
    }
}
