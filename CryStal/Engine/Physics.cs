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
        public static readonly Vector2 gravity = new(0f, 100f);

        public static List<GameObject> PhysicsObjects => GameObjectFactory.objects; //.Where(x => x.HasGravity && x is not Tile).ToList();

        public static void CalculateCollition(GameObject obj, GameObject target)
        {
<<<<<<< HEAD
            //gets distance between objects and its absolute value
            Vector2 objCenter = obj.Hitbox.Size * half;
            Vector2 targetCenter = target.Hitbox.Size * half;
            Vector2 distance = (obj.Position + obj.Hitbox.Position + objCenter)-(target.Position + target.Hitbox.Position + targetCenter);
            Vector2 absDistance = new(Math.Abs(distance.X), Math.Abs(distance.Y));

            //gets the minimum x and y distance of the objects
            Vector2 bounds = targetCenter + objCenter;

            if (absDistance.X < bounds.X && absDistance.Y < bounds.Y)
=======
            //gets distance between objects
            Vector2 distance = obj.Center - target.Center;
            Vector2 absDistance = new(Math.Abs(distance.X), Math.Abs(distance.Y));

            //gets the minimum x and y distance of the objects
            Vector2 bounds = target.Hitbox.Size * half + obj.Hitbox.Size * half;

            if (absDistance.X < bounds.X && absDistance.Y < bounds.Y) // checks if both x and y is smaller than the minimum distance
>>>>>>> 36647cb1002374f365c14fbf0591d9a122e5e5bc
            {
                //gets overlap
                Vector2 overlap = bounds - absDistance;
                Vector2 difference = (overlap.Y > overlap.X)? new Vector2(overlap.X, 0) : new Vector2(0, overlap.Y);

                //resets velocity before changing positions
                obj.ResetVelocity();

                //subracts half of the overlap from object positions
                obj.Position += difference * half;
                target.Position -= difference * half;
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

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ApplyGravity();
            UpdatePositions(deltaTime);
            SolveCollitionsWithSubsteps(8);// solves collitions more than once to make physics more accurate
<<<<<<< HEAD
            UpdatePositions(deltaTime, graphics); //updates object positions
=======
            ApplyConstraint(graphics);
>>>>>>> 36647cb1002374f365c14fbf0591d9a122e5e5bc

            //SolveCollitions();
        }

        private static void UpdatePositions(float deltaTime)
        {
            foreach (GameObject obj in PhysicsObjects)
            {
                obj.Update(deltaTime);
<<<<<<< HEAD
                obj.Clamp(graphics);
=======
            }
        }
        private static void ApplyConstraint(GraphicsDevice graphics)
        {
            foreach(GameObject obj in PhysicsObjects)
            {
                obj.Position = new Vector2(MathHelper.Clamp(obj.Position.X, 0, graphics.Viewport.Width - Game1.TileSize), MathHelper.Clamp(obj.Position.Y, 0, graphics.Viewport.Height - Game1.TileSize));
>>>>>>> 36647cb1002374f365c14fbf0591d9a122e5e5bc
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
