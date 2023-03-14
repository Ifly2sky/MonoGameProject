using CryStal.Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine.Models
{
    public class Cell
    {
        public List<GameObject> Objects = new();
    }
    public class Grid
    {
        private Cell[,] cells;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Grid(int x, int y)
        {
            Width = x;
            Height = y;

            cells = new Cell[Width, Height];
        }

        public void UpdateGrid()
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
        public Cell GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return null;
            }
            return cells[x, y];
        }
    }
}
