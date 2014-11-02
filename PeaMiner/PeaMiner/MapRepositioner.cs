using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public class MapRepositioner
    {
        public static Rectangle displayRect;
        public static Rectangle Reposition(Rectangle rect)
        {
            Rectangle rect2 = rect;

            if (rect.X < displayRect.X) rect2.X = 0;
            if (rect.X + rect.Width > displayRect.X + displayRect.Width) rect2.X = displayRect.X + displayRect.Width - rect.Width;

            if (rect.Y < displayRect.Y) rect2.Y = 0;
            if (rect.Y + rect.Height > displayRect.Y + displayRect.Height) rect2.Y = displayRect.Y + displayRect.Height - rect.Height;

            return rect2;
        }


        public static Vector2 GetRightCoordinates(Rectangle worldRectangle, Rectangle collisionRect)
        {
            Vector2 mapSize = new Vector2(displayRect.Width, displayRect.Height);	

            //Vector2 newPos = new Vector2(displayRect.X, displayRect.Y);
            Vector2 newPos = new Vector2(collisionRect.X, collisionRect.Y);

            if (collisionRect.X + collisionRect.Width >= mapSize.X)
            {
                newPos.X = mapSize.X - collisionRect.Width;
            }
            if (newPos.X < 0)
            {
                newPos.X = 0;
            }

            if (collisionRect.Y + collisionRect.Height >= mapSize.Y)
            {
                newPos.Y = mapSize.Y - collisionRect.Height;
            }
            if (newPos.Y < 0)
            {
                newPos.Y = 0;
            }

            /////
            newPos.X -= (collisionRect.X - worldRectangle.X);
            newPos.Y -= (collisionRect.Y - worldRectangle.Y);

            return newPos;
        }
    }
}
