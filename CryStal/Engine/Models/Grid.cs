using CryStal.Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine.Models
{
    public class Cell
    {
        public List<GameObject> Objects = new();
    }
    public static class Grid
    {
        private static Cell[,] cells;

        public static int Width;
        public static int Height;

        static Grid()
        {
            Width = 40;
            Height = 20;

            cells = new Cell[Width, Height];
        }

        public static void UpdateGrid()
        {
            cells = new Cell[Width, Height];

            foreach (GameObject obj in GameObjectFactory.objects)
            {
                int x = (int)Math.Floor(obj.Position.X * Game1.InverseTileSize);
                int y = (int)Math.Floor(obj.Position.Y * Game1.InverseTileSize);

                if (cells[x, y] != null)
                {
                    cells[x, y].Objects.Add(obj);
                }
                else
                {
                    cells[x, y] = new Cell();
                    cells[x, y].Objects.Add(obj);
                }
            }
        }
        public static Cell GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return null;
            }
            return cells[x, y];
        }
        //world coordinate can be x or y
        public static Vector2 GetGridCoordinates(Vector2 worldCoordinate)
        {
            return new Vector2((int)Math.Floor(worldCoordinate.X * Game1.InverseTileSize), (int)Math.Floor(worldCoordinate.Y * Game1.InverseTileSize));
        }
    }
}
