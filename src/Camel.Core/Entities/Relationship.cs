using Camel.Core.Enums;

namespace Camel.Core.Entities;

public class Relationship
{
    public int FirstUserId { get; set; }
    public int SecondUserId { get; set; }
    public User FirstUser { get; set; }
    public User SecondUser { get; set; }

    public RelationshipType Type { get; set; }

    public Relationship(int firstUserId, int secondUserId, RelationshipType type)
    {
        FirstUserId = firstUserId;
        SecondUserId = secondUserId;
        Type = type;
    }
}