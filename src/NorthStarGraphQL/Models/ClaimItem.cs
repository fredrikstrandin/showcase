namespace NorthStarGraphQL.Models
{
    public class ClaimItem
    {
        public ClaimItem(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; }
        public string Value { get; }

    }
}