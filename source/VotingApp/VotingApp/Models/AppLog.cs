using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.Models
{
    public partial class AppLog
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        [StringLength(10)]
        public string LogLevel { get; set; } = null!;
        [StringLength(60)]
        public string ClassName { get; set; } = null!;
        [StringLength(60)]
        public string MethodName { get; set; } = null!;
        [StringLength(500)]
        public string LogMessage { get; set; } = null!;
    }
}
