using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;


namespace A5k.Weapons
{
    class Missile:SpaceObject
    {
        //private float posX, posY;

        private float xVel, yVel;

        private Vector2 velocity;

        private float speed = .1f;

        private float maxSpeed = 8; 

        private Texture2D texture;
        private float textureScale = .5f;
        float duration = 120;
        SpaceObject target;

        public Missile(float spawnPosX, float spawnPosY, float spawnRotation, Texture2D shipTexture, Faction fac, SpaceObject target)
        {
            pos = new Vector2(spawnPosX, spawnPosY);
            rotation = spawnRotation;
            xVel = (float)Math.Cos(spawnRotation)*(speed+3);
            yVel = (float)Math.Sin(spawnRotation)*(speed+3);
            texture = shipTexture;
            faction = fac;
            this.target = target;
            velocity = new Vector2(xVel, yVel);
            
        }

        override public void Update(List<SpaceObject> newObjects)
        {
            /*
             Styring/AI
             */
            if (!(target is null))
            {
                Vector2 targetVector = target.pos - pos;
                targetVector.NormalizeFast();
                targetVector = targetVector * maxSpeed;

                targetVector = targetVector - velocity;

                targetVector.NormalizeFast();

                targetVector = targetVector * speed ;

                velocity = velocity + targetVector;
            }
            else
            {
                velocity = velocity + Vector2.NormalizeFast(velocity) * speed;
                
            }
            if (velocity.LengthSquared > maxSpeed*maxSpeed)
            {
                velocity.NormalizeFast();
                velocity = velocity * maxSpeed;
            }

            pos.X += velocity.X;
            pos.Y += velocity.Y;
            duration--;
            if (duration < 0) isdead = true;
            rotation = (float)Math.Atan2(velocity.Y, velocity.X);

        }

        override public void Draw()
        {
            SpriteDrawer.Draw(texture, pos, Vector2.One*textureScale, Color.Azure, new Vector2(((float)texture.Width) / 2, ((float)texture.Height) / 2), rotation - (float)Math.PI / 2);
        }

        /*
        public void setSpeed(float newSpeed)
        {
            xVel = newSpeed * xVel / speed;
            yVel = newSpeed * yVel / speed;
            speed = newSpeed;
        }
        */
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
