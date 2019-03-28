using System.Collections.Generic;
using System.Xml;

namespace FinalTask
{
    public class Users
    {
        public List<User> ParseXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\1.xml");

            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/users/user");

            List<User> users = new List<User>();


            foreach (XmlNode node in nodes)
            {
                User user = new User();

                user.username = node.SelectSingleNode("username").InnerText;
                user.password = node.SelectSingleNode("password").InnerText;
                user.id = node.Attributes["id"].Value;

                users.Add(user);
            }
            return users;
        }
    }
    public class User
    {
        public string id;
        public string username;
        public string password;
    }
}

