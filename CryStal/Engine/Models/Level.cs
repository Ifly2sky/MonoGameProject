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

        public Level(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");

            LoadTextures();
        }

        public void LoadLevel(Stream fileStream)
        {
            LoadTiles(fileStream);
        }
        public void Unload()
        {
            if(tiles != null)
            {
                foreach (Tile tile in tiles)
                {
                    tile.UnloadTile();
                }
            }
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
                ' ' => new Tile(textures[0], CollitionType.Passable),
                '#' => new Tile(textures[1], CollitionType.Impassable),
                'S' => new Tile(textures[2], CollitionType.Impassable),
                'C' => new Tile(textures[3], CollitionType.Impassable),
                'L' => new Tile(textures[4], CollitionType.Impassable),
                '/' => new Tile(textures[5], CollitionType.Impassable, new Hitbox(new Vector2(Game1.TileSize, Game1.TileSize * 0.5f), new Vector2(0, Game1.TileSize * 0.5f))),
                '^' => new Tile(textures[6], CollitionType.Impassable),
                '\\' => new Tile(textures[7], CollitionType.Impassable, new Hitbox(new Vector2(Game1.TileSize, Game1.TileSize * 0.5f), new Vector2(0, Game1.TileSize * 0.5f))),
                _ => new Tile(textures[0], CollitionType.Passable)
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
            textures.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
