using System.ComponentModel.DataAnnotations;

namespace SharedServices.Mutual.Enumerations
{
    public enum SearchOptions
    {
        [Display(Name = "Tout")]
        All,
        [Display(Name = "Mes services")]
        MyServices,
        [Display(Name = "Près de chez moi")]
        NearMe
    }
}