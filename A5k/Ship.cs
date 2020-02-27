using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using A5k.Weapons;


namespace A5k
{
    class Ship:SpaceObject
    {
        //private float posX, posY;


        private float xVel, yVel;

        private float maxSpeed;
        private float acceleration;
        private Texture2D texture;

        private SpaceObject target = null;

        private int framesSinceDamage = 9999;

        private Weapon weapon;

        public Ship(float spawnPosX, float spawnPosY, float spawnRotation, Texture2D shipTexture)
        {
            pos = new Vector2(spawnPosX, spawnPosY);
            rotation = spawnRotation;
            xVel = 0;
            yVel = 0;
            acceleration = 5;
            maxSpeed = 20;
            texture = shipTexture;
            radius = Math.Max(shipTexture.Height, shipTexture.Width)/2;
            faction = Faction.Team2;
            weapon = new Weapon(this, 0, 0, 0, new Texture2D(0,0,0), SpriteDrawer.LoadTexture("PNG\\Lasers\\laserRed01.png", true, false), 0, 0);
            weapon.setAttackSpeed(.2f);
        }

        override public void Update(List<SpaceObject> newObjects)
        {
        /*
         Styring/AI
         */

            if(target != null)
            {
                float maxRange = 300;
                if(Vector2.DistanceSquared(this.pos, target.pos) < maxRange*maxRange)
                {
                    weapon.ShootDirection(newObjects, (float)Math.Atan2(target.pos.Y - this.pos.Y, target.pos.X - this.pos.X));
                }
                else
                {

                }


            }
            weapon.Update(newObjects);
            pos.X += xVel;
            pos.Y += yVel;

            rotation += .02f;
            if(framesSinceDamage <9999) framesSinceDamage++;
        }

        override public void Draw()
        {
            SpriteDrawer.Draw(texture, pos, Vector2.One, Color.White, new Vector2(((float)texture.Width) / 2, ((float)texture.Height) / 2), rotation - (float)Math.PI / 2);
            if(framesSinceDamage < 30 )
            {
                SpriteDrawer.DrawShield(texture, pos, Vector2.One*1.1f, Color.Azure, new Vector2(((float)texture.Width) / 2, ((float)texture.Height) / 2), rotation - (float)Math.PI / 2, (30f-framesSinceDamage)/40);
            }
        }

        public override void TakeDamage(float damage, SpaceObject source)
        {
            framesSinceDamage = 0;

            //throw new NotImplementedException();
        }

        public void setTarget(SpaceObject newTarget)
        {
            this.target = newTarget;
        }

        public override void Collide(SpaceObject collider)
        {
            //throw new NotImplementedException();
        }
    }
}
