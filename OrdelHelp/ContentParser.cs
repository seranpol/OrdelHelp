using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrdelHelp
{
    internal class ContentParser
    {
        public static string[] GetContent()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "OrdelHelp.unigram_freq.csv";

            using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
            using StreamReader reader = new StreamReader(stream);
            
            var raw = reader.ReadToEnd();
            var lines = raw.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var content = lines.Select(l => l.Substring(0, l.IndexOf(','))).Skip(1).ToArray();
            return content;

            //var path = @"./unigram_freq.csv";
            //var raw = File.ReadAllText(path);

        }
    }
}
