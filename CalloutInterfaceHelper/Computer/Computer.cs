namespace CalloutInterfaceHelper.Computer
{
    using System.Collections.Generic;

    /// <summary>
    /// The police computer.
    /// </summary>
    internal static class Computer
    {
        private static readonly Dictionary<Rage.Vehicle, VehicleRecord> VehicleDatabase = new Dictionary<Rage.Vehicle, VehicleRecord>();

        /// <summary>
        /// Runs a plate check.
        /// </summary>
        /// <param name="vehicle">The vehicle to run the plate check against.</param>
        public static void PlateCheck(Rage.Vehicle vehicle)
        {
            if (!vehicle)
            {
                return;
            }

            if (VehicleDatabase.TryGetValue(vehicle, out VehicleRecord vehicleRecord))
            {
                Events.RaisePlateCheckEvent(vehicleRecord);
            }
            else
            {
                var record = new VehicleRecord(vehicle);
                VehicleDatabase[vehicle] = record;
                Events.RaisePlateCheckEvent(vehicleRecord);
            }
        }
    }
}
