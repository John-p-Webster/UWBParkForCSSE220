using UWBPark.src.ServerSide;

namespace UWBPark.src.Devices{
    /// <summary>
    /// Represents a factory class for creating different types of devices.
    /// </summary>
    static class DeviceFactory{
        /// <summary>
        /// Creates a device based on the specified parameters.
        /// </summary>
        /// <param name="DeviceID">The ID of the device.</param>
        /// <param name="DeviceLocation">The location of the device.</param>
        /// <param name="DeviceType">The type of the device.</param>
        /// <param name="DeviceStatus">The status of the device.</param>
        /// <param name="LicensePlateNumber">The license plate number (optional, only applicable for Tag devices).</param>
        /// <returns>The created device.</returns>
        public static Device CreateDevice(int DeviceID, Coordinate DeviceLocation, string DeviceType, int DeviceStatus, string LicensePlateNumber = null){
            switch(DeviceType){
                case "Tag":
                    if (LicensePlateNumber != null){
                        return new Tag(DeviceID, DeviceLocation, DeviceStatus, LicensePlateNumber);
                    }
                    else{
                        throw new ArgumentException("LicensePlateNumber must be given a value");
                    }
                case "Anchor":
                    return new Anchor(DeviceID, DeviceLocation, DeviceStatus);
                case "Camera":
                    return new Camera(DeviceID, DeviceLocation, DeviceStatus);
                default:
                    throw new System.ArgumentException("Invalid device type");
            }
        }
    }
}