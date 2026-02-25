using Fest_form.GlobalData.Enum;

using Microsoft.AspNetCore.WebUtilities;

using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fest_form.data.Entity
{
    public class DanceTeam
    {
        private List<Performance> _performances = new List<Performance>();

        [Key]
        public Guid TeamId { get; set; }

        [Required(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(75, ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorLengthValidation")]
        [Trim]
        public string TeamName { get; set; } = null!;

        public Guid TeamLeaderId { get; set; }
        [Required(ErrorMessageResourceType =
           typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        public Person TeamLeader { get; set; } = null!;

        [Required(ErrorMessageResourceType =
          typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [EmailAddress(ErrorMessageResourceType =
          typeof(Resources.Resource), ErrorMessageResourceName = "EmailErrorValidation")]
        [Trim]
        public string Mail { get; set; } = null!;


        [Required(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [RegularExpression(@"^\+?\d{1,3}\d{9}$", ErrorMessageResourceType
            = typeof(Resources.Resource),
           ErrorMessageResourceName = "PhoneNumberValidation")]
        [Trim]
        public string TeamPhoneNumber { get; set; } = null!;

        public TeamLevelEnum TeamLevel { get; set; } = TeamLevelEnum.None;
        [Trim]

        public string? Organization { get; set; }

       
        public List<Performance> Performances
        {
            get => _performances;
            set
            {
                _performances = value ?? new List<Performance>();
            }

        }
    };
}
