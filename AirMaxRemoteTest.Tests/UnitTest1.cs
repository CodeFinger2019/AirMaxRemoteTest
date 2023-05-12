using AirMaxRemoteTest.Data;
using AirMaxRemoteTest.Helpers;
using System.Text;

namespace AirMaxRemoteTest.Tests
{
    [TestClass]
    public class VehicleDataDecoderTests
    {
        [TestMethod]
        public void TestDatData()
        {
            // Arrange
            var decoder = new VehicleDataDecoder();

            // Act
            VehicleData vehicleData = decoder.Decode("FF0006A6C92B3939393939316500DDCE0000D91100000100BC9EE00116C0EEFF");

            // Assert
            Assert.IsNotNull(vehicleData);
            Assert.AreEqual(255, vehicleData.MessageType);
            Assert.AreEqual("12/04/1993 18:25:26", vehicleData.CurrentTime.ToString());
            Assert.AreEqual("999991", vehicleData.DeviceID);
            Assert.AreEqual(101, vehicleData.CurrentSpeedMPH);
            Assert.AreEqual("5295.7", vehicleData.Odometer.ToString());
            Assert.AreEqual(4569,  Convert.ToUInt16( vehicleData.TripID));
            Assert.AreEqual(true, vehicleData.TripStart);
            Assert.AreEqual(false, vehicleData.TripEnd);
            Assert.AreEqual(52.496524810791016, vehicleData.Latitude);
            Assert.AreEqual(7156.39453125, vehicleData.Longitude);
        }
    }
}