using System.Security.Cryptography.X509Certificates;

namespace UWBPark.src.ServerSide
{
    /// <summary>
    /// Represents a 2D coordinate with X and Y values.
    /// </summary>
    public struct Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Coordinate coordinate2)
        {
            double deltaX = coordinate2.X - X;
            double deltaY = coordinate2.Y - Y;
            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            return distance;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

    }
    
}