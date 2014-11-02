using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public class Free : GameObjectBlock
    {
        public Free(Point pos, Vector2 dimensions) :
            base(pos, dimensions, GameObjectType.Free, "")
        {

        }
    }
}
