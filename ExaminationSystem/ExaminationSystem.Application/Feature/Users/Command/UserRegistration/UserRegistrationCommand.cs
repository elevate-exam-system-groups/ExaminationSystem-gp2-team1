using ExaminationSystem.Application.Feature.Users.Dto;
using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Users.Commond.UserRegistration
{
    public sealed record UserRegistrationCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password
    ):IRequest<Result<UserResponseDto>>;
    
}
