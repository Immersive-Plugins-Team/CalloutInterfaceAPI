namespace CalloutInterfaceHelper.Records
{
    using System;
    using LSPD_First_Response.Engine.Scripting.Entities;

    /// <summary>
    /// Represents a ped record.
    /// </summary>
    public class PedRecord : EntityRecord<Rage.Ped>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PedRecord"/> class.
        /// </summary>
        /// <param name="ped">The underlying ped.</param>
        public PedRecord(Rage.Ped ped)
            : base(ped)
        {
        }

        /// <summary>
        /// Gets the advisory text.
        /// </summary>
        public string Advisory { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the ped's birthday.
        /// </summary>
        public DateTime Birthday { get; internal set; } = DateTime.MinValue;

        /// <summary>
        /// Gets the number of citations.
        /// </summary>
        public int Citations { get; internal set; } = 0;

        /// <summary>
        /// Gets the ped's first name.
        /// </summary>
        public string First { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether or not the ped is male.
        /// </summary>
        public bool IsMale { get; internal set; } = true;

        /// <summary>
        /// Gets a value indicating whether or not the ped is wanted.
        /// </summary>
        public bool IsWanted { get; internal set; } = false;

        /// <summary>
        /// Gets the peds last name.
        /// </summary>
        public string Last { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the license state.
        /// </summary>
        public ELicenseState LicenseState { get; internal set; } = ELicenseState.Valid;

        /// <summary>
        /// Gets the persona for the Ped.
        /// </summary>
        public Persona Persona { get; internal set; } = null;
    }
}
