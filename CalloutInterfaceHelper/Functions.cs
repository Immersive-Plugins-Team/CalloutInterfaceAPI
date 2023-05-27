namespace CalloutInterfaceAPI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Rage.Native;

    /// <summary>
    /// Miscellanous helper functions.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Indicates whether the Callout Interface is available.
        /// </summary>
        public static readonly bool IsCalloutInterfaceAvailable;

        private static readonly List<ColorCondition> ColorTable;
        private static DateTime lastDateTime;
        private static DateTime nextDateTime;

        static Functions()
        {
            IsCalloutInterfaceAvailable = LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins()
                .Any(x => x.GetName().Name.Equals("CalloutInterface") && x.GetName().Version.CompareTo(new Version("1.4.0.0")) >= 0);

            ColorTable = new List<ColorCondition>()
            {
                new ColorCondition((h, s, b) => b < 0.1f, "Black"),
                new ColorCondition((h, s, b) => s < 0.1f && b > 0.9f, "White"),
                new ColorCondition((h, s, b) => s < 0.1f, "Gray"),
                new ColorCondition((h, s, b) => (h < 30 || h >= 330) && s > 0.5 && s < 0.9 && b > 0.5 && b < 0.9, "Pink"),
                new ColorCondition((h, s, b) => h < 30 || h >= 330, "Red"),
                new ColorCondition((h, s, b) => h < 90 && s > 0.6 && b < 0.2, "Brown"),
                new ColorCondition((h, s, b) => h < 90 && s < 0.4 && b > 0.5 && b < 0.8, "Orange"),
                new ColorCondition((h, s, b) => h < 90 && s < 0.4 && b > 0.8, "Tan"),
                new ColorCondition((h, s, b) => h < 90, "Yellow"),
                new ColorCondition((h, s, b) => h < 150 && b < 0.5 && s < 0.6, "Olive"),
                new ColorCondition((h, s, b) => h < 150, "Green"),
                new ColorCondition((h, s, b) => h < 210, "Cyan"),
                new ColorCondition((h, s, b) => h < 270, "Blue"),
                new ColorCondition((h, s, b) => h < 330, "Purple"),
            };

            lastDateTime = DateTime.Today + Rage.World.TimeOfDay;
            nextDateTime = lastDateTime;
        }

        /// <summary>
        /// Converts a Color struct into a readable name.  Kinda.  I did my best.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>A name of the color that is reasonably accurate.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetColorName(Color color)
        {
            var h = color.GetHue();
            var s = color.GetSaturation();
            var b = color.GetBrightness();
            foreach (var condition in ColorTable)
            {
                if (condition.Condition(h, s, b))
                {
                    return condition.ColorName;
                }
            }

            return "Unknown";
        }

        /// <summary>
        /// Gets a consistent date time.
        /// </summary>
        /// <returns>A DateTime object that syncs with the in-game time of day and the current date.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
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
        /// Sends a message to the Callout Interface.  Does not support color codes but it does support newlines (\n).
        /// If CalloutInterface is not available, does nothing.
        /// </summary>
        /// <param name="callout">The callout associated with the message.</param>
        /// <param name="message">The message to send.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SendMessage(LSPD_First_Response.Mod.Callouts.Callout callout, string message)
        {
            if (IsCalloutInterfaceAvailable)
            {
                External.CalloutInterfaceInvoker.SendMessage(callout, message);
            }
        }

        /// <summary>
        /// Sends a vehicle for the external (non-MDT) plate display.
        /// If CalloutInterface is not available, does nothing.
        /// </summary>
        /// <param name="vehicle">The targeted vehicle.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SendVehicle(Rage.Vehicle vehicle)
        {
            if (vehicle && IsCalloutInterfaceAvailable)
            {
                External.CalloutInterfaceInvoker.SendVehicle(vehicle);
            }
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
        /// Represents a potential color combination based on HSB.
        /// </summary>
        internal class ColorCondition
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ColorCondition"/> class.
            /// </summary>
            /// <param name="condition">The condition for which the color applies.</param>
            /// <param name="colorName">The name of the relevant color.</param>
            public ColorCondition(Func<float, float, float, bool> condition, string colorName)
            {
                this.Condition = condition;
                this.ColorName = colorName;
            }

            /// <summary>
            /// Gets the condition.
            /// </summary>
            public Func<float, float, float, bool> Condition { get; }

            /// <summary>
            /// Gets the color name.
            /// </summary>
            public string ColorName { get; }
        }
    }
}
