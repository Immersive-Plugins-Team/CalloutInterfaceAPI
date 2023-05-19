namespace CalloutInterfaceHelper
{
    using System;
    using LSPD_First_Response.Engine.Scripting.Entities;

    /// <summary>
    /// Represents a ped record.
    /// </summary>
    public class PedRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PedRecord"/> class.
        /// </summary>
        /// <param name="ped">The underlying ped.</param>
        public PedRecord(Rage.Ped ped)
        {
            if (ped)
            {
                this.IsMale = ped.IsMale;
                this.Ped = ped;
                var persona = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(ped);
                if (persona != null)
                {
                    this.Advisory = persona.AdvisoryText;
                    this.Birthday = persona.Birthday;
                    this.Citations = persona.Citations;
                    this.First = persona.Forename;
                    this.IsWanted = persona.Wanted;
                    this.Last = persona.Surname;
                    this.LicenseState = persona.ELicenseState;
                }
            }
        }

        /// <summary>
        /// Gets the advisory text.
        /// </summary>
        public string Advisory { get; } = string.Empty;

        /// <summary>
        /// Gets the ped's birthday.
        /// </summary>
        public DateTime Birthday { get; } = DateTime.MinValue;

        /// <summary>
        /// Gets the number of citations.
        /// </summary>
        public int Citations { get; } = 0;

        /// <summary>
        /// Gets the ped's first name.
        /// </summary>
        public string First { get; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether or not the ped is male.
        /// </summary>
        public bool IsMale { get; } = true;

        /// <summary>
        /// Gets a value indicating whether or not the ped is wanted.
        /// </summary>
        public bool IsWanted { get; } = false;

        /// <summary>
        /// Gets the peds last name.
        /// </summary>
        public string Last { get; } = string.Empty;

        /// <summary>
        /// Gets the license state.
        /// </summary>
        public ELicenseState LicenseState { get; } = ELicenseState.Valid;

        /// <summary>
        /// Gets the underlying Ped.
        /// </summary>
        public Rage.Ped Ped { get; } = null;
    }
}
