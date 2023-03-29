using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine.Factories
{
    public static class GameObjectFactory
    {
        public static List<GameObject> objects = new();
        public static GameObject CreateGameObject(Hitbox hitbox, Vector2 Position)
        {
            GameObject newObject = new()
            {
                Position = Position,
                Hitbox = hitbox
            };
            newObject.ResetVelocity();

            objects.Add(newObject);
            return newObject;
        }
        public static Tile CreateTile(Texture2D texture, CollitionType collitionType) 
        {
<<<<<<< HEAD
            Tile tile = new Tile(texture, collitionType);
=======
            Tile tile = new(texture, collitionType);
>>>>>>> 36647cb1002374f365c14fbf0591d9a122e5e5bc
            //objects.Add(tile);

            return tile;
        }
        public static GameObject CreateGameObject()
        {
            GameObject newObject = new();
            objects.Add(newObject);
            return newObject;
        }

        public static void AddGameObject(GameObject newObject)
        {
            objects.Add(newObject);
        }
    }
}
