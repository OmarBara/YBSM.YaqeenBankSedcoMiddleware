namespace Core.Domain.ModelDTO
{
    public class UserSignInRequestDto
    {
        public UserSignInRequestDto()
        {
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}