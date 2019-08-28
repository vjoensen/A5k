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

        private float speed = 20;

        private Texture2D texture;
        //float radius = 9;
        float radius = 9;
        

        public Bullet(float spawnPosX, float spawnPosY, float spawnRotation, Texture2D shipTexture)
        {
            pos = new Vector2(spawnPosX, spawnPosY);
            rotation = spawnRotation;
            xVel = (float)Math.Cos(spawnRotation)*speed;
            yVel = (float)Math.Sin(spawnRotation)*speed;
            texture = shipTexture;

        }

        override public void Update(List<SpaceObject> newObjects)
        {
        /*
         Styring/AI
         */
            pos.X += xVel;
            pos.Y += yVel;

            //rotation = ??

        }

        override public void Draw()
        {
            SpriteDrawer.Draw(texture, pos, Vector2.One, Color.Azure, new Vector2(((float)texture.Width) / 2, ((float)texture.Height) / 2), rotation - (float)Math.PI / 2);
        }


    }
}
