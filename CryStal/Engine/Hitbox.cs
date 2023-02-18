using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine
{
    public class Hitbox
    {
        public Vector2 Size { get; set; } = new Vector2(Game1.TileSize, Game1.TileSize);
        public Vector2 Position { get; set; } = Vector2.Zero;

        public Hitbox() { }

        public Hitbox(Vector2 size, Vector2 position)
        {
            Size = size;
            Position = position;
        }
    }
}
