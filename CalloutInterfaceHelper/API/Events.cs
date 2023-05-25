namespace CalloutInterfaceHelper.API
{
    using CalloutInterfaceHelper.Records;

    /// <summary>
    /// Callout Interface Helper events.
    /// </summary>
    public class Events
    {
        /// <summary>
        /// The event handler delegate for OnPedCheck.
        /// </summary>
        /// <param name="record">The resulting record.</param>
        /// <param name="source">The source of the request.</param>
        public delegate void PedCheckEventHandler(PedRecord record, string source);

        /// <summary>
        /// The event handler delegate for OnPlateCheck.
        /// </summary>
        /// <param name="record">The resulting record.</param>
        /// <param name="source">The source of the request.</param>
        public delegate void PlateCheckEventHandler(VehicleRecord record, string source);

        /// <summary>
        /// An event for a ped record.
        /// </summary>
        public static event PedCheckEventHandler OnPedCheck;

        /// <summary>
        /// An event for a vehicle record
        /// </summary>
        public static event PlateCheckEventHandler OnPlateCheck;

        /// <summary>
        /// Raises a ped check event.
        /// </summary>
        /// <param name="record">The result of the ped check.</param>
        /// <param name="source">The source of the request.</param>
        internal static void RaisePedCheckEvent(PedRecord record, string source)
        {
            OnPedCheck?.Invoke(record, source);
        }

        /// <summary>
        /// Raises a plate check event.
        /// </summary>
        /// <param name="record">The result of the plate check.</param>
        /// <param name="source">The source of the request.</param>
        internal static void RaisePlateCheckEvent(VehicleRecord record, string source)
        {
            OnPlateCheck?.Invoke(record, source);
        }
    }
}
