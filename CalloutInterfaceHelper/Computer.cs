namespace CalloutInterfaceHelper
{
    using System;
    using System.Collections.Generic;
    using CalloutInterfaceHelper.API;
    using CalloutInterfaceHelper.External;
    using CalloutInterfaceHelper.Records;
    using LSPD_First_Response.Engine.Scripting.Entities;

    /// <summary>
    /// The police computer.
    /// </summary>
    public static class Computer
    {
        private static readonly Dictionary<Rage.Ped, PedRecord> PedDatabase = new Dictionary<Rage.Ped, PedRecord>();
        private static readonly Dictionary<Rage.Vehicle, VehicleRecord> VehicleDatabase = new Dictionary<Rage.Vehicle, VehicleRecord>();
        private static DateTime lastDateTime = DateTime.Today + Rage.World.TimeOfDay;
        private static DateTime nextDateTime = lastDateTime;
        private static float invalidVehicleDocumentRate = 0.05f;
        private static int invalidVehicleDocumentCount = 0;

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
        /// Gets the persona for the driver of a vehicle if available.
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns>The persona, or null.</returns>
        public static Persona GetDriverPersona(Rage.Vehicle vehicle)
        {
            if (vehicle && vehicle.HasDriver && vehicle.Driver)
            {
                return LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(vehicle.Driver);
            }

            return null;
        }

        /// <summary>
        /// Retrieves a vehicle record without doing an official plate check.
        /// </summary>
        /// <param name="vehicle">Rage.Vehicle vehicle.</param>
        /// <returns>The vehicle record.</returns>
        public static VehicleRecord GetVehicleRecord(Rage.Vehicle vehicle)
        {
            VehicleRecord record = null;
            if (vehicle)
            {
                if (VehicleDatabase.ContainsKey(vehicle))
                {
                    record = VehicleDatabase[vehicle];
                }
                else
                {
                    record = new VehicleRecord(vehicle);
                    VehicleDatabase[vehicle] = record;
                }
            }

            return record;
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
                if (record.InsuranceStatus != VehicleDocumentStatus.Valid || record.RegistrationStatus != VehicleDocumentStatus.Valid)
                {
                    invalidVehicleDocumentCount++;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the invalid document rate is currently exceeded.
        /// </summary>
        /// <returns>True if there are too many invalid document results in the vehicle database.</returns>
        internal static bool IsInvalidVehicleDocumentRateExceeded()
        {
            if (VehicleDatabase.Count > 0)
            {
                return ((float)invalidVehicleDocumentCount / VehicleDatabase.Count) > invalidVehicleDocumentRate;
            }

            return false;
        }

        /// <summary>
        /// Sets the rate at which documents should return as invalid.
        /// </summary>
        /// <param name="rate">The rate as a value from 0-1f.</param>
        internal static void SetInvalidVehicleDocumentRate(float rate)
        {
            invalidVehicleDocumentRate = rate;
        }
    }
}
