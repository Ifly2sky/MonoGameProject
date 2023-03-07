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

        public static List<GameObject> PhysicsObjects => GameObjectFactory.objects; //.Where(x => x.HasGravity && x is not Tile).ToList();

        public static void CalculateCollition(GameObject obj, GameObject target)
        {
            //gets distance between objects and its absolute value
            Vector2 objCenter = obj.Hitbox.Size * half;
            Vector2 targetCenter = target.Hitbox.Size * half;
            Vector2 distance = (obj.Position + obj.Hitbox.Position + objCenter)-(target.Position + target.Hitbox.Position + targetCenter);
            Vector2 absDistance = new(Math.Abs(distance.X), Math.Abs(distance.Y));

            //gets the minimum x and y distance of the objects
            Vector2 bounds = targetCenter + objCenter;

            if (absDistance.X < bounds.X && absDistance.Y < bounds.Y)
            {
                Vector2 overlap = (bounds - absDistance); // size of the overlap

                // side in which the overlap is the largest
                Vector2 difference = new((overlap.Y > overlap.X)? overlap.X : 0, (overlap.Y > overlap.X) ? 0 : overlap.Y);

                //resets velocity to stop objects movement
                obj.ResetVelocity();

                //subracts half of the overlap from position
                obj.Position += difference * half;
            }
        }

        private static void ApplyGravity()
        {
            foreach(GameObject obj in PhysicsObjects)
            {
                obj.Accelerate(gravity);
            }
        }

        public static void Update(GameTime gameTime, GraphicsDevice graphics)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // gets deltatime
            ApplyGravity(); //applys gravity
            //SolveCollitionsWithSubsteps(8);// solves collitions more than once to make physics more accurate
            UpdatePositions(deltaTime, graphics); //updates object positions

        }

        private static void UpdatePositions(float deltaTime, GraphicsDevice graphics)
        {
            foreach (GameObject obj in PhysicsObjects)
            {
                obj.Update(deltaTime);
                obj.Clamp(graphics);
            }
        }

        private static void SolveCollitions()
        {
            foreach(GameObject obj in PhysicsObjects)
            {
                foreach (GameObject target in PhysicsObjects)
                {
                    if (obj != target)
                    {
                        CalculateCollition(obj, target);
                    }
                }
            }
        }

        private static void SolveCollitionsWithSubsteps(int sub_steps)
        {
            for(int i = 0; i < sub_steps; i++)
            {
                SolveCollitions();
            }
        }
    }
}
