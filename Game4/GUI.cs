using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class GUI
    {
        //responsible for setting up the ui
        //setting dimensions and initial coordinates
        //updating view

        Board board;
        public Tile[] tiles;
        public Card[] cards_player, cards_bot;

        private bool showMouse = true;
            public bool ShowMouse { get { return showMouse; } }

        private int width = 1280, height = 720;
            public int Width { get { return width; } }
            public int Height { get { return height; } }


        private int boardAssetWidth = 460, boardAssetHeight = 610; //for that extra border, blame my OCD
        private int boardWidth = 450, boardHeight = 600;

        private int offsetX = 0, offsetY = 10, cardOffset = 40;

        private int tileWidth = 150, tileHeight = 200;


        SpriteFont font;
        Vector2 fontPos;

        private int boardLeft, boardTop;

        public GUI(ref Tile[] tiles, ref Card[] cards_player, ref Card[] cards_bot)
        {
            this.tiles = tiles;
            this.cards_player = cards_player;
            this.cards_bot = cards_bot;

            boardLeft = width / 2 - boardAssetWidth / 2 + offsetX;
            boardTop = height / 2 - boardAssetHeight / 2 + offsetY;
            board = new Board(boardLeft, boardTop, boardAssetWidth, boardAssetHeight);

            boardLeft = width / 2 - boardWidth / 2 + offsetX;
            boardTop = height / 2 - boardHeight / 2 + offsetY;

            for (int i = 0; i < tiles.Length; i++)
                tiles[i].Rectangle = new Rectangle(boardLeft + i % 3 * tileWidth, boardTop + i / 3 * tileHeight, tileWidth, tileHeight);

            for (int i = 0; i < cards_player.Length; i++)
            {
                if (i < 3)
                {
                    cards_player[i].Rectangle = new Rectangle(cardOffset, boardTop + i * tileHeight, tileWidth, tileHeight);
                    cards_bot[i].Rectangle = new Rectangle(width - cardOffset - tileWidth, boardTop + i * tileHeight, tileWidth, tileHeight);
                }
                else
                {
                    cards_player[i].Rectangle = new Rectangle(cardOffset + tileWidth, tileHeight / 2 + boardTop + i % 3 * tileHeight, tileWidth, tileHeight);
                    cards_bot[i].Rectangle = new Rectangle(width - cardOffset - tileWidth * 2, tileHeight / 2 + boardTop + i % 3 * tileHeight, tileWidth, tileHeight);
                }
            }
        }

        /*
        public void Reset(ref Tile[] tiles, ref Card[] cards_player, ref Card[] cards_bot)
        {
            this.tiles = tiles;
            this.cards_player = cards_player;
            this.cards_bot = cards_bot;

            int boardLeft = width / 2 - boardWidth / 2 + offsetX;
            int boardTop = height / 2 - boardHeight / 2 + offsetY;

            for (int i = 0; i < cards_player.Length; i++)
            {
                if (i < 3)
                {
                    cards_player[i].Rectangle = new Rectangle(cardOffset, boardTop + i * tileHeight, tileWidth, tileHeight);
                    cards_bot[i].Rectangle = new Rectangle(width - cardOffset - tileWidth, boardTop + i * tileHeight, tileWidth, tileHeight);
                }
                else
                {
                    cards_player[i].Rectangle = new Rectangle(cardOffset + tileWidth, tileHeight / 2 + boardTop + i % 3 * tileHeight, tileWidth, tileHeight);
                    cards_bot[i].Rectangle = new Rectangle(width - cardOffset - tileWidth * 2, tileHeight / 2 + boardTop + i % 3 * tileHeight, tileWidth, tileHeight);
                }
            }
        }//*/

        public void LoadContent(ContentManager content, Texture2D pixel)
        {
            board.LoadContent(content);
            foreach (Tile t in tiles)
                t.LoadContent(pixel);
            for (int i = 0; i < cards_bot.Length; i++)
            {
                cards_player[i].LoadContent(content);
                cards_bot[i].LoadContent(content);
            }

            font = content.Load<SpriteFont>("Arial");

            fontPos = new Vector2(boardLeft, offsetY);
        }

        public void Unload()
        {
            board.Unload();
            foreach (Tile t in tiles)
                t.Unload();
            for (int i = 0; i < cards_bot.Length;i++ )
            { 
                cards_player[i].Unload();
                cards_bot[i].Unload();
            }
        }

        private string endText = "";
        public string EndText { get{ return endText; }set { endText = value; } }

        public void Draw(SpriteBatch spriteBatch)
        {
            board.Draw(spriteBatch);
            foreach (Tile t in tiles)
                t.Draw(spriteBatch);
            for (int i = 0; i < cards_bot.Length; i++)
            {
                cards_player[i].Draw(spriteBatch);
                cards_bot[i].Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, endText, fontPos, Color.Black);
        }

        public void Update()
        {
            for (int i = 0; i < cards_bot.Length; i++)
            {
                cards_player[i].Update();
                cards_bot[i].Update();
            }
        }
    }
}
