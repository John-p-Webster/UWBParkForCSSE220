using System.ComponentModel;
using UWBPark.src.Devices;

namespace UWBPark.src.ServerSide
{
    /// <summary>
    /// Represents a user profile in the UWBPark system.
    /// </summary>
    public class Profile(string Name, int ID, string LicensePlateNumber)
    {
        /// <summary>
        /// Gets or sets the name of the profile.
        /// </summary>
        public string Name {get; private set;}= Name;

        /// <summary>
        /// Gets or sets the ID of the profile.
        /// </summary>
        public int ID {get; private set;}= ID;

        private bool isRegistered;
        private int tagID;

        /// <summary>
        /// Gets or sets the license plate number associated with the profile.
        /// </summary>
        public string ?LicensePlateNumber {get; private set;} = LicensePlateNumber;

        /// <summary>
        /// Gets or sets the tag associated with the profile.
        /// </summary>
        public Tag ?tag {get; private set;} = null;

        /// <summary>
        /// Assigns a tag to the profile.
        /// </summary>
        /// <param name="tag">The tag to assign.</param>
        public void AssignTag(Tag tag){
            this.tag = tag;
            Console.WriteLine($"Tag ID {tag.DeviceID} assigned to account ID {this.ID}");
        }
    }
}
