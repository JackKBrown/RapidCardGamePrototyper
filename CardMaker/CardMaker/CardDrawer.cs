using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMaker
{
    class CardDrawer
    {
        private static readonly FontFamily defaultFF = new FontFamily("");
        private static readonly float fontSize = 9;
        public static readonly Font defaultFont = new Font(defaultFF, fontSize);

        public static void MergeLayers(List<Layer> layers, int CardWidth, int CardHeight)
        {
            //potential challenges tansparency not being included
            var bitmap = new Bitmap(CardWidth, CardHeight);
            using (var g = Graphics.FromImage(bitmap))
{
                foreach (var layer in layers)
                {
                    g.DrawImage(layer.Image, layer.X, layer.Y);
                }
            }
            //output image
        }

        public static Layer CreateTextLayer(int x, int y, int width, int height, string text)
        {
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, defaultFont);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            //drawing.Clear(backColor);

            Brush textBrush = new SolidBrush(Color.Black);
            drawing.DrawString(text, defaultFont, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            Layer layer = new Layer(x, y, img);
            return layer;
        }

        public static Layer CreateLayerFromFile(int x, int y, int width, int height, string imagePath)
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
