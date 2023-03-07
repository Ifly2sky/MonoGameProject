using CryStal.Engine.Factories;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CryStal.Engine
{
    public static class Physics
    {
        const float half = 0.5f;
        public static readonly Vector2 gravity = new(0f, 25f);

        public static List<GameObject> PhysicsObjects => GameObjectFactory.objects; //.Where(x => x.HasGravity && x is not Tile).ToList();

        public static void CalculateCollition(GameObject obj, GameObject target)
        {
            //gets distance between objects and its absolute value
            Vector2 distance = target.Center - obj.Center;
            Vector2 absDistance = distance.Abs(); //Abs is custom

            //gets the minimum x and y distance of the objects
            Vector2 bounds = target.Hitbox.Size * half + obj.Hitbox.Size * half;

            if (absDistance.X < bounds.X && absDistance.Y < bounds.Y)
            {
                Vector2 overlap = bounds - absDistance; // size of the overlap

                // gets side in which the overlap is the largest
                Vector2 difference = new((overlap.Y < overlap.X)? 0 : overlap.X, (overlap.Y > overlap.X) ? 0 : overlap.Y);

                //moves both half of the overlap
                obj.Position += difference * half;
                target.Position -= difference * half;
            }
        }

        public static void Update(GameTime gameTime, GraphicsDevice graphics)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // gets deltatime
            UpdatePositions(deltaTime, graphics); //updates object positions
            SolveCollitionsWithSubsteps(8);// solves collitions more than once to make physics more accurate

        }

        private static void UpdatePositions(float deltaTime, GraphicsDevice graphics)
        {
            foreach (GameObject obj in PhysicsObjects)
            {
                if(obj.HasGravity)
                    obj.Accelerate(gravity);
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

        /// <summary>
        /// Returns absolute value of vector2
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
        }
    }
}
