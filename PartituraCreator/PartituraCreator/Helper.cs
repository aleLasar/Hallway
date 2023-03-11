using PartituraCreator.Model;

namespace PartituraCreator;

public static class Helper
{
    public static Vector3 Limit(this Vector3 position)
    {
        if (position.x > 10)
            position = new Vector3(10, position.y, position.z);

        if (position.x < 0)
            position = new Vector3(0, position.y, position.z);

        if (position.y > 10)
            position = new Vector3(position.x, 10, position.z);

        if (position.y < 0)
            position = new Vector3(position.x, 0, position.z);

        return position;
    }
}
