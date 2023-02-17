using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine
{
    public class Physics
    {
        const float half = 0.5f;

        public static void CheckCollition(GameObject obj, GameObject target)
        {
            Vector2 objCenter = obj.Size * half;
            Vector2 targetCenter = target.Size * half;

            Vector2 distance = obj.Position + targetCenter - target.Position + targetCenter;
            Vector2 bounds = targetCenter + objCenter;

            if(distance.X < bounds.X && distance.Y < bounds.Y)
            {
                obj.Position -= bounds - distance;
            }
        }
    }
}
