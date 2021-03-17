using System.Text.Json.Serialization;

namespace WarOfHeroesUsersAPI.Users.Models
{
    public class GoogleUser
    {
        [JsonPropertyName("provider")] 
        public string Provider { get; set; }

        [JsonPropertyName("id")] 
        public string ID { get; set; }

        [JsonPropertyName("email")] 
        public string Email { get; set; }

        [JsonPropertyName("name")] 
        public string Name { get; set; }

        [JsonPropertyName("photoUrl")] 
        public string PhotoUrl { get; set; }

        [JsonPropertyName("firstName")] 
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")] 
        public string LastName { get; set; }

        [JsonPropertyName("authToken")] 
        public string AuthToken { get; set; }

        [JsonPropertyName("idToken")] 
        public string IdToken { get; set; }

        [JsonPropertyName("authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonPropertyName("response")] 
        public object Response { get; set; }
    }
}