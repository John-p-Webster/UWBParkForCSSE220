namespace UWBPark.src.ServerSide
{
    /// <summary>
    /// Represents the data structure for server-side data.
    /// </summary>
    public struct ServerData
    {
        public int AnchorID1;
        public int AnchorID2;
        public int TagID;
        // Distance from anchor 1 to tag
        public double Distance1;
        // Distance from anchor 2 to tag
        public double Distance2;

        public override string ToString()
        {
            return $"AnchorID1: {AnchorID1}, AnchorID2: {AnchorID2}, TagID: {TagID}, Distance1: {Distance1}, Distance2: {Distance2}";
        }
    }
}
