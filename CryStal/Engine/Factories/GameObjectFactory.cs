using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
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
            GameObject newObject = new GameObject();

            newObject.Position = Position;
            newObject.Hitbox = hitbox;

            objects.Add(newObject);
            return newObject;
        }
        public static GameObject CreateGameObject()
        {
            GameObject newObject = new GameObject();
            objects.Add(newObject);
            return newObject;
        }

        public static void AddGameObject(GameObject newObject)
        {
            objects.Add(newObject);
        }
    }
}
