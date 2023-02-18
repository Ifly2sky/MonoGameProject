using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
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

                _ => new Tile(textures[0], CollitionType.Passable),
            };
        }

        public void LoadTextures()
        {
            textures.Add(null);
            textures.Add(Content.Load<Texture2D>("Template"));
        }

        public void DrawLevel(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in tiles)
            {
                if (tile.Texture != null) 
                {
                    spriteBatch.Draw(tile.Texture, tile.Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0f);
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
