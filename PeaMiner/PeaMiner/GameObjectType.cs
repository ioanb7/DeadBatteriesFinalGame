using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public enum GameObjectType
    {
        Free = 0,
        Dirt = 1,
        Pea = 2,
        Teleporter = 3,
        BusStart = 4,
        BusFinish = 5,
        PlayerSpawner = 9,

        Player = 99,
        Bullet
    }
}
