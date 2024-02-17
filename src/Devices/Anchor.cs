using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using UWBPark.src.ServerSide;

namespace UWBPark.src.Devices
{   
    /// <summary>
    /// Represents an anchor device used for location calculation.
    /// </summary>
    public class Anchor : Device
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Anchor"/> class.
        /// </summary>
        /// <param name="DeviceID">The ID of the anchor device.</param>
        /// <param name="DeviceLocation">The location of the anchor device.</param>
        /// <param name="DeviceStatus">The status of the anchor device.</param>
        public Anchor(int DeviceID, Coordinate DeviceLocation, int DeviceStatus) : base(DeviceID, DeviceLocation, "Anchor", DeviceStatus)
        {
        }

        /// <summary>
        /// Receives data from the server.
        /// </summary>
        /// <param name="data">The server data.</param>
        public override void ReceiveData(ServerData data)
        {
            //TODO implement
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Calculates the location of a tag using trilateration.
        /// </summary>
        /// <param name="Anchor2">The second anchor device.</param>
        /// <param name="serverData">The server data containing distance information.</param>
        /// <returns>The calculated location of the tag.</returns>
        public Coordinate CalculateLocation(Anchor Anchor2, ServerData serverData)
        {
            Anchor anchor1 = this;
            double tagRangeAnchor1 = serverData.Distance1;
            double tagRangeAnchor2 = serverData.Distance2;

            double distanceBetweenAnchors = anchor1.DeviceLocation.DistanceTo(Anchor2.DeviceLocation);

            // Calculate the coordinates of the tag using trilateration

            double d2 = tagRangeAnchor1;
            double d1 = tagRangeAnchor2;
            double d3 = distanceBetweenAnchors;
            try
            {
                if (d1 > d2 + d3 || d2 > d1 + d3 || d3 > d1 + d2)
                {
                    throw new System.ArgumentException("The distances are not valid for trilateration");
                }
                else
                {
                    double cos_d1 = (d2 * d2 + d3 * d3 - d1 * d1) / (2 * d2 * d3);
                    double tagX = d2 * cos_d1;
                    double tagY = d2 * Math.Sqrt(1 - cos_d1 * cos_d1);
                    return new Coordinate(tagX, tagY);
                }
            }
            catch (System.ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return new Coordinate(-1, -1);
            }

        }
    }
}