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
    class Card : Obj
    {
        private bool selected;
        public bool Selected { get { return selected; } set { selected = value; } }
        private bool selectable;
        public bool Selectable { get { return selectable; } set { selectable = value; } }
        private Rectangle rectangle;
        public Rectangle Rectangle { get { return rectangle; } }

        private const int width = 100;
        private const int height = 100; 

        private Color color, neutral = Color.Gray, hover = Color.Red, marked = Color.Blue, played = Color.Green;
        //private int initX, initY;


        public Card() { }

        public Card(bool selectable, int x, int y)
        {
            this.selectable = selectable;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            //initX = x;
            //initY = y;
            selected = false;
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
                if(!selected)
                    color = hover;
                if (mouse.LeftButton == ButtonState.Pressed) // && available if not checked in game
                {
                    selected = true;
                    color = marked;
                    to = Rectangle.Empty;
                    from = rectangle;

                    //rectangle.X = mouse.X - rectangle.Width / 2;
                    //rectangle.Y = mouse.Y - rectangle.Height / 2;
                }
            }
            else
            {
                if (selected && to != Rectangle.Empty)
                {
                    selectable = false;
                    color = played;

                    rectangle.X = to.X + to.Width / 2 - rectangle.Width / 2;
                    rectangle.Y = to.Y + to.Height / 2 - rectangle.Height / 2;
                    to = Rectangle.Empty;
                    
                }
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    selected = false;
                }
                if (!selected && selectable)//enable if not checked in game
                    color = neutral;
            }

            
        }




        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rectangle, color);
        }


    }
}
