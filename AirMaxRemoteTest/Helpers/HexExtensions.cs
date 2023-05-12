using System.Text.RegularExpressions;

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
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}