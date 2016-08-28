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
        Rectangle board;
        public Board(int x = 0, int y = 0, int offsetX = 0, int offsetY = 0, int w = 450, int h = 600)
        {
            spriteAsset = "board";
            x -= w / 2;
            y -= h / 2;
            x += offsetX;
            y += offsetY;
            board = new Rectangle(x,y,w,h);
        }

        public void Unload()
        {
            sprite.Dispose();
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("_sprite\\" + this.spriteAsset);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, board, Color.White);
        }
    }
}
