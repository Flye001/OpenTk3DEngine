using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using System.Drawing.Imaging;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace OpenTkEngine
{
    internal class Texture
    {
        public int Handle { get; set; }

        public Texture(string imagePath)
        {
            Handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            if (!File.Exists(imagePath)) throw new Exception("IMAGE NO EXISTY!");

            //StbImage.stbi_set_flip_vertically_on_load(1);
            //using (Stream stream = File.OpenRead(imagePath))
            //{
            //    ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

            //    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            //}
            List<byte> pixels = new();
            Bitmap img = new Bitmap(imagePath);
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    //if (pixel.R != 128 && pixel.G != 128 && pixel.B != 128)
                    //{
                    //    Console.WriteLine("Woah");
                    //}

                    pixels.Add(pixel.R);
                    pixels.Add(pixel.G);
                    pixels.Add(pixel.B);
                    pixels.Add(pixel.A);
                }
            }
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, img.Width, img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
        }

        public void Use()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

    }
}
