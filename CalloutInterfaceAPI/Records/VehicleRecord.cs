namespace CalloutInterfaceAPI.Records
{
    using LSPD_First_Response.Engine.Scripting.Entities;

    /// <summary>
    /// Represents a vehicle record looked up on the computer.
    /// </summary>
    public class VehicleRecord : EntityRecord<Rage.Vehicle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRecord"/> class.
        /// </summary>
        /// <param name="vehicle">The Rage.Vehicle to base the record on.</param>
        public VehicleRecord(Rage.Vehicle vehicle)
            : base(vehicle)
        {
        }

        /// <summary>
        /// Gets the vehicle's class.
        /// </summary>
        public string Class { get; internal set; } = "Unknown";

        /// <summary>
        /// Gets the vehicle's color.
        /// </summary>
        public string Color { get; internal set; } = "Unknown";

        /// <summary>
        /// Gets the vehicle insurance status.
        /// </summary>
        public VehicleDocumentStatus InsuranceStatus { get; internal set; } = VehicleDocumentStatus.Valid;

        /// <summary>
        /// Gets the vehicle's license plate.
        /// </summary>
        public string LicensePlate { get; internal set; } = "Unknown";

        /// <summary>
        /// Gets the vehicle's license plate style.
        /// </summary>
        public Rage.LicensePlateStyle LicensePlateStyle { get; internal set; } = Rage.LicensePlateStyle.BlueOnWhite1;

        /// <summary>
        /// Gets the vehicle's make.
        /// </summary>
        public string Make { get; internal set; } = "Unknown";

        /// <summary>
        /// Gets the vehicle's model.
        /// </summary>
        public string Model { get; internal set; } = "Unknown";

        /// <summary>
        /// Gets the vehicle's owner.
        /// </summary>
        public string OwnerName { get; internal set; } = "Unknown";

        /// <summary>
        /// Gets the vehicle owner's persona.
        /// </summary>
        public Persona OwnerPersona { get; internal set; } = null;

        /// <summary>
        /// Gets the vehicle registration status.
        /// </summary>
        public VehicleDocumentStatus RegistrationStatus { get; internal set; } = VehicleDocumentStatus.Valid;
    }
}
