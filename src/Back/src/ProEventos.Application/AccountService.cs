using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersistence _userPersistence;

        public AccountService(UserManager<User> userManager,
                                SignInManager<User> signInManager,
                                IMapper mapper,
                                IUserPersistence userPersistence)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userPersistence = userPersistence;
        }


        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users
                                .SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName.ToLower());

                return await _signInManager.CheckPasswordSignInAsync(user, password, false);

            }

            catch (Exception ex)
            {

                throw new Exception($"Erro ao verificar a senha do usuário. Error: {ex.Message}");
            }
        }

        public async Task<UserDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    return _mapper.Map<UserDto>(user);
                }

                throw new Exception($"Erro ao tentar criar usuário. Erros: {result.Errors}");

            }
            catch (Exception ex)
            {

                throw new Exception($"Erro ao tentar criar usuário. Error: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserByNameAsync(string userName)
        {
            try
            {
                var user = await _userPersistence.GetUserByUserNameAsync(userName);
                if (user == null) return null;
                return _mapper.Map<UserUpdateDto>(user);
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro ao tentar recuperar usuário por UserName. Error: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccountAsync(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userUpdateDto.UserName);
                if (user == null) return null;

                _mapper.Map(userUpdateDto, user);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);

                _userPersistence.Update(user);

                if (await _userPersistence.SaveChangesAsync())
                {
                    var userRetorno = await _userPersistence.GetUserByUserNameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }

                return null;
              
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro ao tentar atualizar usuário. Error: {ex.Message}");
            }
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName == userName.ToLower());
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro ao verificar se o usuário existe. Error: {ex.Message}");
            }
        }
    }
}