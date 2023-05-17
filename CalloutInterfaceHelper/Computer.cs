namespace CalloutInterfaceHelper
{
    using System.Collections.Generic;

    /// <summary>
    /// The police computer.
    /// </summary>
    public static class Computer
    {
        private static readonly Dictionary<Rage.Vehicle, VehicleRecord> VehicleDatabase = new Dictionary<Rage.Vehicle, VehicleRecord>();

        /// <summary>
        /// Runs a plate check.
        /// </summary>
        /// <param name="vehicle">The vehicle to run the plate check against.</param>
        /// <param name="source">Some identifier to include so we know where the plate check request came from.</param>
        public static void PlateCheck(Rage.Vehicle vehicle, string source)
        {
            if (!vehicle)
            {
                return;
            }

            CalloutInterfaceFunctions.SendVehicle(vehicle);
            if (VehicleDatabase.TryGetValue(vehicle, out VehicleRecord vehicleRecord))
            {
                Events.RaisePlateCheckEvent(vehicleRecord, source);
            }
            else
            {
                var record = new VehicleRecord(vehicle);
                VehicleDatabase[vehicle] = record;
                Events.RaisePlateCheckEvent(record, source);
            }
        }
    }
}
