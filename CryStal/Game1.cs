using CryStal.Engine;
using CryStal.Engine.Factories;
using CryStal.Engine.Models;
using CryStal.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace CryStal
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Player player;
        public Level level;
        Grid gameGrid;

        List<GameObject> tempObj = new();

        public static readonly int Scale = 2;
        public static readonly int TileSize = 16 * Scale;
        public static readonly float InverseTileSize = 0.03125f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            player = new Player(new Vector2(TileSize, TileSize), 100);

            using (Stream fileStream = TitleContainer.OpenStream("Content/Level00.txt"))
                level = new Level(Services, fileStream);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player.Texture = Content.Load<Texture2D>("Template");

            int gridX = (int)Math.Ceiling(_graphics.GraphicsDevice.Viewport.Width * InverseTileSize);
            int gridY = (int)Math.Ceiling(_graphics.GraphicsDevice.Viewport.Height * InverseTileSize);

            gameGrid = new Grid(gridX, gridY);
        }

        bool spawned = false;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !spawned)
            {
                GameObject newObj = GameObjectFactory.CreateGameObject(player.Hitbox, player.Position + new Vector2(TileSize, TileSize));
                newObj.texture = Content.Load<Texture2D>("Template");
                tempObj.Add(newObj);
                spawned = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                spawned = false;
            }

            Physics.Update(gameTime, _graphics.GraphicsDevice);
            gameGrid.UpdateGrid();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            player.Draw(_spriteBatch);
            level.DrawLevel(_spriteBatch);

            foreach(GameObject obj in tempObj)
            {
                obj.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}