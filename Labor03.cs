using System;
using System.Linq;
using System.Xml.Linq;

namespace Program
{
    class Labor03
    {
        public static void Run()
        {
            XDocument doc = XDocument.Load("http://users.nik.uni-obuda.hu/prog3/_data/war_of_westeros.xml");

            var houses = doc.Descendants("house").Select(t => t.Value).Distinct().Count();

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
                .Count(u => u.Element(u.Element("outcome").Value).Elements("house").Any(v => v.Value == t.Value)))
                .Select(v => v.Value).Distinct();

            var largestArmyBattle = doc.Descendants("name").OrderByDescending(t => t.Descendants("size").Max(u => u.Value)).First().Value;

            var mostAttackingCommanders = doc.Descendants("commander").OrderByDescending(t => doc.Descendants("attacker")
                .Count(u => u.Value == t.Value)).Select(v => v.Value).Distinct().Take(3);



            houses.ConsoleWriteLine("1. {$} number of houses had battle");

            ambushes.ConsoleWriteLine("2. Ambushes:");

            defenderVictoryWithMajorPrisoner.ConsoleWriteLine("3. There were {$} number of battle where the defenders won with a major prisoner");

            starkVictory.ConsoleWriteLine("4. The Stark house won {$} number of battles");

            multipleHouseBattles.ConsoleWriteLine("5. Battles with more than 2 houses:");

            threeCommonRegion.ConsoleWriteLine("6. The three most common regions:");

            mostCommonRegion.ConsoleWriteLine("7. The most common region is {$}");

            multipleHouseBattlesInThreeCommonRegion.ConsoleWriteLine("8. Battles with more than 2 houses in the three most common region:");

            housesVictoryOrder.ConsoleWriteLine("9. Houses in order of number of victory:");

            largestArmyBattle.ConsoleWriteLine("10. The {$} battle had the largest army");

            mostAttackingCommanders.ConsoleWriteLine("11. The three most attacking commanders:");
        }
    }
}
