using AdminPanel.Identity.Domain.Entities;

namespace AdminPanel.Identity.Domain.Interfaces;

public interface ITokenRepository
{
    string CreateJwtToken(User user);
}