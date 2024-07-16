
using System;
using System.Diagnostics;


using System.IO;
using System.Linq;


namespace ZFPimImporter.IO
{
    
    public class PowerPointReader
    {
        
        public static void RunShell(string fileName,string pathToLibs,string pptxPath,string outputPath)
        {
            
            /*using (var converter = new GroupDocs.Conversion.Converter("input.pdf"))
            {
                GroupDocs.Conversion.Contracts.SavePageStream getPageStream = page => new FileStream(string.Format("converted-page-{0}.png", page), FileMode.Create);

                var options = new ImageConvertOptions { Format = GroupDocs.Conversion.FileTypes.ImageFileType.png };
                converter.Convert(getPageStream, options);
            }*/

            var pdfPath = Path.Combine(new DirectoryInfo(pptxPath).Parent.FullName, pptxPath.Replace(".pptx", "").Replace(".ppt", "").Replace(".ppx", "") + ".pdf");

            string[] cmds = new[]
            {
                "/c",
                Path.Combine(pathToLibs, "libs", "libreoffice", "program", "soffice"),
                "--headless",
                "--convert-to",
                "pdf",
                pptxPath,
                "&",
                Path.Combine(pathToLibs, "gswin64c.exe"),
                "-sDEVICE=pngalpha",
                "-o",
                Path.Combine(outputPath, "imagesName-%02d.png"),
                "-r144",
                pdfPath
            };


            string combine = string.Join(" ", cmds);
            
            Console.WriteLine("COMBINE:"+combine);
            
            
            return;
            try
            {
                //To convert a figure to an image file: and to render the same image at 500dpi
                String ars = "-dNOPAUSE -sDEVICE=png -r500 -o" + "output" + "%d.jpg " + "input";
                Process proc = new Process();
                proc.StartInfo.FileName = fileName;
                proc.StartInfo.Arguments = ars;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
               // proc.StartInfo.FileName = Path.GetFileName(ghostScriptPath);
                proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(fileName);
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void Open(string path)
        {
          //  if (!File.Exists(path))
          //      throw new FileNotFoundException("Could not find input file.", path);
          //  this.Open(path, GhostscriptVersionInfo.GetLastInstalledVersion(GhostscriptLicense.AFPL, GhostscriptLicense.GPL), false);
        }
        
        /*public static void ConvertPdfToImages(string pdfFile, string imagePath)
        {
            Console.WriteLine($"pdfFile:{pdfFile} imagePath:{imagePath}" );

            using GhostscriptRasterizer rasterizer = new GhostscriptRasterizer();
            Console.WriteLine($"after rasterizer" );

          //  rasterizer.Open(pdfFile);
            rasterizer.Open(pdfFile,
                GhostscriptVersionInfo.GetLastInstalledVersion(GhostscriptLicense.AFPL, GhostscriptLicense.GPL), false);
            Console.WriteLine($"pdfFile:{pdfFile} imagePath:{imagePath}" );
            //var images = rasterizer.GetPage(100, 2);
            for (int pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
            {
                Console.WriteLine($"pageNumber:{pageNumber} path:{imagePath + "\\Page" + pageNumber + ".jpg"}" );

                var image = rasterizer.GetPage(100, pageNumber);
                //var image = rasterizer.GetPage(100, 100, pageNumber);
                image.Save(imagePath + "\\Page" + pageNumber + ".jpg", ImageFormat.Jpeg);
            }
        }*/
    }
}
