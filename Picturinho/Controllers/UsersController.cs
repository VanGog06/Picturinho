using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Picturinho.Entities;
using Picturinho.Helpers;
using Picturinho.Models;
using Picturinho.Models.User;
using Picturinho.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Picturinho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly AppSettings appSettings;
        private readonly IMapper mapper;

        public UsersController(IUserService userService, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            this.userService = userService;
            this.appSettings = appSettings.Value;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> AuthenticateAsync([FromBody] AuthenticateModel model)
        {
            User user = await userService.AuthenticateAsync(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                user.Id,
                user.Username,
                user.FirstName,
                user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            User user = mapper.Map<User>(model);

            try
            {
                await userService.CreateAsync(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> GetAll()
        {
            IEnumerable<User> users = userService.GetAll();
            IEnumerable<UserModel> model = mapper.Map<IEnumerable<UserModel>>(users);

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetByIdAsync([FromRoute] int id)
        {
            User user = await userService.GetByIdAsync(id);
            UserModel model = mapper.Map<UserModel>(user);

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateModel model)
        {
            User user = mapper.Map<User>(model);
            user.Id = id;

            try
            {
                await userService.UpdateAsync(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await userService.DeleteAsync(id);
            return Ok();
        }
    }
}