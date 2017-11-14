using System.ComponentModel.DataAnnotations;

namespace WebPortal.Models
{
    public class MenuModel
    {
        [StringLength(50)]
        public string Action { get; set; }

        [StringLength(50)]
        public string Controller { get; set; }

        [StringLength(100)]
        [Required]
        public string Description { get; set; }

        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public string Url { get; set; }
    }
}