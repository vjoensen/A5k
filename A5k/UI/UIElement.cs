using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5k.UI
{
    class UIElement
    {

        protected Texture2D background;
        protected View view;
        protected Vector2 position;
        protected UIElement parent;
        protected List<UIElement> children;
        float Width;
        float Height;
        Vector2 margin;
        public UIElement(View v)
        {
            view = v;
            margin = new Vector2(5, 5);
            background = SpriteDrawer.LoadTexture("PNG\\UI\\metalPanel_blue.png", true, false);
            Width = 200;
            Height = 200;
            position = new Vector2(0, 0);
            children = new List<UIElement>();
        }

        public void setPos(float x, float y)
        {
            this.position.X = x;
            this.position.Y = y;
        }

        public virtual Vector2 getPos()
        {
            return parent.getPos() + position+margin;
        }

        public void setTexture(String path, bool flipY, bool flipX)
        {
            background = SpriteDrawer.LoadTexture(path, flipY, flipX);
        }

        public void setParent(UIElement parent)
        {
            this.parent = parent;
        }

        public void addChild(UIElement newChild)
        {
            this.children.Add(newChild);
        }

        public void setSize(float height, float width)
        {
            this.Height = height;
            this.Width = width;
        }


        public void Draw()
        {

            SpriteDrawer.DrawUI(background, this.getPos()-view.viewSize/2, new Vector2(Width, Height), Color.White, Vector2.Zero, 0);
            foreach(UIElement child in children)
            {
                child.Draw();
            }
            
        }
    }
}
