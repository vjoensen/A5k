using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace A5k
{

    class View
    {
        private Vector2 position;
        public Vector2 viewSize;
        public float zoom;
        public double rotation;



        public View(Vector2 startPosition, Vector2 viewSize, float startZoom = 1f, double rotation = 0.0)
        {
            this.position = startPosition;
            this.viewSize = viewSize;
            
            this.zoom = startZoom;
            this.rotation = rotation;
        }

        public void Update()
        {

        }

        public float getX()
        {
            return position.X;
        }
        public float getY()
        {
            return position.Y;
        }
        public void SetPosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }

        public Matrix4 ApplyTransforms()
        {
           
            Matrix4 transform = Matrix4.Identity;
            //transform = Matrix4.CreateOrthographic(640.0f, 480.0f, 1.0f, 100.0f);
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-position.X, -position.Y, 0));
            transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-(float)rotation));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(zoom, zoom, 1.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(1f / viewSize.X, 1f / viewSize.Y, 0));
            //transform = Matrix4.Mult(transform, Matrix4.CreateOrthographic(640.0f, 480.0f, 0.0f, 100.0f));


            return transform;
        }

    }
}
