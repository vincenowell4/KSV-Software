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
            SubmittedVotes = new HashSet<SubmittedVote>();
            VoteAuthorizedUsers = new HashSet<VoteAuthorizedUser>();
            VoteOptions = new HashSet<VoteOption>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("UserID")]
        public int? UserId { get; set; }
        [StringLength(350)]
        public string VoteTitle { get; set; } = null!;
        [StringLength(1000)]
        public string VoteDiscription { get; set; } = null!;
        public bool AnonymousVote { get; set; }
        public int VoteTypeId { get; set; }
        [StringLength(100)]
        public string? VoteAccessCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? VoteOpenDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? VoteCloseDateTime { get; set; }
        public bool PrivateVote { get; set; }
        public byte[]? VoteAudioBytes { get; set; }
        public int RoundNumber { get; set; }
        public int NextRoundId { get; set; }
        public int RoundDays { get; set; }
        public int RoundHours { get; set; }
        public int RoundMinutes { get; set; }
        public int TimeZoneId { get; set; }

        [ForeignKey(nameof(TimeZoneId))]
        [InverseProperty(nameof(VoteTimeZone.CreatedVotes))]
        public virtual VoteTimeZone TimeZone { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(VotingUser.CreatedVotes))]
        public virtual VotingUser? User { get; set; }
        [ForeignKey(nameof(VoteTypeId))]
        [InverseProperty("CreatedVotes")]
        public virtual VoteType VoteType { get; set; } = null!;
        [InverseProperty(nameof(SubmittedVote.CreatedVote))]
        public virtual ICollection<SubmittedVote> SubmittedVotes { get; set; }
        [InverseProperty(nameof(VoteAuthorizedUser.CreatedVote))]
        public virtual ICollection<VoteAuthorizedUser> VoteAuthorizedUsers { get; set; }
        [InverseProperty(nameof(VoteOption.CreatedVote))]
        public virtual ICollection<VoteOption> VoteOptions { get; set; }
    }
}
