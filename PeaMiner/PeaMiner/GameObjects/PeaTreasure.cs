using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public class Pea : GameObjectBlockBonus
    {
        public Pea(Point pos, Vector2 dimensions) :
            base(pos, dimensions, GameObjectType.Pea, "pea")
        {

        }

        public override void Consume(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
