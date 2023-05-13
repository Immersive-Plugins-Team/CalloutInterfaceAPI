using System.Linq;
using System.Runtime.CompilerServices;

namespace CalloutInterfaceHelper
{
    public static class CalloutInterfaceFunctions
    {
        public static readonly bool isCalloutInterfaceAvailable;

        static CalloutInterfaceFunctions()
        {
            isCalloutInterfaceAvailable = LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins()
                .Any(x => x.GetName().Name.Equals("CalloutInterface"));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SendCalloutDetails(LSPD_First_Response.Mod.Callouts.Callout callout, string priority, string agency)
        {
            if (isCalloutInterfaceAvailable)
            {
                CalloutInterface.API.Functions.SendCalloutDetails(callout, priority, agency);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SendMessage(LSPD_First_Response.Mod.Callouts.Callout callout, string message)
        {
            if (!isCalloutInterfaceAvailable)
            {
                return;
            }

            CalloutInterface.API.Functions.SendMessage(callout, message);
        }
    }
}
