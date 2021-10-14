using System.Text;

namespace MyCommute.Domain.Models
{
    public record Address(string Street, string Number, string ZipCode, string City, string Country)
    {
        public string ToQueryString()
        {
            return $"{Street} {Number}, {ZipCode} {City}, {Country}";
        }
    }
}