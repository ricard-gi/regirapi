using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace regirapi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Relació amb les tasques
        [JsonIgnore]
        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    }
}
