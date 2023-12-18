using CryStal.Engine;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;

namespace CryStal.StateMachines.CollitionStateMachine
{
    internal class ImpassableTileState : CollitionState
    {
        internal override string Name
        {
            get { return "ImpassableTile"; }
        }
        internal override void HandleCollition(GameObject obj, GameObject target)
        {
            //check for multiple tiles
            if (obj is Tile && target is Tile)
            {
                // we don't need to calculate anything
                return;
            }

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

                Vector2 tileDistance;
                Vector2 difference;

                tileDistance = obj.VectorDistanceTo(target.LastCenter);
                difference = new((tileDistance.Y > tileDistance.X) ? 0 : overlap.X, (tileDistance.Y < tileDistance.X) ? 0 : overlap.Y);
                target.Position += difference * direction;
            }
        }
    }
}
