using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public class Map : List<List<GameObjectBlock>>
    {
        private GameObjectType defaultType;
        private int _width, _height;

        Rectangle displayRect;
        Rectangle blockRect;
        public Map(Rectangle displayRect)
        {
            this.displayRect = displayRect;
        }

        public bool isOutsideMap(Rectangle rect)
        {
            if (rect.X < 0) return true;
            if (rect.X + rect.Width > displayRect.Width) return true;

            if (rect.Y < 0) return true;
            if (rect.Y + rect.Height > displayRect.Height) return true;

            return false;
        }

        public void setDefaultType(GameObjectType inDefaultType)
        {
            defaultType = inDefaultType;
        }

        public void resize(int width, int height)
        {
            //if(defaultType == null)
            //    throw new Exception("Initialize default type");

            if(getGameObjectBlock(new Point(0, 0), defaultType) == null)
                throw new Exception("Can't initialize your object. Make the factory work.");

            _width = width;
            _height = height;

            for(int i = 0; i < height; i++)
            {
                List<GameObjectBlock> row = new List<GameObjectBlock>();

                for(int j = 0; j < width; j++)
                {
                    GameObjectBlock block = getGameObjectBlock(new Point(i, j), defaultType);
                    row.Add(block);
                }

                this.Add(row);
            }

            blockRect = new Rectangle(0, 0, displayRect.Width / width, displayRect.Height / height);


        }

        public void set(int i, int j, GameObjectType type)
        {
            GameObjectBlock block = getGameObjectBlock(new Point(i,j), type);
            if(block != null)
                this[i][j] = block;
        }

        public GameObjectBlock getGameObjectBlock(Point pos, GameObjectType type)
        {
            Vector2 dimensions = new Vector2(blockRect.Width, blockRect.Height);

            switch(type)
            {
                case GameObjectType.Dirt:
                    return new Dirt(pos, dimensions);
                case GameObjectType.Free:
                    return new Free(pos, dimensions);
                case GameObjectType.Pea:
                    return new Pea(pos, dimensions);
                case GameObjectType.PlayerSpawner:
                    return new PlayerSpawner(pos, dimensions);
                case GameObjectType.Teleporter:
                    return new Teleporter(pos, dimensions);
            }

            return null;
        }



        public IEnumerable<GameObjectBlock> getAllGameObjects()
        {
            foreach (List<GameObjectBlock> row in this)
            {
                foreach (GameObjectBlock block in row)
                {
                    yield return block;
                }
            }
        }
        public Point getPlayerPos()
        {
            int i = 0;
            int j = 0;

            foreach (List<GameObjectBlock> row in this)
            {
                j = 0;
                foreach (GameObjectBlock block in row)
                {
                    if (block.Type == GameObjectType.PlayerSpawner)
                        return new Point(j, i);
                    j++;
                }
                i++;
            }

            return new Point(0, 0);
        }

        public bool isTraversable(int i, int j)
        {
            return this[i][j].Type != GameObjectType.Dirt;
        }

        public Rectangle getBlockRectangle()
        {
            return blockRect;
        }

        


        public int getWidth()
        {
            return _width;
        }

        public int getHeight()
        {
            return _height;
        }
    }
}
