using CryStal.Engine.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace CryStal.Engine.Models
{
    public class Level : IDisposable
    {
        public Tile[,] tiles;
        public string levelPath = @"C:\Users\Miko\source\repos\CryStal\CryStal\Content\";
        public List<Texture2D> textures = new();

        public ContentManager Content 
        {
            get { return content; }
        }
        readonly ContentManager content;

        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        public Level(IServiceProvider serviceProvider, Stream fileStream)
        {
            content = new ContentManager(serviceProvider, "Content");

            LoadTextures();

            LoadTiles(fileStream);
        }

        private void LoadTiles(Stream fileStream)
        {
            int width;
            List<string> lines = new();

            using (StreamReader reader = new(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception($"The length of line {lines.Count} is different from all preceeding lines.");
                    line = reader.ReadLine();
                }
            }

            tiles = new Tile[width, lines.Count];

            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    tiles[x, y] = LoadTile(tileType);
                    tiles[x, y].Position = new Vector2(Game1.TileSize * x, Game1.TileSize * y);
                }
            }

        }

        private Tile LoadTile(char tiletype)
        {
            return tiletype switch
            {
                ' ' => GameObjectFactory.CreateTile(textures[0], CollitionType.Passable),
                '#' => GameObjectFactory.CreateTile(textures[1], CollitionType.Impassable),
                'S' => GameObjectFactory.CreateTile(textures[2], CollitionType.Impassable),
                'C' => GameObjectFactory.CreateTile(textures[3], CollitionType.Impassable),
                'L' => GameObjectFactory.CreateTile(textures[4], CollitionType.Impassable),
                '/' => GameObjectFactory.CreateTile(textures[5], CollitionType.Impassable),
                '^' => GameObjectFactory.CreateTile(textures[6], CollitionType.Impassable),
                '\\' => GameObjectFactory.CreateTile(textures[7], CollitionType.Impassable),

                _ => GameObjectFactory.CreateTile(textures[0], CollitionType.Passable)
            };
        }

        public void LoadTextures()
        {
            textures.Add(null);
            textures.Add(Content.Load<Texture2D>("Template"));
            textures.Add(Content.Load<Texture2D>("Stone"));
            textures.Add(Content.Load<Texture2D>("CobbleStone"));
            textures.Add(Content.Load<Texture2D>("LightStone"));
            textures.Add(Content.Load<Texture2D>("CrystalSpikes0"));
            textures.Add(Content.Load<Texture2D>("CrystalSpikes1"));
            textures.Add(Content.Load<Texture2D>("CrystalSpikes2"));
        }

        public void DrawLevel(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in tiles)
            {
                if (tile.texture != null) 
                {
                    spriteBatch.Draw(tile.texture, tile.Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0f);
                }
            }
        }

        public void Dispose()
        {
            Content.Unload();
            GC.SuppressFinalize(this);
        }
    }
}
