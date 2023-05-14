using AirMaxRemoteTest.Data;
using AirMaxRemoteTest.Helpers;
using System.Text;



namespace AirMaxRemoteTest.Tests
{
    [TestClass]
    public class VehicleDataDecoderTests
    {
        [TestMethod]
        public void Decode_ValidData_ReturnsVehicleData()
        {
            // Arrange
            string hexData =
                "04485100000123456789000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F202122232425262728292A2B2C2D2E2F303132333435363738393A3B3C3D3E3F";
            byte[] dataBytes = HexExtensions.HexToBytes(hexData);
            string dataString = Encoding.ASCII.GetString(dataBytes); 
            var decoder = new VehicleDataDecoder();

            // Act
            VehicleData result = decoder.Decode(dataString);


            VehicleData vehicleData = decoder.Decode(hexData);

            Assert.IsNotNull(vehicleData);
            Assert.AreEqual(255, vehicleData.MessageType);
            Assert.AreEqual("12/04/1993 18:25:26", vehicleData.CurrentTime.ToString("dd/MM/yyyy HH:mm:ss"));
            Assert.AreEqual("999991", vehicleData.DeviceID);
            Assert.AreEqual(101, vehicleData.CurrentSpeedMPH);
            Assert.AreEqual("5295.7", vehicleData.Odometer.ToString("F1"));
            Assert.AreEqual(true, vehicleData.TripStart);
            Assert.AreEqual(false, vehicleData.TripEnd);
            Assert.AreEqual(52.496524810791016, vehicleData.Latitude);
            Assert.AreEqual(7156.39453125, vehicleData.Longitude);
        }
    }
}