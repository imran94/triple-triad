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
        private bool available; //, 0 available, 1 active
            public bool Available { get { return available; } set { available = value; } }
        private Rectangle rectangle;
            public Rectangle Rectangle { get { return rectangle; } }

        private Color color, neutral = Color.Gray, hover = Color.Red, marked = Color.Blue;

        public Tile() { }

        public Tile(bool available, int x, int y, int w, int h)
        {
            this.available = available;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = w;
            rectangle.Height = h;

            color = neutral;
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
            mouse = Mouse.GetState();

            if (rectangle.Contains(mouse.X, mouse.Y))
            {
                color = hover;
                if (mouse.LeftButton == ButtonState.Pressed && from != Rectangle.Empty) // && available if not checked in game
                {
                    //color = marked;
                    available = false;
                    to = rectangle;
                    from = Rectangle.Empty;

                    color = neutral;
                }
            }
            else
            {
                //if(available) //enable if not checked in game
                color = neutral;
            }
        }




        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rectangle, color);
        }


    }
}
