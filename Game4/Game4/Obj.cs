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
    class Obj
    {
        protected Texture2D sprite;

        protected Vector2 location;
        protected float rotation = 0.0f;
        protected float scale = 1.0f;
        protected string spriteAsset;

        public Obj(Vector2 coordinate)
        {
            location = coordinate;
        }

        public Obj()
        {
        }

        public virtual void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("temp\\" + this.spriteAsset);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            spriteBatch.Draw(sprite, location, null, Color.White, rotation, center, scale, SpriteEffects.None, 0);
        }

        public virtual void Update()
        { }

    }
}