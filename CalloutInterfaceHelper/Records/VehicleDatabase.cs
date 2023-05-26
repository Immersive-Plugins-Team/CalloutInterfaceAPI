namespace CalloutInterfaceHelper.Records
{
    using System;
    using System.Collections.Generic;
    using CalloutInterfaceHelper.API;
    using CalloutInterfaceHelper.External;
    using Rage;

    /// <summary>
    /// Represents a database of vehicle records.
    /// </summary>
    internal class VehicleDatabase : RecordDatabase<Rage.Vehicle, VehicleRecord>
    {
        private int invalidDocumentCount = 0;

        /// <summary>
        /// Gets or sets the maximum invalid document rate.
        /// </summary>
        internal float MaxInvalidDocumentRate { get; set; } = 0.05f;

        /// <inheritdoc />
        internal override void Prune(int minutes)
        {
            Game.LogTrivial($"CalloutInterfaceHelper pruning vehicle data older than {minutes} minute(s)");
            Game.LogTrivial($"  {this.Entities.Count} vehicles");
            Game.LogTrivial($"  {this.invalidDocumentCount} invalid documents");

            var deadKeys = new List<Rage.PoolHandle>();
            var threshold = DateTime.Now - TimeSpan.FromMinutes(minutes);
            foreach (var pair in this.Entities)
            {
                var record = pair.Value;
                if (!record.Entity || record.CreationDateTime < threshold)
                {
                    deadKeys.Add(pair.Key);
                    this.invalidDocumentCount -= (record.InsuranceStatus != VehicleDocumentStatus.Valid || record.RegistrationStatus != VehicleDocumentStatus.Valid) ? 1 : 0;
                }
            }

            foreach (var key in deadKeys)
            {
                this.Entities.Remove(key);
            }

            Game.LogTrivial($"  removed {deadKeys.Count} entries");
        }

        /// <summary>
        /// Creates a database record.
        /// </summary>
        /// <param name="vehicle">The underlying vehicle.</param>
        /// <returns>The database record based on the vehicle.</returns>
        protected override VehicleRecord CreateRecord(Rage.Vehicle vehicle)
        {
            var record = new VehicleRecord(vehicle);
            if (vehicle)
            {
                record.Class = vehicle.Class.ToString();
                record.Color = Functions.GetColorName(vehicle.PrimaryColor);
                record.InsuranceStatus = this.GetDocumentStatus(vehicle, VehicleDocument.Insurance);
                record.LicensePlate = vehicle.LicensePlate;
                record.LicensePlateStyle = vehicle.LicensePlateStyle;
                record.Make = Functions.GetVehicleMake(vehicle.Model.Hash);
                record.Model = Functions.GetVehicleModel(vehicle.Model.Hash);
                record.OwnerName = LSPD_First_Response.Mod.API.Functions.GetVehicleOwnerName(vehicle);
                record.OwnerPersona = Computer.GetOwnerPersona(vehicle);
                record.RegistrationStatus = this.GetDocumentStatus(vehicle, VehicleDocument.Registration);
            }

            this.invalidDocumentCount += (record.InsuranceStatus != VehicleDocumentStatus.Valid || record.RegistrationStatus != VehicleDocumentStatus.Valid) ? 1 : 0;
            return record;
        }

        /// <summary>
        /// Gets a vehicle registration status.
        /// </summary>
        /// <param name="vehicle">The vehicle to look up.</param>
        /// <param name="document">The type of vehicle document.</param>
        /// <returns>The status.</returns>
        private VehicleDocumentStatus GetDocumentStatus(Rage.Vehicle vehicle, VehicleDocument document)
        {
            if (!vehicle)
            {
                return VehicleDocumentStatus.Unknown;
            }

            bool invalidDocumentRateExceeded = this.Entities.Count > 0 && (float)this.invalidDocumentCount / this.Entities.Count > this.MaxInvalidDocumentRate;
            var status = StopThePedFunctions.GetVehicleDocumentStatus(vehicle, document);
            if (status != VehicleDocumentStatus.Unknown)
            {
                if (status != VehicleDocumentStatus.Valid && invalidDocumentRateExceeded)
                {
                    status = VehicleDocumentStatus.Valid;
                    StopThePedFunctions.SetVehicleDocumentStatus(vehicle, document, status);
                }

                return status;
            }

            if (!invalidDocumentRateExceeded)
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
    }
}
