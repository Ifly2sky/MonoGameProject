using CryStal.Engine.Factories;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public static readonly Vector2 gravity = new(0f, 500f);

        public static void DoCollition(GameObject obj, GameObject target, float deltaTime)
        {
            Vector2 objCenter = obj.Hitbox.Size * half;
            Vector2 targetCenter = target.Hitbox.Size * half;
            Vector2 distance = (obj.Position + obj.Hitbox.Position + objCenter)-(target.Position + target.Hitbox.Position + targetCenter);

            Vector2 absDistance = new(Math.Abs(distance.X), Math.Abs(distance.Y));

            Vector2 bounds = targetCenter + objCenter;

            if (absDistance.X < bounds.X && absDistance.Y < bounds.Y)
            {
                Vector2 overlap = (bounds - absDistance);
                Vector2 difference = new Vector2((overlap.Y > overlap.X)? overlap.X : 0, (overlap.Y > overlap.X) ? 0 : overlap.Y);

                obj.ResetVelocity();

                obj.Position += difference * half;
            }
        }

        private static void ApplyGravity()
        {
            foreach(GameObject obj in GameObjectFactory.objects)
            {
                obj.Accelerate(gravity);
            }
        }

        public static void Update(GameTime gameTime, GraphicsDevice graphics)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ApplyGravity();
            UpdatePositions(deltaTime, graphics);
            //SolveCollitions(deltaTime);
            SolveCollitionsWithSubsteps(deltaTime, 8);
        }

        private static void UpdatePositions(float deltaTime, GraphicsDevice graphics)
        {
            foreach (GameObject obj in GameObjectFactory.objects)
            {
                obj.Update(deltaTime, graphics);
            }
        }

        private static void SolveCollitions(float deltaTime)
        {
            foreach(GameObject obj in GameObjectFactory.objects)
            {
                foreach (GameObject target in GameObjectFactory.objects)
                {
                    if (obj != target)
                    {
                        DoCollition(obj, target, deltaTime);
                    }
                }
            }
        }

        private static void SolveCollitionsWithSubsteps(float deltaTime, int sub_steps)
        {
            float subDeltaTime = deltaTime / sub_steps;
            for(int i = 0; i < sub_steps; i++)
            {
                SolveCollitions(subDeltaTime);
            }
        }
    }
}
