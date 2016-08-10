﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        System.Random rnd;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        Tile[] tiles = new Tile[9];
        Card[] playerCards = new Card[3];
        Card[] compCards = new Card[3];

        int windowW, windowH;

        protected override void Initialize()
        {
            windowW = 1280; windowH = 680;

            graphics.PreferredBackBufferWidth = windowW;
            graphics.PreferredBackBufferHeight = windowH;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;

            //tiles[0] = new Tile(0, 10, 10, 100, 100);
            //tiles[1] = new Tile(0, 400, 10, 100, 100);

            int w = 150, h = 200,
                offsetX = 0, offsetY = 10,
                left = (int)(windowW / 2 - 1.5 * w + offsetX),
                top = (int)(windowH / 2 - 1.5 * h + offsetY);

            for (int i = 0; i < 9; i++)
                tiles[i] = new Tile(true, left + i / 3 * w, top + i % 3 * h, w, h);

            rnd = new System.Random();

            //temporary values
            for (int i = 0; i < 3; i++)
            {
                playerCards[i] = new Card(true, w, top + i * 105, rnd);
                compCards[i] = new Card(false, windowW - w, top + i * 105, rnd);
            }

            base.Initialize();
        }



        Texture2D pixel;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

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
                    Debug.WriteLine("Card moved");
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
            int cardIndex = 0;
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
                cardIndex = rnd.Next(0, 2);
            }
            else if (selectableCards == 2)
            {
                cardIndex = rnd.Next(0, 1);
            }
            else
            {
                for (int i = 0; i < compCards.Length; i++)
                {
                    if (compCards[i].Selectable)
                    {
                        cardIndex = i;
                    }
                }
            }

            int tileIndex = rnd.Next(0, 9);

            while (!tiles[tileIndex].Available)
            {
                tileIndex = rnd.Next(0, 9);
            }

            Card selectedCard = compCards[cardIndex];
            Tile selectedTile = tiles[tileIndex];

            Rectangle cardRectangle = selectedCard.Rectangle;
            Rectangle tileRectangle = selectedTile.Rectangle;

            Debug.WriteLine("cardIndex: " + cardIndex);
            Debug.WriteLine("tileIndex: " + tileIndex);

            selectedCard.Update(ref cardRectangle, ref tileRectangle);
        }
    }
}














