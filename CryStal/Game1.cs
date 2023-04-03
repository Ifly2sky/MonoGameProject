using CryStal.Engine;
using CryStal.Engine.Factories;
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
        public Level level;

        List<GameObject> tempObj = new();

        //debug stuff
        Stopwatch simulationTimer = new();
        Stopwatch drawTimer = new();

        private long simulationTime = 1;

        //Default Font
        SpriteFont Arial;

        //tile information
        public const int Scale = 3;
        public const int TileSize = 16 * Scale;
        public const float InverseTileSize = 0.02083333333333333333f; // 1/tileSIze

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

            player = new Player(new Vector2(TileSize, TileSize), 100);

            using (Stream fileStream = TitleContainer.OpenStream("Content/Level00.txt"))
                level = new Level(Services, fileStream);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Arial = Content.Load<SpriteFont>("Arial");

            player.Texture = Content.Load<Texture2D>("Template");

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
                for(int i = 0; i < 5; i++)
                {
                    GameObject newObj = GameObjectFactory.CreatePhysicsObject(player.Hitbox, new Vector2(TileSize, TileSize * i));
                    newObj.texture = Content.Load<Texture2D>("Stone");
                    tempObj.Add(newObj);
                }
                spawned = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space) && spawned)
            {
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
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin();

            player.Draw(_spriteBatch);
            level.DrawLevel(_spriteBatch);

            foreach(GameObject obj in tempObj)
            {
                obj.Draw(_spriteBatch);
            }

            DrawDebugTimer();
            drawTimer.Restart();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        private void DrawDebugTimer()
        {
            _spriteBatch.DrawString(Arial, $"Simulation Time: {simulationTime}ms", new Vector2(4, 0), Color.WhiteSmoke);
            _spriteBatch.DrawString(Arial, $"Object Count: {GameObjectFactory.objects.Count}", new Vector2(4, 16), Color.WhiteSmoke);
            _spriteBatch.DrawString(Arial, $"Draw Time: {drawTimer.ElapsedMilliseconds}ms", new Vector2(4, 32), Color.WhiteSmoke);
        }
    }
}