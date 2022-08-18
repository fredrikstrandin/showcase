namespace RelationManager.Models
{
    public class FollowingCreateRequest
    {
        public FollowingCreateRequest(string userId, string followingId)
        {
            UserId = userId;
            FollowingId = followingId;
        }

        public string UserId { get; }
        public string FollowingId { get; }
    }
}
