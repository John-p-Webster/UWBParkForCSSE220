using System.Collections;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UWBPark.src;
using UWBPark.src.ServerSide;

void Main() {
    ParkingManager parkingManager = new ParkingManager();
    
    // Create a new thread for the Server instance
    /// <summary>
    /// Creates a new thread and starts a server instance.
    /// </summary>
    Thread serverThread = new Thread(() => {
        Server server = new Server();
        server.StartServer();
    });
    // Start the thread
    serverThread.Start();

    parkingManager.HandleAddDevice(new Coordinate(0, 0), "Anchor", 1);
    parkingManager.HandleAddDevice(new Coordinate(100, 0), "Anchor", 1);
    parkingManager.HandleAddDevice(new Coordinate(10, 0), "Tag", 1, "ABC123");
    parkingManager.HandleAddDevice(new Coordinate(10, 0), "Tag", 1, "ABC345");
    parkingManager.HandleAddDevice(new Coordinate(10, 0), "Tag", 1, "ABC567");


    parkingManager.HandleAddParkingLot("Parking Lot 1", new FourPointBoundary(new Coordinate(0, 0), new Coordinate(100, 0), new Coordinate(100, 100), new Coordinate(0, 100)), 10);

    parkingManager.HandlePrintAllReports();

}

Main();