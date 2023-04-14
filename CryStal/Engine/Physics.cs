using CryStal.Engine.Factories;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CryStal.Engine
{
    public static class Physics
    {
        public static readonly Vector2 gravity = new(0f, 50f); 

        public static void Update(GameTime gameTime, GraphicsDevice graphics)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // gets deltatime
            UpdatePositions(deltaTime, graphics); //updates object positions
            Grid.UpdateGrid();
            UpdateCollitionsWithSubsteps(8);// solves collitions more than once to make physics more accurate
        }

        private static void CalculateCollition(GameObject obj, GameObject target)
        {
            //gets distance between objects and its absolute value
            Vector2 distance = obj.Center - target.Center;
            Vector2 absDistance = distance.Abs();

            //gets the minimum x and y distance of the objects
            Vector2 bounds = target.Hitbox.Size * 0.5f + obj.Hitbox.Size * 0.5f;

            if (absDistance.X < bounds.X && absDistance.Y < bounds.Y)
            {
                Vector2 overlap = bounds - absDistance; // size of the overlap

                // gets side in which the overlap is the largest
                Vector2 difference = new((overlap.Y < overlap.X) ? 0 : overlap.X, (overlap.Y > overlap.X) ? 0 : overlap.Y);
                Vector2 direction = new(distance.X < 0 ? -1 : 1, distance.Y < 0 ? -1 : 1);

                //moves both half of the overlap
                bool objectIsPhysicsObject = obj is PhysicsObject, targetIsPhyicsObject = target is PhysicsObject;

                if (objectIsPhysicsObject && targetIsPhyicsObject)
                {
                    obj.Position += difference * direction * 0.5f;
                    target.Position -= difference * direction * 0.5f;
                    return;
                }

                if(objectIsPhysicsObject)
                {
                    obj.Position += difference * direction;
                    return;
                }
                target.Position -= difference * direction;
            }
        }
        private static void CalculateCollition(Cell objCell, Cell targetCell)
        {
            foreach(GameObject obj in objCell.Objects)
            {
                foreach (GameObject target in targetCell.Objects)
                {
                    if(
                        obj != target && 
                        target.CollisionType == CollitionType.Impassable && 
                        obj.CollisionType == CollitionType.Impassable
                      )
                    {
                        CalculateCollition(obj, target);
                    }
                }
            }
        }
        private static void UpdatePositions(float deltaTime, GraphicsDevice graphics)
        {
            foreach (PhysicsObject obj in GameObjectFactory.objects.OfType<PhysicsObject>())
            {
                if (obj.HasGravity)
                    obj.Accelerate(gravity);
                obj.Update(deltaTime);
                obj.Clamp(graphics);
            }
        }
        private static void UpdateCollitions(int maxHeight, int maxWidth, int minHeight = 0, int minWidth = 0)
        {
            for(int x = minWidth; x <= maxWidth; x++)
            {
                for (int y = minHeight; y <= maxHeight; y++)
                {   
                    Cell objCell = Grid.GetCell(x, y);

                    if(objCell == null)
                    {
                        continue;
                    }

                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (x + dx >= minWidth && x + dx <= maxWidth && y + dy >= minHeight && y + dy <= maxHeight)
                            {
                                Cell targetCell = Grid.GetCell(x + dx, y + dy);

                                if (targetCell != null)
                                {
                                    CalculateCollition(objCell, targetCell);
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void UpdateCollitionsWithSubsteps(int sub_steps)
        {
            Task[] tasks = new Task[4];

            int dividedGridWidth = (int)(Grid.Width * 0.5);
            int dividedGridHeight = (int)(Grid.Height * 0.5);

            for (int i = 0; i < sub_steps; i++)
            {
                tasks[0] = Task.Factory.StartNew(() => UpdateCollitions(dividedGridHeight, dividedGridWidth));
                tasks[1] = Task.Factory.StartNew(() => UpdateCollitions(dividedGridHeight, dividedGridWidth * 2, 0, dividedGridWidth));
                tasks[2] = Task.Factory.StartNew(() => UpdateCollitions(dividedGridHeight * 2, dividedGridWidth, dividedGridHeight, 0));
                tasks[3] = Task.Factory.StartNew(() => UpdateCollitions(dividedGridHeight * 2, dividedGridWidth * 2, dividedGridHeight, dividedGridWidth));
            }
            Task.WaitAll(tasks);
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
