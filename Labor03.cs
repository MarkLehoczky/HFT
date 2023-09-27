using System;
using System.Linq;
using System.Xml.Linq;

namespace Program
{
    class Program
    {
        public static void Main()
        {
            XDocument doc = XDocument.Load("http://users.nik.uni-obuda.hu/prog3/_data/war_of_westeros.xml");

            var houseCount = doc.Descendants("house").Select(t => t.Value).Distinct().Count();

            var ambushes = doc.Descendants("type").Where(t => t.Value == "ambush").Select(u => u.Parent.Element("name").Value);

            var defenderVictoryWithMajorPrisoner = doc.Descendants("battle").Where(t => t.Element("outcome").Value == "defender")
                .Intersect(doc.Descendants("battle").Where(t => int.Parse(t.Element("majorcapture").Value) > 0))
                .Count();

            var starkVictory = doc.Descendants("battle").Where(t => t.Element(t.Element("outcome").Value).Elements("house")
            .Any(v => v.Value == "Stark")).Count();

            var multipleHouseBattles = doc.Descendants("name").Where(t => t.Parent.Descendants("house")
            .Count() > 2).Select(u => u.Value);

            var threeCommonRegion = doc.Descendants("region").OrderByDescending(t => doc.Descendants("region")
            .Count(u => u.Value == t.Value)).Select(v => v.Value).Distinct().Take(3);

            var mostCommonRegion = doc.Descendants("region").OrderByDescending(t => doc.Descendants("region")
            .Count(u => u.Value == t.Value)).First().Value;

            var multipleHouseBattlesInThreeCommonRegion = doc.Descendants("name").Where(t => t.Parent.Descendants("house").Count() > 2)
            .Where(u => threeCommonRegion.Contains(u.Parent.Element("region").Value)).Select(w => w.Value);               

            var housesVictoryOrder = doc.Descendants("house").OrderByDescending(t => doc.Descendants("battle")
            .Count( u => u.Element(u.Element("outcome").Value).Elements("house").Any(v => v.Value == t.Value)))
            .Select(v => v.Value).Distinct();

            var largestArmyBattle = doc.Descendants("name").OrderByDescending(t => t.Descendants("size").Max(u => u.Value)).First().Value;

            var mostAttackingCommanders = doc.Descendants("commander").OrderByDescending(t => doc.Descendants("attacker")
            .Count(u => u.Value == t.Value)).Select(v => v.Value).Distinct().Take(3);



            Console.WriteLine($"1. `{houseCount}` number of houses had battle");
            Console.WriteLine();

            Console.WriteLine("2. Ambushes:");
            foreach (var item in ambushes)
                Console.WriteLine($" > `{item}`");
            Console.WriteLine();

            Console.WriteLine($"3. There were `{defenderVictoryWithMajorPrisoner}` number of battle where the defenders won with a major prisoner");
            Console.WriteLine();

            Console.WriteLine($"4. The Stark house won `{starkVictory}` number of battles");
            Console.WriteLine();

            Console.WriteLine("5. Battles with more than 2 houses:");
            foreach (var item in multipleHouseBattles)
                Console.WriteLine($" > `{item}`");
            Console.WriteLine();

            Console.WriteLine("6. The three most common regions:");
            foreach (var item in threeCommonRegion)
                Console.WriteLine($" > `{item}`");
            Console.WriteLine();

            Console.WriteLine($"7. The most common region is `{mostCommonRegion}`");
            Console.WriteLine();

            Console.WriteLine("8. Battles with more than 2 houses in the three most common region:");
            foreach (var item in multipleHouseBattlesInThreeCommonRegion)
                Console.WriteLine($" > `{item}`");
            Console.WriteLine();

            Console.WriteLine("9. Houses in order of number of victory:");
            foreach (var item in housesVictoryOrder)
                Console.WriteLine($" > `{item}`");
            Console.WriteLine();

            Console.WriteLine($"10. The `{largestArmyBattle}` battle had the largest army");
            Console.WriteLine();

            Console.WriteLine("11. The three most attacking commanders:");
            foreach (var item in mostAttackingCommanders)
                Console.WriteLine($" > `{item}`");
            Console.WriteLine();
        }
    }
}
