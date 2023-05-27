namespace CalloutInterfaceAPI
{
    /// <summary>
    /// The type of vehicle document a policeman might be interested in.
    /// </summary>
    public enum VehicleDocument
    {
        Insurance,
        Registration,
    }

    /// <summary>
    /// Replaces StopThePed's VehicleStatus enum for safety.
    /// </summary>
    public enum VehicleDocumentStatus
    {
        Expired,
        None,
        Unknown,
        Valid,
    }
}
