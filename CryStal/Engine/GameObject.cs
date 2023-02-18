using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine
{
    public class GameObject
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Hitbox Hitbox { get; set; } = new Hitbox();
    }
}
