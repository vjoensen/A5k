using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;


namespace A5k
{
    class Ship:SpaceObject
    {
        //private float posX, posY;


        private float xVel, yVel;

        private float maxSpeed;
        private float acceleration;
        private Texture2D texture;

        private float radius;

        public Ship(float spawnPosX, float spawnPosY, float spawnRotation, Texture2D shipTexture)
        {
            pos = new Vector2(spawnPosX, spawnPosY);
            rotation = spawnRotation;
            xVel = 0;
            yVel = 0;
            acceleration = 5;
            maxSpeed = 20;
            texture = shipTexture;
            radius = Math.Max(shipTexture.Height, shipTexture.Width);

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
            SpriteDrawer.Draw(texture, pos, Vector2.One, Color.Azure, new Vector2(((float)texture.Width) / 2, ((float)texture.Height) / 2), rotation);
        }


    }
}
