
namespace PartituraCreator.Model;

public class Vector3
{
    public float x { get; private set; }
    public float y { get; private set; }
    public float z { get; private set; }

    public Vector3()
    {
        x = y = z = 0;
    }

    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static bool operator == (Vector3 left, Vector3 right)
    {
        return left.x == right.x && left.y == right.y && left.z == right.z;
    }

    public static bool operator !=(Vector3 left, Vector3 right) => !(left == right);

    public override bool Equals(object? obj)
    {
        if(obj.GetType() != typeof(Vector3))
            return false;

        var vector = (Vector3)obj;
        return this.x == vector.x && this.y == vector.y && this.z == vector.z;
    }
}
