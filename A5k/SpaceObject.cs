using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5k
{
    public enum Faction
    {
        Team1,
        Team2
    }
    abstract class SpaceObject
    {
        protected Faction faction;
        protected bool isdead = false;
        protected float radius = 1;
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
        public bool isDead()
        {
            return isdead;
        }
        public Faction getFaction()
        {
            return faction;
        }
        

        public abstract void TakeDamage(float damage, SpaceObject source);
        public abstract void Collide(SpaceObject collider);
        public abstract void Draw();
        public abstract void Update(List<SpaceObject> newObjects);
    }
}
