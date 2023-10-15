using System;
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
        private static string SYMBOL_BUFFER = "      ";
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

            //search string for symbol tags till none are found
            string outtext = "";
            if(text.Contains('@') || text.Contains("\\n"))
            {
                return DrawSymbString(x,y,width,height, text, font, drawing, true,true);
            }
            else
            {
                outtext = text;
                Brush BlackBrush = new SolidBrush(Color.Black);
                drawing.DrawString(outtext, font, BlackBrush, layoutRect, format);
                drawing.Save();
                BlackBrush.Dispose();
                drawing.Dispose();

                Layer layer = new Layer(x, y, width, height, img);
                return layer;
            }
        }

        public float getlnheight(Font font)
        {
            RectangleF layoutRect = new RectangleF(0, 0, 300, 300);
            Image img = new Bitmap(300, 300);
            Graphics drawing = Graphics.FromImage(img);
            SizeF spaceSingle = drawing.MeasureString(" ", font);
            SizeF spacedouble = drawing.MeasureString("hi \r\n hi ", font);
            Console.WriteLine(spaceSingle);
            Console.WriteLine(spacedouble);
            return spacedouble.Height - spaceSingle.Height;
        }

        private Layer DrawSymbString(int x, int y, int width, int height, string text, Font font, Graphics drawing, bool centreX, bool centreY)
        {
            List<(Image,float)> lines = new List<(Image, float)>();
            string[] words = text.Split(' ');
            float cursor = 0;
            SizeF spaceSZ = drawing.MeasureString(" ", font);
            SizeF buffer_sz = drawing.MeasureString(SYMBOL_BUFFER, font);// there is a string format option for this?
            float lineheight = (float)Math.Ceiling(getlnheight(font));
            Image currentLine = new Bitmap(width, (int)Math.Ceiling(lineheight));
            SizeF puncSize = drawing.MeasureString(".", font);
            char[] punctuation = { '.', ',' };
            
            //potentially if there is a center x option set I can check for that to find the correct width for the symbol?
            Console.WriteLine(buffer_sz);

            foreach (string word in words)
            {
                if (word.Length == 0)
                { continue; }
                Image word_image = new Bitmap(1, 1);
                //check if word is actually a symbol
                float yPos = 0;
                if (word[0] == '@')
                {
                    //convert current line buffer into an image

                    //do symbol things
                    string SymbolImage = $"Img/{word.Trim('@').TrimEnd('.').TrimEnd(',')}.png";
                    Bitmap original = (Bitmap)Image.FromFile(SymbolImage);
                    yPos = (float)(lineheight * 0.1);
                    word_image = new Bitmap(original, new Size((int)(lineheight * 0.8), (int)(lineheight * 0.8)));
                    int punctuationind = Array.IndexOf(punctuation, word[word.Length-1]);
                    if (punctuationind>0)
                    {
                        //TODO put in the line positioning here and add an else so it positions the y pos in here as well
                        SizeF wordSZ = drawing.MeasureString(word[word.Length - 1].ToString(), font);
                        Image punc_image = DrawTextChunk((int)Math.Ceiling(wordSZ.Width),
                            (int)Math.Ceiling(wordSZ.Height), word[word.Length - 1].ToString(), font);
                        
                        Image subline = new Bitmap(punc_image.Width+word_image.Width, (int)Math.Ceiling(lineheight));
                        using (var g = Graphics.FromImage(subline))
                        {
                            Bitmap bmap = new Bitmap(word_image);
                            Bitmap pmap = new Bitmap(punc_image);
                            g.DrawImage(bmap, 0, yPos, word_image.Width, word_image.Height);
                            g.DrawImage(pmap, word_image.Width, 0, punc_image.Width, punc_image.Height);
                        }
                        word_image = subline;
                        yPos = 0;
                    }
                    
                }
                else if (word[0] == '\\')
                {
                    if (word[1] == 'n')
                    {
                        lines.Add((currentLine, cursor));
                        currentLine = new Bitmap(width, (int)Math.Ceiling(lineheight));
                        cursor = 0;
                        continue;
                    }
                }
                else
                {
                    //word is word
                    SizeF wordSZ = drawing.MeasureString(word, font);
                    word_image = DrawTextChunk((int)Math.Ceiling(wordSZ.Width),
                        (int)Math.Ceiling(wordSZ.Height), word, font);
				}
                
                float tempcursor = cursor + spaceSZ.Width + word_image.Width;
                if (tempcursor > width)//line has overflowed
                {
                    lines.Add((currentLine, cursor));
                    currentLine = new Bitmap(width, (int)Math.Ceiling(lineheight));
                    cursor = 0;
                }
                using (var g = Graphics.FromImage(currentLine))
                {
                    Bitmap bmap = new Bitmap(word_image);
                    g.DrawImage(bmap,cursor, yPos, word_image.Width, word_image.Height);
                }
                cursor = cursor + (cursor == 0 ? 0 : spaceSZ.Width) + word_image.Width;
            }
            lines.Add((currentLine, cursor));
            
            // here we need to work out from the string format, our list of symbollayers what the offset is for drawing our string?
            //if we're centering

            Image img = new Bitmap(width, height);
            float cursorheight = 0;
            if (centreY)
            {
				cursorheight = ((height - (lineheight * lines.Count)) / 2);
				//cursorheight = ((height - (lineheight * lines.Count)) / 2);
			}
            using (var g = Graphics.FromImage(img))
            {
                foreach ((Image lnImage, float lnCursor) in lines)
                {
                    Bitmap bmap = new Bitmap(lnImage);
                    float xOffset = 0;
                    if (centreX) xOffset = (width - lnCursor) / 2;
                    g.DrawImage(bmap, xOffset, cursorheight, lnImage.Width, lnImage.Height);
                    cursorheight += lnImage.Height;
                }
            }

            Layer layer = new Layer(x, y, width, height, img);
            return layer;

        }

        private Image DrawTextChunk(int width, int height, string text, Font font)
        {
            RectangleF layoutRect = new RectangleF(0, 0, width, height);
            Brush BlackBrush = new SolidBrush(Color.Black);
            Image img = new Bitmap(width, height);
            Graphics drawing = Graphics.FromImage(img);
            drawing.DrawString(text, font, BlackBrush, layoutRect);
            drawing.Save();
            BlackBrush.Dispose();
            drawing.Dispose();
            return img;
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

    public class Symbol
    {
        //position of the layer on the final card
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public Image Image;

        public Symbol(float x, float y, float width, float height, Image image)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Image = image;
        }
    }

}
