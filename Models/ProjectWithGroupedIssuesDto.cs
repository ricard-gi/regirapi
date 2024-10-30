using System.Collections.Generic;

namespace regirapi.Models
{
    public class ProjectWithGroupedIssuesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<IssueDto> HighPriorityIssues { get; set; } = new List<IssueDto>();
        public List<IssueDto> MediumPriorityIssues { get; set; } = new List<IssueDto>();
        public List<IssueDto> LowPriorityIssues { get; set; } = new List<IssueDto>();
    }

    public class IssueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}


