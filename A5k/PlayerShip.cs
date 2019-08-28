using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;


namespace A5k
{
    class PlayerShip:SpaceObject
    {
        //private float posX, posY;
        

        private float xVel, yVel;

        private float maxSpeed;
        private float acceleration;
        private Texture2D texture;

        private float radius;


        private int shootCD = 10;
        private int currentShootCD = 10;
        Texture2D bulletTexture;

        View view;



        public PlayerShip(float spawnPosX, float spawnPosY, float spawnRotation, Texture2D shipTexture, Texture2D bulletTexture, View view)
        {
            pos = new Vector2(spawnPosX, spawnPosY);
            rotation = spawnRotation;
            xVel = 0;
            yVel = 0;
            acceleration = 3;
            maxSpeed = 10;
            texture = shipTexture;
            this.bulletTexture = bulletTexture;
            this.view = view;

            radius = Math.Max(shipTexture.Height, shipTexture.Width);
        }

        override public void Update(List<SpaceObject> newObjects)
        {
            if (Input.KeyDown(OpenTK.Input.Key.A))
            {
                xVel -= acceleration;
            }else if (Input.KeyDown(OpenTK.Input.Key.D))
            {
                xVel += acceleration;
            }else
            {
                xVel = 0;
            }
            

            if (Input.KeyDown(OpenTK.Input.Key.W))
            {
                yVel += acceleration;
            }
            else if (Input.KeyDown(OpenTK.Input.Key.S))
            {
                yVel -= acceleration;
            }else
            {
                yVel = 0;
            }
            
            if (xVel*xVel + yVel * yVel > maxSpeed * maxSpeed)
            {
                float speed = (float)Math.Sqrt(xVel * xVel + yVel * yVel);
                xVel = xVel * maxSpeed / speed;
                yVel = yVel * maxSpeed / speed;
            }
            pos.X += xVel;
            pos.Y += yVel;

            rotation = (float)Math.Atan2(Input.mousePosition.Y + view.getY() - this.pos.Y, Input.mousePosition.X + view.getX() - this.pos.X) ;
            view.SetPosition(pos);

            if(shootCD > 0)
            {
                shootCD--;
            }

            if (Input.KeyDown(OpenTK.Input.Key.Space) && shootCD == 0)
            {
                Bullet newShot = new Bullet(this.pos.X, this.pos.Y, this.rotation, bulletTexture);
                newObjects.Add(newShot);
                shootCD = currentShootCD;
            }


        }

        override public void Draw()
        {
            SpriteDrawer.Draw(texture, pos, Vector2.One, Color.Azure, new Vector2(((float)texture.Width) / 2, ((float)texture.Height) / 2), rotation - (float)Math.PI / 2);
        }

    }
}
