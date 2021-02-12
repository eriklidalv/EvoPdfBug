using System;
using System.IO;
using EvoWordToPdf;

namespace EvoPdfBug
{
    class Class1
    {
        public byte[] GetByteArrayAsync()
        {
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string pathSource = $"{dir}\\..\\..\\..\\error.pdf";

            // https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.read?view=net-5.0
            using (var fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read))
            {
                // Read the source file into a byte array.
                byte[] bytes = new byte[fsSource.Length];
                int numBytesToRead = (int)fsSource.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                
                return bytes;
            }
        }

        public Document CreateDocument(byte[] byteArray, bool forcePortrait = true)
        {
            using (var stream = new MemoryStream(byteArray))
            {
                try
                {
                    var document = new Document(stream); // <- ERROR

                    if (forcePortrait)
                    {
                        for (var i = 0; i < document.Pages.Count; i++)
                        {
                            var pdfPage = document.Pages[i];
                            if (pdfPage.Orientation == PdfPageOrientation.Landscape && pdfPage.RotationAngle == 0)
                            {
                                pdfPage.Orientation = PdfPageOrientation.Portrait;
                                pdfPage.RotationAngle = RotationAngle.Rotate_90;
                            }
                        }
                    }

                    return document;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e}");
                    return null;
                }
            }
        }
    }
}
