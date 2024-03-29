﻿using CryStal.Engine;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.StateMachines.CollitionStateMachine
{
    internal class ImpassableState:CollitionState
    {
        internal override string Name
        {
            get { return "Impassable"; }
        }
        internal override void HandleCollition(GameObject obj, GameObject target)
        {
            //gets distance between objects and its absolute value
            Vector2 distance = obj.Center - target.Center;
            Vector2 absDistance = distance.Abs();

            //gets the minimum x and y distance of the objects
            Vector2 bounds = target.Hitbox.Size * 0.5f + obj.Hitbox.Size * 0.5f;

            if (absDistance.X < bounds.X && absDistance.Y < bounds.Y)
            {

                Vector2 overlap = bounds - absDistance;  //overlap

                // gets side in which the overlap is the largest
                Vector2 direction = new(distance.X < 0 ? -1 : 1, distance.Y < 0 ? -1 : 1);
                Vector2 difference = new((overlap.Y < overlap.X) ? 0 : overlap.X, (overlap.Y > overlap.X) ? 0 : overlap.Y);

                if (target is Tile)
                {
                    if(target.CollitionHandler.GetCollitionStateName() == "Platform")
                    {
                        return;
                    }
                    if (target.CollitionHandler.GetCollitionStateName() == "Spike")
                    {
                        return;
                    }
                    Vector2 tileDistance = target.VectorDistanceTo(obj.LastCenter);
                    difference = new((tileDistance.Y > tileDistance.X) ? 0 : overlap.X, (tileDistance.Y < tileDistance.X) ? 0 : overlap.Y);
                    obj.Position += difference * direction;
                    return;
                }

                obj.Position += difference * direction * 0.5f;
                target.Position -= difference * direction * 0.5f;
            }
        }
    }
}
