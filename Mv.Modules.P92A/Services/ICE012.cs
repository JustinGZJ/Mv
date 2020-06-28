using System.Collections;

namespace Mv.Modules.P92A.Service
{
    public interface ICE012
    {
        (bool, string) PostData(Hashtable hashtable);
    }
}