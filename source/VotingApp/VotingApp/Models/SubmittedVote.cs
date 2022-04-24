using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.Models
{
    [Table("SubmittedVote")]
    public partial class SubmittedVote
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("CreatedVoteID")]
        public int CreatedVoteId { get; set; }
        public int VoteChoice { get; set; }
        [Column("UserID")]
        public int? UserId { get; set; }
        public bool Validated { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCast { get; set; }

        [ForeignKey(nameof(CreatedVoteId))]
        [InverseProperty("SubmittedVotes")]
        public virtual CreatedVote CreatedVote { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(VotingUser.SubmittedVotes))]
        public virtual VotingUser? User { get; set; }
    }
}
