using System.Diagnostics;
using System.Linq;

namespace OrdelHelp
{
    internal class Program
    {
        private static char[] vowels = "euioay".ToCharArray();

        static void Main(string[] args)
        {
            Execute();


            //var content = ContentParser.GetContent();
            //var currentWord = (word: "", count: 0);

            //var counted = content
            //    .Where(word => word.Length == 7)
            //    .Where(word => new HashSet<char>(word).Count == word.Length)
            //    .Select(item =>
            //{
            //    var count = item.Count(c => vowels.Contains(c));
            //    return currentWord = (item, count);
            //}).OrderByDescending (a => a.count);

            //foreach (var item in counted.Take(50))
            //{
            //    Console.WriteLine(item);
            //}

        }

        private static void Execute()
        {


            var input = "__le___ ei..g.l.ig.a.. productnsh";


            var content = ContentParser.GetContent();
            var analyzer = new Analyser(content);

            var candidates = analyzer.GetCandidates(input);

            Console.WriteLine("Primary candidates");
            foreach (var item in candidates.Take(10))
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"Words found: {candidates.Length} in {analyzer.Count} words");


            //content = File.ReadAllText("./words_secondary.txt");
            //analyzer = new Analyser(content);
            //candidates = analyzer.GetCandidates(input);

            //Console.WriteLine();
            //Console.WriteLine("Secondary candidates");
            //foreach (var item in candidates)
            //{
            //    Console.WriteLine(item);
            //}

            //Console.WriteLine($"Words found: {candidates.Length} in {analyzer.Count} words");
        }
    }
}
