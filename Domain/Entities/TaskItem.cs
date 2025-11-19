using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taskapi.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string Section { get; set; } = "today";
        public string Color { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public DateTime DueDate { get; internal set; }
    }
}