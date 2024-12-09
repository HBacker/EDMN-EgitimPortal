using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EgitimPortalFinal.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Display(Name = "Ders Başlığı")]
        [Required(ErrorMessage = "Lütfen Ders Başlığı Giriniz!")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olmalıdır.")]
        public string Title { get; set; }

        [Display(Name = "Ders Açıklaması")]
        [Required(ErrorMessage = "Lütfen Ders Açıklaması Giriniz!")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olmalıdır.")]
        public string Description { get; set; }

        [Display(Name = "Ders İçeriği")]
        [Required(ErrorMessage = "Lütfen Ders İçeriği Giriniz!")]
        public string Content { get; set; }

        [Display(Name = "Ders Etiketi")]
        [Required(ErrorMessage = "Lütfen Ders Etiketi Giriniz!")]
        public string Tag { get; set; }

        [Display(Name = "Ders Fotoğrafı")]
        [Url(ErrorMessage = "Lütfen geçerli bir URL giriniz.")]
        public string PhotoUrl { get; set; }

        public List<string> Tags
        {
            get
            {
                return string.IsNullOrEmpty(Tag)
                    ? new List<string>()
                    : Tag.Split(',').Select(t => t.Trim()).ToList();
            }
        }
    }
}
