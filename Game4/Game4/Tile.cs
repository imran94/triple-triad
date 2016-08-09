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
        Color color = Color.Gray;

        private int state;
            public int State { get { return state; } set { state = value; } }
        private Rectangle rectangle;
            public Rectangle Rectangle { get { return rectangle; } }


        public Tile() { }

        public Tile(int state, int x, int y, int w, int h)
        {
            this.state = state;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = w;
            rectangle.Height = h;
        }



        public void LoadContent(Texture2D texture)
        {
            sprite = texture;
        }


        public void Unload()
        {
            sprite.Dispose();
        }


        public override void Update()
        {
            if (state > 0)
            {
                color = Color.Red;
            }
            else
            {
                color = Color.Gray;
            }
        }




        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rectangle, color);
        }


    }
}
