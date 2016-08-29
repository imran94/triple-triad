using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Game4
{
    class Card
    {
        private Texture2D sprite;

        private int north, south, east, west;

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Vector2 fontPos;

        private int id;
            public int ID { get { return id; } }
        private bool selected;
            public bool Selected { get { return selected; } set { selected = value; } }
        private bool selectable;
            public bool Selectable { get { return selectable; } set { selectable = value; } }
        private Rectangle rectangle;
            public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        private bool dealt;
            public bool Dealt { get { return dealt; } set { dealt = value; } }

        private const int width = 100;
        private const int height = 100;

        public Color color, colorNeutral = Color.Gray, colorHover = Color.Red, colorMarked = Color.Blue, colorPlayed = Color.Green;
        //private int initX, initY;
        
        public Card() { }

        public Card(bool selectable, int x, int y, Random rnd)
        {
            north = rnd.Next(1, 9);
            south= rnd.Next(1, 9);
            east = rnd.Next(1, 9);
            west = rnd.Next(1, 9);

            this.selectable = selectable;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            selected = false;
            color = colorNeutral;
        }

        public Card(int id, bool selectable, int x, int y, int n, int s, int e, int w)
        {
            this.id = id;
            this.selectable = selectable;
            north = n;
            south = s;
            east = e;
            west = w;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            dealt = false;

            selected = false;
            color = colorNeutral;
        }

        public virtual void LoadContent(ContentManager content, Texture2D texture)
        {
            sprite = texture;
            font = content.Load<SpriteFont>("Arial");
            fontPos = new Vector2();
        }
        
        public void Unload() { sprite.Dispose();}

        MouseState mouse;
        public virtual void Update(ref Rectangle from, ref Rectangle to)
        {
            //mouse = Mouse.GetState();

            //if (rectangle.Contains(mouse.X, mouse.Y))
            //{
            //    if (!selected)
            //    color = colorHover;
            //    if (mouse.LeftButton == ButtonState.Pressed) // && available if not checked in game
            //    {
            //        selected = true;
            //        color = colorMarked;
            //        to = Rectangle.Empty;
            //        from = rectangle;
            //    }
            //}
            //else
            //{
            //    if (selected && to != Rectangle.Empty)
            //    {
            //        selectable = false;
            //        dealt = true;
            //        color = colorPlayed;

            //        rectangle.X = to.X + to.Width / 2 - rectangle.Width / 2;
            //        rectangle.Y = to.Y + to.Height / 2 - rectangle.Height / 2;
            //        to = Rectangle.Empty;
            //    }
            //    if (mouse.LeftButton == ButtonState.Pressed)
            //    {
            //        selected = false;
            //    }
            //    if (!selected && selectable)//enable if not checked in game
            //        color = colorNeutral;
            //}
        }

        public void move(Rectangle to)
        {
            rectangle.X = to.X + to.Width / 2 - rectangle.Width / 2;
            rectangle.Y = to.Y + to.Height / 2 - rectangle.Height / 2;

            dealt = true;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            fontPos.X = rectangle.X + rectangle.Width / 2;
            fontPos.Y = rectangle.Y;

            //spriteBatch.Begin();
                spriteBatch.Draw(sprite, rectangle, color);

                spriteBatch.DrawString(font, north.ToString(), fontPos, Color.Black);
                fontPos.Y += rectangle.Height - 20;
                spriteBatch.DrawString(font, south.ToString(), fontPos, Color.Black);

                fontPos.X = rectangle.X;
                fontPos.Y = rectangle.Y + rectangle.Height / 2;

                spriteBatch.DrawString(font, west.ToString(), fontPos, Color.Black);
                fontPos.X += rectangle.Width - 20;
                spriteBatch.DrawString(font, east.ToString(), fontPos, Color.Black);
            //spriteBatch.End();
        }


    }//end class========================================================
}//end namespace========================================================
