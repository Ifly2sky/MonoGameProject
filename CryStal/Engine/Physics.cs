﻿using CryStal.Engine.Factories;
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
            //gets distance between objects
            Vector2 distance = obj.Center - target.Center;
            Vector2 absDistance = new(Math.Abs(distance.X), Math.Abs(distance.Y));

            //gets the minimum x and y distance of the objects
            Vector2 bounds = target.Hitbox.Size * half + obj.Hitbox.Size * half;

            if (absDistance.X < bounds.X && absDistance.Y < bounds.Y) // checks if both x and y is smaller than the minimum distance
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
            SolveCollitionsWithSubsteps(8);
            UpdatePositions(deltaTime, graphics); //updates object positions

            //SolveCollitions();
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
