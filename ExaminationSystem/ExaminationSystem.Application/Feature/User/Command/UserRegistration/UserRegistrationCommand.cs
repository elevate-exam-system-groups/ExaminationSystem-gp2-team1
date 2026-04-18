using ExaminationSystem.Application.Feature.User.Dto;
using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.User.Commond.UserRegistration
{
    public record UserRegistrationCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password
    ):IRequest<Result<UserResponseDto>>;
    
}
