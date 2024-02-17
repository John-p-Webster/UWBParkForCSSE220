    
namespace UWBPark.src.ServerSide{    
    /// <summary>
    /// Represents a parking space or parkinglot defined by four points.
    /// </summary>
    public struct FourPointBoundary
    {
        public Coordinate Vertex1 { get; set; }
        public Coordinate Vertex2 { get; set; }
        public Coordinate Vertex3 { get; set; }
        public Coordinate Vertex4 { get; set; }
        public Coordinate[] Vertexes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FourPointBoundary"/> struct with the specified points.
        /// </summary>
        /// <param name="vertex1">The first point of the parking space.</param>
        /// <param name="vertex2">The second point of the parking space.</param>
        /// <param name="vertex3">The third point of the parking space.</param>
        /// <param name="vertex4">The fourth point of the parking space.</param>
        public FourPointBoundary(Coordinate vertex1, Coordinate vertex2, Coordinate vertex3, Coordinate vertex4)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Vertex3 = vertex3;
            Vertex4 = vertex4;
            Vertexes = new Coordinate[] { Vertex1, Vertex2, Vertex3, Vertex4 };
        }
        
        /// <summary>
        /// Increases the X coordinate of all vertices by the specified amount.
        /// </summary>
        /// <param name="amount">The amount by which to increase the X coordinate.</param>
        public void increaseX(double amount)
        {
            var tempVertex1 = Vertex1;
            tempVertex1.X += amount;
            Vertex1 = tempVertex1;

            var tempVertex2 = Vertex2;
            tempVertex2.X += amount;
            Vertex2 = tempVertex2;

            var tempVertex3 = Vertex3;
            tempVertex3.X += amount;
            Vertex3 = tempVertex3;

            var tempVertex4 = Vertex4;
            tempVertex4.X += amount;
            Vertex4 = tempVertex4;
        }
        
        public override string ToString()
        {
            return $"Vertex1: {Vertex1}, Vertex2: {Vertex2}, Vertex3: {Vertex3}, Vertex4: {Vertex4}";
        }
    }
}