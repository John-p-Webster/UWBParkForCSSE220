using System.Runtime.ConstrainedExecution;
using UWBPark.src.ServerSide;

namespace UWBPark.src.Devices
{   
    /// <summary>
    /// Represents a generic device.
    /// </summary>
    public abstract class Device
    {
        public int DeviceID { get; protected set; }
        public string DeviceType { get; protected set; }
        public Coordinate DeviceLocation { get; set; }
        public int DeviceStatus { get; set; }
        static readonly int ONLINE = 1;
        static readonly int OFFLINE = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="DeviceID">The device ID.</param>
        /// <param name="DeviceLocation">The device location.</param>
        /// <param name="DeviceType">The device type.</param>
        /// <param name="DeviceStatus">The device status.</param>
        public Device(int DeviceID, Coordinate DeviceLocation, string DeviceType, int DeviceStatus)
        {
            this.DeviceID = DeviceID;
            this.DeviceType = DeviceType;
            this.DeviceLocation = DeviceLocation;
            this.DeviceStatus = DeviceStatus;
        }

        /// <summary>
        /// Receives data from the server.
        /// </summary>
        /// <param name="data">The server data.</param>
        public abstract void ReceiveData(ServerData data);

        /// <summary>
        /// Gets the status of the device.
        /// </summary>
        /// <returns>The status of the device.</returns>
        public string GetStatus()
        {
            if (this.DeviceStatus == ONLINE)
            {
                return $"{this.DeviceType} {this.DeviceID} is online";
            }
            else
            {
                return $"{this.DeviceType} {this.DeviceID} is offline";
            }
        }
    }
}