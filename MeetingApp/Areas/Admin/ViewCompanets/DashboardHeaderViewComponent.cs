using AutoMapper;
using BlogApp.Entity.DTOs.Users;
using BlogApp.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Areas.Admin.ViewCompanets
{
    public class DashboardHeaderViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public DashboardHeaderViewComponent(UserManager<AppUser> userManager , IMapper mapper)
        {
            this._userManager = userManager;
            this._mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var logenunuser = await _userManager.GetUserAsync(HttpContext.User);
            var map = _mapper.Map<UserDto>(logenunuser);

            var role = string.Join("", await _userManager.GetRolesAsync(logenunuser));
            map.Role = role;

            return View(map);
        }
    }
}
