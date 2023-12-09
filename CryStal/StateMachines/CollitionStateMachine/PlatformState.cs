using CryStal.Engine;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.StateMachines.CollitionStateMachine
{
    /// <summary>
    /// not reccomended for gameObjects
    /// </summary>
    internal class PlatformState : CollitionState
    {
        internal override void HandleCollition(GameObject obj, GameObject target)
        {
            //gets distance between objects and its absolute value
            Vector2 distance = obj.Center - (target.Position + new Vector2(Game1.TileSize * 0.5f, 0));
            Vector2 absDistance = distance.Abs();

            //gets the minimum x and y distance of the objects
            Vector2 bounds = target.Hitbox.Size * 0.5f + obj.Hitbox.Size * 0.5f;

            if (obj.Position.Y < target.Position.Y && absDistance.Y < bounds.Y)
            {
                Vector2 overlap = bounds - absDistance;  //overlap

                // gets side in which the overlap is the largest
                Vector2 direction = new(1, distance.Y < 0 ? -1 : 1);
                Vector2 difference = new(0, overlap.Y);
                target.Position += difference * direction;
            }
        }
    }
}
