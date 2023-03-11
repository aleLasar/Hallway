using System.Numerics;

namespace PartituraCreator.Model;

public class Obstacle
{
    public Guid Id { get; set; }
    public Vector3 Position { get; set; }
    public int Duration { get; set; }
}
