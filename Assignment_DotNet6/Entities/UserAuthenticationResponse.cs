using Microsoft.AspNetCore.Identity;

namespace Assignment_DotNet6.Entities
{
    public class UserAuthenticationResponse
    {
        public dynamic TokenResponse { get; set; }
        public UserAuthenticationStatus AuthenticationStatus { get; set; }
        public Doctor userDetail { get; set; }
    }
}
