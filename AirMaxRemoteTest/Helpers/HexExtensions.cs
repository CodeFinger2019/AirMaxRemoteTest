using System.Globalization;
using System.Text.RegularExpressions;
using Serilog;


namespace AirMaxRemoteTest.Helpers
{
    public static class HexExtensions
    {

     
        public static byte[] HexToBytes(this string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException(nameof(hex));
            }

            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid hex string. Length must be a multiple of 2.", nameof(hex));
            }

            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length; i += 2)
            {
                if (!byte.TryParse(hex.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture,
                        out byte b))
                {
                    throw new ArgumentException("Invalid hex string. Contains non-hex characters.", nameof(hex));
                }

                bytes[i / 2] = b;
            }

            return bytes;
        }
    }
}