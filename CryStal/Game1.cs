using CryStal.Engine;
using CryStal.Engine.Models;
using CryStal.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CryStal
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Player player;

        List<GameObject> tempObj = new();

        //debug stuff
        Stopwatch simulationTimer = new();
        Stopwatch drawTimer = new();

        private long simulationTime = 1;

        //Default Font
        public static SpriteFont Arial;

        //tile information
        public const int Scale = 3;
        public const int TileSize = 16 * Scale;
        public const float InverseTileSize = 0.02083333333333333333f; // 1/tileSIze

        public static Action OnUpdate;
        public delegate void DrawAction(SpriteBatch spriteBatch);
        public static DrawAction OnDraw;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = 1600;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.ApplyChanges();

            player = new Player(new Vector2(TileSize, TileSize), 140);

            LevelHandler.InitializeLevel(Services);

            OnDraw += player.Draw;
            OnDraw += LevelHandler.DrawLevel;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Arial = Content.Load<SpriteFont>("Arial");
            player.Texture = Content.Load<Texture2D>("Template");

            LevelHandler.LoadLevel("Demo");

            //int gridX = (int)Math.Ceiling(_graphics.GraphicsDevice.Viewport.Width * InverseTileSize);
            //int gridY = (int)Math.Ceiling(_graphics.GraphicsDevice.Viewport.Height * InverseTileSize);

            simulationTimer.Start();
            drawTimer.Start();
        }

        bool spawned = false;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !spawned)
            {
                /*for(int i = 0; i < 10; i++)
                {
                    PhysicsObject newObj = new PhysicsObject(player.Hitbox, new Vector2(TileSize, TileSize * i));
                    newObj.texture = Content.Load<Texture2D>("Stone");
                    OnDraw += newObj.Draw;
                    tempObj.Add(newObj);
                }*/
                LevelHandler.LoadLevel("Demo2");
                spawned = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Enter) && spawned)   
            {
                LevelHandler.LoadLevel("Demo");
                spawned = false;
            }

            simulationTimer.Restart();
            Physics.Update(gameTime, _graphics.GraphicsDevice);
            simulationTime = simulationTimer.ElapsedMilliseconds;
            simulationTimer.Stop();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            OnDraw(_spriteBatch);

            DrawDebugTimer();
            drawTimer.Restart();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        private void DrawDebugTimer()
        {
            _spriteBatch.DrawString(Arial, $"Simulation Time: {simulationTime}ms", new Vector2(4, 0), Color.WhiteSmoke);
            _spriteBatch.DrawString(Arial, $"Object Count: {GameObject.allObjects.Count}", new Vector2(4, 16), Color.WhiteSmoke);
            _spriteBatch.DrawString(Arial, $"Draw Time: {drawTimer.ElapsedMilliseconds}ms", new Vector2(4, 32), Color.WhiteSmoke);
        }
    }
}