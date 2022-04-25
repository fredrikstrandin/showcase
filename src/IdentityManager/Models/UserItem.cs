using IdentityModel;
using System.Security.Claims;

namespace IdentityManager.Models
{
    public class UserItem
    {
        //
        // Summary:
        //     Gets or sets the subject identifier.
        public string SubjectId { get; set; }
        
        //
        // Summary:
        //     Gets or sets the username.
        public string Username { get; set; }

        //
        // Summary:
        //     Gets or sets the provider name.
        public string ProviderName { get; set; }

        //
        // Summary:
        //     Gets or sets the provider subject identifier.
        public string ProviderSubjectId { get; set; }

        //
        // Summary:
        //     Gets or sets if the user is active.
        public bool IsActive { get; set; } = true;

        //
        // Summary:
        //     Gets or sets the claims.
        public ICollection<Claim> Claims
        {
            get;
            set;
        } = new HashSet<Claim>(new ClaimComparer());

    }
}
