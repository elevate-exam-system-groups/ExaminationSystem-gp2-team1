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
using ExaminationSystem.Domin.Comman.Result;
namespace ExaminationSystem.Application.Feature.User.Command.UserRegistration
{
    public class UserRegistrationHandler(IUserService userService,
        IGenericRepository<Entities.User> userRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<UserRegistrationCommand, IResult>
    {
        private readonly IUserService _userService;
        private readonly IGenericRepository<Entities.User> _userRepository;
        private readonly IUnitOfWork unitOfWork;
        public async Task<IResult> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            var roleId = await _userService.GetRoleIdByNameAsync("Student");
            var existingUser = await _userRepository.ExistsAsync(u => u.Email == request.Email);
            if (existingUser)
            {
                return new Result<Entities.User>(new Error(ErrorCode.EmailIsAlreadyUsed, "A user with this email already exists."));
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
            var result = await unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                return new Result<Entities.User>(user);
            }
            else
            {
                return new Result<Entities.User>(new Error(ErrorCode.RegistrationFailed, "User registration failed."));
            }

            throw new NotImplementedException();
        }
    }
}
