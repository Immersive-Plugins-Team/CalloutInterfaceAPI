namespace CalloutInterfaceHelper
{
    /// <summary>
    /// Callout Interface Helper events.
    /// </summary>
    public class Events
    {
        /// <summary>
        /// The event handler delegate for OnPlateCheck.
        /// </summary>
        /// <param name="record">The resulting record.</param>
        public delegate void PlateCheckEventHandler(VehicleRecord record);

        /// <summary>
        /// An event for a vehicle record
        /// </summary>
        public static event PlateCheckEventHandler OnPlateCheck;

        /// <summary>
        /// Raises a plate check event.
        /// </summary>
        /// <param name="record">The result of the plate check.</param>
        internal static void RaisePlateCheckEvent(VehicleRecord record)
        {
            OnPlateCheck?.Invoke(record);
        }
    }
}
