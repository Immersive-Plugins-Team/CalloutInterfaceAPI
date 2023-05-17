namespace CalloutInterfaceHelper
{
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Safe interface to StopThePed functions.
    /// </summary>
    public static class StopThePedFunctions
    {
        /// <summary>
        /// Indicates whether the Callout Interface is available.
        /// </summary>
        public static readonly bool IsStopThePedAvailable;

        static StopThePedFunctions()
        {
            IsStopThePedAvailable = LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins()
                .Any(x => x.GetName().Name.Equals("StopThePed"));
        }

        /// <summary>
        /// Gets the vehicle's document status from StopThePed.
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <param name="document">The type of document.</param>
        /// <returns>The relevant status if it's available.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static VehicleDocumentStatus GetVehicleDocumentStatus(Rage.Vehicle vehicle, VehicleDocument document)
        {
            if (IsStopThePedAvailable)
            {
                return External.StopThePedInvoker.GetVehicleDocumentStatus(vehicle, document);
            }

            return VehicleDocumentStatus.Unknown;
        }
    }
}
