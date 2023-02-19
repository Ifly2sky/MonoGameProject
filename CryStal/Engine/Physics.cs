using CryStal.Engine.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine
{
    public static class Physics
    {
        const float half = 0.5f;
        public static readonly Vector2 gravity = new(0f, 10f);

        public static bool CheckCollition(GameObject obj, GameObject target, GameTime gameTime)
        {
            Vector2 objCenter = obj.Hitbox.Size * half;
            Vector2 targetCenter = target.Hitbox.Size * half;
            Vector2 distance = (obj.Position + obj.Hitbox.Position + objCenter)-(target.Position + target.Hitbox.Position + targetCenter);

            Vector2 absDistance = new(Math.Abs(distance.X), Math.Abs(distance.Y));

            Vector2 direction = new Vector2(1, 1);
            if (absDistance.X - distance.X == 0)
            {
                direction.X = -1;
            }
            if (absDistance.Y - distance.Y == 0)
            {
                direction.Y = -1;
            }

            Vector2 bounds = targetCenter + objCenter;

            if(absDistance.X < bounds.X && absDistance.Y < bounds.Y)
            {
                return true;
            }
            return false;
        }

        public static void ApplyGravity()
        {
            foreach(GameObject obj in GameObjectFactory.objects)
            {
                obj.Accelerate(gravity);
            }
        }

        public static void Update(GameTime gameTime)
        {
            ApplyGravity();
            UpdatePositions(gameTime);
        }

        public static void UpdatePositions(GameTime gameTime)
        {
            foreach (GameObject obj in GameObjectFactory.objects)
            {
                obj.Update(gameTime);
            }
        }
    }
}
