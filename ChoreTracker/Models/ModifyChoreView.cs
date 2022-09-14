using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChoreTracker.Models
{
    public class ModifyChoreView
    {
        public int ChoreId { get; set; }
        public string ChoreName { get; set; } = null!;
        
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Value { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public List<User> Users { get; set; } = null!;

    }
}
