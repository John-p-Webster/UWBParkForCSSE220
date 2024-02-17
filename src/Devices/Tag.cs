using System.Drawing;
using System.Xml.XPath;
using UWBPark.src.ServerSide;

namespace UWBPark.src.Devices
{   
    /// <summary>
    /// Represents a tag device.
    /// </summary>
    public class Tag : Device
    {
        /// <summary>
        /// Gets or sets the profile associated with the tag.
        /// </summary>
        public Profile? Profile { get; private set; }

        /// <summary>
        /// Gets the license plate number associated with the tag.
        /// </summary>
        public string LicensePlateNumber { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="DeviceID">The ID of the device.</param>
        /// <param name="DeviceLocation">The location of the device.</param>
        /// <param name="DeviceStatus">The status of the device.</param>
        /// <param name="LicensePlateNumber">The license plate number associated with the tag.</param>
        public Tag(int DeviceID, Coordinate DeviceLocation, int DeviceStatus, string LicensePlateNumber) : base(DeviceID, DeviceLocation, "Tag", DeviceStatus)
        {
            this.LicensePlateNumber = LicensePlateNumber;
        }

        /// <summary>
        /// Checks if the tag device is within the specified boundary coordinates.
        /// </summary>
        /// <param name="boundary">The boundary coordinates to check against.</param>
        /// <returns><c>true</c> if the tag device is within the boundary coordinates; otherwise, <c>false</c>.</returns>
        public bool IsWithinCoordinates(FourPointBoundary boundary)
        {   
            if (boundary.Vertex1.X <= this.DeviceLocation.X && boundary.Vertex2.X >= this.DeviceLocation.X 
             && boundary.Vertex1.Y <= this.DeviceLocation.Y && boundary.Vertex3.Y >= this.DeviceLocation.Y)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Receives data from the server.
        /// </summary>
        /// <param name="data">The server data to receive.</param>
        public override void ReceiveData(ServerData data)
        {
            throw new NotImplementedException();
        }
    }
}