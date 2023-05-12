using LSPD_First_Response.Mod.Callouts;

namespace CalloutInterfaceHelper
{
    public class RegisteredCallout : CalloutInfoAttribute
    {
        public RegisteredCallout(string name, CalloutProbability probability, string description, string priority = "", string agency = "")
            : base(name, probability)
        {
            this.Agency = agency;
            this.Description = description;
            this.Priority = priority;
        }

        public string Agency { get; private set; }

        public string Description { get; private set; }

        public string Priority { get; private set; }
    }
}
