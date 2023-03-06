using System.Collections.Concurrent;
using System.Text;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Profiles
{
    public class DictionaryProfile : ConcurrentDictionary<string, string>, IProfile
    {
        public string Id { get; set; } // We technically don't need this, but it felt weird not to store it

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Id);
            sb.AppendLine($"{this.Count()} key(s)");
            sb.AppendLine("---------");
            foreach (var value in this)
            {
                sb.AppendLine($"{value.Key}: \"{value.Value}\"");
            }
            return sb.ToString();
        }

        public string Get(string keys)
        {
            foreach (var key in keys.Split(',').Select(s => s.Trim()))
            {
                if (ContainsKey(key))
                {
                    return this[key];
                }
            }

            return null;
        }

        public void Remove(string key)
        {
            TryRemove(key, out var _);
        }
    }
}
