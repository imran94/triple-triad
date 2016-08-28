using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Game4
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont textFont;

        //Texture2D board;
        //string boardAsset = "board";
        Board board;

        Vector2 fontPos;

        System.Random rnd;

        const int player = 1, bot = -1; //or any other datatype or value as appropriate

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        Tile[] tiles = new Tile[9];
        Card[] playerCards = new Card[3];
        Card[] compCards = new Card[3];

        int windowW, windowH;
        int w, h, top, left;

        const int numOfCard = 3;

        private int[,] cardVal = new int[numOfCard, 4];

        //init==============================================================================
        protected override void Initialize()
        {
            windowW = 1280; windowH = 680;

            graphics.PreferredBackBufferWidth = windowW;
            graphics.PreferredBackBufferHeight = windowH;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;

            

            w = 150; h = 200;
            int offsetX = 0, offsetY = 10;

            board = new Board(windowW / 2, windowH / 2, offsetX, offsetY);

            left = (int)(windowW / 2 - 1.5 * w + offsetX);
            top = (int)(windowH / 2 - 1.5 * h + offsetY);

            for (int i = 0; i < 9; i++)
                tiles[i] = new Tile(i, true, left + i / 3 * w, top + i % 3 * h, w, h);

            rnd = new System.Random();


            //generate a 2d array, pass same value to both player and comp cards
            CardValueGenerator cardValGen = new CardValueGenerator(ref cardVal, numOfCard);
            for (int i = 0; i < 3; i++)
            {
                playerCards[i] = new Card(player, i, true, w, top + i * 205, cardVal[i,0], cardVal[i, 1], cardVal[i, 2], cardVal[i, 3]);
                compCards[i] = new Card(bot, i, false, windowW - w*2, top + i * 205, cardVal[i, 0], cardVal[i, 1], cardVal[i, 2], cardVal[i, 3]);
            }





            base.Initialize();
        }
        //end init==============================================================================



        Texture2D pixel;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textFont = Content.Load<SpriteFont>("Arial");
            fontPos = new Vector2();

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            

            board.LoadContent(Content);

            foreach (Tile t in tiles)
                t.LoadContent(pixel);

            foreach (Card c in playerCards)
                c.LoadContent(Content, pixel);

            foreach (Card c in compCards)
                c.LoadContent(Content, pixel);
        }

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();

            board.Unload();

            foreach (Tile t in tiles)
                t.Unload();

            foreach (Card c in playerCards)
                c.Unload();

            foreach (Card c in compCards)
                c.Unload();
        }
        
        MouseState mouse;
        Rectangle from, to;
        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Tile t in tiles)
                if(t.Available) //update might be more taxing than just checking just if tile is available
                    t.Update(ref from, ref to);

            foreach (Card c in playerCards)
            {
                if (c.Selectable) //update might be more taxing than just checking just if tile is available
                    c.Update(ref from, ref to);

                if (c.Moved)
                {
                    Debug.WriteLine("player moved");
                    nextTurn();
                    c.Moved = false;
                }
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin();
                fontPos.X = w;
                fontPos.Y = top;

                //spriteBatch.Draw(board, new Rectangle(0,0,460,610) ,Color.White);
                board.Draw(spriteBatch);

                foreach (Tile t in tiles)
                    t.Draw(spriteBatch);
                foreach (Card c in playerCards)
                    c.Draw(spriteBatch);
                foreach (Card c in compCards)
                    c.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void nextTurn()
        {
            int cardIndex, tileIndex;

            do
            {
                cardIndex = rnd.Next(0, numOfCard);
            }
            while (compCards[cardIndex].Dealt);
            compCards[cardIndex].Dealt = true;

            do
            {
                tileIndex = rnd.Next(0, 9);
            }
            while (!tiles[tileIndex].Available);
            tiles[tileIndex].Available = false;

            //Card selectedCard = compCards[cardIndex];
            //Tile selectedTile = tiles[tileIndex];

            //from = selectedCard.Rectangle;
            to = tiles[tileIndex].Rectangle;
            compCards[cardIndex].move(to);
            compCards[cardIndex].Selectable = false;

            Debug.WriteLine("cardIndex: " + cardIndex);
            Debug.WriteLine("tileIndex: " + tileIndex);
        }

        /*
        private int pickCard()
        {
            
            int selectableCards = 0;
            foreach (Card c in compCards)
            {
                if (c.Selectable)
                {
                    selectableCards++;
                }
            }

            if (selectableCards == 3)
            {
                return rnd.Next(0, 2);
            }
            else if (selectableCards == 2)
            {
                return rnd.Next(0, 1);
            }

            for (int i = 0; i < compCards.Length; i++)
            {
                if (compCards[i].Selectable)
                {
                    return i;
                }
            }

            return 0;
        }//*/





    }//end class========================================================
}//end namespace========================================================














