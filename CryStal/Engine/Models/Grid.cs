/*using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public static int Width = 40;
        public static int Height = 20;

        static Grid()
        {
            cells = new Cell[Width, Height];
        }
        public static void UpdateGrid()
        {
            cells = new Cell[Width, Height];

           foreach(GameObject obj in GameObject.allObjects)
           {
                if(obj == null)
                {
                    continue;
                }
                if(obj.CollitionHandler.GetCollitionStateName() == "Passable")
                {
                    continue;
                }

                int x = (int)Math.Floor(obj.Position.X * Game1.INVERCETILESIZE);
                int y = (int)Math.Floor(obj.Position.Y * Game1.INVERCETILESIZE);

                if(x >= Width || y >= Height || x<0 || y<0)
                {
                    continue;
                }

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
        public static Size GetGridCoordinates(Vector2 worldCoordinates)
        {
            return new Size((int)Math.Floor(worldCoordinates.X * Game1.INVERCETILESIZE), (int)Math.Floor(worldCoordinates.Y * Game1.INVERCETILESIZE));
        }
    }
}*/
