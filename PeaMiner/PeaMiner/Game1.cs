#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;
#endregion

namespace PeaMiner
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        GraphicsContentLoader graphicsContentLoader;
        SpriteBatch spriteBatch;

        SpriteFont messageFont;

        //Map map;
        Player player;
        Bus bus;

        Rectangle displayRect;



        Menu menu;
        Cursor cursor;

        public Game1()
            : base()
        {
            int width = 1280;
            int height = 720;

            MapRepositioner.displayRect = displayRect = new Rectangle(0, 0, width, height);

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            menu = new Menu(new Vector2(width, height));
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            TheGame.Instance.map = new Map(displayRect);

            MapImporter.Import("Maps/map1/map.txt", TheGame.Instance.map, out player, out bus);

            TheGame.Instance.gameState = GameState.MainMenu;

            cursor = new Cursor();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            messageFont = Content.Load<SpriteFont>("PeasSpriteFont");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsContentLoader = new GraphicsContentLoader(Content);
            cursor.LoadContent(graphicsContentLoader);

            // TODO: use this.Content to load your game content here

            initParticleSystem();
            foreach(GameObjectBlock gameObjectBlock in TheGame.Instance.map.getAllGameObjects())
            {
                gameObjectBlock.LoadContent(graphicsContentLoader);
            }

            player.LoadContent(graphicsContentLoader);
            if (bus != null)
            {
                bus.LoadContent(graphicsContentLoader);
            }

            if (menu.isInitialized == false)
                menu.LoadContent(graphicsContentLoader);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardStateCustom keyboardState = KeyboardStateCustom.getKeyboardState(Keyboard.GetState());

            if (TheGame.Instance.isGameRunning == false)
                this.Exit();

            cursor.pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            cursor.Update(gameTime);


            if (TheGame.Instance.gameState == GameState.InGame)
            {

                updateParticleSystem(gameTime);
                TheGame.Instance.Update(gameTime);
                player.Update(gameTime);
                player.UpdateControl(keyboardState);

                foreach (GameObjectBlock gameObjectBlock in TheGame.Instance.map.getAllGameObjects())
                {
                    if (!gameObjectBlock.isInitialized)
                        gameObjectBlock.LoadContent(graphicsContentLoader);
                    else
                        gameObjectBlock.Update(gameTime);
                }

                foreach (GameObject gameObject in TheGame.Instance.gameObjectList)
                {
                    if (!gameObject.isInitialized)
                        gameObject.LoadContent(graphicsContentLoader);
                    else
                        gameObject.Update(gameTime);
                }

                for (int i = TheGame.Instance.map.Count - 1; i >= 0; i--)
                {
                    for (int j = TheGame.Instance.map[i].Count - 1; j >= 0; j--)
                    {
                        GameObjectBlock gameObjectBlock = TheGame.Instance.map[i][j];
                        if (gameObjectBlock.getCollisionRectangle().Intersects(player.getWorldRectangle()) && gameObjectBlock.Type == GameObjectType.Pea)
                        {
                            TheGame.Instance.map.set(gameObjectBlock.pointPos.X, gameObjectBlock.pointPos.Y, GameObjectType.Free);

                            player.Peas += 3;
                        }
                        if (bus != null)
                        {
                            if (gameObjectBlock.getCollisionRectangle().Intersects(bus.getWorldRectangle()) && gameObjectBlock.Type == GameObjectType.Dirt)
                            {
                                TheGame.Instance.map.set(gameObjectBlock.pointPos.X, gameObjectBlock.pointPos.Y, GameObjectType.Free);
                            }
                        }
                        for (int w = TheGame.Instance.gameObjectList.Count - 1; w >= 0; w--)
                        {
                            GameObject gameObject = TheGame.Instance.gameObjectList[w];
                            if (gameObjectBlock.getCollisionRectangle().Intersects(gameObject.getWorldRectangle()) && gameObjectBlock.Type == GameObjectType.Dirt && gameObject.Type == GameObjectType.Bullet)
                            {
                                TheGame.Instance.map.set(gameObjectBlock.pointPos.X, gameObjectBlock.pointPos.Y, GameObjectType.Free);
                                TheGame.Instance.gameObjectList.RemoveAt(w);
                            }



                        }


                        if (gameObjectBlock.getCollisionRectangle().Intersects(player.getWorldRectangle()) && gameObjectBlock.Type == GameObjectType.Teleporter)
                            MapImporter.Import("Maps/map2/map.txt", TheGame.Instance.map, out player, out bus);
                    }
                }

                if (!player.isInitialized)
                    player.LoadContent(graphicsContentLoader);

                for (int w = TheGame.Instance.gameObjectList.Count - 1; w >= 0; w--)
                {
                    GameObject gameObject = TheGame.Instance.gameObjectList[w];
                    if (gameObject.isActive == false)
                    {
                        TheGame.Instance.gameObjectList.RemoveAt(w);
                    }
                }
                if (bus != null)
                {
                    if (!bus.isInitialized)
                        bus.LoadContent(graphicsContentLoader);
                    bus.Update(gameTime);
                }
            }
            else if (TheGame.Instance.gameState == GameState.MainMenu)
            {
                menu.Update(gameTime);
                menu.UpdateControl(Mouse.GetState());
            }


            if (keyboardState.IsKeyDown(KeysMe.PauseGame) && TheGame.Instance.gameState == GameState.InGame)
            {
                TheGame.Instance.gameState = GameState.Paused;
            }
            else if (!keyboardState.IsKeyDown(KeysMe.PauseGame) && TheGame.Instance.gameState == GameState.Paused)
            {
                TheGame.Instance.gameState = GameState.InGame;
            }

            base.Update(gameTime);
        }
        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (TheGame.Instance.gameState == GameState.InGame || TheGame.Instance.gameState == GameState.Paused)
                {
                    foreach (GameObjectBlock gameObjectBlock in TheGame.Instance.map.getAllGameObjects())
                    {
                        gameObjectBlock.Draw(spriteBatch);
                    }
                    foreach (GameObject gameObject in TheGame.Instance.gameObjectList)
                    {
                        gameObject.Draw(spriteBatch);
                    }

                    spriteBatch.DrawString(messageFont, player.Peas.ToString(), new Vector2(10, 10), Color.Red);
                    player.Draw(spriteBatch);
                    if (bus != null)
                    {
                        bus.Draw(spriteBatch);
                    }
                    drawParticleSystem(spriteBatch);
                }
                if (TheGame.Instance.gameState == GameState.Paused)
                {
                    spriteBatch.DrawString(messageFont, "Paused!", new Vector2(50, 50), Color.Blue);
                }
                if (TheGame.Instance.gameState == GameState.MainMenu)
                {
                    menu.Draw(spriteBatch);
                }
                cursor.Draw(spriteBatch);
            spriteBatch.End();
        }
        #region particles - DOESNT WORK

            
        int Ex = 0;
        private Texture2D Pointer;
        private Vector2 ML;
        ParticleEngine particleEngine;

        private void updateParticleSystem(GameTime gameTime)
        {
            return;
            if(player.playerMoved == false)
            {
                particleEngine.Update();

                Vector2 Vec = new Vector2(player.getPos().X + player.getWorldRectangle().Width / 2, player.getPos().Y + player.getWorldRectangle().Height / 2);
                ML = Vec;
                particleEngine.EmitterLocation = Vec;

                Ex++;
            }
        }
        private void initParticleSystem()
        {
            return;
            List<Texture2D> textures = new List<Texture2D>();
            
            textures.Add(graphicsContentLoader.Get("Particles/FireRed"));
            textures.Add(graphicsContentLoader.Get("Particles/FireYellow"));
            textures.Add(graphicsContentLoader.Get("Particles/Smoke"));
            textures.Add(graphicsContentLoader.Get("Particles/FirePurple"));
            particleEngine = new ParticleEngine(textures, new Vector2(400, 240));
        }
        private void drawParticleSystem(SpriteBatch spriteBatch)
        {
            return;
            particleEngine.Draw(spriteBatch);
        }
        #endregion

    }
}
