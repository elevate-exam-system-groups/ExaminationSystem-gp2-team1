using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using System.Security.Cryptography;

using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Entities;
using BCrypt.Net;
using ExaminationSystem.Domin.Common.Result;
using AutoMapper;
using ExaminationSystem.Application.Feature.User.Dto;
using ExaminationSystem.Application.Feature.User.Commond.UserRegistration;
namespace ExaminationSystem.Application.Feature.User.Command.UserRegistration
{
    public sealed class UserRegistrationCommandHandler(IUserService userService ,
        IGenericRepository<Entities.User> userRepository ,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<UserRegistrationCommand, Result<UserResponseDto>>
    {
        private readonly IUserService _userService;
        private readonly IGenericRepository<Entities.User> _userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        public async Task<Result<UserResponseDto>> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            var roleId =  await _userService.GetRoleIdByNameAsync("Student");

            var existingUser = await _userRepository.ExistsAsync(u => u.Email == request.Email);

            if (existingUser)
            {        
              return new Result<UserResponseDto>(  new Error(ErrorCode.EmailIsAlreadyUsed , "A user with this email already exists."));                           
            }

            var user = new Entities.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = roleId
            };
            
            _userRepository.Add(user);

            var result =  await unitOfWork.SaveChangesAsync();

            if (result > 0)
            {
               var userDto = _mapper.Map<UserResponseDto>(user);
                return new Result<UserResponseDto>(userDto);
            }
            else
            {
                return new Result<UserResponseDto>(new Error(ErrorCode.RegistrationFailed, "User registration failed."));
            }

            throw new NotImplementedException();
        }

      
    }
}
