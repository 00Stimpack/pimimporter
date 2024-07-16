using System.Diagnostics;
using System.IO;

namespace ZFPimImporter.IO
{
    public class Processor
    {
        public static long GetStreamLength(Stream stream)
        {
            long originalPosition = 0;
            long totalBytesRead = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, 0, 4096)) > 0)
                {
                    totalBytesRead += bytesRead;
                }

            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }

            return totalBytesRead;
        }
        
        public static Process OpenApplications(string fileName, string workingPath,string arguments = "\"-username\" \"somecrazydude\"",bool createWindow = true,System.Action<string> output = null, System.Action<string> error = null)
        {
            
            Process proc = new Process();
            if (error != null)
                proc.ErrorDataReceived += (sender, args) => {error.Invoke(args.Data); };
            if (output != null)
                proc.OutputDataReceived += (sender, args) => {output.Invoke(args.Data); };
           
            if(!createWindow)
                proc.StartInfo.CreateNoWindow = false;
            
            proc.StartInfo.WorkingDirectory =workingPath;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.FileName = fileName;
            
            proc.Start();
            return proc;
        }
    }
}