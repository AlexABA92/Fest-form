using Fest_form.GlobalData.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Fest_form.data.Entity
{
    public class Performance
    {
        

        [Key]
        public Guid PerformanceId { get; set; }
        public Guid  DanceTeamId { get; set; }
        public DanceTeam DanceTeam { get; set; } = null!;
        [Required(ErrorMessageResourceType =
           typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [Trim]
        public string PerformanceName { get; set; } = null!;

        public Guid ChoreographerDirectorId { get; set; }
        public Person ChoreographerDirector { get; set; } = null!;
        public Guid? ConcertmasterId { get; set; }
        public Person? Concertmaster { get; set; }

       
        [Required(ErrorMessageResourceType =
             typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        
        public int? CategoryId {get; set; }
        [Required(ErrorMessageResourceType =
             typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        public Category? PerformanceGroup { get; set; } = null!;
      
        public int? GenreId { get; set; }
        [Required(ErrorMessageResourceType =
             typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        public Genre? Genre { get; set; } = null!;
      
        public int ParticipantsNumberId { get; set; }
        public ParticipantsNumber? ParticipantsNumber { get; set; } = null!;

        public int? ParticipantsNameListId { get; set; }
        public ParticipantsList? ParticipantsNameList { get; set; }

        [Required(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [Trim]
        public string PerformanceTime { get; set; } = null!;

        public StartPointEnum StartPoint { get; set; } = StartPointEnum.None;
        [Trim]

        public string? PhonogramFileURL { get; set; }
        [Url(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "ValidationUrlError")]
        [Trim]
        public string? YouTubeVideoURL { get; set; }
    }
}
