using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;

namespace A5k
{
    
    class Spritebatch
    {
        private static int shaderProgram;
        private static View view;
        private uint VBO, VAO, EBO;
        public Spritebatch(View v)
        {
            view = v;


            /* old
              float[] vertices = {
                 0.5f,  0.5f, 0.0f,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f, // top right
                 0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 0.0f, // bottom right
                -0.5f, -0.5f, 0.0f,   0.0f, 0.0f, 1.0f,   0.0f, 0.0f, // bottom left
                -0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 0.0f,   0.0f, 1.0f  // top left 
            };
            */
            float twidth = 99;
            float theight = 75;


            float[] vertices = {
                 twidth,  theight, 0.0f,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f, // top right
                 twidth, 0f, 0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 0.0f, // bottom right
                0f, 0f, 0.0f,   0.0f, 0.0f, 1.0f,   0.0f, 0.0f, // bottom left
                0f,  theight, 0.0f,   1.0f, 1.0f, 0.0f,   0.0f, 1.0f  // top left 
            };
            uint[] indices = {
                3, 1, 0, // first triangle
                1, 2, 3  // second triangle
            };

            GL.GenVertexArrays(1, out VAO);
            GL.GenBuffers(1, out VBO);
            GL.GenBuffers(1, out EBO);
            

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

            int vShader = CompileShader(ShaderType.VertexShader, @"Shaders\1Vert\vertexShaderTex.c");
            int fShader = CompileShader(ShaderType.FragmentShader, @"Shaders\5Frag\fragmentShaderTex.c");

            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vShader);
            GL.AttachShader(shaderProgram, fShader);
            GL.LinkProgram(shaderProgram);

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // color attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (3 * sizeof(float)));
            GL.EnableVertexAttribArray(1);
            // texture coord attribute
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (6 * sizeof(float)));
            GL.EnableVertexAttribArray(2);

        }
        public void Draw(Texture2D texture, Vector2 position, Vector2 scale, Color color, Vector2 origin )
        {

            int transformLoc = GL.GetUniformLocation(shaderProgram, "transform");

            Matrix4 t = view.ApplyTransforms();
            GL.UniformMatrix4(transformLoc, false, ref t);

            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(VAO);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

        }

        public Texture2D LoadTexture(string file)
        {
            Bitmap bitmap = new Bitmap(file);

            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
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

            return new Texture2D(tex, bitmap.Width, bitmap.Height);
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
    }
}
