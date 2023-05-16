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
        //
        int CardWidth;
        int CardHeight;
        public CardDrawer()
        {
            //define necessary values such as size
        }

        public void Draw()
        {
            //get Layers
            List<Layer> layers = new List<Layer>();
            MergeLayers(layers);
            return;
        }

        private void MergeLayers(List<Layer> layers)
        {
            //potential challenges tansparency not being included
            var bitmap = new Bitmap(this.CardWidth, this.CardHeight);
            using (var g = Graphics.FromImage(bitmap))
{
                foreach (var layer in layers)
                {
                    g.DrawImage(layer.Image, layer.X, layer.Y);
                }
            }
        }
    }

    class Layer
    {
        //position of the layer on the final card
        public int X;
        public int Y;
        public Image Image;
        public Layer(int x, int y, string imagePath)
        {
            X = x;
            Y = y;
            Image = Bitmap.FromFile(imagePath);
        }
    }

}
