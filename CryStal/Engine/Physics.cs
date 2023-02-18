using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
            Vector2 objCenter = obj.Hitbox.Size * half;
            Vector2 targetCenter = target.Hitbox.Size * half;
            Vector2 distance = (obj.Position + obj.Hitbox.Position + objCenter)-(target.Position + target.Hitbox.Position + targetCenter);

            Vector2 absDistance = new(Math.Abs(distance.X), Math.Abs(distance.Y));

            Vector2 direction = new Vector2(1, 1);
            if (absDistance.X - distance.X == 0)
            {
                direction.X = -1;
            }
            if (absDistance.Y - distance.Y == 0)
            {
                direction.Y = -1;
            }

            Vector2 bounds = targetCenter + objCenter;

            if(absDistance.X < bounds.X && absDistance.Y < bounds.Y)
            {
                obj.Position -= (absDistance - bounds) * direction;
            }
        }
    }
}
