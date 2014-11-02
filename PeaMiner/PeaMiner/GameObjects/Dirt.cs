using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public class Dirt : GameObjectBlock
    {
        public Dirt(Point pos, Vector2 dimensions) :
            base(pos, dimensions, GameObjectType.Dirt, "dirt")
        {
            
        }
    }
}
