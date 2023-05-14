using System;
using System.Text;
using AirMaxRemoteTest.Data;
using Serilog;
using ILogger = Serilog.ILogger;
using System.Text.Json;

namespace AirMaxRemoteTest.Helpers
{
    public class VehicleDataDecoder
    {
        private readonly ILogger _log;

        public VehicleDataDecoder(ILogger log = null)
        {
            _log = log;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public VehicleData Decode(string hexData, ILogger log = null)
        {
            log ??= _log;

            if (string.IsNullOrEmpty(hexData))
            {
                log?.Error("Data cannot be null or empty.", nameof(hexData));
                throw new ArgumentException("Data cannot be null or empty.", nameof(hexData));
            }

            try
            {
                byte[] dataBytes = hexData.HexToBytes();

                if (dataBytes.Length < 32)
                {
                    log?.Error("invalid hex data", hexData);
                    throw new ArgumentException("Invalid hex data format  - length.", nameof(hexData));
                }

                VehicleData vehicleData = new VehicleData();

                if (vehicleData == null)
                {
                    log?.Error("Invalid hex data format - null.", nameof(vehicleData));
                    throw new ArgumentException("Invalid hex data format - null.", nameof(vehicleData));
                }

                // Grab JSON
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonVehicleData = JsonSerializer.Serialize(vehicleData, options);

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

                log?.Information(" \r\n");
                log?.Information("=========== beginning developer test message ======================", jsonVehicleData);
                log?.Information("=========== Header Start =======================================", jsonVehicleData);
                log?.Information("MESSAGE TYPE: " + vehicleData.MessageType, jsonVehicleData);
                log?.Information("EVENT TIME: " + vehicleData.CurrentTime, jsonVehicleData);
                log?.Information("DEVICE ID: " + vehicleData.DeviceID, jsonVehicleData);
                log?.Information("=========== Header End =======================================", jsonVehicleData);
                log?.Information("=========== Message data start =================================");
                log?.Information("CURRENT SPEED: " + vehicleData.CurrentSpeedMPH, jsonVehicleData);
                log?.Information("ODOMETER: " + vehicleData.Odometer, jsonVehicleData);
                log?.Information("TRIP ID: " + vehicleData.TripID, jsonVehicleData);
                log?.Information("TRIP START: " + vehicleData.TripStart, jsonVehicleData);

                _log.Information("TRIP END: " + vehicleData.TripEnd, jsonVehicleData);
                _log.Information("TRIP LATITUDE: " + vehicleData.Latitude, jsonVehicleData);
                _log.Information("TRIP LONGITUDE: " + vehicleData.Longitude, jsonVehicleData);
                _log.Information("=========== Message data end =================================", jsonVehicleData);
                _log.Information("=========== End of Developer Test Message =======================", jsonVehicleData);
                return vehicleData;
            }
            catch (Exception ex)
            {
                _log.Error("invalid hex data", ex.Message);
                throw new Exception("An error occurred while decoding the data.", ex);
            }
        }
    }
}