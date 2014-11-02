using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public abstract class GameObjectBlock : GameObject
    {
        private string textureLocation;
        private Texture2D texture;

        public Point pointPos;

        public GameObjectBlock(Point pos, Vector2 dimensions, GameObjectType type, string textureLocation) :
            base(new Vector2(pos.Y * dimensions.X, pos.X * dimensions.Y), dimensions)
        {
            this.Type = type;
            this.textureLocation = textureLocation;
            this.pointPos = pos;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if(texture != null)
                spriteBatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, dimensions.Width, dimensions.Height), Color.White);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void LoadContent(GraphicsContentLoader graphicsContentLoader)
        {
            if(textureLocation != "")
                texture = graphicsContentLoader.Get(textureLocation);

            isInitialized = true;
        }
    }
}
