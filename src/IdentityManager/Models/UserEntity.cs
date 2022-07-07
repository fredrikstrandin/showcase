using IdentityModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Claims;

namespace IdentityManager.Models
{
    public class UserEntity
    {
        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        [BsonId]
        public ObjectId SubjectId { get; set; }

        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        public string UsernameNormalize { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        
        public byte[] Salt { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        public ICollection<Claim> Claims { get; set; } = new HashSet<Claim>(new ClaimComparer());

        public static implicit operator UserItem(UserEntity user)
        {
            if(user == null)
            {
                return null;
            }

            return new UserItem()
            {
                SubjectId = user.SubjectId.ToString(),
                Username = user.Username,
                IsActive = user.IsActive,
                Claims = user.Claims                
            };
        }

    }
}
