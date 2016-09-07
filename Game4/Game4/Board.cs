using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class Board : Obj
    {
        Rectangle rectangle;
        private string asset = "board";

        public Board(int top, int left, int width, int height)
        {
            spriteAsset = asset;
            rectangle = new Rectangle(top,left,width,height);
        }


        public void Unload()
        {
            sprite.Dispose();
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>(spriteFolder+spriteAsset);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rectangle, Color.White);
        }
    }
}
