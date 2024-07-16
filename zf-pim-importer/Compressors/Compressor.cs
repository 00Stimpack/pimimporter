using System;
using System.IO;
using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Writers.Tar;

public class FileCompression
{
    public static void DecompressFile(string filePath, string destinationPath)
    {
        var extension = Path.GetExtension(filePath);

        switch (extension)
        {
            case ".zip":
                using (var archive = ZipArchive.Open(filePath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            entry.WriteToDirectory(destinationPath, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }

                break;
            case ".tar":
            case ".tar.gz":
            case ".tgz":
                using (var archive = TarArchive.Open(filePath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            entry.WriteToDirectory(destinationPath, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }

                break;
            case ".gz":
                using (var stream = new FileStream(filePath, FileMode.Open))
                /*
                using (var decompressor = new GZipDecoder(stream))
                {
                    var outputStream = new FileStream(destinationPath, FileMode.Create);
                    decompressor.Decompress(stream, outputStream);
                }
                */

                break;
            case ".7z":
                using (var archive = SevenZipArchive.Open(filePath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            entry.WriteToDirectory(destinationPath, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }

                break;
            case ".asar":
                // Assuming a third-party library such as NodeASAR
                //NodeASAR.Decode(filePath, destinationPath);
                break;
            default:
                Console.WriteLine($"No decompressor found for {filePath}");
                break;
        }
    }
}

/*
public static void CompressFile(string filePath, string destinationPath)
{
    var extension = Path.GetExtension(destinationPath);

    switch (extension)
    {
        case ".zip":
            using (var archive = ZipArchive.Create())
            {
                archive.AddAllFromDirectory(filePath);
                archive.SaveTo(destinationPath, CompressionType.Deflate);
            }
            break;
        case ".tar":
            using (var archive = WriterFactory.Open(new FileStream(destinationPath, FileMode.Create), ArchiveType.Tar))
            {
                archive.WriteAll(filePath, "*", SearchOption.AllDirectories);
            }
            break;
        case ".gz":
            using (var stream = new FileStream(destinationPath, FileMode.Create))
            using (var compressor = new GZipEncoder())
            using (var writer = new StreamWriter(compressor))
            using (var reader = new StreamReader(filePath))
            {
                compressor.Write(stream, reader.BaseStream);
                */

