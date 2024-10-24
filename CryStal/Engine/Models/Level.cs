using CryStal.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
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
        public List<Texture2D> BackgroundTextures = new(); 
        public List<Texture2D> specularMaps = new();
        private List<GameObject> levelEntities = new();
        private List<GameObject> lightingTiles = new();

        private Player _player = null;
        private World _world = null;

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

        public Level(World world, Player player, IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");

            _world = world;
            _player = player;

            LoadTextures();
            LoadSpecularMaps();
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
                    tile.Unload(_world);
                }
            }
            foreach(GameObject entity in levelEntities) 
            {
                entity.Unload(_world);
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
                    Tile tile = LoadTile(tileType);
                    if(tile.specularMap != null)
                        lightingTiles.Add(tile);
                    tiles[x, y] = tile;
                    tiles[x, y].Position = new Vector2(Game1.TILESIZE * x, Game1.TILESIZE * y);
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

                int x = Game1.TILESIZE * int.Parse(data[i * 5 + 1]) + int.Parse(data[i * 5 + 3]);
                int y = Game1.TILESIZE * int.Parse(data[i * 5 + 2]) + int.Parse(data[i * 5 + 4]);
                entity.Position = new Vector2(x, y);
                if(entity is PhysicsObject)
                {
                    PhysicsObject physicsObject = (PhysicsObject)entity;
                }
            }
        }
        private GameObject GetEntity(string entityId)
        {
            switch(entityId)
            {
                case "P":
                    return _player;
                case "B":
                    PhysicsObject newEntity = new(_world);
                    newEntity.texture = textures[1];
                    levelEntities.Add(newEntity);
                    return newEntity;
                case "T":
                    Tile tile = new(textures[4]);
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
                ' ' => new Tile(textures[0]),
                '#' => new Tile(_world, textures[1]),
                'S' => new Tile(_world, textures[2]),
                'C' => new Tile(_world, textures[3]),
                'L' => new Tile(_world, textures[4]),
                '/' => new Tile(_world, textures[5], Game1.TILESIZE, Game1.TILESIZE * 0.5f),
                '^' => new Tile(_world, textures[6], specularMaps[1]),
                '\\' => new Tile(_world, textures[7], Game1.TILESIZE, Game1.TILESIZE * 0.5f),
                'P' => new Tile(_world, textures[8], Game1.TILESIZE, Game1.TILESIZE * 0.1f),
                _ => new Tile(textures[0])
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
            BackgroundTextures.Add(null);
            BackgroundTextures.Add(Content.Load<Texture2D>("DemoBackground2"));
        }
        public void LoadSpecularMaps()
        {
            specularMaps.Add(null);
            specularMaps.Add(Content.Load<Texture2D>("CrystalSpikes1Specular"));
        }
        public void DrawBackground(SpriteBatch spriteBatch, Camera camera, int index)
        {
            spriteBatch.Draw(BackgroundTextures[index], Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 0f);
        }
        public void DrawLevel(SpriteBatch spriteBatch, Camera camera)
        {
            foreach(Tile tile in tiles)
            {
                if (tile.texture != null && tile.specularMap == null) 
                {
                    Vector2 drawPos = tile.Position;
                    drawPos.X -= camera.X;
                    drawPos.Y -= camera.Y;
                    if (drawPos.X < camera.Width &&
                        drawPos.X > -tile.Width &&
                        drawPos.Y < camera.Height &&
                        drawPos.Y > -tile.Height)
                    {
                        spriteBatch.Draw(tile.texture, drawPos, null, Color.White, 0f, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 0f);
                    }
                }
            }
            foreach(GameObject levelObject in levelEntities)
            {
                levelObject.Draw(spriteBatch, camera);
            }
        }
        //temporary function, should be merged into everything
        public void DrawLevelLighting(SpriteBatch spriteBatch, Camera camera, Effect effect)
        {
            foreach (Tile tile in lightingTiles)
            {
                if (tile.texture != null)
                {
                    Vector2 drawPos = tile.Position;
                    drawPos.X -= camera.X;
                    drawPos.Y -= camera.Y;
                    if (drawPos.X < camera.Width &&
                        drawPos.X > -tile.Width &&
                        drawPos.Y < camera.Height &&
                        drawPos.Y > -tile.Height)
                    {
                        spriteBatch.Draw(tile.texture, drawPos, null, Color.White, 0f, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 0f);
                    }
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
