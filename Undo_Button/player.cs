using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Undo_Button;

public class Player
{
    MouseState mouseState;
    private Vector2 position = new Vector2(450, 1662);
    const int GRAVITY = 981;
    private int speed = 600;
    private float speedy = 0;
    private float jump = 0;
    private bool jumping = false;
    private float jetFuel = 0.15f;
    const float maxJetFuel = 0.15f;
    private float vel = 0;
    const int bgHeight = 3125;
    const int floorHeight = 400;
    const int plrHeight = 198;
    const int terminalVel = 373;
    private Tiles tileMap;
    private bool isColliding = false;

    public Tiles TileMap
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

    public Vector2 Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }
    public int Speed
    {
        get
        {
            return speed;
        }
    }
    public float Speedy 
    {
        get
        {
            return speedy;
        }
        set
        {
            speedy = value;
        }
    }
    public float Jump 
    {
        get
        {
            return jump;
        }
        set
        {
            jump = value;
        }
    }
    public float JetFuel 
    {
        get
        {
            return jetFuel;
        }
        set
        {
            jetFuel = value;
        }
    }
    public float Vel
    {
        get
        {
            return vel;
        }
        set
        {
            vel = value;
        }
    }

    public void Update(GameTime gameTime)
    {
        mouseState = Mouse.GetState();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        position.Y += vel * dt;
        vel += GRAVITY * dt - speedy;
        if (vel >= terminalVel / dt)
        {
            vel = terminalVel / dt;
        }
        if (jump > 0)
        {
            jumping = true;
            speedy -= GRAVITY * dt;
            jump -= 150 * dt;
            
        }
        if (jump <= 0)
        {
            speedy = 0;
            jumping = false;
        }


        bool wasColliding = isColliding;
        isColliding = false;

        foreach (Tile tile in tileMap.TilesList)
        {
            if (IsIntersecting(tile, position))
            {
                isColliding = true;
                
                if(position.Y > tile.tilePos.Y - plrHeight)
                {
                    position.Y = tile.tilePos.Y - plrHeight;
                    jump = 0;
                    vel = 0;
                    if (jetFuel <= maxJetFuel)
                    {
                        jetFuel += dt * 5;
                    }
                }

                
                
                
              
                Console.WriteLine($"Collision detected at Tile: ({tile.tilePos.X}, {tile.tilePos.Y})");
                break; // No need to check further; collision detected
            }
        }
       
       
        if (position.Y > bgHeight - floorHeight - plrHeight)
        {
            position = new Vector2(450, 1662);
            jump = 0;
            vel = 0;
            if (jetFuel <= maxJetFuel)
            {
                jetFuel += dt * 5;
            }
        }

      
        
    }

    private bool IsIntersecting(Tile tile, Vector2 objB) {
        int widthA = Tile.TileWidth;
        int widthB = 98;
        int heightA = Tile.TileHeight;
        int heightB = 198;

    if (tile.tileType == 0)
    {
        return false;
    }

    var objA = tile.tilePos;

    return !(objA.X > objB.X + widthB) &&
       !(objA.X + widthA < objB.X) &&
       !(objA.Y > objB.Y + heightB) &&
       !(objA.Y + heightA < objB.Y);
    }
}