using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Program
{
    class Extra01
    {
        public static void Run()
        {
            #region Creating people


            List<Person> people = new(){
                new Person("Nagy Ádám", 23, "Budapest", "06-20/632-1254", "nagy.adam@email.com", "Festő"),
                new Person("Kiss Péter", 32, "Budapest", "06-20/589-8210", "peter.kiss@email.com", "Menedzser"),
                new Person("Kovács Anna", 26, "Debrecen", "06-20/315-1123", "anna.97@email.com", "Programozó"),
                new Person("Szemere Laura", 19, "Budapest", "06-20/523-1875", "laura.tanar@egyetem.gov", "Tanár"),
                new Person("Nagy Tamás", 27, "Hollókő", "06-20/445-1962", "nagy.tomi@pekseg.hu", "Pék")
            };


            #endregion

            #region Saving people in XML format


            XDocument xml = new();
            XElement root = new("People");
            xml.Add(root);
            people.ForEach(person => { root.Add(person.ToXML()); });
            xml.Save("people.xml");


            #endregion

            #region Saving people in Json format


            File.WriteAllText("people.json", JsonConvert.SerializeObject(people, Formatting.Indented));


            #endregion

            #region Catalog LINQ actions


            var doc = XDocument.Load("catalog.xml");
            

            var threeMostExpensiveName = doc.Descendants("book").OrderByDescending(t => double.Parse(t.Element("price").Value)).Select(u => u.Element("title").Value).Take(3);
            
            var fantasyCount = doc.Descendants("genre").Where(t => t.Value == "Fantasy").Count();
            
            var mostRecentBookAuthor = doc.Descendants("book").OrderByDescending(t => DateTime.Parse(t.Element("publish_date").Value)).Select(u => u.Element("author").Value).First();
            
            var averagePrice = doc.Descendants("price").Select(t => double.Parse(t.Value)).Average();
            
            var isMikeGalosWriter = doc.Descendants("author").Select(t => t.Value).Contains("Mike Galos");
            
            var isMikeGalosWriterCorrectName = doc.Descendants("author").Select(t => t.Value).Contains("Galos, Mike");
            
            var cheapestBookName = doc.Descendants("book").OrderBy(t => double.Parse(t.Element("price").Value)).Select(u => u.Element("title").Value).First();
            
            var allBookByRelease = doc.Descendants("book").OrderBy(t => DateTime.Parse(t.Element("publish_date").Value)).Select(u => u.Element("title").Value);
            
            var alphabeticWriters = doc.Descendants("author").OrderBy(t => t.Value).Select(u => u.Value);
            
            var alphabeticWritersCorrectName = doc.Descendants("author").OrderBy(t => t.Value.Split(", ")[1] + " " + t.Value.Split(", ")[0]).Select(u => u.Value.Split(", ")[1] + " " + u.Value.Split(", ")[0]);
            
            var bookGenreStatistics = doc.Descendants("genre").Select(t => t.Value + " (#"
                    + doc.Descendants("genre").Count(u => u.Value == t.Value).ToString() + "): ~"
                    + doc.Descendants("book").Where(u => u.Element("genre").Value == t.Value).Average(v => double.Parse(v.Element("price").Value)).ToString("0.##") + "$ price").Distinct();
            
            var appearsMicrosoft = doc.Descendants("description").Select(t => t.Value).Any(u => u.Contains("Microsoft"));



            threeMostExpensiveName.ConsoleWriteLine("Three most expesive books:");
            
            Console.WriteLine("There are `{0}` number of fantasy books\n", fantasyCount);
            
            Console.WriteLine("The most recent book's author is {0}\n", mostRecentBookAuthor);
            
            Console.WriteLine("The average price of the books is `{0: 0.##}`\n", averagePrice);
            
            Console.WriteLine("The catalog {0} at least one book from `Mike Galos`\n", isMikeGalosWriter ? "contains" : "does not contain");
            
            Console.WriteLine("The catalog {0} at least one book from `Galos, Mike`\n", isMikeGalosWriterCorrectName ? "contains" : "does not contain");
            
            Console.WriteLine("The `{0}` book is the cheapest\n", cheapestBookName);
            
            allBookByRelease.ConsoleWriteLine("All books in order of release:");
            
            alphabeticWriters.ConsoleWriteLine("Authors in alphabetical order:");
            
            alphabeticWritersCorrectName.ConsoleWriteLine("Authors in alphabetical order:");
            
            bookGenreStatistics.ConsoleWriteLine("Statistics of book genres:");
            
            Console.WriteLine("The word `Microsoft` {0} in at least one of the descriptions\n", appearsMicrosoft ? "appeares" : "does not appear");


            #endregion

            #region Catalog Collection actions


            var catalog = doc.Descendants("book").Select(t => Book.Parse(t));

            Console.WriteLine();
            Console.WriteLine();
            catalog.ConsoleWriteLine("Catalog collection:");


            #endregion
        }
    }

    class Book
    {
        public string id { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string genre { get; set; }
        public double price { get; set; }
        public DateTime publish_date { get; set; }
        public string description { get; set; }

        public Book() { }

        public Book(string id, string author, string title, string genre, double price, DateTime publish_date, string description)
        {
            this.id = id;
            this.author = author;
            this.title = title;
            this.genre = genre;
            this.price = price;
            this.publish_date = publish_date;
            this.description = description;
        }

        public static Book Parse(XElement element)
        {
            string? id = element.Attribute("id")?.Value;
            string? author = element.Element("author")?.Value;
            string? title = element.Element("title")?.Value;
            string? genre = element.Element("genre")?.Value;
            double price;
            DateTime publish_date;
            string? description = element.Element("description")?.Value;

            if (id == null
                || author == null
                || genre == null
                || !double.TryParse(element.Element("price").Value, out price)
                || !DateTime.TryParse(element.Element("publish_date").Value, out publish_date)
                || description == null)
                throw new NotSupportedException();

            return new Book(id, author, title, genre, price, publish_date, description);
        }

        public override string ToString()
        {
            return $"{author}: {title}(#{id} - {publish_date.ToShortDateString()})[{genre}] - {description.Replace("\n", " ")}";
        }
    }

    public class Person
    {
        [JsonProperty][JsonPropertyName("Name")] public string Name { get; set; }
        [JsonProperty][JsonPropertyName("Age")] public int Age { get; set; }
        [JsonProperty][JsonPropertyName("Location")] public string Location { get; set; }
        [JsonProperty][JsonPropertyName("PhoneNumber")] public string PhoneNumber { get; set; }
        [JsonProperty][JsonPropertyName("EmailAddress")] public string EmailAddress { get; set; }
        [JsonProperty][JsonPropertyName("Occupation")] public string Occupation { get; set; }

        public Person()
        {
            Name = "";
            Age = 0;
            Location = "";
            PhoneNumber = "";
            EmailAddress = "";
            Occupation = "";
        }

        public Person(string name, int age, string location, string phoneNumber, string emailAddress, string occupation)
        {
            Name = name;
            Age = age;
            Location = location;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Occupation = occupation;
        }

        public XElement ToXML()
        {
            return new XElement("Person",
                        new XAttribute("type", "class"),
                        new XAttribute("visibility", "public"),

                        new XElement("Name", Name, new XAttribute("type", Name.GetType().Name)),
                        new XElement("Age", Age.ToString(), new XAttribute("type", Age.GetType().Name)),
                        new XElement("Location", Location, new XAttribute("type", Location.GetType().Name)),
                        new XElement("PhoneNumber", PhoneNumber, new XAttribute("type", PhoneNumber.GetType().Name)),
                        new XElement("EmailAddress", EmailAddress, new XAttribute("type", EmailAddress.GetType().Name)),
                        new XElement("Occupation", Occupation, new XAttribute("type", Occupation.GetType().Name))
                );
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public override string ToString()
        {
            return $"{Name}({Location} - {Age}) - {Occupation} ({PhoneNumber}; {EmailAddress})";
        }
    }
}
