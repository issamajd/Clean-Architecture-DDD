using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DDD.AppUsers;
using DDD.Customers;
using DDD.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DDD.Accounts;

public class AccountAppService : IAccountAppService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IConfiguration _configuration;

    public AccountAppService(SignInManager<AppUser> signInManager, 
        UserManager<AppUser> userManager,
        ICustomerRepository customerRepository,
        IProviderRepository providerRepository,
        IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _customerRepository = customerRepository;
        _providerRepository = providerRepository;
        _configuration = configuration;
    }

    public async Task<TokenDto> SignIn(SignInDto signInDto)
    {
        var user = await _userManager.FindByNameAsync(signInDto.Username);
        if (user == null || 
            !await _userManager.CheckPasswordAsync(user, signInDto.Password))
            throw new UnauthorizedAccessException();
        
        
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
            (_configuration["Jwt:Key"]);

        var customer = await _customerRepository.FindAsync(x => x.UserId == user.Id);
        var provider = await _providerRepository.FindAsync(x => x.UserId == user.Id);

        var claims = new List<Claim>
        {
            new("Id", Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.UserName),
            new(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };
        if (customer != null)
            claims.Add(new Claim("CustomerId", customer.Id.ToString()));
        if (provider != null)
            claims.Add(new Claim("ProviderId", provider.Id.ToString()));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
      
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return new TokenDto
        {
            token = jwtToken
        };

    }
}