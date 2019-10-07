using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5k.Weapons
{
    class Shotgun : Weapon
    {
        private int shellCount;
        public Shotgun(SpaceObject parent, float spawnPosX, float spawnPosY, float spawnRotation, Texture2D weaponTexture, Texture2D bulletTexture, float muzzleOffsetX, float muzzleOffsetY) : base(parent, spawnPosX, spawnPosY, spawnRotation, weaponTexture, bulletTexture, muzzleOffsetX, muzzleOffsetY)
        {
            shellCount = 8;

        }

        override public void Shoot(List<SpaceObject> newObjects)
        {
            if (currentShootCD == 0)
            {
                for (int i = 0; i < shellCount; i++) { 

                    float spreadOffset = (float)spreadRNG.NextDouble()*15;
                    Bullet newShot = new Bullet(parent.pos.X + (float)Math.Cos(parent.rotation + muzzleOffsetDirection) * (muzzleOffsetDistance + spreadOffset),
                           parent.pos.Y + (float)Math.Sin(parent.rotation + muzzleOffsetDirection) * (muzzleOffsetDistance + spreadOffset),
                           parent.rotation + ((float)spreadRNG.NextDouble() - .5f) * spread,
                           bulletTexture,
                           parent.getFaction());

                    newShot.setSpeed(7 + (float)spreadRNG.NextDouble());
                    newObjects.Add(newShot);
                }
                currentShootCD = shootCD;
            }
        }
    }
}
