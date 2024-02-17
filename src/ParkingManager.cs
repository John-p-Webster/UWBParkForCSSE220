using System.Collections;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Transactions;
using UWBPark.src.Devices;
using UWBPark.src.Locations;
using UWBPark.src.ServerSide;

namespace UWBPark.src{
    /// <summary>
    /// Represents a parking manager that manages parking lots, profiles, and devices.
    /// </summary>
    public class ParkingManager{
        private Dictionary<int, ParkingLot> parkingLots = new Dictionary<int, ParkingLot>();
        private Dictionary<string, Profile> profiles = new Dictionary<string, Profile>();
        private Dictionary<int, Device> devices = new Dictionary<int, Device>();
        private int currentDeviceID = 0;
        private int currentParkinglotID = 0;
        private int currentParkingSpaceID = 0;
        private int currentProfileID = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParkingManager"/> class.
        /// </summary>
        public ParkingManager(){
            // Updates the parking manager when a new message is received from the server
            Server.NewMessageReceived += Update;
        }
        
        /// <summary>
        /// Handles the addition of a parking lot.
        /// </summary>
        /// <param name="name">The name of the parking lot.</param>
        /// <param name="id">The ID of the parking lot.</param>
        /// <param name="bounds">The boundary of the parking lot.</param>
        public void HandleAddParkingLot(string name, FourPointBoundary bounds, int parkingSpaces = 0){
            double parkingSpaceWidth = 10;
            double parkingSpaceHeight = 10;
            FourPointBoundary parkingSpaceBounds = new FourPointBoundary(new Coordinate(0,0), 
                                                                         new Coordinate(parkingSpaceWidth, 0), 
                                                                         new Coordinate(parkingSpaceWidth, parkingSpaceHeight), 
                                                                         new Coordinate(0, parkingSpaceHeight));
            if (parkingSpaces != 0){
                parkingLots.Add(currentParkinglotID, new ParkingLot(name, currentParkinglotID, bounds));
                for (int i = 0; i < parkingSpaces; i++){
                    parkingSpaceBounds.increaseX(parkingSpaceWidth);
                    Console.WriteLine(parkingLots[currentParkinglotID].AddParkingSpace(currentParkingSpaceID, parkingSpaceBounds));
                    currentParkingSpaceID++;
                }
            }
            else{
                parkingLots.Add(currentParkinglotID, new ParkingLot(name, currentParkinglotID, bounds));
                Console.Out.WriteLine($"Parking Lot, '{name}' created with ID, {currentParkinglotID}, and bounds {bounds}");
            }
            currentParkinglotID++;
        }

        /// <summary>
        /// Handles the addition of a profile.
        /// </summary>
        /// <param name="Name">The name of the profile.</param>
        /// <param name="id">The ID of the profile.</param>
        /// <param name="LicensePlateNumber">The license plate number of the profile.</param>
        /// <returns>The newly created profile.</returns>
        public Profile HandleAddProfile(string Name, string LicensePlateNumber){
            Profile newProfile = new Profile(Name, currentProfileID, LicensePlateNumber);
            profiles.Add(Name, newProfile);
            Console.Out.WriteLine($"Profile, '{Name}', created with ID {currentProfileID} and License Plate Number {LicensePlateNumber}");
            currentProfileID++;
            return newProfile;
        }

        /// <summary>
        /// Handles the addition of a parking space.
        /// </summary>
        /// <param name="ParkingLotName">The name of the parking lot.</param>
        /// <param name="ParkingSpaceID">The ID of the parking space.</param>
        /// <param name="Bounds">The boundary of the parking space.</param>
        public void HandleAddParkingSpace(int parkinglotID, int ParkingSpaceID, FourPointBoundary Bounds){
            string result = this.parkingLots[parkinglotID].AddParkingSpace(ParkingSpaceID, Bounds);
            Console.WriteLine(result);
        }

        /// <summary>
        /// Handles the addition of a device.
        /// </summary>
        /// <param name="DeviceLocation">The location of the device.</param>
        /// <param name="DeviceType">The type of the device.</param>
        /// <param name="DeviceStatus">The status of the device.</param>
        public void HandleAddDevice(Coordinate DeviceLocation, string DeviceType, int DeviceStatus, String LicensePlateNumber = null){
            Device newDevice = DeviceFactory.CreateDevice(currentDeviceID, DeviceLocation, DeviceType, DeviceStatus, LicensePlateNumber);
            devices.Add(currentDeviceID, newDevice);
            currentDeviceID++;
            Console.WriteLine($"New {newDevice.DeviceType} Device created with ID {newDevice.DeviceID}");
        }

        /// <summary>
        /// Handles printing all reports for all parking lots.
        /// </summary>
        public void HandlePrintAllReports(){
            foreach (ParkingLot parkingLot in parkingLots.Values){
                Console.WriteLine("________________________________________________________");
                parkingLot.ToString();
                Console.WriteLine("________________________________________________________");
                parkingLot.PrintAllReports();
            }
        }

        /// <summary>
        /// Updates the parking manager based on the received server data.
        /// </summary>
        /// <param name="serverData">The server data received.</param>
        public void Update(ServerData serverData){
            Tag tag;
            Coordinate tagLocation;

            if (!devices.ContainsKey(serverData.TagID) || !devices.ContainsKey(serverData.AnchorID1) || !devices.ContainsKey(serverData.AnchorID2)){
                Console.WriteLine("Device not found, server is not functioning properly.");
                return;
            }

            // Actual update code
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nUpdate received");
            Console.WriteLine(serverData.ToString());
            
            // Calculates and updates the location of the tag 
            tag = (Tag)devices[serverData.TagID];
            Anchor anchor1 = (Anchor)devices[serverData.AnchorID1];
            Anchor anchor2 = (Anchor)devices[serverData.AnchorID2];
            tagLocation = anchor1.CalculateLocation(anchor2, serverData);
            tag.DeviceLocation = tagLocation;
            Console.WriteLine($"Tag {tag.DeviceID} is at {tagLocation}");

            // Updates the parking lots based on the location of the tag
            tag.DeviceLocation = tagLocation;
            foreach (ParkingLot parkingLot in parkingLots.Values){
                parkingLot.Update(tag);
            }
            HandlePrintAllReports();
        }
        /// <summary>
        /// Tests the triangulation algorithm with a specific coordinate and tag.
        /// </summary>
        /// <param name="testingCoordinate">The coordinate to test.</param>
        /// <param name="testTagID">The ID of the tag to test.</param>
        public void UpdateTest(Coordinate testingCoordinate, int testTagID){
            Tag tag;
            Coordinate tagLocation;
            // Tests with a specific coordinate
            tag = (Tag)devices[testTagID];
            tagLocation = (Coordinate)testingCoordinate;

            // Updates the parking lots based on the location of the tag
            tag.DeviceLocation = tagLocation;
            Console.WriteLine(tag.DeviceLocation.ToString());
            foreach (ParkingLot parkingLot in parkingLots.Values){
                parkingLot.Update(tag);
                parkingLot.PrintAllReports();
            }
        }
    }
}
