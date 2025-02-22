using System.Collections.Generic;

namespace Undo_Button;

public class Tiles
{
    const int bgHeight = 3125;
    const int floorHeight = 400;
    const int plrHeight = 200;

    private List<Tile> tilesList = new List<Tile>();

    public List<Tile> TilesList
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