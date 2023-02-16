using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine.Models
{
    public class Level
    {
        private int[,] _levelTiles;
        string _levelPath;

        public int[,] LevelTiles
        {
            get { return _levelTiles; }
            set { _levelTiles = value; }
        }

        public Level(string path)
        {
            _levelPath = path;
        }
    }
}
