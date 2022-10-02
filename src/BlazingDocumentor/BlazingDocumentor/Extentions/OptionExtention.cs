using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingDocumentor.Extentions
{
    public static class SyncOptionExtention
    {
        public static bool PublicMemberOnly()
        {
            return true;// General.GetLiveInstanceAsync().GetAwaiter().GetResult().PublicMemberOnly;
        }
    }
}
