using CryStal.Engine;
using CryStal.Engine.Models;
using CryStal.Entities;
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
        internal override string Name
        {
            get { return "Platform"; }
        }
        internal override void HandleCollition(GameObject obj, GameObject target)
        {
            if (obj.Position.Y < target.Position.Y || target.LastCenter.Y > obj.Position.Y)
            {
                return;
            }
            //gets distance between objects and its absolute value
            Vector2 distance = obj.Center - target.Center;//-obj.Center + (target.Center + new Vector2(Game1.TileSize * 0.5f, 0));
            Vector2 absDistance = distance.Abs();

            //gets the minimum x and y distance of the objects
            Vector2 bounds = target.Hitbox.Size * 0.5f + obj.Hitbox.Size * 0.5f;

            if (absDistance.X < bounds.X && absDistance.Y < bounds.Y)
            {
                if(target is Player)
                {
                    Player player = (Player)target; 
                    if(player.stateName == "JumpingState")
                    {
                        return;
                    }
                }
                Vector2 overlap = bounds - absDistance;  //overlap

                // gets side in which the overlap is the largest
                Vector2 direction = new(0, -1);
                Vector2 difference = new(0, overlap.Y);
                target.Position += difference * direction;
            }
        }
    }
}
