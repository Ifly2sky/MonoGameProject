using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine
{
    public static class LevelHandler
    {
        static Level level = null;
        static string currentLevelPath;
        static string currentEntityPath;

        public static void InitializeLevel(IServiceProvider services)
        {
            level = new Level(services);
        }

        public static void LoadLevel(string levelName)
        {
            level.Unload();

            currentLevelPath = GetLevelPath(levelName);
            using (Stream fileStream = TitleContainer.OpenStream(currentLevelPath))
                level.LoadLevel(fileStream);

            currentEntityPath = GetEntityDataPath(levelName);
            using (Stream fileStream = TitleContainer.OpenStream(currentEntityPath))
                level.SetEntities(fileStream);
        }
        public static void DrawLevel(SpriteBatch spriteBatch)
        {
            level.DrawLevel(spriteBatch);
        }
        private static string GetLevelPath(string levelName)
        {
            return levelName switch
            {
                "Demo" => "Content/Level00.txt",
                "Demo2" => "Content/Level00(2).txt",
                _ => null
            };
        }
        private static string GetEntityDataPath(string levelName)
        {
            return levelName switch
            {
                "Demo" => "Content/Level00entities.txt",
                "Demo2" => "Content/Level00entities.txt",
                _ => null
            };
        }
    }
}
