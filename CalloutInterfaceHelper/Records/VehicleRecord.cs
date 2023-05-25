namespace CalloutInterfaceHelper.Records
{
    using System;
    using CalloutInterfaceHelper.API;
    using CalloutInterfaceHelper.External;
    using LSPD_First_Response.Engine.Scripting.Entities;
    using Rage.Native;

    /// <summary>
    /// Represents a vehicle record looked up on the computer.
    /// </summary>
    public class VehicleRecord
    {
        private Persona ownerPersona = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRecord"/> class.
        /// </summary>
        /// <param name="vehicle">The Rage.Vehicle to base the record on.</param>
        public VehicleRecord(Rage.Vehicle vehicle)
        {
            this.RecordCreated = DateTime.Now;
            if (vehicle)
            {
                this.Class = vehicle.Class.ToString();
                this.Color = Functions.GetColorName(vehicle.PrimaryColor);
                this.InsuranceStatus = GetDocumentStatus(vehicle, VehicleDocument.Insurance);
                this.LicensePlate = vehicle.LicensePlate;
                this.LicensePlateStyle = vehicle.LicensePlateStyle;
                this.Make = GetVehicleMake(vehicle.Model.Hash);
                this.Model = GetVehicleModel(vehicle.Model.Hash);
                this.OwnerName = LSPD_First_Response.Mod.API.Functions.GetVehicleOwnerName(vehicle);
                this.RegistrationStatus = GetDocumentStatus(vehicle, VehicleDocument.Registration);
                this.Vehicle = vehicle;
            }
        }

        /// <summary>
        /// Gets the vehicle's class.
        /// </summary>
        public string Class { get; } = string.Empty;

        /// <summary>
        /// Gets the vehicle's color.
        /// </summary>
        public string Color { get; } = string.Empty;

        /// <summary>
        /// Gets the vehicle insurance status.
        /// </summary>
        public VehicleDocumentStatus InsuranceStatus { get; } = VehicleDocumentStatus.Unknown;

        /// <summary>
        /// Gets the vehicle's license plate.
        /// </summary>
        public string LicensePlate { get; } = string.Empty;

        /// <summary>
        /// Gets the vehicle's license plate style.
        /// </summary>
        public Rage.LicensePlateStyle LicensePlateStyle { get; } = Rage.LicensePlateStyle.BlueOnWhite1;

        /// <summary>
        /// Gets the vehicle's make.
        /// </summary>
        public string Make { get; } = string.Empty;

        /// <summary>
        /// Gets the vehicle's model.
        /// </summary>
        public string Model { get; } = string.Empty;

        /// <summary>
        /// Gets the vehicle's owner.
        /// </summary>
        public string OwnerName { get; } = string.Empty;

        /// <summary>
        /// Gets the vehicle owner's persona.
        /// </summary>
        public Persona OwnerPersona
        {
            get
            {
                // since getting the owner persona requires enumeration of all world peds, use lazy lookup
                if (this.ownerPersona == null && this.Vehicle)
                {
                    this.ownerPersona = GetOwnerPersona(this.Vehicle);
                }

                return this.ownerPersona;
            }
        }

        /// <summary>
        /// Gets the real world DateTime for when the record was created.
        /// </summary>
        public DateTime RecordCreated { get; }

        /// <summary>
        /// Gets the vehicle registration status.
        /// </summary>
        public VehicleDocumentStatus RegistrationStatus { get; } = VehicleDocumentStatus.Unknown;

        /// <summary>
        /// Gets the underlying Rage.Vehicle object for the record.
        /// </summary>
        public Rage.Vehicle Vehicle { get; } = null;

        /// <summary>
        /// Gets a vehicle registration status.
        /// </summary>
        /// <param name="vehicle">The vehicle to look up.</param>
        /// <param name="document">The type of vehicle document.</param>
        /// <returns>The status.</returns>
        internal static VehicleDocumentStatus GetDocumentStatus(Rage.Vehicle vehicle, VehicleDocument document)
        {
            if (!vehicle)
            {
                return VehicleDocumentStatus.Unknown;
            }

            var status = StopThePedFunctions.GetVehicleDocumentStatus(vehicle, document);
            if (status != VehicleDocumentStatus.Unknown)
            {
                if (status != VehicleDocumentStatus.Valid && Computer.IsInvalidVehicleDocumentRateExceeded())
                {
                    status = VehicleDocumentStatus.Valid;
                    StopThePedFunctions.SetVehicleDocumentStatus(vehicle, document, status);
                }

                return status;
            }

            if (!Computer.IsInvalidVehicleDocumentRateExceeded())
            {
                int random = RandomNumberGenerator.Next(0, 30);
                if (random == 7 || random == 12)
                {
                    return VehicleDocumentStatus.Expired;
                }
                else if (random == 10)
                {
                    return VehicleDocumentStatus.None;
                }
            }

            return VehicleDocumentStatus.Valid;
        }

        /// <summary>
        /// Retrieves the make of a vehicle.
        /// </summary>
        /// <param name="hash">The vehicle's model hash.</param>
        /// <returns>A string representing the vehicle's make.</returns>
        internal static string GetVehicleMake(uint hash)
        {
            try
            {
                var make = Rage.Game.GetLocalizedString(NativeFunction.Natives.GET_MAKE_NAME_FROM_VEHICLE_MODEL<string>(hash));
                if (!string.IsNullOrEmpty(make))
                {
                    return make;
                }
            }
            catch
            {
            }

            return "unknown";
        }

        /// <summary>
        /// Retrieves the model of a vehicle.
        /// </summary>
        /// <param name="hash">The vehicle's model hash.</param>
        /// <returns>A string representing the vehicle's model.</returns>
        internal static string GetVehicleModel(uint hash)
        {
            try
            {
                var model = Rage.Game.GetLocalizedString(NativeFunction.Natives.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL<string>(hash));
                if (!string.IsNullOrEmpty(model))
                {
                    return model;
                }
            }
            catch
            {
            }

            return "unknown";
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
            var driver = Computer.GetDriverPersona(vehicle);
            if (driver != null && string.Compare(driver.FullName, name, true) == 0)
            {
                return driver;
            }

            foreach (var ped in Rage.World.EnumeratePeds())
            {
                var persona = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(ped);
                if (persona != null && string.Compare(persona.FullName, name, true) == 0)
                {
                    return persona;
                }
            }

            return null;
        }
    }
}
