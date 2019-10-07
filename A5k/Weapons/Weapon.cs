using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5k
{
    class Weapon
    {

        protected float rotaOffset;
        protected float posOffsetDistance;
        protected float posOffsetDirection;
        protected float muzzleOffsetDistance;
        protected float muzzleOffsetDirection;
        protected SpaceObject parent;
        protected Texture2D texture;
        protected Texture2D bulletTexture;
        protected float shootCD = 10;
        protected float currentShootCD = 10;
        protected float spread;
        protected Random spreadRNG;

        public Weapon(SpaceObject parent, float spawnPosX, float spawnPosY, float spawnRotation, Texture2D weaponTexture, Texture2D bulletTexture, float muzzleOffsetX , float muzzleOffsetY )
        {
            this.parent = parent;

            rotaOffset = spawnRotation;
            posOffsetDirection = (float)Math.Atan2(-spawnPosX, spawnPosY);
            posOffsetDistance = (float)Math.Sqrt(spawnPosX * spawnPosX + spawnPosY * spawnPosY);

            muzzleOffsetDirection = (float)Math.Atan2(-(spawnPosX + muzzleOffsetX), (spawnPosY + muzzleOffsetY));
            muzzleOffsetDistance = (float)Math.Sqrt(((spawnPosX + muzzleOffsetX) * (spawnPosX + muzzleOffsetX)) + ((spawnPosY + muzzleOffsetY) * (spawnPosY + muzzleOffsetY)));

            this.texture = weaponTexture;
            this.bulletTexture = bulletTexture;

            spreadRNG = new Random();
            spread = .1f;
            
        }

        public void setAttackSpeed(float atkSpeed)
        {
            shootCD = 10 / atkSpeed;
        }

        public virtual void Shoot(List<SpaceObject> newObjects)
        {
            if (currentShootCD == 0)
            {
                Bullet newShot = new Bullet(parent.pos.X + (float)Math.Cos(parent.rotation + muzzleOffsetDirection) * muzzleOffsetDistance, 
                       parent.pos.Y + (float)Math.Sin(parent.rotation + muzzleOffsetDirection) * muzzleOffsetDistance, 
                       parent.rotation + ((float)spreadRNG.NextDouble()-.5f)*spread, 
                       bulletTexture, 
                       parent.getFaction());

                newObjects.Add(newShot);
                currentShootCD = shootCD;
            }
        }

        public void ShootDirection(List<SpaceObject> newObjects, float direction)
        {
            if (currentShootCD == 0)
            {
                Bullet newShot = new Bullet(parent.pos.X + (float)Math.Cos(parent.rotation + muzzleOffsetDirection) * muzzleOffsetDistance,
                       parent.pos.Y + (float)Math.Sin(parent.rotation + muzzleOffsetDirection) * muzzleOffsetDistance,
                       direction + ((float)spreadRNG.NextDouble() - .5f) * spread,
                       bulletTexture,
                       parent.getFaction());

                newObjects.Add(newShot);
                currentShootCD = shootCD;
            }
        }

        public void Draw()
        {
            if (texture.Height!=0)
            {
                float recoilX = 0, recoilY = 0;
                recoilX = (float)Math.Cos(parent.rotation + rotaOffset) * Math.Max(currentShootCD - 5, 0);
                recoilY = (float)Math.Sin(parent.rotation + rotaOffset) * Math.Max(currentShootCD - 5, 0);
                SpriteDrawer.Draw(texture,
                    parent.pos + new Vector2((float)Math.Cos(parent.rotation + posOffsetDirection) * posOffsetDistance - recoilX, (float)Math.Sin(parent.rotation + posOffsetDirection) * posOffsetDistance - recoilY),
                    Vector2.One,
                    Color.White,
                    new Vector2(((float)texture.Width) / 2, ((float)texture.Height) / 2),
                    parent.rotation - (float)Math.PI / 2);
            }
        }

        public void Update(List<SpaceObject> newObjects)
        {
            if(currentShootCD > 0 ) currentShootCD--;
        }

    }
}
