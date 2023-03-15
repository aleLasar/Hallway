using PartituraCreator.Model;

namespace PartituraCreator;

public static class Helper
{
    private const int _Shifter = 60;

    public static Vector3 Limit(this Vector3 position)
    {
        if (position.x > 9.5f)
            position = new Vector3(9.5f, position.y, position.z);

        if (position.x < 0.5f)
            position = new Vector3(0.5f, position.y, position.z);

        if (position.y > 9.5f)
            position = new Vector3(position.x, 9.5f, position.z);

        if (position.y < 0.5f)
            position = new Vector3(position.x, 0.5f, position.z);

        position = new Vector3(position.x, position.y, position.z + _Shifter);

        return position;
    }
}
