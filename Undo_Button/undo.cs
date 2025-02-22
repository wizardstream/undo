using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Transform;

namespace Undo_Button;

public class Undo
{
    private Player plr;
    private Vector2 undoPosition;
    private float saveJetfuel;
    private float saveVel;
    private float saveSpeedY;
    private float saveJump;
    private Tiles tileMap;
    Random rand = new Random();

    public Player Plr
    {
        get
        {
            return plr;
        }
        set
        {
            plr = value;
        }
    }
    public Vector2 UndoPosition
    {
        get
        {
            return undoPosition;
        }
        set
        {
            undoPosition = value;
        }
    }
    public Tiles Tilemap
    {
        get
        {
            return tileMap;
        }
        set
        {
            tileMap = value;
        }
    }
    public void undoUpdate()
    {
        if(Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.S))
        {
            undoPosition = plr.Position;
            saveJetfuel = plr.JetFuel;
            saveVel = plr.Vel;
            saveSpeedY = plr.Speedy;
            saveJump = plr.Jump;


        }
        if(Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            if(rand.Next(0, 21) > 10)
            {
                return;
            }
            else
            {
                int newRand = rand.Next(0, tileMap.TilesList.Count);
                var tileType = rand.Next(0, 10);
                tileMap.TilesList[newRand].tileType = tileType;
            }
            plr.Position = undoPosition;
            plr.JetFuel = saveJetfuel;
            plr.Vel = saveVel;
            plr.Speedy = saveSpeedY;
            plr.Jump = saveJump;

        }
    }
}