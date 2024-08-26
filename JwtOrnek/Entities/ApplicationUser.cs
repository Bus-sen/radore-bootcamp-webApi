using System.Globalization;

namespace JwtOrnek.Entities
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
