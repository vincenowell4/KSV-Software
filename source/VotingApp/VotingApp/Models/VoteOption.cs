using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.Models
{
    public partial class VoteOption
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("CreatedVoteID")]
        public int CreatedVoteId { get; set; }
        [StringLength(250)]
        public string VoteOptionString { get; set; } = null!;

        [ForeignKey(nameof(CreatedVoteId))]
        [InverseProperty("VoteOptions")]
        public virtual CreatedVote CreatedVote { get; set; } = null!;
    }
}
