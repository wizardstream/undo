using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Comora;
using System;

namespace Undo_Button;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D PlayButton;
    Texture2D logo;
    Texture2D playerSprite;
    Texture2D bg;
    Texture2D tileSprite;
    Texture2D up;
    Texture2D down;
    Texture2D left;
    Texture2D right;
    Texture2D wasd;
    Texture2D ctrlS;
    Texture2D undoPointSprite;
    Player plr = new Player();
    Camera camera;
    private string filePath = "map.json";
    private string fileName = "level.json";
    MapManager mapManager = new MapManager();    
    enum Room
    {
        Intro,
        Menu,
        Game
    }
    Room mode = Room.Game;
    //int level = 0;
    MouseState mouseState;
    double introTimer = 7;
    Undo quickSave = new Undo();
    Tiles tiles = new Tiles();

    UndoPlace undoPosition = new UndoPlace();

    const int bgHeight = 3125;
    const int floorHeight = 400;
    const int plrHeight = 200;

    public int[,] LoadLevelData(string filename)
    {
        using (var streamReader = new StreamReader(filename))
        {
            var serializer = new JsonSerializer();
            return (int[,])serializer.Deserialize(streamReader, typeof(int[,]));
        }
    }

    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
      
        this.camera = new Camera(_graphics.GraphicsDevice);
        quickSave.Plr = plr;
        plr.TileMap = tiles;

        //tiles.TilesList = MapManager.LoadMap(filePath);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        PlayButton = Content.Load<Texture2D>("Sprites/P");
        logo = Content.Load<Texture2D>("Mineplack Studios");
        playerSprite = Content.Load<Texture2D>("Sprites/Rectangle");
        bg = Content.Load<Texture2D>("background");
        tileSprite = Content.Load<Texture2D>("Sprites/TileSmall");
        up = Content.Load<Texture2D>("Sprites/Up");
        down = Content.Load<Texture2D>("Sprites/Down");
        left = Content.Load<Texture2D>("Sprites/Left");
        right = Content.Load<Texture2D>("Sprites/Right");
        wasd = Content.Load<Texture2D>("Sprites/WASD");
        ctrlS = Content.Load<Texture2D>("Sprites/ctrls");
        undoPointSprite = Content.Load<Texture2D>("undoPointbgn");

        InitMap();

        // TODO: use this.Content to load your game content here
    }

    private void InitMap()
    {
        var levelData = LoadLevelData(fileName);

        int xPos = 0;
        int yPos = 0;

        List<Vector2> positions = new List<Vector2>();

        // Iterate through the array and print each element
        for (int i = 0; i < levelData.GetLength(0); i++)
        {            
            for (int j = 0; j < levelData.GetLength(1); j++)
            {
                if (levelData[i, j] > 0) {
                    positions.Add(new Vector2(xPos, yPos));
                }
                xPos += 100;
                Console.Write(levelData[i, j] + " ");
            }
            yPos += 100;
            xPos = 0;
            Console.WriteLine();
        }

 
 
 
          tiles.TilesList = positions;
    }


    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);

        if(capabilities.IsConnected)
        {
            Console.WriteLine("Gamepad is connected");
            GamePadState state = GamePad.GetState(PlayerIndex.One);
            if(capabilities.HasLeftXThumbStick) 
            {
                Console.WriteLine("Left thumbstick is.");
                if (state.ThumbSticks.Left.X > 0.5f)
                {
                    Console.WriteLine("Left thumbstick is pushed to the right");
                }
            }
            if(capabilities.HasRightXThumbStick) 
            {
                Console.WriteLine("Rigt thumbstick is.");
                if (state.ThumbSticks.Right.X > 0.5f)
                {
                    Console.WriteLine("Right thumbstick is pushed to the right");
                }
            }
        }
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        this.camera.Position = new Vector2(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight/2);

        if (mode == Room.Intro)
        {
            introTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (introTimer < 0)
            {
                mode = Room.Menu;
            }
        }

        mouseState = Mouse.GetState();

        if(mouseState.LeftButton == ButtonState.Pressed && mouseState.Y > _graphics.PreferredBackBufferHeight/2 - 200 && mouseState.Y < _graphics.PreferredBackBufferHeight/2 + 200  && mouseState.X > _graphics.PreferredBackBufferWidth/2 - 400 && mouseState.X < _graphics.PreferredBackBufferWidth/2 + 400 && mode == Room.Menu) 
        {
            mode = Room.Game;
        }

        if(mode == Room.Game)
        {
            this.camera.Position = plr.Position + new Vector2(50, 50);
            if(this.camera.Position.X < _graphics.PreferredBackBufferWidth/2){
                this.camera.Position = new Vector2(_graphics.PreferredBackBufferWidth/2, plr.Position.Y + 50);
            }
            if(Keyboard.GetState().IsKeyDown(Keys.D))
            {
              plr.Position+= new Vector2(plr.Speed*dt, 0);
            }
            if(Keyboard.GetState().IsKeyDown(Keys.A))
            {
                plr.Position -= new Vector2(plr.Speed*dt, 0);
            }
            if(Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Space))
            {                
                if(plr.Jump == 0)
                {
                    plr.Jump = 1;
                }            
            
                plr.JetFuel -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(plr.JetFuel > 0)
                {
                    plr.Speedy += 65;
                    if(plr.Speedy > 65)
                    {
                        plr.Speedy = 65;
                    }
                    plr.Jump = 1;
                    
                }

            }

            quickSave.undoUpdate();
            plr.Update(gameTime);
        }

    
        this.camera.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkRed);

        // TODO: Add your drawing code here

        _spriteBatch.Begin(this.camera);

        if(mode == Room.Intro)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Draw(logo, new Vector2(_graphics.PreferredBackBufferWidth/2 - 1280/2, _graphics.PreferredBackBufferHeight/2 - 720/2), Color.White);
        }

        if(mode == Room.Menu)
        {
            
            _spriteBatch.Draw(PlayButton, new Vector2(_graphics.PreferredBackBufferWidth/2 - 400, _graphics.PreferredBackBufferHeight/2 - 200), Color.White);
        }

        if(mode == Room.Game)
        {
            _spriteBatch.Draw(bg, new Vector2(0,0), Color.White);
            _spriteBatch.Draw(bg, new Vector2(8000,0), Color.White);


            _spriteBatch.Draw(wasd, new Vector2(0, 1300), Color.White);
            _spriteBatch.Draw(right, new Vector2(2700, 2100), Color.White);
            _spriteBatch.Draw(left, new Vector2(3100, 2100), Color.White);
            _spriteBatch.Draw(up, new Vector2(2700, 1800), Color.White);
            _spriteBatch.Draw(up, new Vector2(3100, 1800), Color.White);
            _spriteBatch.Draw(ctrlS, new Vector2(6200, 1000), Color.White);
            _spriteBatch.Draw(right, new Vector2(6500, 1000), Color.White);
            _spriteBatch.Draw(down, new Vector2(6800, 1000), Color.White);

            _spriteBatch.Draw(playerSprite, plr.Position, Color.White);


            foreach (Vector2 tile in tiles.TilesList)
            {
                _spriteBatch.Draw(tileSprite, tile, Color.White);
            }

            
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
