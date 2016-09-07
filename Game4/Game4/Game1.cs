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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //======================================================================================
        //======================================================================================
        //begin ================================================================================
        //======================================================================================

        GUI gui;

        public const int NUM_OF_CARD = 5, NUM_OF_TILE = 9, NUM_OF_SIDE = 4;
        static Tile[] tiles = new Tile[NUM_OF_TILE];
        static Card[] cards_player = new Card[NUM_OF_CARD];
        static Card[] cards_bot = new Card[NUM_OF_CARD];


        private int[,] cardVal = new int[NUM_OF_CARD, NUM_OF_SIDE];
        private int turn;


        //init==============================================================================
        protected override void Initialize()
        {

            for (int i = 0; i < NUM_OF_TILE; i++)
                tiles[i] = new Tile(i);

            //generate a 2d array, pass same value to both player and comp cards
            CardValueGenerator cardValGen = new CardValueGenerator(ref cardVal, NUM_OF_CARD);
            for (int i = 0; i < NUM_OF_CARD; i++)
            {
                cards_player[i] = new Card((int)Enum.Player.Human, i, cardVal[i, (int)Enum.CardSide.N], cardVal[i, (int)Enum.CardSide.S], cardVal[i, (int)Enum.CardSide.E], cardVal[i, (int)Enum.CardSide.W]);
                cards_bot[i] = new Card((int)Enum.Player.Bot, i, cardVal[i, (int)Enum.CardSide.N], cardVal[i, (int)Enum.CardSide.S], cardVal[i, (int)Enum.CardSide.E], cardVal[i, (int)Enum.CardSide.W]);
            }

            turn = startGame();


            gui = new GUI(ref tiles, ref cards_player, ref cards_bot);
            this.IsMouseVisible = gui.ShowMouse;

            Window.Position = new Point(100, 20);
            graphics.PreferredBackBufferWidth = gui.Width;
            graphics.PreferredBackBufferHeight = gui.Height;
            graphics.ApplyChanges();

            base.Initialize();
        }
        //end init==============================================================================



        Texture2D pixel;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            
            gui.LoadContent(Content, pixel);
        }

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();
            gui.Unload(); 
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin();
            gui.Draw(spriteBatch);
<<<<<<< HEAD
            //spriteBatch.DrawString(font, endText, fontPos, Color.Black);
            //spriteBatch.DrawString(font, "PRESS R KEY TO RESTART", fontPos2, Color.Black);
=======
            spriteBatch.DrawString(font, endText, fontPos, Color.Black);
            spriteBatch.DrawString(font, "PRESS R KEY TO RESTART", fontPos2, Color.Black);
>>>>>>> origin/lettuce
            spriteBatch.End();

            base.Draw(gameTime);
        }











        //begin update=============================================================

        Control human = new Human(ref cards_player, ref tiles);
        //Control bot = new Human(ref cards_bot, ref tiles);
        Control bot = new Bot(ref cards_bot, ref tiles, ref cards_player);

        bool swap = false;
        int count = 0;


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                restart();

            if (count < tiles.Length) //stop updating after 9 turns
            {
                switch (turn)
                {
                    case (int)Enum.Player.Human:
                        {
                            swap = human.Update();
                            break;
                        }
                    case (int)Enum.Player.Bot:
                        {
                            swap = bot.Update();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            else
            {
                if (Control.Score > 0)
                {
                    gui.EndText = "YOU WIN, PRESS ENTER TO RESTART";
                }
                else if (Control.Score < 0)
                {
                    gui.EndText = "YOU LOSE, PRESS ENTER TO RESTART";
                }
                else
                {
                    gui.EndText = "DRAW, PRESS ENTER TO RESTART";
                }
            }


            if (swap)
            {
                turn = swapPlayer(turn);
                count++;
            }

            gui.Update();
            base.Update(gameTime);
        }//end update=============================================================



        private int startGame()
        {
            //set turn and consequently initial score

            Random random = new Random();
            int rng = random.Next(0,2);

            if(rng >= 1)
                rng = (int)Enum.Player.Human; //yeah i re-use variable rng
            else
                rng = (int)Enum.Player.Bot;

            Control.Score += -1 * rng; //score is biased away from P1 because P1 has 1 extra turn by default
            //Debug.WriteLine(Control.Score);
            return rng ;
        }


        private int swapPlayer(int turn)
        {
            return turn *= -1; //change according to type and value that represents player
        }

        private void restart()
        {
<<<<<<< HEAD
            Program.restart = true;
            this.Exit();
=======
            endText = "";
            count = 0;

            turn = startGame();

            foreach (Tile t in tiles)
                t.Reset();

            CardValueGenerator cardValGen = new CardValueGenerator(ref cardVal, NUM_OF_CARD);
            for (int i = 0; i < NUM_OF_CARD; i++)
            {
                cards_player[i].Reset((int)Enum.Player.Human, cardVal[i, (int)Enum.CardSide.N], cardVal[i, (int)Enum.CardSide.S], cardVal[i, (int)Enum.CardSide.E], cardVal[i, (int)Enum.CardSide.W]);
                cards_bot[i].Reset((int)Enum.CardState.Rest, cardVal[i, (int)Enum.CardSide.N], cardVal[i, (int)Enum.CardSide.S], cardVal[i, (int)Enum.CardSide.E], cardVal[i, (int)Enum.CardSide.W]);
            }

            human.Reset(ref cards_player, ref tiles);
            bot.Reset(ref cards_bot, ref tiles);

            gui.Reset(ref tiles, ref cards_player, ref cards_bot);
>>>>>>> origin/lettuce
        }





















        
        






    }//end class========================================================
}//end namespace========================================================














