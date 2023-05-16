namespace CalloutInterfaceHelper.External
{
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Functions for interacting with StopThePed.
    /// </summary>
    public static class StopThePedFunctions
    {
        /// <summary>
        /// Indicates whether the Callout Interface is available.
        /// </summary>
        public static readonly bool IsStopThePedAvailable;

        [MethodImpl(MethodImplOptions.NoInlining)]
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
        public static VehicleDocumentStatus GetVehicleDocumentStatus(Rage.Vehicle vehicle, VehicleDocument document)
        {
            if (vehicle && IsStopThePedAvailable)
            {
                var status = document == VehicleDocument.Insurance ? StopThePed.API.Functions.getVehicleInsuranceStatus(vehicle) : StopThePed.API.Functions.getVehicleRegistrationStatus(vehicle);
                switch (status)
                {
                    case StopThePed.API.STPVehicleStatus.Expired:
                        return VehicleDocumentStatus.Expired;
                    case StopThePed.API.STPVehicleStatus.None:
                        return VehicleDocumentStatus.None;
                    case StopThePed.API.STPVehicleStatus.Valid:
                        return VehicleDocumentStatus.Valid;
                }
            }

            return VehicleDocumentStatus.Unknown;
        }
    }
}
