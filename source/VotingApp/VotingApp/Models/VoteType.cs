using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.Models
{
    [Table("VoteType")]
    public partial class VoteType
    {
        public VoteType()
        {
            CreatedVotes = new HashSet<CreatedVote>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(500)]
        public string Type { get; set; } = null!;
        [StringLength(500)]
        public string VoteTypeDescription { get; set; } = null!;

        [InverseProperty(nameof(CreatedVote.VoteType))]
        public virtual ICollection<CreatedVote> CreatedVotes { get; set; }
    }
}
