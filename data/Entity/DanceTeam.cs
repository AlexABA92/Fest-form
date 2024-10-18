using Fest_form.GlobalData.Enum;

using Microsoft.AspNetCore.WebUtilities;

using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fest_form.data.Entity
{
    public class DanceTeam
    {
       
        [Key]
        public Guid TeamId { get; set; }

        [Required(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(75, ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorLengthValidation")]
        public string TeamName { get; set; } = string.Empty;


        [Required(ErrorMessageResourceType =
           typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        
        public Person TeamLeader { get; set; } = null!;

        [Required(ErrorMessageResourceType =
          typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [EmailAddress(ErrorMessageResourceType =
          typeof(Resources.Resource), ErrorMessageResourceName = "EmailErrorValidation")]
        public string Mail { get; set; } = string.Empty;


        [Required(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [RegularExpression(@"^\+?\d{1,3}\d{9}$", ErrorMessageResourceType
            = typeof(Resources.Resource),
           ErrorMessageResourceName = "PhoneNumberValidation")]
        public string TeamPhoneNumber { get; set; } = string.Empty;

        public TeamLevelEnum TeamLevel { get; set; } = TeamLevelEnum.None;

        public string? Organization
        {
            get;set;
        } = string.Empty;  

        private List<Performance> _performances = new List<Performance>();
        public List<Performance> Performances
        {
            get
            {
                if (_performances.Count == 0)
                {
                    _performances.Add(new Performance());
                }
                return _performances;
            }
            set
            {
                _performances = value ?? new List<Performance>();
            }

        }
    };
}
