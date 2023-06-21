using System.ComponentModel.DataAnnotations;
namespace Core.Domain
{
    public enum City
    {
        Breda,
        [Display(Name="Den Bosch")]
        DenBosch,
        Tilburg
    }
}