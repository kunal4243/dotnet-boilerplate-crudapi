using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoilerPlate.Data.Entities;

[Table("clienttable")]
public class Client
{

    [Key]
    [Column("clientid")]
    public required string ClientId { get; set; }

    [Column("clientsecret")]
    public required string ClientSecret { get; set; }

    [Column("Expiry")]
    public required DateTime Expiry { get; set; }

}

