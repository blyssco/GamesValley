using Microsoft.AspNetCore.Identity;

namespace GamesValley.Models;

public class ApplicationUser : IdentityUser
{
    public string CreatorName { get; set; }
    public ICollection<Games> Games { get; set;} = new List<Games>();
}
