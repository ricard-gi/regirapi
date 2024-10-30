using System;

namespace regirapi.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // Tipus de tasca: "Feature", "Bug", o "General"
        public int Priority { get; set; } // 1 = alta, 2 = mitjana, 3 = baixa
        public string Status { get; set; } // Estat de la tasca: "Pending", "In Progress", "Testing", "Done"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relació amb Project
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        // Relació amb User
        public int? UserId { get; set; } // Pot estar assignada a un usuari o no
        public User User { get; set; }
    }
}
