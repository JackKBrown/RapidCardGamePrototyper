﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardMaker
{
    class CardDrawer
    {
        private static readonly CardDrawer cardDrawer = new CardDrawer();
        PrivateFontCollection privateFontCollection = new PrivateFontCollection();
        private static string SYMBOL_BUFFER = "    ";
        public string FONT_FILE = "NotoSans-Regular.ttf";
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

            //search string for symbol tags till none are found
            Regex rgx = new Regex(@"(.*@[^ ]*)");
            string[] symbols = rgx.Split(text);
            string totalstring = "";
            foreach(string substring in symbols)
            {
                //Console.WriteLine(substring);
                string[] text_symbol = substring.Split('@');
                if (text_symbol.Length > 1)
                {
                    string subtext = totalstring + text_symbol[0];
                    string sympath = @"Img/" + text_symbol[1] + ".png";
                    DrawSymbolMidText(subtext, sympath, font, drawing);
                    totalstring = subtext +SYMBOL_BUFFER;
                }
                else
                {
                    totalstring = totalstring + text_symbol[0];
                } 
            }
            //foreach symbol tag search for if that symbol exists and if so draw onto the img
            //DrawSymbolMidText()

            drawing.DrawString(totalstring, font, BlackBrush, layoutRect, format);

            //Console.WriteLine(drawing.MeasureString(text, font)); //https://stackoverflow.com/questions/4258696/find-a-character-in-a-user-drawn-string-at-a-given-point
            

            drawing.Save();

            BlackBrush.Dispose();
            drawing.Dispose();

            Layer layer = new Layer(x, y, width, height, img);
            return layer;
        }

        private void DrawSymbolMidText(string subtext, string symbolpath, Font font, Graphics drawing)
        {
            Console.WriteLine("called with subtext '" + subtext +"' and a symbol path of: " + symbolpath);
            SizeF stringsz = drawing.MeasureString(subtext, font);
            SizeF stringWBuffsz = drawing.MeasureString(subtext + SYMBOL_BUFFER, font);
            SizeF buffersz = drawing.MeasureString(SYMBOL_BUFFER, font);
            Console.WriteLine("stringsz = " + stringsz.ToString());
            Console.WriteLine("stringWBuffsz = " + stringWBuffsz.ToString());
            Console.WriteLine("buffersz = " + buffersz.ToString());
            // single ln height
            float lineheight = buffersz.Height;
            if (stringsz.Height > lineheight)
            {
                string[] words = subtext.Split(' ');
            }
            //TODO
            //you can measure the size of some text using the MeasureText function which gives you which line(y coord)
            //its going to go on then I need to recursively find out what text is on each line so I can find the position it is in that line (x coord)
            //with that I can then find out the rough height it needs to be ( the font data should tell me the line height)
            //then I can draw the symbol into that spot (provided symbols are square images which is what we've done so far) I can then replace the text
            //with enough whitespace to leave the spot for the symbol blank repeat the process for each symbol.
            drawing.Save();
        }

        private string[] SplitTextByWidth(Graphics graphics, Font font, string text, float maxWidth)
        {
            string[] words = text.Split(' ');
            List<string> lines = new List<string>();
            string line = "";

            foreach (string word in words)
            {
                string tempLine = line + (string.IsNullOrEmpty(line) ? "" : " ") + word;
                float tempWidth = graphics.MeasureString(tempLine, font).Width;

                if (tempWidth <= maxWidth)
                {
                    line = tempLine;
                }
                else
                {
                    lines.Add(line);
                    line = word;
                }
            }

            if (!string.IsNullOrEmpty(line))
            {
                lines.Add(line);
            }

            return lines.ToArray();
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
