namespace CalloutInterfaceHelper
{
    using LSPD_First_Response.Mod.Callouts;

    /// <summary>
    /// Represents an attribute that provides information about a callout for the Callout Interface.
    /// </summary>
    public class CalloutInterfaceAttribute : CalloutInfoAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalloutInterfaceAttribute"/> class with the specified name, probability, description, priority, and agency.
        /// </summary>
        /// <param name="name">The name of the callout.</param>
        /// <param name="probability">The probability of the callout being triggered.</param>
        /// <param name="description">The description of the callout.</param>
        /// <param name="priority">The priority of the callout (optional).</param>
        /// <param name="agency">The agency associated with the callout (optional).</param>
        public CalloutInterfaceAttribute(string name, CalloutProbability probability, string description, string priority = "", string agency = "")
            : base(name, probability)
        {
            this.Agency = agency;
            this.Description = description;
            this.Priority = priority;
        }

        /// <summary>
        /// Gets the agency associated with the callout (e.g. LSSD, LSPD).
        /// </summary>
        public string Agency { get; private set; }

        /// <summary>
        /// Gets the description of the callout.  This is used by Callout Interface to list the callout on the callouts tab.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the priority of the callout (e.g. CODE 2, CODE 3).
        /// </summary>
        public string Priority { get; private set; }
    }
}
