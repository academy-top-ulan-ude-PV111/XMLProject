using System.Xml;

namespace XMLProject
{
    class User
    {
        public string? Name { set; get; }
        public DateTime BirthDay { set; get; }
        public string? City { set; get; }

        public User()
        {
            Name = "";
            BirthDay = DateTime.Now;
            City = "";
        }
        public User(string? name, int age, string? city)
        {
            Name = name;
            BirthDay = DateTime.Now.AddYears(-age);
            City = city;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            User userLeo = new("Leo", 35, "St peterburg");

            var users = new List<User>()
            {
                new("Bob", 35, "Moscow"),
                new("Joe", 22, "St Peterburg"),
                new("Tim", 41, "Kazan"),
            };

            XmlDocument document = new();
            try
            {
                document.Load("users.xml");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            XmlElement? root = document.DocumentElement;
            if(root != null)
            {
                foreach(XmlElement element in root)
                {
                    User user = new();
                    foreach (XmlElement node in element.ChildNodes)
                    {
                        //XmlNode? attrName = element.Attributes.GetNamedItem("Name");
                        //user.Name = attrName?.Value;
                        switch (node.Name)
                        {
                            case "Name":
                                user.Name = node.InnerText; break;
                            case "City":
                                user.City = node.InnerText; break;
                            case "BirthDay":
                                int day = 0, month = 0, year = 0;
                                foreach (XmlNode nodeDate in node.ChildNodes)
                                {
                                    
                                    switch (nodeDate.Name)
                                    {
                                        case "Day": day = Int32.Parse(nodeDate.InnerText); break;
                                        case "Month": month = Int32.Parse(nodeDate.InnerText); break;
                                        case "Year": year = Int32.Parse(nodeDate.InnerText); break;
                                        default:
                                            break;
                                    }
                                }
                                user.BirthDay = new DateTime(year, month, day);
                                break;
                            default:
                                break;
                        }
                    }
                    Console.WriteLine($"{user.Name} {user.BirthDay} {user.City}");
                }
                
            }

            XmlElement elementLeo = document.CreateElement("User");

            XmlElement leoName = document.CreateElement("Name");
            XmlElement leoCity = document.CreateElement("City");
            XmlElement leoBirthDate = document.CreateElement("BirthDate");

            XmlElement dateDay = document.CreateElement("Day");
            XmlElement dateMonth = document.CreateElement("Month");
            XmlElement dateYear = document.CreateElement("Year");

            XmlText nameText = document.CreateTextNode(userLeo.Name);
            XmlText cityText = document.CreateTextNode(userLeo.City);

            XmlText dayText = document.CreateTextNode(userLeo.BirthDay.Day.ToString());
            XmlText monthText = document.CreateTextNode(userLeo.BirthDay.Month.ToString());
            XmlText yearText = document.CreateTextNode(userLeo.BirthDay.Year.ToString());

            dateDay.AppendChild(dayText);
            dateMonth.AppendChild(monthText);
            dateYear.AppendChild(yearText);

            leoBirthDate.AppendChild(dateDay);
            leoBirthDate.AppendChild(dateMonth);
            leoBirthDate.AppendChild(dateYear);

            leoName.AppendChild(nameText);
            leoCity.AppendChild(cityText);

            elementLeo.AppendChild(leoName);
            elementLeo.AppendChild(leoCity);
            elementLeo.AppendChild(leoBirthDate);

            root.AppendChild(elementLeo);

            document.Save("users2.xml");

            foreach(var u in users)
            {
                XmlElement uNew = document.CreateElement("User");

                XmlElement uName = document.CreateElement("Name");
                XmlElement uCity = document.CreateElement("City");
                XmlElement uBirthDate = document.CreateElement("BirthDate");

                XmlElement uDay = document.CreateElement("Day");
                XmlElement uMonth = document.CreateElement("Month");
                XmlElement uYear = document.CreateElement("Year");
                uName.InnerText = u.Name;
                uCity.InnerText = u.City;

                uDay.InnerText = u.BirthDay.Day.ToString();
                uMonth.InnerText = u.BirthDay.Month.ToString();
                uYear.InnerText = u.BirthDay.Year.ToString();

                uBirthDate.AppendChild(uDay);
                uBirthDate.AppendChild(uMonth);
                uBirthDate.AppendChild(uYear);

                uNew.AppendChild(uName);
                uNew.AppendChild(uCity);
                uNew.AppendChild(uBirthDate);

                root.AppendChild(uNew);
            }

            document.Save("users3.xml");

            //XmlNode? first = root.FirstChild;
            //if (first is not null)
            //    root.RemoveChild(first);

            //document.Save("users3.xml");

        }
    }
}