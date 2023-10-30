using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamesValley.Models;

public class Games
{
    [Key]
    public int GameId { get; set; }
    public string GameName { get; set; }
    public string GameDescription { get; set; }


    public string UserId {  get; set; }
    public ApplicationUser User { get; set; }

}
