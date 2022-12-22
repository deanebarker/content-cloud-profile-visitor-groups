using System.Collections.Concurrent;
using System.Text;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups
{
    public class Profile : ConcurrentDictionary<string, string>
    {
        public string Id { get; set; } // We technically don't need this, but it felt weird not to store it

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{this.Count()} keys()");
            foreach (var value in this)
            {
                sb.AppendLine($"{value.Key}: \"{value.Value}\"");
            }
            return sb.ToString();
        }

        public string Get(string keys)
        {
            foreach(var key in keys.Split(',').Select(s => s.Trim()))
            {
                if(ContainsKey(key))
                {
                    return this[key];
                }
            }

            return null;
        }
    }
}
