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



        //Tile t = new Tile(0, 10, 10, 100, 100);
        //Tile[] tiles = new Tile[2];
        Tile[,] tiles = new Tile[3, 3];

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

            int w = 150, h = 200, offsetX = 0, offsetY = 10, left = (int)(windowW /2 - 1.5*w + offsetX), top = (int)(windowH / 2 - 1.5 * h + offsetY);

            for (int i = 0; i < 9; i++)
            {
                tiles[i / 3, i % 3] = new Tile(0, left + i / 3 * w, top + i % 3 * h, w, h);
            }

            base.Initialize();
        }



        Texture2D pixel;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            //t.LoadContent(pixel);

            foreach (Tile ts in tiles)
            { ts.LoadContent(pixel); }
        }




        protected override void UnloadContent()
        {
            spriteBatch.Dispose();

            //t.Unload();
            foreach (Tile ts in tiles)
            { ts.Unload(); }
        }




        
        MouseState mouse;
        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            /*
            if (t.Rectangle.Contains(mouse.X, mouse.Y))
            {
                t.State = 1;
                Debug.WriteLine(mouse.X + "," + mouse.Y);
            }
            else
            {
                t.State = 0;
            }
            t.Update();
            //*/


            foreach (Tile ts in tiles)
            {
                if (ts.Rectangle.Contains(mouse.X, mouse.Y))
                {
                    ts.State = 1;
                    //Debug.WriteLine(mouse.X + "," + mouse.Y);
                }
                else
                {
                    ts.State = 0;
                }
                ts.Update();
            }

            //

            base.Update(gameTime);
        }






        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin();
                //t.Draw(spriteBatch);
                foreach (Tile ts in tiles)
                { ts.Draw(spriteBatch); }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}














