using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5k.Weapons
{
    class MissileLauncher:Weapon
    {
        SpaceObject target;
        public MissileLauncher(SpaceObject parent, 
            float spawnPosX, 
            float spawnPosY, 
            float spawnRotation, 
            Texture2D weaponTexture, 
            Texture2D bulletTexture, 
            float muzzleOffsetX, 
            float muzzleOffsetY) : base(parent, spawnPosX, spawnPosY, spawnRotation, weaponTexture, bulletTexture, muzzleOffsetX, muzzleOffsetY)
        {
            shootCD = 50;
        }

        public void setTarget(SpaceObject newTarget)
        {
            this.target = newTarget;
        }

        public override void Shoot(List<SpaceObject> newObjects)
        {
            if (currentShootCD == 0)
            {
                if(target is null || target.isDead())
                {
                    target = null;
                }
                Missile newShot = new Missile(parent.pos.X + (float)Math.Cos(parent.rotation + muzzleOffsetDirection) * muzzleOffsetDistance, 
                       parent.pos.Y + (float)Math.Sin(parent.rotation + muzzleOffsetDirection) * muzzleOffsetDistance, 
                       parent.rotation + ((float)spreadRNG.NextDouble()-.5f)*spread, 
                       bulletTexture, 
                       parent.getFaction(),
                       target);

                newObjects.Add(newShot);
                currentShootCD = shootCD;
            }
        }
    }
}
