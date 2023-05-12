using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine.Models
{
    public class Hitbox
    {
        public Vector2 Size = new Vector2(Game1.TileSize, Game1.TileSize);
        public Vector2 Position = Vector2.Zero;

        public Hitbox() { }

        public Hitbox(Vector2 size, Vector2 position)
        {
            Size = size;
            Position = position;
        }
    }
}
