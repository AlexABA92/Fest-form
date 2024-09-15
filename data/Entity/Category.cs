using System.ComponentModel.DataAnnotations;

namespace Fest_form.data.Entity
{
    public class Category
    {
      
        public int CategoryId { get; set;}
        public string CategoryName { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;

    }
}
