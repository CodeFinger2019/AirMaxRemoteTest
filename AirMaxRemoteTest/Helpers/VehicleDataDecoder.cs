using System.Text;
using AirMaxRemoteTest.Data;

namespace AirMaxRemoteTest.Helpers;

public class VehicleDataDecoder
{
    
    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }

    public VehicleData Decode(string hexData)
    {
        if (string.IsNullOrEmpty(hexData))
        {
            throw new ArgumentException("Data cannot be null or empty.", nameof(hexData));
        }

        byte[] dataBytes = hexData.HexToBytes();

        if (dataBytes.Length < 32)
        {
            throw new ArgumentException("Invalid hex data format.", nameof(hexData));
        }

        VehicleData vehicleData = new VehicleData();

        if (vehicleData == null)
        {
            throw new ArgumentException("Invalid hex data format.", nameof(vehicleData));
        }


        // Decode header fields
        vehicleData.MessageType = dataBytes[0];
        vehicleData.CurrentTime = UnixTimeStampToDateTime(BitConverter.ToUInt32(dataBytes, 2));
        vehicleData.DeviceID = Encoding.ASCII.GetString(dataBytes, 6, 6);

        // Decode data fields
        vehicleData.CurrentSpeedMPH = BitConverter.ToUInt16(dataBytes, 12);
        vehicleData.Odometer = BitConverter.ToUInt32(dataBytes, 14) / 10.0f;
        vehicleData.TripID = BitConverter.ToUInt32(dataBytes, 18);
        vehicleData.TripStart = dataBytes[22] == 1;
        vehicleData.TripEnd = dataBytes[23] == 1;
        vehicleData.Latitude = BitConverter.ToUInt32(dataBytes, 24) / 600000.0f;
        vehicleData.Longitude = BitConverter.ToUInt32(dataBytes, 28) / 600000.0f;

        return vehicleData;
    }
}