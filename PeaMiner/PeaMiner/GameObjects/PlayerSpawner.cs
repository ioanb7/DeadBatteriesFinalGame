using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public class PlayerSpawner : GameObjectBlock
    {
        public PlayerSpawner(Point pos, Vector2 dimensions) :
            base(pos, dimensions, GameObjectType.PlayerSpawner, "playerSpawner")
        {
            
        }
    }
}
