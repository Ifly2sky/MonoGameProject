using Microsoft.Xna.Framework.Graphics;
using CryStal.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using CryStal.Engine.Factories;

namespace CryStal.Engine.Models
{
    /// <summary>
    /// Checks what kind of behavior a tile has when colliding
    /// </summary>
    public enum CollitionType
    {
        /// <summary>
        /// A Passable tile is a tile with no collition
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An Impassable tile is one with collition
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A Platform is only passable from the sides and below but not from the top. Unless a key is pressed that alows it
        /// </summary>
        Platform = 2,
    }
    public class Tile : GameObject
    {
        public Texture2D Texture;
        public CollitionType CollisionType;

        public Tile(Texture2D texture, CollitionType collisionType)
        {
            Texture = texture;
            CollisionType = collisionType;
        }
    }
}
