using Fest_form.data.Enum;

using System.ComponentModel.DataAnnotations;

namespace Fest_form.data.Entity
{
    public class DanceTeam
    {
        [Key]
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Persоn TeamLider { get; set; } = null!;
        public string TeamPhoneNumber { get; set; } = null!;
        public TeamLevelEnum TeamLavel { get; set; } = TeamLevelEnum.None;
        public Performance[] Performances { get; set; } = null!;
    }
}
