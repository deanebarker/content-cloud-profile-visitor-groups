using System.Xml.Linq;
using System.Xml.XPath;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Profiles
{
    public class XmlProfile : IProfile
    {
        private XDocument xml;

        public string Id { get; set; }

        public void LoadXml(string data)
        {
            xml = XDocument.Parse(data);
        }

        public string this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                xml.Root.Add(new XElement(key, value));
            }
        }
        
        public string Get(string key)
        {
            var rootElementName = xml.Root.Name;
            key = $"/{rootElementName}/{key}";

            return xml.XPathSelectElement(key)?.Value;
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return xml.ToString();
        }
    }
}
