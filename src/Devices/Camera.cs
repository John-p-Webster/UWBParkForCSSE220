using UWBPark.src.ServerSide;

namespace UWBPark.src.Devices
{   
    public class Camera(int DeviceID, Coordinate DeviceLocation, int DeviceStatus) : Device(DeviceID, DeviceLocation, "Camera", DeviceStatus)
    {
        public override void ReceiveData(ServerData data)
        {
            //TODO implement
            throw new System.NotImplementedException();
        }
    }
}