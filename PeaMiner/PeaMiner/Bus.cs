using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public class Bus : GameObject
    {
        Texture2D texture;
        string textureLocation;
        Point currentPointRectangle;

        Vector2 destinationPos;


        public Bus(Vector2 pos, Vector2 destinationPos, Vector2 dimensions) :
            base(pos, dimensions)
        {
            textureLocation = "bus";
            currentPointRectangle = new Point(0, 0);
            this.destinationPos = destinationPos;
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, dimensions.Width, dimensions.Height), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (pos.X < destinationPos.X)
                pos.X++;
            if (pos.X > destinationPos.X)
                pos.X--;
            if (pos.Y < destinationPos.Y)
                pos.Y++;
            if (pos.Y > destinationPos.Y)
                pos.Y--;
        }

        public int getBlockX()
        {
            return currentPointRectangle.X;
        }

        public int getBlockY()
        {
            return currentPointRectangle.Y;
        }



        private Rectangle getPlayerCollisionRectangle()
        {
            return getWorldRectangle();
        }




        private bool isValidRectangle(int i, int j)
        {
            bool valid = i >= 0 && i < TheGame.Instance.map.getHeight()
                && j >= 0 && j < TheGame.Instance.map.getWidth();

            return valid; // valid false? isTraversable won't be called.

        }


        public override void LoadContent(GraphicsContentLoader graphicsContentLoader)
        {
            if (textureLocation != "")
                texture = graphicsContentLoader.Get(textureLocation);

            isInitialized = true;
        }
        public override Rectangle getCollisionRectangle()
        {
            return getWorldRectangleAdjusted(100, 20, 100, 150);
        }

        public Rectangle getCenterRectangle()
        {
            return new Rectangle((int)pos.X + getWorldRectangle().Width / 2, (int)pos.Y + getWorldRectangle().Height / 2, 0, 0);
            //return new Rectangle((int)pos.X + getWorldRectangle().Width / 2 - 20 , (int)pos.Y + getWorldRectangle().Height / 2 - 20, 40, 40);
        }

        public Vector2 getPos()
        {
            return pos;
        }

        public void setPos(Vector2 vec)
        {
            pos = vec;
        }
    }
}
