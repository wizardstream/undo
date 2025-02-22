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
            plr.Position = undoPosition;
            plr.JetFuel = saveJetfuel;
            plr.Vel = saveVel;
            plr.Speedy = saveSpeedY;
            plr.Jump = saveJump;
        }
    }
}