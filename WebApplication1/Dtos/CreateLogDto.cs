using System;
using System.ComponentModel.DataAnnotations;

namespace TechNationAPI.Dtos
{
    public class CreateLogDto
    {
        [Required]
        public int ResponseSize { get; set; }
        public int Id { get; set; }
        [Required]
        public CacheStatus CacheStatus { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public float TimeTaken { get; set; }
        public string Version { get; set; }
        public DateTime Date { get; set; }
        public string  MinhaCdnLog { get; set; }
        public string AgoraLog { get; set; }

    }
}
