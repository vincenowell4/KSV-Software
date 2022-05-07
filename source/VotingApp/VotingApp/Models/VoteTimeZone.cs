using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.Models
{
    public partial class VoteTimeZone
    {
        public VoteTimeZone()
        {
            CreatedVotes = new HashSet<CreatedVote>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(250)]
        public string TimeName { get; set; } = null!;

        [InverseProperty(nameof(CreatedVote.TimeZone))]
        public virtual ICollection<CreatedVote> CreatedVotes { get; set; }
    }
}
