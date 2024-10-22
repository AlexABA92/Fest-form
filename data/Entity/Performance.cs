using Fest_form.GlobalData.Enum;

using Microsoft.AspNetCore.WebUtilities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Fest_form.data.Entity
{
    public class Performance
    {
        

        [Key]
        public Guid PerformanceId { get; set; }
        public Guid  DanceTeamId { get; set; }
        [Required(ErrorMessageResourceType =
           typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        public string PerformanceName { get; set; } = string.Empty;
        
        public Person ChoreographerDirector { get; set; } = null!;
        public Person Concertmaster { get; set; } = null!;
        [Required(ErrorMessageResourceType =
             typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        public Category PerformanceGroup { get; set; } = null!;
        [Required(ErrorMessageResourceType =
             typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        public Genre Genre { get; set; } = null!;
        [Required(ErrorMessageResourceType =
             typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        public ParticipantsNumber ParticipantsNumber { get; set; } = null!;
        public ParticipantsList ParticipantsNameList { get; set; } = new ParticipantsList();
        [Required(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        public string PerformanceTime { get; set; } = null!;

        public StartPointEnum StartPoint { get; set; } = StartPointEnum.None;
        public string PhonogramFileURL { get; set; } = string.Empty!;
        [Url(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "ValidationUrlError")]
        public string? YouTubeVideoURL { get; set; } = string.Empty!;
    }
}
