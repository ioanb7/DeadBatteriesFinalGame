using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PeaMiner
{
    public class TheGame
    {
        public Map map;
        public TheGame()
        {
            gameObjectList = new List<GameObject>();
        }

        public GameState gameState;
        public bool isGameRunning = true;

        public List<GameObject> gameObjectList;



        private decimal lastFiredBullet = 0;
        static private decimal bulletFireInterval = 10000;


        public void Update(GameTime gameTime)
        {
            lastFiredBullet += gameTime.TotalGameTime.Milliseconds;
        }

        public void Fire(Vector2 firePos, Direction playerDirection)
        {
            if (lastFiredBullet - bulletFireInterval >= 0)
            {
                lastFiredBullet %= bulletFireInterval;
                //fire a bullet
            }
            else
            {
                return;
            }
            float bulletSpeed = 20.0f * 10;

            Vector2 bulletVelocity = new Vector2(0, 0);
            if (playerDirection == Direction.Right)
                bulletVelocity.X += bulletSpeed;
            if (playerDirection == Direction.Down)
                bulletVelocity.Y += bulletSpeed;
            if (playerDirection == Direction.Left)
                bulletVelocity.X += -bulletSpeed;
            if (playerDirection == Direction.Up)
                bulletVelocity.Y += -bulletSpeed;

            gameObjectList.Add(new Bullet(firePos,
                bulletVelocity,
                GameObjectType.Player));

        }













        #region stuff

        static readonly TheGame _instance = new TheGame();

        public static TheGame Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion
    }
}