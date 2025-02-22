using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class MapData
{
    public List<Vector2Wrapper> Positions { get; set; } = new List<Vector2Wrapper>();
}

public class Vector2Wrapper
{
    public float X { get; set; }
    public float Y { get; set; }

    public Vector2Wrapper() { }

    public Vector2Wrapper(Vector2 vector)
    {
        X = vector.X;
        Y = vector.Y;
    }

    public Vector2 ToVector2() => new Vector2(X, Y);
}
