using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine.Models.PhysicsObjects
{
    public class AABB
    {
        float _top;
        float _bottom;
        float _left;
        float _right;

        public float Top { get {return _top; } set { _top = value; } }
        public float Bottom { get {return _bottom; } set { _bottom = value; } }
        public float Left { get {return _left; } set { _left = value; } }
        public float Right { get {return _right; } set { _right = value; } }

        public AABB(float top, float bottom, float left, float right)
        {
            _top = top;
            _bottom = bottom;
            _left = left;
            _right = right;
        }
        public void setBounds(float top, float bottom, float left, float right)
        {
            _top = top;
            _bottom = bottom;
            _left = left;
            _right = right;
        }
    }
}
