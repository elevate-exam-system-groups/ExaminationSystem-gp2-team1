using AutoMapper;
using BCrypt.Net;
using ExaminationSystem.Application.Feature.User.Commond.UserRegistration;
using ExaminationSystem.Application.Feature.User.Dto;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Domin.Entities.Enums;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExaminationSystem.Application.Feature.User.Command.UserRegistration
{
    public sealed class UserRegistrationCommandHandler
        : IRequestHandler<UserRegistrationCommand, Result<UserResponseDto>>
    {
        private readonly IGenericRepository<Entities.User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<UserRegistrationCommandHandler> _logger;

        public UserRegistrationCommandHandler(
            IGenericRepository<Entities.User> userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMediator mediator,
            ILogger<UserRegistrationCommandHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<UserResponseDto>> Handle(
            UserRegistrationCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting registration for {Email}", request.Email);

            var exists = await _userRepository.ExistsAsync(u => u.Email == request.Email);

            if (exists)
            {
                _logger.LogWarning("Email already exists: {Email}", request.Email);

                return new Result<UserResponseDto>(
                    new Error(ErrorCode.EmailIsAlreadyUsed, "Email already exists."));
            }

            var user = new Entities.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Status = AccountStatus.pending,
            };

            user.Roles.Add(new UserRole { RoleId = UserRoleCode.Student });

            _userRepository.Add(user);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
            {
                _logger.LogError("Failed to save user for {Email}", request.Email);

                return new Result<UserResponseDto>(
                    new Error(ErrorCode.RegistrationFailed, "Registration failed."));
            }

            _logger.LogInformation("User saved successfully: {Email}", user.Email);

            await _mediator.Publish(
                new UserRegisteredEvent(user.Email),
                cancellationToken);

            _logger.LogInformation("UserRegisteredEvent published for {Email}", user.Email);

            var dto = _mapper.Map<UserResponseDto>(user);

            return new Result<UserResponseDto>(dto);
        }
    }
}