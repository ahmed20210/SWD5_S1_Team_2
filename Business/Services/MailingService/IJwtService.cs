using Domain.Entities;

namespace Business.Services.JwtService
{
    public interface IJwtService
    {
        Task <string> GenerateToken(User user);
    }
}
