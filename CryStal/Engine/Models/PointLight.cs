using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CryStal.Engine.Models
{
    public class PointLight
    {
        public Vector2 Position;

        public float constant;
        public float linear;
        public float quadratic;

        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;

        public PointLight(Vector2 Position, float constant, float linear, float quadratic, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            this.Position = Position;
            this.constant = constant;
            this.linear = linear;
            this.quadratic = quadratic;
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
        }

        public void Use(Effect shader)
        {

            shader.Parameters["lightPosition"].SetValue(Position);
            
            shader.Parameters["constantT"].SetValue(constant);
            shader.Parameters["linearT"].SetValue(linear);
            shader.Parameters["quadraticT"].SetValue(quadratic);
            
            shader.Parameters["lightAmbient"].SetValue(ambient);
            shader.Parameters["lightDiffuse"].SetValue(diffuse);
        }
    }
}
