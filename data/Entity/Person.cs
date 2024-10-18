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
        
        public string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType =
            typeof(Resources.Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(50, ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorLengthValidation")]
        [RegularExpression(@"^[A-Za-zА-Яа-яЁёІіЇїЄєҐґ'\-]+$", ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "NameSymbolValidation")]
        public string PersonLastName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorLengthValidation")]
        [RegularExpression(@"^[A-Za-zА-Яа-яЁёІіЇїЄєҐґ'\-]+$", ErrorMessageResourceType
            = typeof(Resources.Resource), ErrorMessageResourceName = "NameSymbolValidation")]
        public string? PersonFatherName { get; set; } = string.Empty;

    }
}
