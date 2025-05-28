using System;
using System.ComponentModel.DataAnnotations;

namespace EverMediaDiplom.ViewModels
{
    public class PhotoSessionViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public decimal ServicePrice { get; set; }
        public int ServiceDurationHours { get; set; }

        [Required(ErrorMessage = "Дата фотосессии обязательна")]
        [Display(Name = "Дата и время фотосессии")]
        public DateTime SessionDate { get; set; }

        [Display(Name = "Особые пожелания")]
        [StringLength(500, ErrorMessage = "Максимальная длина 500 символов")]
        public string? SpecialRequests { get; set; }

        [Display(Name = "Ваше имя")]
        public string ClientName { get; set; }

        [Display(Name = "Телефон")]
        [Phone(ErrorMessage = "Некорректный формат телефона")]
        public string ClientPhone { get; set; }
    }
}