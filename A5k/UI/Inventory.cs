using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5k.UI
{
    class Inventory:UIElement
    {
        Item[] inventoryTiles = new Item[20];
        //Texture2D tile;
        //Texture2D background;
        //View view;
        //Vector2 position;

        public Inventory(View v):base(v)
        {
            this.setTexture("PNG\\UI\\metalPanel_blue.png", true, false);
            this.setSize(650, 500);
            for(int i = 0; i<20; i++)
            {
                UIElement tile = new UIElement(v);
                tile.setParent(this);
                tile.setPos(100*(i%5), 100*(i/5));
                tile.setSize(100, 100);
                tile.setTexture("PNG\\UI\\glassPanel_corners.png", true, false);
                this.addChild(tile);
            }
            
        }


        override public Vector2 getPos()
        {
            return position;
        }
        /*
        public void Draw()
        {
            
            SpriteDrawer.DrawUI(background, position, new Vector2(4,4), Color.White, Vector2.Zero, 0);

            SpriteDrawer.DrawUI(tile, new Vector2(50, 50), new Vector2(4, 4), Color.White, Vector2.Zero, 0);
        }
        */
    }
}
