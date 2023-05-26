namespace CalloutInterfaceHelper
{
    using CalloutInterfaceHelper.API;
    using CalloutInterfaceHelper.External;
    using CalloutInterfaceHelper.Records;
    using LSPD_First_Response.Engine.Scripting.Entities;

    /// <summary>
    /// The police computer.
    /// </summary>
    public static class Computer
    {
        private static readonly PedDatabase PedDatabase = new PedDatabase();
        private static readonly VehicleDatabase VehicleDatabase = new VehicleDatabase();

        /// <summary>
        /// Retrieves a ped record without doing an official ped check.
        /// </summary>
        /// <param name="ped">Rage.Ped ped.</param>
        /// <returns>The ped record.</returns>
        public static PedRecord GetPedRecord(Rage.Ped ped)
        {
            return PedDatabase.GetRecord(ped);
        }

        /// <summary>
        /// Retrieves a vehicle record without doing an official plate check.
        /// </summary>
        /// <param name="vehicle">Rage.Vehicle vehicle.</param>
        /// <returns>The vehicle record.</returns>
        public static VehicleRecord GetVehicleRecord(Rage.Vehicle vehicle)
        {
            return VehicleDatabase.GetRecord(vehicle);
        }

        /// <summary>
        /// Runs a ped check.
        /// </summary>
        /// <param name="ped">The ped to run a ped check against.</param>
        /// <param name="source">Some identifier to include so we know where the ped check request came from.</param>
        public static void PedCheck(Rage.Ped ped, string source)
        {
            if (ped)
            {
                var record = PedDatabase.GetRecord(ped);
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
            if (vehicle)
            {
                CalloutInterfaceFunctions.SendVehicle(vehicle);
                var record = VehicleDatabase.GetRecord(vehicle);
                Events.RaisePlateCheckEvent(record, source);
            }
        }

        /// <summary>
        /// Gets the persona for the driver of a vehicle if available.
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns>The persona, or null.</returns>
        internal static Persona GetDriverPersona(Rage.Vehicle vehicle)
        {
            if (vehicle && vehicle.HasDriver && vehicle.Driver)
            {
                return PedDatabase.GetRecord(vehicle.Driver).Persona;
            }

            return null;
        }

        /// <summary>
        /// Gets the Ped Persona for the owner of the vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle being looked up.</param>
        /// <returns>The relevant persona.</returns>
        internal static Persona GetOwnerPersona(Rage.Vehicle vehicle)
        {
            if (!vehicle)
            {
                return null;
            }

            var name = LSPD_First_Response.Mod.API.Functions.GetVehicleOwnerName(vehicle);
            var driver = GetDriverPersona(vehicle);
            if (driver != null && string.Compare(driver.FullName, name, true) == 0)
            {
                return driver;
            }

            foreach (var ped in Rage.Game.LocalPlayer.Character.GetNearbyPeds(16))
            {
                var persona = PedDatabase.GetRecord(ped).Persona;
                if (persona != null && string.Compare(persona.FullName, name, true) == 0)
                {
                    return persona;
                }
            }

            return null;
        }

        /// <summary>
        /// Prunes the databases completely.
        /// </summary>
        internal static void PurgeAll()
        {
            PedDatabase.Prune(0);
            VehicleDatabase.Prune(0);
        }

        /// <summary>
        /// Sets the rate at which documents should return as invalid.
        /// </summary>
        /// <param name="rate">The rate as a value from 0-1f.</param>
        internal static void SetMaxInvalidDocumentRate(float rate)
        {
            VehicleDatabase.MaxInvalidDocumentRate = rate;
        }

        /// <summary>
        /// Sets the rate at which licenses should return as invalid.
        /// </summary>
        /// <param name="rate">The rate as a value from 0-1f.</param>
        internal static void SetMaxInvalidLicenseRate(float rate)
        {
            PedDatabase.MaxInvalidLicenseRate = rate;
        }

        /// <summary>
        /// Sets the rate at which persons should come back wanted.
        /// </summary>
        /// <param name="rate">The rate as a value from 0-1f.</param>
        internal static void SetMaxWantedRate(float rate)
        {
            PedDatabase.MaxWantedRate = rate;
        }
    }
}
