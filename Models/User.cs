using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace regirapi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        // Relació amb les tasques assignades a l'usuari
        [JsonIgnore]
        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    }
}
