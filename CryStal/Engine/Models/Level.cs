using CryStal.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CryStal.Engine.Models
{
    public class Level : IDisposable
    {
        public Tile[,] tiles;
        public string levelPath = @"C:\Users\Miko\source\repos\CryStal\CryStal\Content\";
        public List<Texture2D> textures = new();
        private List<GameObject> levelEntities = new();

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
                    tile.Unload();
                }
            }
            foreach(GameObject entity in levelEntities) 
            {
                entity.Unload();
            }
            levelEntities.Clear();
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
        public void SetEntities(Stream fileStream)
        {
            List<string> data = new();

            using (StreamReader reader = new(fileStream))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    data.AddRange(line.Split(',', StringSplitOptions.TrimEntries));
                    line = reader.ReadLine();
                }
            }

            for (int i = 0; i < data.Count * 0.2; i++)
            {
                GameObject entity = GetEntity(data[i * 5]);

                if (entity == null)
                {
                    return;
                }

                int x = Game1.TileSize * int.Parse(data[i * 5 + 1]) + int.Parse(data[i * 5 + 3]);
                int y = Game1.TileSize * int.Parse(data[i * 5 + 2]) + int.Parse(data[i * 5 + 4]);
                entity.Position = new Vector2(x, y);
                if(entity is PhysicsObject)
                {
                    PhysicsObject physicsObject = (PhysicsObject)entity;
                    physicsObject.ResetVelocity();
                }
            }
        }
        private GameObject GetEntity(string entityId)
        {
            switch(entityId)
            {
                case "P":
                    return GameObject.allObjects.Find(x => x.ID == entityId);
                case "B":
                    PhysicsObject newEntity = new(new Hitbox(), Vector2.Zero, "Impassable", "B");
                    newEntity.texture = textures[1];
                    levelEntities.Add(newEntity);
                    return newEntity;
                case "T":
                    Tile tile = new(textures[4], "Impassable");
                    levelEntities.Add(tile);
                    return tile;
                default:
                    return null;
            }
        }
        private Tile LoadTile(char tiletype)
        {
            return tiletype switch
            {
                ' ' => new Tile(textures[0], "Passable"),
                '#' => new Tile(textures[1], "ImpassableTile"),
                'S' => new Tile(textures[2], "ImpassableTile"),
                'C' => new Tile(textures[3], "ImpassableTile"),
                'L' => new Tile(textures[4], "ImpassableTile"),
                '/' => new Tile(textures[5], "Spike", new Hitbox(new Vector2(Game1.TileSize, Game1.TileSize * 0.5f), new Vector2(0, Game1.TileSize * 0.5f))),
                '^' => new Tile(textures[6], "Spike"),
                '\\' => new Tile(textures[7], "Spike", new Hitbox(new Vector2(Game1.TileSize, Game1.TileSize * 0.5f), new Vector2(0, Game1.TileSize * 0.5f))),
                'P' => new Tile(textures[8], "Platform", new Hitbox(new Vector2(Game1.TileSize, Game1.TileSize * 0.5f), Vector2.Zero)),
                _ => new Tile(textures[0], "Passable")
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
            textures.Add(Content.Load<Texture2D>("Platform"));
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
            foreach(GameObject levelObject in levelEntities)
            {
                levelObject.Draw(spriteBatch);
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
