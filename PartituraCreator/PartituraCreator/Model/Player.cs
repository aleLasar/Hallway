
namespace PartituraCreator.Model;

public class Player
{
    public Guid Id { get; set; }
    public bool LeftHand { get; set; }
    public List<Obstacle> Obstacles { get; set; }
}
