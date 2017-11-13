using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        Random random = new Random();
        List<Enemies> enemies = new List<Enemies>();
        Ship ship;
        float spawn = 0;
        enum GameState
        {
            ShDoomed,
            MainMenu,
            Option,
            Playing,
        }
        GameState CurrentGameState = GameState.ShDoomed;
        int screenWidth = 800, screenHight=600;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        Button bgWellcome;
        cButton btnPlay,btnMenu;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.ApplyChanges();
            IsMouseVisible = true;
            ship = new Ship(Content);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
           
            spriteBatch = new SpriteBatch(GraphicsDevice);
                 background = Content.Load<Texture2D>("background");
                 graphics.PreferredBackBufferWidth = screenWidth;
                 graphics.PreferredBackBufferHeight = screenHight;
                 graphics.ApplyChanges();
                 graphics.IsFullScreen = true;
                 IsMouseVisible = true;

                 bgWellcome = new Button();
                 btnPlay = new cButton(Content.Load<Texture2D>("Play2"), graphics.GraphicsDevice);
                 btnMenu = new cButton(Content.Load<Texture2D>("Menu"), graphics.GraphicsDevice);

                 btnMenu.setPosition(new Vector2(350,140));
                 btnPlay.setPosition(new Vector2(350,240));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
          
         
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
                switch (CurrentGameState)
                {
                    case GameState.ShDoomed:
                        if (bgWellcome.IsClicked() == true) CurrentGameState = GameState.MainMenu;
                        bgWellcome.Update(mouse);
                        break;
                    case GameState.MainMenu:
                        if (btnPlay.IsClicked() == true) CurrentGameState = GameState.Playing;
                        btnMenu.Update(mouse);
                        btnPlay.Update(mouse);
                        
                        break;
                    case GameState.Playing:
                        spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        foreach (Enemies enemy in enemies)
                        {
                            enemy.Update(graphics.GraphicsDevice);
                        }
                        LoadEnemies();
                        ship.Move();
                        break;
                }
            base.Update(gameTime);
        }

           public void LoadEnemies()
            {
                int randY1 = random.Next(1, 1000);
                int randX1 = random.Next(1, 1000);
                int randY2 = random.Next(1, 1000);
                int randX2 = random.Next(1, 1000);
                int randY3 = random.Next(1, 1000);
                int randX3 = random.Next(1, 1000);
                int randY4 = random.Next(1, 1000);
                int randX4 = random.Next(1, 1000);
                if (spawn > 1)
                {
                    spawn = 0;
                    if (enemies.Count() < 10)
                        enemies.Add(new Enemies(Content.Load<Texture2D>("1"), new Vector2(randX1, randY1)));
                    enemies.Add(new Enemies(Content.Load<Texture2D>("2"), new Vector2(randX2, randY2)));
                    enemies.Add(new Enemies(Content.Load<Texture2D>("3"), new Vector2(randX3, randY3)));
                    enemies.Add(new Enemies(Content.Load<Texture2D>("4"), new Vector2(randX4, randY4)));
                }
                for (int i = 0; i < enemies.Count; i++)
                 {
                     if (!enemies[i].isVisible)
                     {
                         enemies.RemoveAt(i);
                         i--;
                     }

            }
    }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
               spriteBatch.Draw(background, Vector2.Zero, Color.White);
                     switch (CurrentGameState)
                           {
                               case GameState.ShDoomed:
                                   spriteBatch.Draw(Content.Load<Texture2D>("iconeshoot"), new Rectangle(0, 0, screenWidth, screenHight), Color.White);
                                   break;
                               case GameState.MainMenu:
                                   spriteBatch.Draw(Content.Load<Texture2D>("Background"), new Rectangle(0, 0, screenWidth, screenHight), Color.White);
                                       btnMenu.Draw(spriteBatch);
                                       btnPlay.Draw(spriteBatch);  
                                   break;
                               case GameState.Playing:
                                   spriteBatch.Draw(background, Vector2.Zero, Color.White);                                   
                                   ship.Draw(spriteBatch);
                                           foreach (Enemies enemy in enemies)
                                           {
                                               enemy.Draw(spriteBatch);
                                           }
                                   break;
                           } 
           
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
