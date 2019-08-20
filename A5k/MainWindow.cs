using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace A5k
{
    public sealed class MainWindow : GameWindow
    {
        private readonly string _title;
        private int _program;
        private int _program2;
        //private int _vertexArray;
        private double _time;
        private int texture;
        uint VBO, VAO, EBO;


        int shaderProgram;
        /*
        public struct Vertex
        {
            public const int Size = (4 + 4) * 4; // size of struct in bytes

            private readonly Vector4 _position;
            private readonly Color4 _color;

            public Vertex(Vector4 position, Color4 color)
            {
                _position = position;
                _color = color;
            }
        }
        */
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
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = true;
            //GL.Disable()

            
            //_program = CreateProgram();

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //
            //
            //GL.PatchParameter(PatchParameterInt.PatchVertices, 6);
            //GL.GenVertexArrays(1, out _vertexArray);
            
            float[] vertices = {
                 0.5f,  0.5f, 0.0f,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f, // top right
                 0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 0.0f, // bottom right
                -0.5f, -0.5f, 0.0f,   0.0f, 0.0f, 1.0f,   0.0f, 0.0f, // bottom left
                -0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 0.0f,   0.0f, 1.0f  // top left 
            };
            
            /*
            float[] vertices = {
                 0.5f,  0.5f, 0.0f,  1.0f, 1.0f,            // top right
                 0.5f, -0.5f, 0.0f,  1.0f, 0.0f,            // bottom right
                -0.5f, -0.5f, 0.0f,  0.0f, 0.0f,            // bottom left
                -0.5f,  0.5f, 0.0f,  0.0f, 1.0f             // top left 
            };
            */
            //float[] vertices = {
            //    -0.5f, -0.5f, 0.0f,
            //     0.5f, -0.5f, 0.0f,
            //     0.0f,  0.5f, 0.0f
            //};


            
            uint[] indices = {
                3, 1, 0, // first triangle
                1, 2, 3  // second triangle
            };
            

            //int VBO, VAO, EBO;
            GL.GenVertexArrays(1, out VAO);
            GL.GenBuffers(1, out VBO);
            GL.GenBuffers(1, out EBO);

            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length, indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.NamedBufferStorage(
                       VBO,
                       sizeof(float) * vertices.Length,      
                       vertices,                           
                       BufferStorageFlags.MapWriteBit);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.NamedBufferStorage(
                       EBO,
                       sizeof(uint) * indices.Length,
                       indices,
                       BufferStorageFlags.MapWriteBit);




            //GL.VertexArrayAttribBinding(VAO, 0, 0);

            /*
            GL.EnableVertexArrayAttrib(VAO, 0);


            GL.VertexArrayAttribFormat(
                VAO,
                0,                      // attribute index, from the shader location = 0
                3,                      // size of attribute, vec4
                VertexAttribType.Float, // contains floats
                false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
                0);
           */
            /*
            GL.VertexArrayAttribBinding(VAO, 1, 0);
            GL.EnableVertexArrayAttrib(VAO, 1);
            GL.VertexArrayAttribFormat(
                VAO,
                1,                      // attribute index, from the shader location = 1
                4,                      // size of attribute, vec4
                VertexAttribType.Float, // contains floats
                false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
                16);                     // relative offset after a vec4

            GL.VertexArrayVertexBuffer(VAO, 0, VBO, IntPtr.Zero, (4+4)*4);
            */

            //GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length, vertices, BufferUsageHint.StaticDraw);




            int vShader = CompileShader(ShaderType.VertexShader, @"Shaders\1Vert\vertexShaderTex.c");
            int fShader = CompileShader(ShaderType.FragmentShader, @"Shaders\5Frag\fragmentShaderTex.c");

            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vShader);
            GL.AttachShader(shaderProgram, fShader);
            GL.LinkProgram(shaderProgram);

            GL.DeleteShader(vShader);
            GL.DeleteShader(fShader);


            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            
            // color attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (3 * sizeof(float)));
            GL.EnableVertexAttribArray(1);
            // texture coord attribute
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (6 * sizeof(float)));
            GL.EnableVertexAttribArray(2);
            

            /*
            texture = LoadTexture("PNG\\playerShip1_red.png");

            GL.UseProgram(_program);

            GL.Uniform1(GL.GetUniformLocation(_program, "ourTexture"), 0);
            */
            //System.Diagnostics.Debug.WriteLine(GL.GetUniformLocation(_program, "texture1"));


           // GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (6 * sizeof(float)));
           // GL.EnableVertexAttribArray(2);
           //
           // GL.BindVertexArray(_vertexArray);

            Closed += OnClosed;
        }
        private void OnClosed(object sender, EventArgs eventArgs)
        {
            Exit();
        }

        public override void Exit()
        {
            Debug.WriteLine("Exit called");
            GL.DeleteVertexArrays(1, ref VAO);
            GL.DeleteProgram(_program);
            base.Exit();
        }

        public int LoadTexture(string file)
        {
            Bitmap bitmap = new Bitmap(file);

            int tex;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out tex);
            GL.BindTexture(TextureTarget.Texture2D, tex);

            System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);


            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            return tex;
        }


        public Bitmap loadBitmap(string filepath)
        {
            return new Bitmap("PNG\\playerShip1_red.png");
        }

        private int CreateProgram()
        {
            try
            {
                var program = GL.CreateProgram();
                var shaders = new List<int>();
                shaders.Add(CompileShader(ShaderType.VertexShader, @"Shaders\1Vert\vertexShader.c"));
                shaders.Add(CompileShader(ShaderType.FragmentShader, @"Shaders\5Frag\fragmentShaderTex.c"));

                foreach (var shader in shaders)
                    GL.AttachShader(program, shader);
                GL.LinkProgram(program);
                var info = GL.GetProgramInfoLog(program);
                if (!string.IsNullOrWhiteSpace(info))
                    throw new Exception($"CompileShaders ProgramLinking had errors: {info}");

                foreach (var shader in shaders)
                {
                    //GL.DetachShader(program, shader);
                    GL.DeleteShader(shader);
                }
                return program;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        private int CompileShader(ShaderType type, string path)
        {
            var shader = GL.CreateShader(type);
            var src = File.ReadAllText(path);
            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);
            var info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(info))
                throw new Exception($"CompileShader {type} had errors: {info}");
            return shader;
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            HandleKeyboard();
        }

        private void HandleKeyboard()
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _time += e.Time;
            Title = $"{_title}: (Vsync: {VSync}) FPS: {1f / e.Time:0}";
            Color4 backColor;
            backColor.A = 1.0f;
            backColor.R = 0.1f;
            backColor.G = 0.1f;
            backColor.B = 0.3f;
            GL.ClearColor(backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(VAO);


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            /*
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);


            GL.UseProgram(_program);

            GL.BindVertexArray(VAO);
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            */
            // add shader attributes here
            //  GL.VertexAttrib1(0, _time);
            //  
            //  Vector4 position;
            //  position.X = (float)Math.Sin(_time) * 0.5f;
            //  position.Y = (float)Math.Cos(_time) * 0.5f;
            //  position.Z = 0.0f;
            //  position.W = 1.0f;
            //  GL.VertexAttrib4(1, position);
            //  
            //  
            //  
            //  GL.DrawArrays(PrimitiveType.Points, 0, 1);
            //  GL.PointSize(100);
            SwapBuffers();
        }
    }

}
