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
            if(text.Contains('@'))
            {
                return DrawSymbString(x,y,width,height, text, font, drawing, false,false);
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

        private string DrawSymbolText(int x, int y, int width, int height, string text, Font font, Graphics drawing, StringFormat format)
        {
            
            string[] words = text.Split(' ');
            List<string> lines = new List<string>();
            List<Symbol> Symbols = new List<Symbol>();
            string line = "";
            SizeF buffer_sz = drawing.MeasureString(SYMBOL_BUFFER, font);// there is a string format option for this?
            //potentially if there is a center x option set I can check for that to find the correct width for the symbol?
            Console.WriteLine(buffer_sz);
            float lineheight = buffer_sz.Height; 
            float sym_size = Math.Min(buffer_sz.Height, buffer_sz.Width);
            foreach (string word in words)
            {
                string tempLine = "";
                if (word[0]=='@')
                {
                    string SymbolImage = $"Img/{word.Trim('@')}.png";
                    Image image = Bitmap.FromFile(SymbolImage);
                    float sym_x = drawing.MeasureString(line, font, width).Width;
                    float sym_y = lineheight*lines.Count; //this needs to be the top left corner
                    float sym_height = sym_size;
                    float sym_width = sym_size;
                    Symbol symbol = new Symbol(sym_x, sym_y, sym_width, sym_height, image);
                    Symbols.Add(symbol);
                    //TODO extract image and run draw symbol
                    // need to check if it on the new line or not?
                    //DrawSymbol();
                    tempLine = line + (string.IsNullOrEmpty(line) ? "" : " ") + SYMBOL_BUFFER;
                }
                else
                {
                    tempLine = line + (string.IsNullOrEmpty(line) ? "" : " ") + word;
                }
                Console.WriteLine("current line height");
                Console.WriteLine(drawing.MeasureString(tempLine, font, width));
                float tempHeight = drawing.MeasureString(tempLine, font, width).Height;

                if (tempHeight > lineheight)
                {
                    lines.Add(line);
                    line = word;
                    
                }
                else
                {
                    line = tempLine;
                }
            }

            if (!string.IsNullOrEmpty(line))
            {
                lines.Add(line);
            }
            // here we need to work out from the string format, our list of symbollayers what the offset is for drawing our string?
            //if we're centering
            string newstring = "";
            Console.WriteLine(lines.Count);
            foreach (string entry in lines)
            {
                Console.WriteLine("line with format and without");
                Console.WriteLine(drawing.MeasureString(newstring, font, width, format).Height);
                Console.WriteLine(drawing.MeasureString(newstring, font, width).Height);
                newstring = newstring + " " + entry;
            }
                float offsetheight = drawing.MeasureString(newstring, font, width,format).Height-
                drawing.MeasureString(newstring, font).Height;
                float offsetwidth = drawing.MeasureString(newstring, font, width, format).Width-
                drawing.MeasureString(newstring, font).Width;
       
            foreach (Symbol symbol in Symbols)
			{
                symbol.Y = symbol.Y + offsetheight +(float)3.33;
                //symbol.X = symbol.X + offsetwidth;
                Console.WriteLine(symbol.Y);
                Console.WriteLine(symbol.X);
                DrawSymbol(symbol, drawing);
			}
            return newstring;

        }

        public float getlnheihgt(Font font)
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
            List<Image> lines = new List<Image>();
            string[] words = text.Split(' ');
            float cursor = 0;
            SizeF spaceSZ = drawing.MeasureString(" ", font);
            SizeF buffer_sz = drawing.MeasureString(SYMBOL_BUFFER, font);// there is a string format option for this?
            float lineheight = getlnheihgt(font);
            Image currentLine = new Bitmap(width, (int)Math.Ceiling(lineheight));
            
            //potentially if there is a center x option set I can check for that to find the correct width for the symbol?
            Console.WriteLine(buffer_sz);

            foreach (string word in words)
            {
                Image word_image = new Bitmap(1, 1);
                //check if word is actually a symbol
                if (word[0] == '@')
                {
                    //convert current line buffer into an image

                    //do symbol things
                    string SymbolImage = $"Img/{word.Trim('@')}.png";
                    Bitmap original = (Bitmap)Image.FromFile(SymbolImage);
                    word_image = new Bitmap(original, new Size((int)(lineheight * 0.8), (int)(lineheight * 0.8)));

                }
                else
                {
                    //word is word
                    SizeF wordSZ = drawing.MeasureString(word, font);
                    word_image = DrawTextChunk((int)Math.Round(wordSZ.Width),
                        (int)Math.Round(wordSZ.Height), word, font);
                }
                float tempcursor = cursor + spaceSZ.Width + word_image.Width;
                if (tempcursor > width)//line has overflowed
                {
                    lines.Add(currentLine);
                    currentLine = new Bitmap(width, height);
                    cursor = 0;
                }
                using (var g = Graphics.FromImage(currentLine))
                {
                    Bitmap bmap = new Bitmap(word_image);
                    g.DrawImage(bmap,cursor, currentLine.Height, word_image.Width, word_image.Height);
                }
                cursor = cursor + (cursor == 0 ? 0 : spaceSZ.Width) + word_image.Width;
            }
            lines.Add(currentLine);
            
            // here we need to work out from the string format, our list of symbollayers what the offset is for drawing our string?
            //if we're centering

            Console.WriteLine(lines.Count);
            Image img = new Bitmap(width, height);
            float cursorheight = lineheight;
            using (var g = Graphics.FromImage(img))
            {
                foreach (Image line in lines)
                {
                    Bitmap bmap = new Bitmap(line);
                    g.DrawImage(bmap, cursorheight, cursor, line.Width, line.Height);
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

        private void DrawSymbol(Symbol symbol, Graphics drawing)
        {
            Bitmap bmap = new Bitmap(symbol.Image);
            bmap.SetResolution(1200, 1200);
            drawing.DrawImage(bmap, symbol.X, symbol.Y, symbol.Width, symbol.Height);
            drawing.Save();
            return;
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
