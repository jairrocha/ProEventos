using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ItokenService _tokenService;

        public AccountController(IAccountService countService, ItokenService tokenService)
        {
            _accountService = countService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {

            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByNameAsync(userName);
                if (user == null) return NoContent();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                                        $"Erro ao tentar recuperar usuário. Erro:  {ex.Message}");
            }

        }


        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {

            try
            {
                if (await _accountService.UserExistsAsync(userDto.UserName))
                    return BadRequest("Usuário já cadastrado!");



                var user = await _accountService.CreateAccountAsync(userDto);
                if (user != null)
                    return Ok(user);

                return BadRequest("Falha ao tentar criar usuário.");

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                                        $"Erro ao tentar registrar usuário. Erro:  {ex.Message}");
            }

        }

        [HttpPost("Login/{userName}")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {

            try
            {
                var user = await _accountService.GetUserByNameAsync(userLoginDto.UserName);
                if (user == null) return Unauthorized("Usuário ou senha inválido");
                
                var result = _accountService.CheckUserPasswordAsync(user, userLoginDto.Password);
                if (user == null) return Unauthorized("Usuário ou senha inválido");

                return Ok(new{
                    UserName = user.UserName,
                    PrimeiroNome = user.PrimeiroNome,
                    Token = _tokenService.CreateToken(user).Result
                    
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                                        $"Erro ao tentar realizar login. Erro:  {ex.Message}");
            }

        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {

            try
            {
                var user = await _accountService.GetUserByNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário inválido");
              
                var userReturn = await _accountService.UpdateAccountAsync(userUpdateDto);
                if (userReturn == null) return NoContent();

                return Ok(userReturn);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                                        $"Erro ao tentar atualizar o usuário. Erro:  {ex.Message}");
            }

        }


    }

}
