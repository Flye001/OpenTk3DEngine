using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using System.Drawing.Imaging;
using StbImageSharp;
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

            StbImage.stbi_set_flip_vertically_on_load(0);
            using (Stream stream = File.OpenRead(imagePath))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }
            //List<byte> pixels = new();
            //Bitmap img = new Bitmap(imagePath);
            //for (int i = 0; i < img.Width; i++)
            //{
            //    for (int j = 0; j < img.Height; j++)
            //    {
            //        Color pixel = img.GetPixel(i, j);

            //        pixels.Add(pixel.R);
            //        pixels.Add(pixel.G);
            //        pixels.Add(pixel.B);
            //        pixels.Add(pixel.A);
            //    }
            //}
            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, ima.Width, img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

    }
}
