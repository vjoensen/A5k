using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5k
{
    abstract class SpaceObject
    {

        float radius = 1;
        public Vector2 pos;
        public float rotation;

        public float getRadius()
        {
            return radius;
        }
        Vector2 getPos()
        {
            return pos;
        }

        public abstract void Draw();
        public abstract void Update(List<SpaceObject> newObjects);
    }
}
