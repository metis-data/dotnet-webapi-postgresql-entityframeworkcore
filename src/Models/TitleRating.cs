using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_webapi_postgresql_entityframeworkcore.Models;

[Table("title_ratings")]
public class TitleRating
{
    [Column("tconst")]
    public string TConst { get;set; }
    [Column("averagerating")]
    public double AverageRating { get;set; }
    [Column("numvotes")]
    public long NumVotes { get;set; }
}