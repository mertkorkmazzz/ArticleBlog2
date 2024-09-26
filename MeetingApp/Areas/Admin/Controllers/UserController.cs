using AutoMapper;
using BlogApp.DAL.UnitOfWork;
using BlogApp.Entity.DTOs.Articles;
using BlogApp.Entity.DTOs.Users;
using BlogApp.Entity.Entities;
using BlogApp.Entity.Enums;
using BlogApp.ResultMessage;
using BlogApp.Service.Extensions;
using BlogApp.Service.Helpers.Images;
using BlogApp.Service.Services.Abstractions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FluentValidation.AspNetCore;



namespace BlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {



        private readonly IUserService _userService;
        private readonly IValidator<AppUser> _validator;
        private readonly IToastNotification _toast;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IValidator<AppUser> validator, IToastNotification toast, IMapper mapper)
        {
            this._userService = userService;
            this._validator = validator;
            this._toast = toast;
            this._mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAllUsersWithRoleAsync();

            return View(result);
        }



        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await _userService.GetAllRolesAsync();
            return View(new UserAddDto { Roles = roles });
        }
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            var map = _mapper.Map<AppUser>(userAddDto);
            var validation = await _validator.ValidateAsync(map);
            var roles = await _userService.GetAllRolesAsync();

            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUserAsync(userAddDto);
                if (result.Succeeded)
                {
                    _toast.AddSuccessToastMessage(Messages.User.Add(userAddDto.Email), new ToastrOptions { Title = "İşlem Başarılı" });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    result.AddToIdentityModelState(this.ModelState);
                    //FluentValidation.AspNetCore.ValidationResultExtension.AddToModelState(validation, ModelState);
                    BlogApp.Service.Extensions.FluentValidationExtensions.AddToModelState(validation, ModelState);
                    return View(new UserAddDto { Roles = roles });

                }
            }
            return View(new UserAddDto { Roles = roles });
        }



        [HttpGet]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await _userService.GetAppUserByIdAsync(userId);

            var roles = await _userService.GetAllRolesAsync();

            var map = _mapper.Map<UserUpdateDto>(user);
            map.Roles = roles;
            return View(map);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            var user = await _userService.GetAppUserByIdAsync(userUpdateDto.Id);

            if (user != null)
            {
                var roles = await _userService.GetAllRolesAsync();
                if (ModelState.IsValid)
                {
                    var map = _mapper.Map(userUpdateDto, user);
                    var validation = await _validator.ValidateAsync(map);

                    if (validation.IsValid)
                    {
                        user.UserName = userUpdateDto.Email;
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        var result = await _userService.UpdateUserAsync(userUpdateDto);
                        if (result.Succeeded)
                        {
                            _toast.AddSuccessToastMessage(Messages.User.Update(userUpdateDto.Email), new ToastrOptions { Title = "İşlem Başarılı" });
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }
                        else
                        {
                            result.AddToIdentityModelState(this.ModelState);
                            return View(new UserUpdateDto { Roles = roles });
                        }
                    }
                    else
                    {
                        FluentValidation.AspNetCore.ValidationResultExtension.AddToModelState(validation, this.ModelState);

                        return View(new UserUpdateDto { Roles = roles });
                    }
                }
            }
            return NotFound();
        }



        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await _userService.DeleteUserAsync(userId);

            if (result.identityResult.Succeeded)
            {
                _toast.AddSuccessToastMessage(Messages.User.Delete(result.email), new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else
            {
                result.identityResult.AddToIdentityModelState(this.ModelState);
            }
            return NotFound();
        }




        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var profile = await _userService.GetUserProfileAsync();

            return View(profile);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileDto userProfileDto)
        {

            if (ModelState.IsValid)
            {
                var result = await _userService.UserProfileUpdateAsync(userProfileDto);
                if (result)
                {
                    _toast.AddSuccessToastMessage("Profil güncelleme işlemi tamamlandı", new ToastrOptions { Title = "İşlem Başarılı" });
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
                else
                {
                    var profile = await _userService.GetUserProfileAsync();
                    _toast.AddErrorToastMessage("Profil güncelleme işlemi tamamlanamadı", new ToastrOptions { Title = "İşlem Başarısız" });
                    return View(profile);
                }
            }
            else
                return NotFound();
        }
    }
}
