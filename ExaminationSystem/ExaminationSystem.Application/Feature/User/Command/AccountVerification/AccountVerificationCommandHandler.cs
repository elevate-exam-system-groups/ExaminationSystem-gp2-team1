using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Users.Command.AccountVerification
{
    public class AccountVerificationCommandHandler(
        IGenericRepository<OtpRecord> otpRepo ,
        IUnitOfWork unitOfWork ,
        ILogger<AccountVerificationCommandHandler> logger) : IRequestHandler<AccountVerificationCommand, Result>
    {
        private readonly ILogger<AccountVerificationCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<OtpRecord> _otpRepo;
        public async Task<Result> Handle(AccountVerificationCommand request, CancellationToken cancellationToken)
        {
            var otp = _otpRepo.Find(x => x.UserId == request.userId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();

            // Implement the account verification logic here
            throw new NotImplementedException();
        }
    }
}
