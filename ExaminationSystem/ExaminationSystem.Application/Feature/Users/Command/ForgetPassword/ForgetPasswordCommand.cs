using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Users.Command.ForgetPassword;

public sealed record ForgetPasswordCommand(string Email) : IRequest<Result<string>>;

