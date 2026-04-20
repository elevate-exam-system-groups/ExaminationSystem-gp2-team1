namespace ExaminationSystem.Application.Feature.User.Dto
{
    internal class UserLoginDto
    {
        public Guid Id;
        public string Email;
        public  string PasswordHash;
        public List<string> Roles;
        

        public UserLoginDto(Guid id, string email, string passwordHash, List<string> roles)
        {
            this.Id = id;
            this.Email = email;
            this.PasswordHash = passwordHash;
            this.Roles = roles;
        }
    }
}