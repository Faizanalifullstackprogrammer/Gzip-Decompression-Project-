// See https://aka.ms/new-console-template for more information
using System.IO.Compression;
using System.Text;
using Brotli;

namespace TestDecompression
{
    internal static class StringCompressor
    {
        const int BUFFER_SIZE = 0x4000;
        public static void Main(string[] args)
        {
            string originalText = "Testing for JSON Bulk Data";
            string encodedString = "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";
            string str1 = CompressString(originalText);
            Console.WriteLine(originalText);
            byte[] bytesencode = Encoding.UTF8.GetBytes(encodedString);
            byte[] gZipBuffer = Convert.FromBase64String(encodedString);

            string decompressedString = DecompressToString(gZipBuffer);


            Console.WriteLine(decompressedString);
        }

        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0x4000, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static byte[] DecompressGzip(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var decompressStream = new System.IO.Compression.BrotliStream(memoryStream, CompressionMode.Decompress))
                    {
                        decompressStream.CopyTo(outputStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }

        public static byte[] Decompress(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {

                using (var outputStream = new MemoryStream())
                {
                    using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        decompressStream.CopyTo(outputStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }

        public static byte[] Decompress1(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {

                using (var outputStream = new MemoryStream())
                {
                    using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        decompressStream.CopyTo(outputStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }

        public static void Compress(Stream inputStream, Stream outputStream)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            using var gzip = new GZipStream(outputStream, CompressionMode.Compress);
            int count;
            while ((count = inputStream.Read(buffer, 0, BUFFER_SIZE)) > 0)
            {
                gzip.Write(buffer, 0, count);
            }
        }

        public static void Decompress(Stream inputStream, Stream outputStream)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            using var gzip = new GZipStream(inputStream, CompressionMode.Decompress);
            int count;
            while ((count = gzip.Read(buffer, 0, BUFFER_SIZE)) > 0)
            {
                outputStream.Write(buffer, 0, count);
            }
        }

        public static byte[] Compress(byte[] input)
        {
            using var inputStream = new MemoryStream(input);
            using var outputStream = new MemoryStream();
            Compress(inputStream, outputStream);
            return outputStream.ToArray();
        }

        public static byte[] Decompress2(byte[] input)
        {
            using var inputStream = new MemoryStream(input);
            using var outputStream = new MemoryStream();
            Decompress(inputStream, outputStream);
            return outputStream.ToArray();
        }

        public static byte[] CompressString2(string input)
            => Compress(Encoding.UTF8.GetBytes(input));
        // Below method Worked and Tested!
        public static string DecompressToString(byte[] input)
            => Encoding.UTF8.GetString(Decompress2(input));
    }
}