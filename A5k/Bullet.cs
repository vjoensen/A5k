using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;


namespace A5k
{
    class Bullet:SpaceObject
    {
        //private float posX, posY;


        private float xVel, yVel;

        private float speed = 8;

        private Texture2D texture;
        private float textureScale = .5f;
        float duration = 120;
        

        public Bullet(float spawnPosX, float spawnPosY, float spawnRotation, Texture2D shipTexture, Faction fac)
        {
            pos = new Vector2(spawnPosX, spawnPosY);
            rotation = spawnRotation;
            xVel = (float)Math.Cos(spawnRotation)*speed;
            yVel = (float)Math.Sin(spawnRotation)*speed;
            texture = shipTexture;
            faction = fac;
            
        }

        override public void Update(List<SpaceObject> newObjects)
        {
        /*
         Styring/AI
         */
            pos.X += xVel;
            pos.Y += yVel;
            duration--;
            if (duration < 0) isdead = true;
            //rotation = ??

        }

        override public void Draw()
        {
            SpriteDrawer.Draw(texture, pos, Vector2.One*textureScale, Color.Azure, new Vector2(((float)texture.Width) / 2, ((float)texture.Height) / 2), rotation - (float)Math.PI / 2);
        }

        public void setSpeed(float newSpeed)
        {
            xVel = newSpeed * xVel / speed;
            yVel = newSpeed * yVel / speed;
            speed = newSpeed;
        }

        public override void Collide(SpaceObject collider)
        {
            collider.TakeDamage(20, this);
            isdead = true;
        }

        public override void TakeDamage(float damage, SpaceObject source)
        {
            isdead = true;
        }
    }
}
