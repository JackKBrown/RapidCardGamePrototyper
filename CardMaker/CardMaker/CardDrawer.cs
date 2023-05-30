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
        public Font defaultFont = new Font("Arial", 16);
        public Font mediumFont = new Font("Arial", 16);
        public Font LargeFont = new Font("Arial", 32);
        public Font BoxFont = new Font("Arial", 40);

        public static CardDrawer Instance()
		{
            return cardDrawer;
		}

        private CardDrawer()
		{
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
                    g.DrawImage(bmap, layer.X, layer.Y);
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

            Layer layer = new Layer(x, y, img);
            return layer;
        }

        public Layer CreateLayerFromFile(int x, int y, int width, int height, string imagePath)
        {
            Image image = Bitmap.FromFile(imagePath);
            Layer layer = new Layer(x, y, image);
            return layer;
        }

    }

    public class Layer
    {
        //position of the layer on the final card
        public string Name;
        public int X;
        public int Y;
        public Image Image;

        public Layer(int x, int y, Image image)
        {
            X = x;
            Y = y;
            Image = image;
        }
    }

}
