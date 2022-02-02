using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.Models
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            CreatedVotes = new HashSet<CreatedVote>();
            SubmittedVotes = new HashSet<SubmittedVote>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; } = null!;

        [InverseProperty(nameof(CreatedVote.User))]
        public virtual ICollection<CreatedVote> CreatedVotes { get; set; }
        [InverseProperty(nameof(SubmittedVote.User))]
        public virtual ICollection<SubmittedVote> SubmittedVotes { get; set; }
    }
}
