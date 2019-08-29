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

        private float rotaOffset;
        private float posOffsetDistance;
        private float posOffsetDirection;

        private float muzzleOffsetDistance;
        private float muzzleOffsetDirection;
        private SpaceObject parent;
        private Texture2D texture;
        private Texture2D bulletTexture;
        private float shootCD = 10;
        private float currentShootCD = 10;
        private float spread;
        Random spreadRNG;

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


        public void Shoot(List<SpaceObject> newObjects)
        {
            if (currentShootCD == 0)
            {
                Console.WriteLine("muzzle:" + muzzleOffsetDistance + " , " + muzzleOffsetDirection);
                Console.WriteLine("pos:" + posOffsetDistance + " , " + posOffsetDirection);
                Bullet newShot = new Bullet(parent.pos.X + (float)Math.Cos(parent.rotation + muzzleOffsetDirection) * muzzleOffsetDistance, 
                       parent.pos.Y + (float)Math.Sin(parent.rotation + muzzleOffsetDirection) * muzzleOffsetDistance, 
                       parent.rotation + ((float)spreadRNG.NextDouble()-.5f)*spread, 
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
