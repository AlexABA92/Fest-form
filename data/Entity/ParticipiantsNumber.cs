using System.ComponentModel.DataAnnotations;

namespace Fest_form.data.Entity
{
    public class ParticipiantsNumber
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; } = null!;
    }
}
