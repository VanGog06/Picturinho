using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Picturinho.Entities;
using Picturinho.Models;
using Picturinho.Services.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picturinho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> AuthenticateAsync([FromBody] AuthenticateRequest model)
        {
            AuthenticateResponse response = await this.userService.AuthenticateAsync(model, this.ipAddress());

            if (response == null)
            {
                return this.BadRequest(new { message = "Username or password is incorrect" });
            }

            this.setTokenCookie(response.RefreshToken);

            return this.Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticateResponse>> RefreshTokenAsync()
        {
            string refreshToken = this.Request.Cookies["refreshToken"];
            AuthenticateResponse response = await this.userService.RefreshTokenAsync(refreshToken, this.ipAddress());

            if (response == null)
            {
                return this.Unauthorized(new { message = "Invalid token" });
            }

            this.setTokenCookie(response.RefreshToken);

            return this.Ok(response);
        }

        [HttpPost("revoke-token")]
        public async Task<ActionResult> RevokeTokenAsync([FromBody] RevokeTokenRequest model)
        {
            string token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
            {
                return this.BadRequest(new { message = "Token is required" });
            }

            bool response = await this.userService.RevokeTokenAsync(token, this.ipAddress());

            if (!response)
            {
                return this.NotFound(new { message = "Token not found" });
            }

            return this.Ok(new { message = "Token revoked" });
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            IEnumerable<User> users = this.userService.GetAll();
            return this.Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetByIdAsync(int id)
        {
            User user = await this.userService.GetByIdAsync(id);
            return this.Ok(user);
        }

        [HttpGet("{id}/refresh-tokens")]
        public async Task<ActionResult<List<RefreshToken>>> GetRefreshTokensAsync(int id)
        {
            User user = await this.userService.GetByIdAsync(id);
            return this.Ok(user.RefreshTokens);
        }

        private void setTokenCookie(string token)
        {
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
            this.Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (this.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return this.Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return this.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}