namespace CalloutInterfaceHelper
{
    using System.Collections.Generic;

    /// <summary>
    /// The police computer.
    /// </summary>
    public static class Computer
    {
        private static readonly Dictionary<Rage.Ped, PedRecord> PedDatabase = new Dictionary<Rage.Ped, PedRecord>();
        private static readonly Dictionary<Rage.Vehicle, VehicleRecord> VehicleDatabase = new Dictionary<Rage.Vehicle, VehicleRecord>();

        /// <summary>
        /// Runs a ped check.
        /// </summary>
        /// <param name="ped">The ped to run a ped check against.</param>
        /// <param name="source">Some identifier to include so we know where the ped check request came from.</param>
        public static void PedCheck(Rage.Ped ped, string source)
        {
            if (!ped)
            {
                return;
            }

            if (PedDatabase.TryGetValue(ped, out PedRecord record))
            {
                Events.RaisePedCheckEvent(record, source);
            }
            else
            {
                record = new PedRecord(ped);
                PedDatabase[ped] = record;
                Events.RaisePedCheckEvent(record, source);
            }
        }

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
            if (VehicleDatabase.TryGetValue(vehicle, out VehicleRecord record))
            {
                Events.RaisePlateCheckEvent(record, source);
            }
            else
            {
                record = new VehicleRecord(vehicle);
                VehicleDatabase[vehicle] = record;
                Events.RaisePlateCheckEvent(record, source);
            }
        }
    }
}
