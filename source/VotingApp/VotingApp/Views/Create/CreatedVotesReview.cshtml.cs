using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VotingApp.Views.Create
{
    public class VoteReviewModel : PageModel
    {
        public void OnGet()
        {
        }

        public TimeSpan RemainingTime(DateTime closetime)
        {
            return closetime - DateTime.Now;
        }
    }
}
