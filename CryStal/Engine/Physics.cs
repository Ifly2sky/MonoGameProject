﻿using CryStal.Engine.Factories;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CryStal.Engine
{
    public static class Physics
    {
        const float half = 0.5f;
        public static readonly Vector2 gravity = new(0f, 25f);
        private static GraphicsDevice _device;

        public static void Update(GameTime gameTime, GraphicsDevice graphics, Grid gameGrid)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // gets deltatime
            UpdatePositions(deltaTime, graphics); //updates object positions
            gameGrid.UpdateGrid();
            UpdateCollitionsWithSubsteps(8, gameGrid);// solves collitions more than once to make physics more accurate

        }

        private static void CalculateCollition(GameObject obj, GameObject target)
        {
            //gets distance between objects and its absolute value
            Vector2 distance = obj.Center - target.Center;
            Vector2 absDistance = distance.Abs();

            //gets the minimum x and y distance of the objects
            Vector2 bounds = target.Hitbox.Size * half + obj.Hitbox.Size * half;

            if (absDistance.X < bounds.X && absDistance.Y < bounds.Y)
            {
                Vector2 overlap = bounds - absDistance; // size of the overlap

                // gets side in which the overlap is the largest
                Vector2 difference = new((overlap.Y < overlap.X) ? 0 : overlap.X, (overlap.Y > overlap.X) ? 0 : overlap.Y);
                Vector2 direction = new(distance.X < 0 ? -1 : 1, distance.Y < 0 ? -1 : 1);

                //moves both half of the overlap
                obj.Position += difference * direction * half;
                target.Position -= difference * direction * half;
            }
        }
        private static void CalculateCollition(Cell objCell, Cell targetCell)
        {
            foreach(GameObject obj in objCell.Objects)
            {
                foreach (GameObject target in targetCell.Objects)
                {
                    if(obj != target)
                    {
                        CalculateCollition(obj, target);
                    }
                }
            }
        }
        private static void UpdatePositions(float deltaTime, GraphicsDevice graphics)
        {
            foreach (GameObject obj in GameObjectFactory.objects)
            {
                if (obj.HasGravity)
                    obj.Accelerate(gravity);
                obj.Update(deltaTime);
                obj.Clamp(graphics);
                _device = graphics;
            }
        }
        private static void UpdateCollitions(Grid grid, int maxHeight, int maxWidth, int minHeight = 0, int minWidth = 0)
        {
            for(int x = minWidth; x <= maxWidth; x++)
            {
                for (int y = minHeight; y <= maxHeight; y++)
                {
                    Cell objCell = grid.GetCell(x, y);

                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (x + dx >= minWidth && x + dx <= maxWidth && y + dy >= minHeight && y + dy <= maxHeight)
                            {
                                Cell targetCell = grid.GetCell(x + dx, y + dy);

                                if (objCell != null && targetCell != null)
                                {
                                    CalculateCollition(objCell, targetCell);
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void UpdateCollitionsWithSubsteps(int sub_steps, Grid grid)
        {
            List<Task> tasks = new();

            int dividedGridHeight = (int)Math.Floor(grid.Height * 0.25);

            for(int i = 0; i < sub_steps; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => UpdateCollitions(grid, dividedGridHeight, grid.Width, 0, 0)));
                tasks.Add(Task.Factory.StartNew(() => UpdateCollitions(grid, dividedGridHeight * 2, grid.Width, dividedGridHeight, 0)));
                tasks.Add(Task.Factory.StartNew(() => UpdateCollitions(grid, dividedGridHeight * 3, grid.Width, dividedGridHeight * 2, 0)));
                tasks.Add(Task.Factory.StartNew(() => UpdateCollitions(grid, dividedGridHeight * 4 + 1, grid.Width, dividedGridHeight * 3, 0)));
            }
            Task.WaitAll(tasks.ToArray());
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
