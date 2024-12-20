using System;
using System.ComponentModel.DataAnnotations;

namespace TechNationAPI.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public int ResponseSize { get; set; }
        public CacheStatus CacheStatus { get; set; }
        public string Url   { get; set; }
        public float TimeTaken { get; set; }
        public string Version { get; set; }
        public DateTime Date { get; set; }
        public string MinhaCdnLog { get; set; }
        public string AgoraLog { get; set; }
    }

    
}
