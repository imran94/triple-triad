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
        //since these lines are used repeatedly, might as well keep this class

        protected Texture2D sprite;
        static protected string spriteFolder = "_sprite\\";
        protected string spriteAsset;
    }
}