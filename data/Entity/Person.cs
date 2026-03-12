using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fest_form.data.Entity
{
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }

        [Required(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(50, ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorLengthValidation")]
        [RegularExpression(@"^[A-Za-zА-Яа-яЁёІіЇїЄєҐґ'\-]+$", ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "NameSymbolValidation")]
        [Trim]
        public string PersonName { get; set; } = null!;

        [Required(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(50, ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorLengthValidation")]
        [RegularExpression(@"^[A-Za-zА-Яа-яЁёІіЇїЄєҐґ'\-]+$", ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "NameSymbolValidation")]
        [Trim]
        public string PersonLastName { get; set; } =null!;

        [StringLength(50, ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorLengthValidation")]
        [RegularExpression(@"^[A-Za-zА-Яа-яЁёІіЇїЄєҐґ'\-]+$", ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "NameSymbolValidation")]
        [Trim]
        public string? PersonFatherName { get; set; }
        [JsonIgnore]
        public List<DanceTeam> LedTeams { get; set; } = new();
        [JsonIgnore]
        public List<Performance> DirectedPerformances { get; set; } = new();
        [JsonIgnore]
        public List<Performance> ConcertmasterPerformances { get; set; } = new();

        [JsonIgnore]
        public List<Participant> Participations { get; set; } = new();

    }
}
