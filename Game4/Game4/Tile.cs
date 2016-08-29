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
    class Tile : Obj
    {
        private int id;
            public int ID { get { return id; } }
        private bool available; //, 0 available, 1 active
            public bool Available { get { return available; } set { available = value; } }
        private Rectangle rectangle;
            public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }
        private Card card;
            public Card Card { get { return card; } set { card = value; } }

        //public Tile All { get { return this; } }

        public Color color, colorNeutral = Color.Gray, colorHover = Color.Red, colorMarked = Color.Blue;

        public Tile() { }

        public Tile(bool available, int x, int y, int w, int h)
        {
            this.available = available;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = w;
            rectangle.Height = h;

            card = null;
            color = colorNeutral;
        }

        public Tile(int id, bool available, int x, int y, int w, int h)
        {
            this.available = available;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = w;
            rectangle.Height = h;

            color = colorNeutral;
        }


        public void LoadContent(Texture2D texture)
        {
            sprite = texture;
        }


        public void Unload()
        {
            sprite.Dispose();
        }

        MouseState mouse;
        public void Update(ref Rectangle from, ref Rectangle to)
        {
            //mouse = Mouse.GetState();

            //if (rectangle.Contains(mouse.X, mouse.Y))
            //{
            //    color = colorHover;
            //    if (mouse.LeftButton == ButtonState.Pressed && from != Rectangle.Empty) // && available if not checked in game
            //    {
            //        //color = marked;
            //        available = false;
            //        to = rectangle;
            //        from = Rectangle.Empty;

            //        color = colorNeutral;
            //    }
            //}
            //else
            //{
            //    //if(available) //enable if not checked in game
            //    color = colorNeutral;
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rectangle, color);
        }

        public void addCard(ref Card card)
        {
            card.move(rectangle);
            available = false;
        }

        public bool hasCard() { return card != null; }


    }//end class========================================================
}//end namespace========================================================