namespace CalloutInterfaceHelper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The police computer.
    /// </summary>
    public static class Computer
    {
        private static readonly Dictionary<Rage.Ped, PedRecord> PedDatabase = new Dictionary<Rage.Ped, PedRecord>();
        private static readonly Dictionary<Rage.Vehicle, VehicleRecord> VehicleDatabase = new Dictionary<Rage.Vehicle, VehicleRecord>();
        private static DateTime lastDateTime = DateTime.Today + Rage.World.TimeOfDay;
        private static DateTime nextDateTime = lastDateTime;

        /// <summary>
        /// Gets a consistent date time.
        /// </summary>
        /// <returns>A DateTime object that syncs with the in-game time of day and the current date.</returns>
        public static DateTime GetDateTime()
        {
            nextDateTime = lastDateTime.Date + Rage.World.TimeOfDay;
            if (nextDateTime < lastDateTime)
            {
                nextDateTime += TimeSpan.FromDays(1.0);
            }

            lastDateTime = nextDateTime;
            return nextDateTime;
        }

        /// <summary>
        /// Checks to see if a vehicle has any insurance or registration flags.
        /// </summary>
        /// <param name="vehicle">Rage.Vehicle vehicle.</param>
        /// <returns>True if there are any flags on the vehicle, false otherwise.</returns>
        public static bool IsVehicleFlagged(Rage.Vehicle vehicle)
        {
            if (!vehicle)
            {
                return false;
            }

            VehicleRecord record = null;
            if (VehicleDatabase.ContainsKey(vehicle))
            {
                record = VehicleDatabase[vehicle];
            }
            else
            {
                record = new VehicleRecord(vehicle);
                VehicleDatabase[vehicle] = record;
            }

            return record.InsuranceStatus == VehicleDocumentStatus.Expired || record.InsuranceStatus == VehicleDocumentStatus.None ||
                record.RegistrationStatus == VehicleDocumentStatus.Expired || record.RegistrationStatus == VehicleDocumentStatus.None ||
                vehicle.IsStolen;
        }

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
