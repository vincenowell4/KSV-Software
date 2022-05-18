// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using VotingApp.Data;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Runtime.Serialization;


namespace VotingApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailService.IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public ResendEmailConfirmationModel(UserManager<IdentityUser> userManager, EmailService.IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty (SupportsGet = true)]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGet(string userEmail)
        {
            if (userEmail != null && userEmail.Length > 0) 
            {
                Input.Email = userEmail;
            }
            ModelState.ClearValidationState("Email");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using (var client = new HttpClient { BaseAddress = new Uri("https://www.google.com") })
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", this._configuration["RecaptchaKey"]),
                    new KeyValuePair<string, string>("response", Request.Form["g-Recaptcha-Response"]),
                    new KeyValuePair<string, string>("remoteip", Request.HttpContext.Connection.RemoteIpAddress.ToString())
                });
                var result = await client.PostAsync("/recaptcha/api/siteverify", content);
                result.EnsureSuccessStatusCode();
                string jsonString = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<RecaptchaResponse>(jsonString);
                if (!response.Success)
                    return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, AppStrings.EmailSentMessage);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);
            var message = new Message(new string[] { Input.Email.ToString() }, AppStrings.EmailSubject,
                AppStrings.EmailMessage + $" <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            _emailSender.SendEmail(message);

            ModelState.AddModelError(string.Empty, AppStrings.EmailSentMessage);
            return Page();
        }

        [DataContract]
        internal class RecaptchaResponse
        {
            [DataMember(Name = "success")]
            public bool Success { get; set; }
            [DataMember(Name = "challenge_ts")]
            public DateTime ChallengeTimeStamp { get; set; }
            [DataMember(Name = "hostname")]
            public string Hostname { get; set; }
            [DataMember(Name = "error-codes")]
            public IEnumerable<string> ErrorCodes { get; set; }
        }
    }
}
