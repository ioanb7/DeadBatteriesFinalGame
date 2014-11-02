using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public abstract class GameObject
    {
        public GameObjectType Type;
        //maintenance
        public bool isActive = true;
        public bool isInitialized = false;

        //position
        protected Vector2 pos;
        protected Rectangle dimensions;

        public GameObject(Vector2 pos, Rectangle dimensions)
        {
            this.pos = pos;
            this.dimensions = dimensions;
        }

        public GameObject(Vector2 pos, Vector2 dimensions)
        {
            this.pos = pos;
            this.dimensions = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void LoadContent(GraphicsContentLoader graphicsContentLoader);

        public Rectangle getWorldRectangle()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, dimensions.Width, dimensions.Height);
        }
        public virtual Rectangle getCollisionRectangle()
        {
            return getWorldRectangle();
        }

        protected Rectangle getWorldRectangleAdjusted(int x, int y, int width, int height)
        {
            Rectangle rect = getWorldRectangle();

            rect.X += x;
            rect.Y += y;
            rect.Width -= width;
            rect.Height -= height;

            return rect;
        }
    }
}
