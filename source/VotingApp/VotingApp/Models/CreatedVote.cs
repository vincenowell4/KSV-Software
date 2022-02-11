using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.Models
{
    [Table("CreatedVote")]
    public partial class CreatedVote
    {
        public CreatedVote()
        {
            Options = new HashSet<Option>();
            SubmittedVotes = new HashSet<SubmittedVote>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("UserID")]
        public int? UserId { get; set; }
        [StringLength(1000)]
        public string VoteDiscription { get; set; } = null!;
        [StringLength(500)]
        public string VoteType { get; set; } = null!;
        public bool Anonymous { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("CreatedVotes")]
        public virtual User? User { get; set; }
        [InverseProperty(nameof(Option.CreatedVote))]
        public virtual ICollection<Option> Options { get; set; }
        [InverseProperty(nameof(SubmittedVote.CreatedVote))]
        public virtual ICollection<SubmittedVote> SubmittedVotes { get; set; }
    }
}
