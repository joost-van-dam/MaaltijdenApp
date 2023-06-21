using System.ComponentModel.DataAnnotations;

namespace WebAPIGraphQL.GraphQL
{
    public class ReservationInput
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public int PackageId { get; set; }
    }
}
