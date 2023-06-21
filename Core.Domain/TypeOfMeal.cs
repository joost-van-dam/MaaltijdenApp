using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public enum TypeOfMeal
    {
        [Display(Name="Brood")]
        Bread,
        [Display(Name="Warme avondmaaltijd")]
        WarmEveningMeal,
        [Display(Name="Drank")]
        Drank
    }
}