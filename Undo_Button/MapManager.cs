using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class MapManager
{
    public static void SaveMap(string path, List<Vector2> positions)
    {
        MapData mapData = new MapData();

        foreach (var pos in positions)
        {
            mapData.Positions.Add(new Vector2Wrapper(pos));
        }

        string json = JsonConvert.SerializeObject(mapData, Formatting.Indented);
        File.WriteAllText(path, json);
    }

    public static List<Vector2> LoadMap(string path)
{
    if (!File.Exists(path))
    {
        Console.WriteLine($"File does not exist at path: {Path.GetFullPath(path)}");
        return new List<Vector2>();
    }

    string json = File.ReadAllText(path);
    MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

    List<Vector2> positions = new List<Vector2>();
    foreach (var pos in mapData.Positions)
    {
        positions.Add(pos.ToVector2());
    }

    return positions;
}

}
