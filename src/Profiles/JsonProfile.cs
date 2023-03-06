using Newtonsoft.Json.Linq;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Profiles
{
    public class JsonProfile : IProfile
    {
        public string Id { get; set; }
        private JObject json = new JObject();

        // This is not on the interface
        // To use this, you need to cast it to JsonProfile
        public void LoadJson(string data)
        {
            json = JObject.Parse(data);
        }

        public string this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                json.Add(key, value);
            }
        }

        public string Get(string key)
        {
            return json.SelectToken(key)?.ToString();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return json.ToString();
        }
    }
}
