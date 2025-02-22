using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Transform;

namespace Undo_Button;

public class Tile
{
    public const int TileHeight = 100;    
    public const int TileWidth = 100;

    public int tileType = 0;

    public Vector2 tilePos;
    
    public Tile(Vector2 pos, int type)
    {
        tilePos = pos;
        tileType = type;
    }
}
    