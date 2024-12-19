using System;
using System.ComponentModel.DataAnnotations;
using TechNationAPI.Models;

namespace TechNationAPI.Dtos
{
    public class CreateLogDto
    {
        [Required]
        public int ResponseSize { get; set; }
        [Required]
        public CacheStatus CacheStatus { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public float TimeTaken { get; set; }
        public string Version { get; set; }
        public DateTime Date { get; set; }
        public string  MinhaCdnLog { get; set; }

    }
}
