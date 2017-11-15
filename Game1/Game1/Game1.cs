using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;






namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class NeonShooterGame : Game
    {
        public static NeonShooterGame Instance { get; private set; }
        public static GameTime GameTime { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        Random random = new Random();
        List<Enemies> enemies = new List<Enemies>();
        Ship ship;
        float spawn = 0;

        bool paused = false;
        bool useBloom = false;


        enum GameState
        {
            ShDoomed,
            MainMenu,
            Option,
            Playing,
            GameOver
        }
        GameState CurrentGameState = GameState.ShDoomed;


        public NeonShooterGame()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            //   bloom = new BloomComponent(this);
            //   Components.Add(bloom);
            //  bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);

        }

        Button bgWellcome;
        cButton btnPlay, btnMenu;

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

            graphics.ApplyChanges();
            graphics.IsFullScreen = true;
            IsMouseVisible = true;

            bgWellcome = new Button();
            btnPlay = new cButton(Content.Load<Texture2D>("Play2"), graphics.GraphicsDevice);
            btnMenu = new cButton(Content.Load<Texture2D>("Menu"), graphics.GraphicsDevice);

            btnMenu.setPosition(new Vector2(350, 140));
            btnPlay.setPosition(new Vector2(350, 240));

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
            /*foreach (Enemies enemy in enemies)
            {
                enemy.Update(graphics.GraphicsDevice, ship.realPosition());
                if (ship.rectanglebox().Intersects(enemy.enemyBox))
                {
                    CurrentGameState = GameState.GameOver;
                }
            }*/

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
                    ship.Move(gameTime);
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        Enemies enemy = enemies[i];
                        if (ship.KillEnemy(enemy))
                        {
                            enemies.Remove(enemy);
                            i--;
                        }
                    }

                    foreach (Enemies enemy in enemies)
                    {
                        enemy.Update(graphics.GraphicsDevice, ship.realPosition());
                        if (ship.rectanglebox().Intersects(enemy.enemyBox))
                        {
                            CurrentGameState = GameState.GameOver;
                        }
                    }
                    LoadEnemies();
                    if (Input.WasKeyPressed(Keys.P))
                        paused = !paused;
                    if (Input.WasKeyPressed(Keys.B))
                        useBloom = !useBloom;
                    break;
                case GameState.GameOver:
                    break;

            }
            base.Update(gameTime);
        }

        public void LoadEnemies()
        {

            int randY1 = random.Next(10, 400);
            int randX1 = random.Next(10, 700);
            int randY2 = random.Next(10, 400);
            int randX2 = random.Next(10, 700);
            int randY3 = random.Next(10, 400);
            int randX3 = random.Next(10, 700);
            int randY4 = random.Next(10, 400);
            int randX4 = random.Next(10, 700);
            if (spawn > 1)
            {
                spawn = 0;
                if (enemies.Count() < 200)
                {

                    enemies.Add(new Enemies(Content.Load<Texture2D>("1"), new Vector2(randX1, randY1)));
                }
                if (enemies.Count() > 8)
                {

                    enemies.Add(new Enemies(Content.Load<Texture2D>("2"), new Vector2(randX2, randY2)));
                }
                if (enemies.Count() > 20)
                {

                    enemies.Add(new Enemies(Content.Load<Texture2D>("3"), new Vector2(randX3, randY3)));
                }
                if (enemies.Count() > 30)
                {

                    enemies.Add(new Enemies(Content.Load<Texture2D>("4"), new Vector2(randX4, randY4)));
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
                    spriteBatch.Draw(Content.Load<Texture2D>("iconeshoot"), new Rectangle(0, 0, 800, 600), Color.White);
                    break;
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("Background"), new Rectangle(0, 0, 800, 600), Color.White);
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
                    spriteBatch.DrawString(Content.Load<SpriteFont>("Font/Font"), "Lives: {PlayerStatus.Lives}", new Vector2(5), Color.White);
                    DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
                    //       DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35);
                    // draw the custom mouse cursor
                    //    spriteBatch.Draw(Content.Load<Texture2D>("Art/Pointer"), Input.MousePosition, Color.White);

                    if (PlayerStatus.IsGameOver)
                    {
                        string text = "Game Over\n" +
                            "Your Score: " + PlayerStatus.Score + "\n" +
                            "High Score: " + PlayerStatus.HighScore;

                        Vector2 textSize = Art.Font.MeasureString(text);
                        spriteBatch.DrawString(Art.Font, text, ScreenSize / 2 - textSize / 2, Color.White);
                    }
                    break;
                case GameState.GameOver:
                    spriteBatch.Draw(Content.Load<Texture2D>("Background"), new Rectangle(0, 0, 800, 600), Color.White);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = Content.Load<SpriteFont>("Font/Font").MeasureString(text).X;
            spriteBatch.DrawString(Content.Load<SpriteFont>("Font/Font"), text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }

    }


}
