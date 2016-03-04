/// <summary>
/// See http://stackoverflow.com/questions/472906/converting-a-string-to-byte-array-without-using-an-encoding-byte-by-byte
/// </summary>
namespace EmployeeRecordSystem.Services.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static byte[] GetBytes(this string content)
        {
            byte[] bytes = new byte[content.Length * sizeof(char)];
            Buffer.BlockCopy(content.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(this byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}