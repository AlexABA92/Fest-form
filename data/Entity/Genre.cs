using Fest_form.Interface;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace Fest_form.data.Entity
{
    public class Genre 
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public List<Performance> Performances { get; set; } = new();
    }
}
