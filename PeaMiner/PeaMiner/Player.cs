using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public class Player : GameObject
    {
        Texture2D texture;
        string textureLocation;
        Point currentPointRectangle;

        bool isJumping = false;
        bool isInAir = false;
        bool isSpaceAvailable = true;
        int jumpHigh = 50;
        int jumpPos = 0;

        public bool playerMoved;

        Direction playerDirection;

        public int Peas { get; set; }

        public Player(Vector2 pos, Vector2 dimensions) :
            base(pos, dimensions)
        {
            textureLocation = "player";
            currentPointRectangle = new Point(0, 0);
            jumpHigh = (int)dimensions.Y  * 4 + 20;
            Peas = 0;
            playerDirection = Direction.Right;
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, dimensions.Width, dimensions.Height), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            updatePlayerPosition();
        }

        private KeyboardStateCustom oldKeyboardCustom;
        public void UpdateControl(KeyboardStateCustom keyboardState)
        {
            if (oldKeyboardCustom != null && oldKeyboardCustom != keyboardState)
                playerMoved = true;


            if (keyboardState.IsKeyDown(KeysMe.Player1Jump) && !isInAir)// && !isJumping)
            {
                isJumping = true;
                isInAir = true;
                jumpPos = (int)getPos().Y;
            }
            else
            {
                if (isInAir == false && !isSpaceAvailable)
                    isSpaceAvailable = true;
            }

            Vector2 velocity = Vector2.Zero;

            if (keyboardState.IsKeyDown(KeysMe.Player1Down))
            {
                velocity.Y++;
                playerDirection = Direction.Down;
            }
            if (keyboardState.IsKeyDown(KeysMe.Player1Up))
            {
                velocity.Y--;
                playerDirection = Direction.Up;
            }
            if (keyboardState.IsKeyDown(KeysMe.Player1Left))
            {
                velocity.X--;
                playerDirection = Direction.Left;
            }
            if (keyboardState.IsKeyDown(KeysMe.Player1Right))
            {
                velocity.X++;
                playerDirection = Direction.Right;
            }

            if(keyboardState.IsKeyDown(KeysMe.Player1Fire))
            {
                TheGame.Instance.Fire(pos + new Vector2(getWorldRectangle().Width, getWorldRectangle().Height) / 5, playerDirection);
            }


            if(isJumping == true)
            {
                if (jumpPos - getPos().Y > jumpHigh)
                {
                    isJumping = false;
                    isInAir = true;
                }
            }

            if (isJumping == true)
            {
                velocity.Y -= 2;
            }

            velocity.Y += 1;

            int pixelsPerSecond = 10;

            //pos += velocity * pixelsPerSecond;

            Vector2 posCopy2 = pos;

            Vector2 posCopy = pos;

            pos += velocity * pixelsPerSecond;

            /*
            for(int i = 1; i < pixelsPerSecond; i++)
            {
                posCopy = posCopy2 + velocity * i;

                List<Direction> collisionDirections = getDirections();

                foreach(Direction direction in collisionDirections)
                {

                    if (direction == Direction.Left)
                        velocity.X = Math.Min(velocity.X, 0);
                    if (direction == Direction.Right)
                        velocity.X = Math.Max(velocity.X, 0);

                    if (direction == Direction.Up)
                        velocity.Y = Math.Min(velocity.Y, 0);
                    if (direction == Direction.Down)
                        velocity.Y = Math.Max(velocity.Y, 0);



                }

                pos = posCopy;
                updatePlayerPosition();
            }*/

            //bottom collision
            if (isValidRectangle(getBlockX() + 1, getBlockY()) && !TheGame.Instance.map.isTraversable(getBlockX() + 1, getBlockY()))
            {
                debugInfo("yea.");
                GameObjectBlock block = TheGame.Instance.map[getBlockX() + 1][getBlockY()];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX() + 1][getBlockY()].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(getPos().X, blockCollision.Y - getPlayerCollisionRectangle().Height - 1)
                        );
                    isJumping = false;
                    isInAir = false;
                }

            }

            if (isValidRectangle(getBlockX() + 1, getBlockY() + 1) && !TheGame.Instance.map.isTraversable(getBlockX() + 1, getBlockY() + 1))
            {
                debugInfo("yea.2");
                GameObjectBlock block = TheGame.Instance.map[getBlockX() + 1][getBlockY() + 1];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX() + 1][getBlockY() + 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(getPos().X, blockCollision.Y - getPlayerCollisionRectangle().Height - 1)
                        );
                    isJumping = false;
                    isInAir = false;
                }

            }

            if (isValidRectangle(getBlockX() + 1, getBlockY() - 1) && !TheGame.Instance.map.isTraversable(getBlockX() + 1, getBlockY() - 1))
            {
                debugInfo("yea.3");
                GameObjectBlock block = TheGame.Instance.map[getBlockX() + 1][getBlockY() - 1];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX() + 1][getBlockY() - 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(getPos().X, blockCollision.Y - getPlayerCollisionRectangle().Height - 1)
                        );
                    isJumping = false;
                    isInAir = false;
                }
            }

            //right

            if (isValidRectangle(getBlockX(), getBlockY() + 1) && !TheGame.Instance.map.isTraversable(getBlockX(), getBlockY() + 1))
            {
                debugInfo("yea.2");
                GameObjectBlock block = TheGame.Instance.map[getBlockX()][getBlockY() + 1];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX()][getBlockY() + 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(blockCollision.X - getPlayerCollisionRectangle().Width - 1,
                                    getPos().Y)
                        );
                }

            }
            if (isValidRectangle(getBlockX() - 1, getBlockY() + 1) && !TheGame.Instance.map.isTraversable(getBlockX() - 1, getBlockY() + 1))
            {
                debugInfo("yea.2");
                GameObjectBlock block = TheGame.Instance.map[getBlockX() - 1][getBlockY() + 1];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX() - 1][getBlockY() + 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(blockCollision.X - getPlayerCollisionRectangle().Width - 1,
                                    getPos().Y)
                        );
                }

            }
            if (isValidRectangle(getBlockX() + 1, getBlockY() + 1) && !TheGame.Instance.map.isTraversable(getBlockX() + 1, getBlockY() + 1))
            {
                debugInfo("yea.2");
                GameObjectBlock block = TheGame.Instance.map[getBlockX() + 1][getBlockY() + 1];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX() + 1][getBlockY() + 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(blockCollision.X - getPlayerCollisionRectangle().Width - 1,
                                    getPos().Y)
                        );
                }

            }

            //left
            if (isValidRectangle(getBlockX(), getBlockY() - 1) && !TheGame.Instance.map.isTraversable(getBlockX(), getBlockY() - 1))
            {
                debugInfo("yea.2");
                GameObjectBlock block = TheGame.Instance.map[getBlockX()][getBlockY() - 1];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX()][getBlockY() - 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(blockCollision.X + blockCollision.Width + 1,
                                    getPos().Y)
                        );
                }

            }
            if (isValidRectangle(getBlockX() - 1, getBlockY() - 1) && !TheGame.Instance.map.isTraversable(getBlockX() - 1, getBlockY() - 1))
            {
                debugInfo("yea.2");
                GameObjectBlock block = TheGame.Instance.map[getBlockX() - 1][getBlockY() - 1];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX() - 1][getBlockY() - 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(blockCollision.X + blockCollision.Width + 1,
                                    getPos().Y)
                        );
                }

            }

            if (isValidRectangle(getBlockX() + 1, getBlockY() - 1) && !TheGame.Instance.map.isTraversable(getBlockX() + 1, getBlockY() - 1))
            {
                debugInfo("yea.2");
                GameObjectBlock block = TheGame.Instance.map[getBlockX() + 1][getBlockY() - 1];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX() + 1][getBlockY() - 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(blockCollision.X + blockCollision.Width + 1,
                                    getPos().Y)
                        );
                }

            }

            //top
            if (isValidRectangle(getBlockX() - 1, getBlockY()) && !TheGame.Instance.map.isTraversable(getBlockX() - 1, getBlockY()))
            {
                debugInfo("yea.2");
                GameObjectBlock block = TheGame.Instance.map[getBlockX() - 1][getBlockY()];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX() - 1][getBlockY()].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(getPos().X,
                                    blockCollision.Y + blockCollision.Height + 1)
                        );
                    isJumping = false;
                    isInAir = false;
                }

            }
            if (isValidRectangle(getBlockX() - 1, getBlockY() - 1) && !TheGame.Instance.map.isTraversable(getBlockX() - 1, getBlockY() - 1))
            {
                debugInfo("yea.2");
                GameObjectBlock block = TheGame.Instance.map[getBlockX() - 1][getBlockY() - 1];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX() - 1][getBlockY() - 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(getPos().X,
                                    blockCollision.Y + blockCollision.Height + 1)
                        );
                    isJumping = false;
                    isInAir = false;
                }

            }
            if (isValidRectangle(getBlockX() - 1, getBlockY() + 1) && !TheGame.Instance.map.isTraversable(getBlockX() - 1, getBlockY() + 1))
            {
                debugInfo("yea.2");
                GameObjectBlock block = TheGame.Instance.map[getBlockX() - 1][getBlockY() + 1];
                Rectangle blockCollision = block.getCollisionRectangle();
                if (TheGame.Instance.map[getBlockX() - 1][getBlockY() + 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle()))
                {
                    setPos(
                        new Vector2(getPos().X,
                                    blockCollision.Y + blockCollision.Height + 1)
                        );
                    isJumping = false;
                    isInAir = false;
                }

            }

            //pos = MapRepositioner.GetRightCoordinates(getWorldRectangle(), getCollisionRectangle());
            pos = MapRepositioner.GetRightCoordinates(getWorldRectangle(), getWorldRectangle());
            pos.Y = Math.Max(pos.Y, 1);

            oldKeyboardCustom = keyboardState;
        }

#region Collisions



        public List<Direction> getDirections()
        {
            List<Direction> list = new List<Direction>();

            if (isBottomCollision())
                list.Add(Direction.Down);
            if (isTopCollision())
                list.Add(Direction.Up);

            if (isLeftCollision())
                list.Add(Direction.Left);
            if (isRightCollision())
                list.Add(Direction.Right);

            return list;
        }

        private bool isBottomCollision()
        {
            return isValidRectangle(getBlockX() + 1, getBlockY()) && TheGame.Instance.map[getBlockX() + 1][getBlockY()].getCollisionRectangle().Intersects(getPlayerCollisionRectangle())
                ;//|| isValidRectangle(getBlockX() + 1, getBlockY() + 1) && TheGame.Instance.map[getBlockX() + 1][getBlockY() + 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle())
                //|| isValidRectangle(getBlockX() + 1, getBlockY() - 1) && TheGame.Instance.map[getBlockX() + 1][getBlockY() - 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle());
        }

        private bool isLeftCollision()
        {
            return isValidRectangle(getBlockX(), getBlockY() - 1) && TheGame.Instance.map[getBlockX()][getBlockY() - 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle())
                || isValidRectangle(getBlockX() - 1, getBlockY() - 1) && TheGame.Instance.map[getBlockX() - 1][getBlockY() - 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle())
                || isValidRectangle(getBlockX() + 1, getBlockY() - 1) && TheGame.Instance.map[getBlockX() + 1][getBlockY() - 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle());
        }

        private bool isRightCollision()
        {
            return isValidRectangle(getBlockX(), getBlockY() + 1) && TheGame.Instance.map[getBlockX()][getBlockY() + 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle())
                || isValidRectangle(getBlockX() - 1, getBlockY() + 1) && TheGame.Instance.map[getBlockX() - 1][getBlockY() + 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle())
                || isValidRectangle(getBlockX() + 1, getBlockY() + 1) && TheGame.Instance.map[getBlockX() + 1][getBlockY() + 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle());
        }

        private bool isTopCollision()
        {
            return isValidRectangle(getBlockX() - 1, getBlockY()) && TheGame.Instance.map[getBlockX() - 1][getBlockY()].getCollisionRectangle().Intersects(getPlayerCollisionRectangle())
                || isValidRectangle(getBlockX() - 1, getBlockY() - 1) && TheGame.Instance.map[getBlockX() - 1][getBlockY() - 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle())
                || isValidRectangle(getBlockX() - 1, getBlockY() + 1) && TheGame.Instance.map[getBlockX() - 1][getBlockY() + 1].getCollisionRectangle().Intersects(getPlayerCollisionRectangle());
        }


        private void updatePlayerPosition()
        {

            //Rectangle playerCenter = getCenterRectangle();
            Rectangle playerCenter = getWorldRectangle();
            

            int i = 0;
            int j = 0;
            foreach (List<GameObjectBlock> row in TheGame.Instance.map)
            {
                foreach (GameObjectBlock block in row)
                {
                    if (playerCenter.Intersects(block.getCollisionRectangle()))
                    {
                        currentPointRectangle = new Point(i, j);
                    }
                    j++;
                }
                i++;
                j = 0;
            }

            debugInfo(currentPointRectangle.X + " " + currentPointRectangle.Y);

            i = currentPointRectangle.X;
            j = currentPointRectangle.Y;
            //j = currentPointRectangle.X;
            //i = currentPointRectangle.Y;
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


#if DEBUG
        private static string lastDebugMessage = "";
        private static int lastMessageId = 0;
#endif


        private void debugInfo(string message)
        {
#if DEBUG
            if (lastDebugMessage != message)
            {
                Debug.WriteLine(lastMessageId + ": " + message);
                lastDebugMessage = message;
                ++lastMessageId;
            }
#endif
        }


        private bool isValidRectangle(int i, int j)
        {
            bool valid = i >= 0 && i < TheGame.Instance.map.getHeight()
                && j >= 0 && j < TheGame.Instance.map.getWidth();

            return valid; // valid false? isTraversable won't be called.

        }
                
#endregion

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
