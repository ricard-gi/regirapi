namespace regirapi.Models
{
    public class CreateIssueDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // Ex: "Feature", "Bug", "General"
        public int Priority { get; set; } // 1 = alta, 2 = mitjana, 3 = baixa
        public string Status { get; set; } // Ex: "Pending", "In Progress", "Testing", "Done"

        // Opcional: Id del projecte associat
        public int? ProjectId { get; set; }

        // Opcional: Id de l'usuari associat
        public int? UserId { get; set; }
    }
}
