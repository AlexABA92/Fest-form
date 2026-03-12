using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace Fest_form.data.Entity
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }

        public Guid PerformanceId { get; set; }
        [JsonIgnore]
        public Performance? Performance { get; set; }

        public Guid PersonId { get; set; }

        public Person Person { get; set; } = null!;
    }
}
