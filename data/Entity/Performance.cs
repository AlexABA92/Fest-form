using Fest_form.data.Enum;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace Fest_form.data.Entity
{
    public class Performance
    {
        [Key]
        public Guid PerformanceId { get; set; }
        
        public Guid  DeanceTeamId { get; set; }
        public string PerformanceName { get; set; } = string.Empty;
        public Persоn Choreographerdirector { get; set; } = null!;
        public Persоn Concertmaster { get; set; } = null!;
        public Category PerformanceGroup { get; set; } = null!;
        public ParticipiantsNumber ParticipiantsNumber { get; set; } = null!;
        public string PerfornmanceTime { get; set; } = null!;
        public string PhonogramFileURL { get; set; } = null!;
        public string YouTubeVideoURL { get; set; } = null!;
    }
}
