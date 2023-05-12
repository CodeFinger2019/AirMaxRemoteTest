using AirMaxRemoteTest.Helpers;
using System.Text;

namespace AirMaxRemoteTest.Data
{
    public class VehicleData
    {
        public int MessageType { get; set; }
        public DateTime CurrentTime { get; set; }
        public string DeviceID { get; set; }
        public float CurrentSpeedMPH { get; set; }
        public float Odometer { get; set; }
        public uint TripID { get; set; }
        public bool TripStart { get; set; }
        public bool TripEnd { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public void ParseData(string hexData)
        {
            if (string.IsNullOrWhiteSpace(hexData))
            {
                throw new ArgumentException("Hex data cannot be null or empty.", nameof(hexData));
            }

            byte[] bytes = hexData.HexToBytes();

            if (bytes.Length < 32)
            {
                throw new ArgumentException("Invalid hex data. Data must be at least 32 bytes in length.", nameof(hexData));
            }

            MessageType = bytes[0];
            CurrentTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(bytes, 2));
            DeviceID = Encoding.ASCII.GetString(bytes, 6, 6).Trim();
            CurrentSpeedMPH = BitConverter.ToUInt16(bytes, 12) / 10.0f;
            Odometer = BitConverter.ToUInt32(bytes, 14) / 10.0f;
            TripID = BitConverter.ToUInt32(bytes, 18);
            TripStart = bytes[22] == 0x01;
            TripEnd = bytes[23] == 0x01;
            Latitude = BitConverter.ToInt32(bytes, 24) / 600000.0f;
            Longitude = BitConverter.ToInt32(bytes, 28) / 600000.0f;
        }
    }
}
