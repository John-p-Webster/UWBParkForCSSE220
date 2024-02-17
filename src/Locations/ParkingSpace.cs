using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using UWBPark.src.Devices;
using UWBPark.src.ServerSide;
namespace UWBPark.src.Locations{
    /// <summary>
    /// Represents a parking space in a parking lot.
    /// </summary>
    public class ParkingSpace(int ID, FourPointBoundary Boundary, String ParkinglotName)
    {
        /// <summary>
        /// Gets the ID of the parking space.
        /// </summary>
        public int ID {get; private set;} = ID; 

        /// <summary>
        /// Gets or sets the boundary of the parking space.
        /// </summary>
        private FourPointBoundary Boundary = Boundary;

        /// <summary>
        /// Gets or sets the tag occupying the parking space.
        /// </summary>
        public Tag? tagOccupyingSpace {get; set;} = null;

        /// <summary>
        /// Gets the name of the parking lot.
        /// </summary>
        public String ParkinglotName {get; private set;} = ParkinglotName;

        /// <summary>
        /// Checks if the specified tag is within the parking space.
        /// </summary>
        /// <param name="tagToCheck">The tag to check.</param>
        /// <returns>True if the tag is within the parking space, otherwise false.</returns>
        public bool ContainsTag(Tag tagToCheck){
            // Checks if the current tag is still within the space
            return tagToCheck.IsWithinCoordinates(this.Boundary);
        }

        /// <summary>
        /// Sets the tag occupying the parking space.
        /// </summary>
        /// <param name="newTag">The new tag to set.</param>
        public void SetTag(Tag newTag){
            this.tagOccupyingSpace = newTag;
        }

        /// <summary>
        /// Prints a report about the parking space, indicating whether it is occupied or empty.
        /// </summary>
        public void PrintReport(){
            if (tagOccupyingSpace != null){
                Console.WriteLine($"Parking Space {this.ID} is occupied by Tag {tagOccupyingSpace.DeviceID}");
            }
            else{
                Console.WriteLine($"Parking Space {this.ID} is empty");
            }
        }

        /// <summary>
        /// Checks if the tag that was previously detected in the space is still in the space.
        /// </summary>
        /// <returns>False if the space is empty or if the tag is no longer in the space, otherwise true.</returns>
        public bool TagStillInSpace(){
            if (tagOccupyingSpace == null){
                // If there was no tag previously in the space, then it is still empty
                return false;
            }
            else if (!ContainsTag(tagOccupyingSpace)){
                // If the tag is no longer in the space, then the space is empty
                this.tagOccupyingSpace = null;
                return false;
            }
            else{
                //Console.WriteLine($"Tag {tagOccupyingSpace.DeviceID} is still in Parking Space {this.ID}");
                return true;
            }
        }

        /// <summary>
        /// Updates the parking space with the specified tag.
        /// </summary>
        /// <param name="tag">The tag to update the parking space with.</param>
        /// <returns>False if the tag is not in the space or if the space is already occupied, otherwise true.</returns>
        public bool Update(Tag tag){
            if(TagStillInSpace()){
                return false; 
            }
            else if(tag.IsWithinCoordinates(this.Boundary)){
                Console.WriteLine($"Tag {tag.DeviceID} is in Parking Space {this.ID} in Parkinglot {this.ParkinglotName}");
                this.tagOccupyingSpace = tag;
                return true;
            }
            else{
                return false;
            }
            
        }

        /// <summary>
        /// Writes information about the parking space to the console, including whether there is a tag in the space and its details.
        /// </summary>
        public void toString()
        {
            if (tagOccupyingSpace != null)
            {
                Console.WriteLine("________________________________________________________");
                Console.WriteLine($"Parking Space {this.ID} is occupied by Tag {tagOccupyingSpace.DeviceID}");
                Console.WriteLine($"Tag ID: {tagOccupyingSpace.DeviceID}");
                Console.WriteLine($"License Plate Number: {tagOccupyingSpace.LicensePlateNumber}");
                Console.WriteLine($"Status: {tagOccupyingSpace.GetStatus()}\n");
                Console.WriteLine(this.Boundary.ToString());
                Console.WriteLine("________________________________________________________");

            }
            else
            {
                Console.WriteLine("________________________________________________________");
                Console.WriteLine($"Parking Space {this.ID} is empty");
                Console.WriteLine(this.Boundary.ToString());
                Console.WriteLine("________________________________________________________");
            }
        }
    }   
}