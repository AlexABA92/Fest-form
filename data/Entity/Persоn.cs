using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fest_form.data.Entity
{
    public class Persоn
    {
        [Key]
        public Guid peson_id { get; set; }
        public string peson_name { get; set; } = string.Empty;

    }
}
