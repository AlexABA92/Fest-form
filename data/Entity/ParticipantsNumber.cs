using System.ComponentModel.DataAnnotations;

namespace Fest_form.data.Entity
{
    public class ParticipantsNumber
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Performance> Performances { get; set; } = new();
    }
}
