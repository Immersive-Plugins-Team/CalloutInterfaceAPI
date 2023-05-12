using System.Linq;

namespace CalloutInterfaceHelper
{
    public static class Functions
    {
        public static readonly bool isCalloutInterfaceAvailable;

        static Functions()
        {
            var assembly = LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins().FirstOrDefault(x => x.Na)
        }
    }
}
