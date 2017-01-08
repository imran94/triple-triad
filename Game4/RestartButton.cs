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
    class RestartButton : Obj
    {
        SpriteFont font;
        Vector2 fontPos;
        const string text = "RESTART";

        Point position;
            public Point Position { get { return position; } set { position = value; } }
        Rectangle rectangle;
            public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        const int width = 400;
        const int height = 200;

        Color recColor, recColorRest = Color.Transparent, recHover = Color.Black;
        Color fontColor, fontColorRest = Color.Black, fontHover = Color.White;

        public RestartButton(Point position)
        {
            this.position = position;
            Point dimensions = new Point(width, height);
            rectangle = new Rectangle(position, dimensions);

            fontPos = new Vector2(position.X + width / 3, position.Y + 20);

            recColor = recColorRest;
            fontColor = fontColorRest;
        }

        public void LoadContent(ContentManager content, Texture2D texture)
        {
            sprite = texture;
            font = content.Load<SpriteFont>("Arial");
        }

        public void Unload()
        {
            sprite.Dispose();
        }

        /**
            * @return true: If a player enters the button with mouse
            */
        public bool clicked()
        {
            return hovering() && Mouse.GetState().LeftButton == ButtonState.Pressed;
        }

        public bool hovering()
        {
            return rectangle.Contains(Mouse.GetState().Position);
        }

        public void Update(GameTime gameTime)
        {
            if (hovering())
            {
                recColor = recHover;
                fontColor = fontHover;
            }
            else
            {
                recColor = recColorRest;
                fontColor = fontColorRest;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rectangle, recColor);
            spriteBatch.DrawString(font, text, fontPos, fontColor);
        }
    }
}
