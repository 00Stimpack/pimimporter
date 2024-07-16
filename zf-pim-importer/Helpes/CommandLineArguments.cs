using System;

namespace ZFPimImporter.Helpes
{
    
    [Serializable]

    public class CommandLineArguments
    {

        public CommandLineHelper.Commands Command { get; set; } = CommandLineHelper.Commands.Nothing;
        
        public string Path { get; set; } = "";

        public DateTime Created  { get; set; } = DateTime.Now;
        
        
    }
}