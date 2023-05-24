namespace CalloutInterfaceHelper
{
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides helper functions related to the Callout Interface.
    /// </summary>
    public static class CalloutInterfaceFunctions
    {
        /// <summary>
        /// Indicates whether the Callout Interface is available.
        /// </summary>
        public static readonly bool IsCalloutInterfaceAvailable;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static CalloutInterfaceFunctions()
        {
            IsCalloutInterfaceAvailable = LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins()
                .Any(x => x.GetName().Name.Equals("CalloutInterface"));
        }

        /// <summary>
        /// Sends the callout details to the Callout Interface.
        /// </summary>
        /// <param name="callout">The callout to send the details for.</param>
        /// <param name="priority">The priority of the callout.</param>
        /// <param name="agency">The agency associated with the callout.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SendCalloutDetails(LSPD_First_Response.Mod.Callouts.Callout callout, string priority, string agency)
        {
            if (IsCalloutInterfaceAvailable)
            {
                External.CalloutInterfaceInvoker.SendCalloutDetails(callout, priority, agency);
            }
        }

        /// <summary>
        /// Sends a message to the Callout Interface.
        /// </summary>
        /// <param name="callout">The callout associated with the message.</param>
        /// <param name="message">The message to send.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SendMessage(LSPD_First_Response.Mod.Callouts.Callout callout, string message)
        {
            if (IsCalloutInterfaceAvailable)
            {
                External.CalloutInterfaceInvoker.SendMessage(callout, message);
            }
        }

        /// <summary>
        /// Sends a vehicle for the plate display.
        /// </summary>
        /// <param name="vehicle">The targeted vehicle.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SendVehicle(Rage.Vehicle vehicle)
        {
            if (IsCalloutInterfaceAvailable)
            {
                External.CalloutInterfaceInvoker.SendVehicle(vehicle);
            }
        }
    }
}
