using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMaker
{
    class CardDrawer
    {
        private static readonly CardDrawer cardDrawer = new CardDrawer();
        PrivateFontCollection privateFontCollection = new PrivateFontCollection();
        string FONT_FILE = "NotoSans-Regular.ttf";
        public Font mediumFont;
        public Font LargeFont;
        public Font NameFont;
        public Font BoxFont;

        public static CardDrawer Instance()
		{
            return cardDrawer;
		}

        private CardDrawer()
		{
            privateFontCollection.AddFontFile(FONT_FILE);
            mediumFont = new Font(FONT_FILE, 16);
            LargeFont = new Font(FONT_FILE, 22);
            NameFont = new Font(FONT_FILE, 32);
            BoxFont = new Font(FONT_FILE, 40);
    }

        public Bitmap MergeLayers(List<Layer> layers, int CardWidth, int CardHeight)
        {
            //potential challenges tansparency not being included
            var bitmap = new Bitmap(CardWidth, CardHeight);
            bitmap.SetResolution(1200,1200);
            

            using (var g = Graphics.FromImage(bitmap))
            {
                foreach (var layer in layers)
                {
                    Bitmap bmap = new Bitmap(layer.Image);
                    bmap.SetResolution(1200, 1200);
                    g.DrawImage(bmap, layer.X, layer.Y, layer.Width, layer.Height);
                }
            }
            return bitmap;
            //bitmap.Save("out.png", System.Drawing.Imaging.ImageFormat.Png);
            //output image
        }

        public Layer CreateTextLayer(int x, int y, int width, int height, string text, Font font, StringFormat format)
        {
            RectangleF layoutRect = new RectangleF(0, 0, width, height);
            Image img = new Bitmap(width, height);
            Graphics drawing = Graphics.FromImage(img);
            Brush BlackBrush = new SolidBrush(Color.Black);

            drawing.DrawString(text, font, BlackBrush, layoutRect, format);

            drawing.Save();

            BlackBrush.Dispose();
            drawing.Dispose();

            Layer layer = new Layer(x, y, width, height, img);
            return layer;
        }

        public Layer CreateLayerFromFileUniform(int x, int y, int minWidth, int minHeight, string imagePath)
        {
            Image image = Bitmap.FromFile(imagePath);
            int Width = image.Width;
            int Height = image.Height;
            //set height to minheight and adjust width
            double HRatio = (double)minHeight / (double)Height;
            Height = minHeight;
            Width = (int)(HRatio * Width);
            //if not wide enough adjust by width instead
            if (Width < minWidth)
            {
                double WRatio = (double)minWidth / (double)Width;
                Height = (int)(WRatio * Height);
                Width = minWidth;
            }
            Layer layer = new Layer(x, y, Width, Height, image);
            return layer;
        }

        public Layer CreateLayerFromFile(int x, int y, int width, int height, string imagePath)
        {
            Image image = Bitmap.FromFile(imagePath);
            Layer layer = new Layer(x, y, width, height, image);
            return layer;
        }

    }

    public class Layer
    {
        //position of the layer on the final card
        public string Name;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public Image Image;

        public Layer(int x, int y, int width, int height, Image image)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Image = image;
        }
    }

}
