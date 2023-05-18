﻿namespace CalloutInterfaceHelper
{
    using System.Collections.Generic;
    using System.Drawing;
    using LSPD_First_Response.Engine.Scripting.Entities;
    using Rage;
    using Rage.Native;

    /// <summary>
    /// Represents a vehicle record looked up on the computer.
    /// </summary>
    public class VehicleRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRecord"/> class.
        /// </summary>
        /// <param name="vehicle">The Rage.Vehicle to base the record on.</param>
        public VehicleRecord(Rage.Vehicle vehicle)
        {
            if (vehicle)
            {
                this.Class = vehicle.Class.ToString();
                this.Color = GetColorName(vehicle.PrimaryColor);
                this.InsuranceStatus = GetDocumentStatus(vehicle, VehicleDocument.Insurance);
                this.LicensePlate = vehicle.LicensePlate;
                this.LicensePlateStyle = vehicle.LicensePlateStyle;
                this.Make = GetVehicleMake(vehicle.Model.Hash);
                this.Model = GetVehicleModel(vehicle.Model.Hash);
                this.OwnerName = LSPD_First_Response.Mod.API.Functions.GetVehicleOwnerName(vehicle);
                this.OwnerPersona = GetOwnerPersona(vehicle);
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
        public Persona OwnerPersona { get; } = null;

        /// <summary>
        /// Gets the vehicle registration status.
        /// </summary>
        public VehicleDocumentStatus RegistrationStatus { get; } = VehicleDocumentStatus.Unknown;

        /// <summary>
        /// Gets the underlying Rage.Vehicle object for the record.
        /// </summary>
        public Rage.Vehicle Vehicle { get; } = null;

        /// <summary>
        /// Gets a human readable color string from a Color object.
        /// </summary>
        /// <param name="color">The Color object.</param>
        /// <returns>The name of the color.</returns>
        internal static string GetColorName(Color color)
        {
            // Define some colors and their RGB values
            Dictionary<string, int[]> colors = new Dictionary<string, int[]>
            {
                { "Black", new int[] { 0, 0, 0 } },
                { "White", new int[] { 255, 255, 255 } },
                { "Silver", new int[] { 192, 192, 192 } },
                { "Gray", new int[] { 128, 128, 128 } },
                { "Dark Gray", new int[] { 64, 64, 64 } },
                { "Light Gray", new int[] { 192, 192, 192 } },
                { "Red", new int[] { 255, 0, 0 } },
                { "Dark Red", new int[] { 139, 0, 0 } },
                { "Pink", new int[] { 255, 192, 203 } },
                { "Maroon", new int[] { 128, 0, 0 } },
                { "Brown", new int[] { 165, 42, 42 } },
                { "Light Brown", new int[] { 205, 133, 63 } },
                { "Dark Brown", new int[] { 101, 67, 33 } },
                { "Orange", new int[] { 255, 165, 0 } },
                { "Yellow", new int[] { 255, 255, 0 } },
                { "Green", new int[] { 0, 128, 0 } },
                { "Light Green", new int[] { 144, 238, 144 } },
                { "Dark Green", new int[] { 0, 100, 0 } },
                { "Blue", new int[] { 0, 0, 255 } },
                { "Light Blue", new int[] { 173, 216, 230 } },
                { "Dark Blue", new int[] { 0, 0, 139 } },
                { "Navy Blue", new int[] { 0, 0, 128 } },
                { "Purple", new int[] { 128, 0, 128 } },
            };

            // Find the color with the closest RGB values
            int minDistance = int.MaxValue;
            string closestColor = "unk";
            foreach (var kvp in colors)
            {
                int[] rgb = kvp.Value;
                int distance = ((color.R - rgb[0]) * (color.R - rgb[0])) + ((color.G - rgb[1]) * (color.G - rgb[1])) + ((color.B - rgb[2]) * (color.B - rgb[2]));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestColor = kvp.Key;
                }
            }

            return closestColor;
        }

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
                return status;
            }

            int random = RandomNumberGenerator.Next(0, 20);
            if (random == 7 || random == 12)
            {
                return VehicleDocumentStatus.Expired;
            }
            else if (random == 10)
            {
                return VehicleDocumentStatus.None;
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
            if (vehicle.Driver)
            {
                var driver = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(vehicle.Driver);
                if (string.Compare(driver.FullName, name, true) == 0)
                {
                    return driver;
                }
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