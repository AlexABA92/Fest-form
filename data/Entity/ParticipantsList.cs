using Newtonsoft.Json;

namespace Fest_form.data.Entity
{
    public class ParticipantsList
    {
        public int Id { get; set; }

        public Guid? Person1Id { get; set; }

        public Person? Person1 { get; set; }

        public Guid? Person2Id { get; set; }

        public Person? Person2 { get; set; }

        public Guid? Person3Id { get; set; }

        public Person? Person3 { get; set; }
        [JsonIgnore]
        public List<Performance> Performances { get; set; } = new();
    }
}
