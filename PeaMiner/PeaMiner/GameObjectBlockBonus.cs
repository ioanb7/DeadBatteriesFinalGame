using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public abstract class GameObjectBlockBonus : GameObjectBlock
    {
        public GameObjectBlockBonus(Point pos, Vector2 dimensions, GameObjectType type, string textureLocation) :
            base(pos, dimensions, type, textureLocation)
        {

        }

        public abstract void Consume(Player player);
    }
}
