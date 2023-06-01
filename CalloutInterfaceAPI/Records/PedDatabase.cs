namespace CalloutInterfaceAPI.Records
{
    using System;
    using System.Collections.Generic;
    using LSPD_First_Response.Engine.Scripting.Entities;

    /// <summary>
    /// Represents a database of ped records.
    /// </summary>
    internal class PedDatabase : RecordDatabase<Rage.Ped, PedRecord>
    {
        private int invalidLicenseCount = 0;
        private int wantedCount = 0;

        /// <summary>
        /// Gets or sets the max invalid license rate.
        /// </summary>
        internal float MaxInvalidLicenseRate { get; set; } = 0.05f;

        /// <summary>
        /// Gets or sets the max wanted rate.
        /// </summary>
        internal float MaxWantedRate { get; set; } = 0.01f;

        /// <inheritdoc />
        internal override void Prune(int minutes)
        {
            Rage.Game.LogTrivial($"CalloutInterfaceAPI pruning ped data older than {minutes} minute(s)");
            Rage.Game.LogTrivial($"  total entries: {this.Entities.Count}");
            Rage.Game.LogTrivial($"  invalid licenses: {this.invalidLicenseCount}");
            Rage.Game.LogTrivial($"  wanted count    : {this.wantedCount}");

            var deadKeys = new List<Rage.PoolHandle>();
            var threshold = DateTime.Now - TimeSpan.FromMinutes(minutes);
            foreach (var pair in this.Entities)
            {
                var record = pair.Value;
                if (!record.Entity || record.CreationDateTime < threshold)
                {
                    deadKeys.Add(pair.Key);
                    this.invalidLicenseCount -= (record.LicenseState == ELicenseState.Expired || record.LicenseState == ELicenseState.Suspended) ? 1 : 0;
                    this.wantedCount -= record.IsWanted ? 1 : 0;
                }
            }

            foreach (var key in deadKeys)
            {
                this.Entities.Remove(key);
            }

            Rage.Game.LogTrivial($"  entries removed : {deadKeys.Count}");
        }

        /// <inheritdoc />
        protected override PedRecord CreateRecord(Rage.Ped ped)
        {
            var record = new PedRecord(ped);
            if (ped)
            {
                record.IsMale = ped.IsMale;
                var persona = this.GetPersona(ped);
                if (persona != null)
                {
                    record.Advisory = persona.AdvisoryText;
                    record.Birthday = persona.Birthday;
                    record.Citations = persona.Citations;
                    record.First = persona.Forename;
                    record.IsWanted = persona.Wanted;
                    record.Last = persona.Surname;
                    record.LicenseState = persona.ELicenseState;
                    record.Persona = persona;
                }
            }

            this.invalidLicenseCount += (record.LicenseState == ELicenseState.Expired || record.LicenseState == ELicenseState.Suspended) ? 1 : 0;
            this.wantedCount += record.IsWanted ? 1 : 0;
            return record;
        }

        private Persona GetPersona(Rage.Ped ped)
        {
            var persona = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(ped);
            if (persona != null)
            {
                bool isPersonaChanged = false;
                if (persona.Wanted && this.Entities.Count > 0 && (float)this.wantedCount / this.Entities.Count > this.MaxWantedRate)
                {
                    persona.Wanted = false;
                    isPersonaChanged = true;
                }

                if (persona.ELicenseState == ELicenseState.Expired || persona.ELicenseState == ELicenseState.Suspended)
                {
                    if (this.Entities.Count > 0 && (float)this.invalidLicenseCount / this.Entities.Count > this.MaxInvalidLicenseRate)
                    {
                        persona.ELicenseState = ELicenseState.Valid;
                        isPersonaChanged = true;
                    }
                }

                if (isPersonaChanged)
                {
                    try
                    {
                        LSPD_First_Response.Mod.API.Functions.SetPersonaForPed(ped, persona);
                    }
                    catch (Exception ex)
                    {
                        Rage.Game.LogTrivial($"CalloutInterfaceAPI encountered an error while setting a persona: {ex.Message}");
                    }
                }
            }

            return persona;
        }
    }
}
