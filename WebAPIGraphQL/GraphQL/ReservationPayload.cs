using Core.Domain;

namespace WebAPIGraphQL.GraphQL
{
    public class ReservationPayload
    {
        public Package package { get; set; }

        public string? Error { get; set; }
    }
}
