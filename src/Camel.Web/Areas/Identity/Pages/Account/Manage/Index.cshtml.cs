// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Azure;
using Camel.Core.Data;
using Camel.Core.Entities;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Camel.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly List<string> _allowedAvatarExtensions = [".png", ".jpg", ".jpg"];

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext dbContext,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
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
            // [Phone]
            // [Display(Name = "Phone number")]
            // public string PhoneNumber { get; set; }

            // [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "File must be .jpg or .png")]
            public IFormFile AvatarFile { set; get; }

            [MaxLength(1024)] public string Userpage { set; get; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var profile = await _dbContext.Profiles.SingleOrDefaultAsync(p => p.Id == user.Id);

            if (profile is null)
            {
                profile = new Profile { Id = user.Id };
                _dbContext.Profiles.Add(profile);
                await _dbContext.SaveChangesAsync();
            }

            Username = userName;

            Input = new InputModel
            {
                Userpage = profile.UserPage
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            // if (Input.PhoneNumber != phoneNumber)
            // {
            //     var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //     if (!setPhoneResult.Succeeded)
            //     {
            //         StatusMessage = "Unexpected error when trying to set phone number.";
            //         return RedirectToPage();
            //     }
            // }
            if (Input.AvatarFile is not null)
            {
                var ext = Path.GetExtension(Input.AvatarFile.FileName);

                if (!_allowedAvatarExtensions.Contains(ext))
                {
                    ModelState.AddModelError("Input.AvatarFile", "Avatar extension must be .png or .jpg");
                    return Page();
                }

                if (Input.AvatarFile.Length >= 262144) // 256 kb
                {
                    ModelState.AddModelError("Input.AvatarFile", "Avatar file is too big");
                    return Page();
                }

                var dataPath = _configuration.GetRequiredSection("DATA_PATH").Value;
                var avatarPath = Path.Combine(dataPath, "a", user.Id + ext);

                var avatarDir = new DirectoryInfo(Path.Combine(dataPath, "a"));
                var existingAvatars = avatarDir.GetFiles(user.Id + ".*");
                foreach (var avatar in existingAvatars)
                    avatar.Delete();

                await using var fs = new FileStream(avatarPath, FileMode.Create);
                await Input.AvatarFile.CopyToAsync(fs);
            }

            await _dbContext.Profiles
                .Where(p => p.Id == user.Id)
                .ExecuteUpdateAsync(p =>
                    p.SetProperty(profile => profile.UserPage, Input.Userpage));

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}