namespace DDD.Accounts;

public interface IAccountAppService
{
    Task<TokenDto> SignIn(SignInDto signInDto);
}