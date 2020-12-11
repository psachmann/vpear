namespace VPEAR.Core
{
    /// <summary>
    /// The current device state.
    /// </summary>
    public enum DeviceStatus
    {
        /// The device has stopped recording data from a patient.
        Stopped = 0,

        /// The device is recording data from a patient.
        Active,

        /// The device doesn't exist anymore, but the patient is kept in the database.
        Archived,

        /// The device is currently not reachable. Reason: new address, out of energy
        NotReachable,
    }
}
