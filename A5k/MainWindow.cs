using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//using Noesis;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using A5k.UI;






namespace A5k
{
    public sealed class MainWindow : GameWindow
    {
        private readonly string _title;

        private double _time;
        private Texture2D texture;
        private Texture2D cursorTexture;

        Vector2 mousePos;
        View view;
        

        private SpriteDrawer spritedrawer;

        private PlayerShip player;
        private List<SpaceObject> spaceObjects;

        private AI enemyAI;
        Inventory inventory;
        private bool inventoryOpen = false;


        private double globalTime = 0;
        //Noesis UI
        private Noesis.View nView;
        private Noesis.Renderer renderer;
        Noesis.Grid xaml;

        public MainWindow()
            : base(640, // initial width
                480, // initial height
                GraphicsMode.Default,
                "A5k",  // initial title
                GameWindowFlags.Default,
                DisplayDevice.Default,
                4, // OpenGL major version
                0, // OpenGL minor version
                GraphicsContextFlags.ForwardCompatible)
        {
            _title += ": OpenGL Version: " + GL.GetString(StringName.Version);
            Input.Initialize(this);

            
            spaceObjects = new List<SpaceObject>();
            view = new View(Vector2.Zero,new Vector2(640, 480), 2f, 0.0f);
            inventory = new Inventory(view);

        }

        protected override void OnResize(System.EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            view.viewSize.X = Width;
            view.viewSize.Y = Height;
            nView.SetSize(this.Width, this.Height);

        }

        protected override void OnLoad(System.EventArgs e)
        {

            Noesis.GUI.Init();
            //EmbeddedXaml[] xamls = new EmbeddedXaml[]
            //{
            //        new EmbeddedXaml { filename = "Reflections.xaml", resource = "Reflections" }
            //};
            //Noesis.GUI.SetXamlProvider(new LocalXamlProvider("."));



            // Data loading
            Noesis.GUI.SetXamlProvider(new NoesisApp.LocalXamlProvider("."));
            //Noesis.Grid xaml = (Noesis.Grid)Noesis.GUI.LoadXaml("UI\\UItest0.xaml");
            xaml = new Noesis.Grid();
            Noesis.GUI.LoadComponent(xaml, "UI\\UItest03.xaml");
            //MyGrid xaml = new MyGrid();

            Noesis.StackPanel sp = (Noesis.StackPanel)xaml.FindName("BottomMenu");
            sp.DataContext = new ViewModel();



            Noesis.Button button = (Noesis.Button)xaml.FindName("button");
            
            button.Click += (object sender, Noesis.RoutedEventArgs args) =>
            {
                System.Console.WriteLine("Button was clicked");
            };

            
            // create view
            nView = Noesis.GUI.CreateView(xaml);
            
            nView.SetSize(this.Width, this.Height);
            // get OpenGL rendering device
            Noesis.RenderDevice device = new Noesis.RenderDeviceGL();

            // init renderer as OpenGL
            renderer = nView.Renderer;
            renderer.Init(device);
            

            nView.Update(0.001); // Ensures view is updated before first render call (avoids crash)


            CursorVisible = true;



            mousePos = new Vector2(this.PointToClient(new Point(Mouse.GetCursorState().X, Mouse.GetCursorState().Y)).X, this.PointToClient(new Point(Mouse.GetCursorState().X, Mouse.GetCursorState().Y)).Y);
            Input.mousePosition = mousePos;
            //GL.Disable()
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha , BlendingFactor.OneMinusSrcAlpha);


            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);


            spritedrawer = new SpriteDrawer(view);
            cursorTexture = SpriteDrawer.LoadTexture("PNG\\crosshair010.png", true, false);

            

            

            texture = SpriteDrawer.LoadTexture("PNG\\playerShip1_red.png", true, false);
            
            player = new PlayerShip(0, 0, 0, texture, SpriteDrawer.LoadTexture("PNG\\Lasers\\laserBlue01.png", true, false),  view);
            enemyAI = new AI(spaceObjects, player);
            Ship enemy1 = new Ship(200, 200, 0, SpriteDrawer.LoadTexture("PNG\\ufoBlue.png", true, false));

            enemyAI.takeControl(enemy1);
            spaceObjects.Add(enemy1);


            Closed += OnClosed;
        }
        private void OnClosed(object sender, EventArgs eventArgs)
        {
            Exit();
        }
        protected override void OnUnload(EventArgs e)
        {
            renderer.Shutdown();
            Noesis.GUI.Shutdown();
        }
        public override void Exit()
        {
            Debug.WriteLine("Exit called");

            base.Exit();
            //renderer.Shutdown();
            //Noesis.GUI.Shutdown();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            HandleKeyboard();
            Input.mousePosition = mousePos;
            ;
            //mousePos.X = this.PointToClient(new Point(Mouse.GetCursorState().X, Mouse.GetCursorState().Y)).X-this.Width/2;
            //mousePos.Y = -this.PointToClient(new Point(Mouse.GetCursorState().X, Mouse.GetCursorState().Y)).Y+this.Height/2;

            mousePos.X = Noesis.Mouse.GetPosition(xaml).X - this.Width / 2;
            mousePos.Y = -Noesis.Mouse.GetPosition(xaml).Y + this.Height / 2;

            for (int i = spaceObjects.Count-1; i>=0; i--)
            {
                if (spaceObjects[i].isDead())
                {
                    spaceObjects.RemoveAt(i);
                }
            }

            enemyAI.Update();

            List<SpaceObject> newSO = new List<SpaceObject>();
            player.Update(newSO);
            foreach (SpaceObject so in spaceObjects)
            {
                so.Update(newSO);
            }
            CheckCollisions();
            spaceObjects.AddRange(newSO);
            view.Update();
            globalTime += e.Time;
            nView.Update(globalTime);
        }

        

        private void HandleKeyboard()
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(OpenTK.Input.Key.Escape))
            {
                Exit();
            }


            /*
            if (Mouse.GetCursorState().IsButtonDown(MouseButton.Left))
            {
                view.SetPosition(new Vector2(this.PointToClient(new Point(Mouse.GetCursorState().X , Mouse.GetCursorState().Y)).X -this.Width/2, this.PointToClient(new Point(Mouse.GetCursorState().X, Mouse.GetCursorState().Y)).Y- this.Height/2));
            }
            */
            
        }


        private void CheckCollisions()
        {
            
            for(int i = 0; i<spaceObjects.Count; i++)
            {
                for(int j = i+1; j < spaceObjects.Count; j++)
                {
                    if (i!=j && spaceObjects[i].getFaction() != spaceObjects[j].getFaction() && checkCol(spaceObjects[i], spaceObjects[j]) )
                    {
                         
                        spaceObjects[i].Collide(spaceObjects[j]);
                        spaceObjects[j].Collide(spaceObjects[i]);
                    }
                }
            }
        }
        
        private bool checkCol(SpaceObject objA, SpaceObject objB)
        {
            /*
            if( DateTime.Now.Second%10 == 0)
            {
                Console.WriteLine("Collision" + objA.getFaction() + objB.getFaction());
                Console.WriteLine(" A: (" + objA.pos.X + "," + objA.pos.Y + ") r: " + objA.getRadius());
                Console.WriteLine(" B: (" + objB.pos.X + "," + objB.pos.Y + ") r: " + objB.getRadius());

            }
            */
            return (objA.pos.X - objB.pos.X) * (objA.pos.X - objB.pos.X) + (objA.pos.Y - objB.pos.Y) * (objA.pos.Y - objB.pos.Y) 
                    < (objA.getRadius() + objB.getRadius()) * (objA.getRadius() + objB.getRadius());
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _time += e.Time;
            Title = //$"{_title}: (Vsync: {VSync}) FPS: {1f / e.Time:0}, "+ 
                "pos:" + view.getX() + "," + view.getY() + " mousePos: " + mousePos.X + "," + mousePos.Y + "shipPos:" + player.pos.X + "," + player.pos.Y;
            Color4 backColor;
            backColor.A = 1.0f;
            backColor.R = 0.1f;
            backColor.G = 0.1f;
            backColor.B = 0.3f;
            GL.ClearColor(backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.ClearStencil(0);
            

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            //SpriteDrawer.Draw(texture, Vector2.Zero, Vector2.One, Color.Azure, new Vector2(((float)texture.Width)/2, ((float)texture.Height)/2));
            if (inventoryOpen)
            {
                inventory.Draw();
            }
            else
            {
                foreach (SpaceObject so in spaceObjects)
                {
                    so.Draw();
                }
                player.Draw();
                SpriteDrawer.DrawCursor(cursorTexture, mousePos, Vector2.One, Color.Azure, new Vector2(((float)cursorTexture.Width) / 2, ((float)cursorTexture.Height) / 2));
            }

            renderer.UpdateRenderTree();

            if (renderer.NeedsOffscreen()) renderer.RenderOffscreen();

            renderer.Render();

            SwapBuffers();
        }



        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            nView.MouseMove(e.X, e.Y);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            nView.MouseButtonDown(e.X, e.Y, TranslateButton(e.Button));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            nView.MouseButtonUp(e.X, e.Y, TranslateButton(e.Button));
        }
        


        private static Noesis.MouseButton TranslateButton(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left: return Noesis.MouseButton.Left;
                case MouseButton.Right: return Noesis.MouseButton.Right;
                case MouseButton.Middle: return Noesis.MouseButton.Middle;
                default: return Noesis.MouseButton.XButton1;
            }
        }
    }

}
