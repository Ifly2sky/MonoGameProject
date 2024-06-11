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
        public static Action OnPhysicsFinalize = new(() => { });

        public static void Update(GameTime gameTime, GraphicsDevice graphics)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // gets deltatime
            UpdatePhysicsObjects(deltaTime, graphics); //updates object positions
            Grid.UpdateGrid();
            UpdateCollitionsWithSubsteps(8);// solves collitions more than once to make physics more accurate
        }

        private static void CalculateCollition(GameObject obj, GameObject target)
        {
            obj.CollitionHandler.HandleCollition(obj, target);
        }
        private static void CalculateCollition(Cell objCell, List<GameObject> targets)
        {
            foreach(GameObject obj 
                in objCell.Objects)
            {
                List<GameObject> sortedTargets = targets.OrderBy(x => x.DistanceTo(obj.Center)).ToList();
                foreach (GameObject target in sortedTargets)
                {
                    if(obj != target)
                    {
                        CalculateCollition(obj, target);
                    }
                }
            }
        }
        private static void UpdatePhysicsObjects(float deltaTime, GraphicsDevice graphics)
        {
            foreach (PhysicsObject obj in PhysicsObject.allPhysicsObjects)
            {
                if (obj.HasGravity)
                    obj.Accelerate(gravity);
                obj.Update(deltaTime);
                //obj.Clamp(graphics);
            }
            OnPhysicsFinalize();
        }
        private static void UpdateCollitions(int maxHeight, int maxWidth, int minHeight = 0, int minWidth = 0)
        {
            for(int x = minWidth; x <= maxWidth; x++)
            {
                for (int y = minHeight; y <= maxHeight; y++)
                {
                    Cell objCell = Grid.GetCell(x, y);
                    List<GameObject> objects = new();

                    if (objCell == null)
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
                                    objects.AddRange(targetCell.Objects);
                                }
                            }
                        }
                    }
                    CalculateCollition(objCell, objects);
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
        /// <returns>Vector2 of absolute values</returns>
        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
        }
    }
}
