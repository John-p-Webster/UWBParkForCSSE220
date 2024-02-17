using System.Security.Cryptography;
using UWBPark.src.Devices;
using UWBPark.src.ServerSide;

namespace UWBPark.src.Locations
{
    /// <summary>
    /// Represents a parking lot.
    /// </summary>
    /// <param name="name">The name of the parking lot.</param>
    /// <param name="id">The ID of the parking lot.</param>
    /// <param name="boundary">The boundary of the parking lot.</param>
    public class ParkingLot(string name, int id, FourPointBoundary boundary)
    {
        private Dictionary<int, ParkingSpace> ParkingSpaces = new Dictionary<int, ParkingSpace>();
        public string Name {get; private set;} = name;
        public int ID {get; private set;} = id;
        public FourPointBoundary Boundary {get; private set;} = boundary;

        /// <summary>
        /// Adds a parking space to the parking lot.
        /// </summary>
        /// <param name="parkingSpaceId">The ID of the parking space.</param>
        /// <param name="parkingSpaceBounds">The boundary of the parking space.</param>
        /// <returns>A message indicating whether the parking space was added successfully.</returns>
        public string AddParkingSpace(int parkingSpaceId, FourPointBoundary parkingSpaceBounds){
            if (!ParkingSpaces.ContainsKey(parkingSpaceId)){
                ParkingSpaces.Add(parkingSpaceId, new ParkingSpace(parkingSpaceId, parkingSpaceBounds, this.Name));
                return $"Parking Space {parkingSpaceId} Added to Parking Lot {this.Name} with bounds {parkingSpaceBounds.ToString()}";
            }
            return $"Parking ID {parkingSpaceId} is already taken";   
        }

        public void PrintAllReports(){
            foreach(ParkingSpace parkingSpace in ParkingSpaces.Values){
                parkingSpace.toString();
            }
        }

        /// <summary>
        /// Updates the parking lot and all of its parking spaces based on the location of a tag.
        /// </summary>
        /// <param name="tag">The tag to locate which parking space it is in.</param>
        public void Update(Tag tag){
            bool tagFound = false;
            foreach(ParkingSpace parkingSpace in ParkingSpaces.Values){
                if(parkingSpace.Update(tag)) {tagFound = true; break;}
            }
            if(!tagFound){
                Console.WriteLine($"Tag {tag.DeviceID} is not in any parking space in Parkinglot {this.Name}");
            }
        }

        /// <summary>
        /// Gets the occupancy of the parking lot.
        /// </summary>
        public int GetOccupancy(){
            int cnt = 0;
            foreach(ParkingSpace parkingSpace in ParkingSpaces.Values){
                if(parkingSpace.tagOccupyingSpace != null){
                    cnt++;
                }
            }
            return cnt;
        }

        /// <summary>
        /// Returns a string representation of the ParkingLot object.
        /// </summary>
        public void ToString()
        {
            int totalSpaces = ParkingSpaces.Count;
            int occupiedSpaces = GetOccupancy();
            double occupancyPercentage = (double)occupiedSpaces / totalSpaces * 100;

            string info = $"Parking Lot: {Name}\n";
            info += $"ID: {ID}\n";
            info += $"Boundary: {Boundary.ToString()}\n";
            info += $"Total Spaces: {totalSpaces}\n";
            info += $"Occupied Spaces: {occupiedSpaces}\n";
            info += $"Occupancy Percentage: {occupancyPercentage}%";
            Console.WriteLine(info);
        }
    }
}



