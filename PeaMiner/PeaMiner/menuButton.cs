using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeaMiner
{
    public enum MenuButtonType
    {
        StartGame,
        Quit
    }
    public abstract class menuButton
    {
        public MenuButtonType menuButtonType;
        public Rectangle surface;
        public string textureLocation;
        public Texture2D texture;

        public menuButton(MenuButtonType type, Rectangle surface, string textureLocation)
        {
            this.menuButtonType = type;
            this.surface = surface;
            this.textureLocation = textureLocation;
        }

        public abstract void onClick(ref GameState gameState);
    }
}
