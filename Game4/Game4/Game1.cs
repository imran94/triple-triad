using Microsoft.Xna.Framework;
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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        Tile[,] tiles = new Tile[3, 3];
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
                tiles[i / 3, i % 3] = new Tile(true, left + i / 3 * w, top + i % 3 * h, w, h);

            //temporary values
            for (int i = 0; i < 3; i++)
            {
                playerCards[i] = new Card(true, w, top + i * 105);
                compCards[i] = new Card(true, windowW - w, top + i * 105);
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
                c.LoadContent(pixel);

            foreach (Card c in compCards)
                c.LoadContent(pixel);
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
                if (c.Selectable) //update might be more taxing than just checking just if tile is available
                    c.Update(ref from, ref to);

            foreach (Card c in compCards)
                if (c.Selectable) //update might be more taxing than just checking just if tile is available
                    c.Update(ref from, ref to);

            Debug.WriteLine(to);

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
    }
}














