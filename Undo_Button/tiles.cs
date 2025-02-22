using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Transform;

namespace Undo_Button;

public class Tiles
{
    const int bgHeight = 3125;
    const int floorHeight = 400;
    const int plrHeight = 200;

    private List<Vector2> tilesList = new List<Vector2>();
    public List<Vector2> TilesList
    {
        get
        {
            return tilesList;
        }
        set
        {
            tilesList = value;
        }
        
    }

}