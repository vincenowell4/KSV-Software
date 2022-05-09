using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VotingApp.Models;

namespace VotingApp.Views.Create
{
    public class VoteReviewModel : PageModel
    {
        public void OnGet()
        {
        }

        public TimeSpan RemainingTime(DateTime closetime, CreatedVote vote)
        {
            return closetime - TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, vote.TimeZone.TimeName);
        }
    }
}
