using System;
using System.Collections.Generic;
using System.Text;

namespace Mv.Modules.Axis.Controlers
{
   public interface IShareData
    {
        public int LoaderHandshake { get; set; }

        public int WeildingHandshake { get; set; }
    }

   public class ShareData:IShareData
    {
        public int LoaderHandshake { get; set; } = 0;

        public int WeildingHandshake { get; set; } = 0;
    }
}
