
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreConcurrentIssue
{
    public class Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public bool Active { get; set; }
    }

    public class Event: Entity
    {
        public string Name { get; set; }

        public long LocationId { get; set; }

        [ForeignKey("LocationId")]
        public Location Location { get; set; }
    }
    
    public class Location: Entity
    {
        public string Name { get; set; }

        public List<Event> Events { get; set; }
    }
}