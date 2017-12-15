using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input.Touch;


namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static Game1 Instance { get; private set; }
        public static GameTime GameTime { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        Random random = new Random();
        List<Enemies> enemies = new List<Enemies>();
        Ship ship;
        List<Boss> boss = new List<Boss>();
        float spawn = 0;
        int scoboss = 500;

        bool paused = false;
        bool useBloom = false;
        Song song;
        SoundEffect playerDead;

        enum GameState
        {
            ShDoomed,
            MainMenu,
            Option,
            Playing,
            GameOver
        }
        GameState CurrentGameState = GameState.ShDoomed;


        public Game1()
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
            song = Content.Load<Song>("Music/FormatFactoryBGmusic");
            MediaPlayer.Play(song);
            playerDead = Content.Load<SoundEffect>("Music/Explosion+3");
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
                            enemy.lifenemy--;
                            if (enemy.lifenemy == 0)
                            {
                                enemies.Remove(enemy);
                                if (enemy.level == 1)
                                    PlayerStatus.AddPoints(5);
                                else if (enemy.level == 2)
                                    PlayerStatus.AddPoints(20);
                                else if (enemy.level == 3)
                                    PlayerStatus.AddPoints(30);
                                else if (enemy.level == 4)
                                    PlayerStatus.AddPoints(50);
                            }
                            i--;
                        }
                    }
                    for (int i = 0; i < boss.Count; i++)
                    {
                        Boss bos = boss[i];
                        if (ship.KillBoss(bos))
                        {
                            bos.lifeboss--;
                            if (bos.lifeboss == 0)
                            {
                                boss.Remove(bos);
                                PlayerStatus.AddPoints(1000);
                            }
                            i--;
                        }
                    }
                    foreach (Boss bos in boss)
                    {
                        bos.Update(graphics.GraphicsDevice, ship.realPosition());
                    }




                    foreach (Enemies enemy in enemies)
                    {
                        enemy.Update(graphics.GraphicsDevice, ship.realPosition());
                        if (ship.rectanglebox().Intersects(enemy.enemyBox))
                        {
                            playerDead.Play();
                            PlayerStatus.RemoveLife();
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                 
                                    Enemies enemy1 = enemies[i];
                            if (ship.rectanglebox().Intersects(enemy1.enemyBox))
                                    enemies.Remove(enemy1);
                                
                            }
                            if (PlayerStatus.IsGameOver)
                                 CurrentGameState = GameState.GameOver;                                                          
                            break;
                        }

                    }

                   foreach (Boss bos in boss)
                    {
                        bos.Update(graphics.GraphicsDevice, ship.realPosition());
                        if (ship.rectanglebox().Intersects(bos.bossBox))
                        {
                            playerDead.Play();
                            PlayerStatus.RemoveLife();
                            for (int i = 0; i < boss.Count; i++)
                            {
                                Boss bos1 = boss[i];
                                boss.Remove(bos1);
                            }

                            if (PlayerStatus.IsGameOver)
                                
                                  CurrentGameState = GameState.GameOver;
                                    
                            int sec = 3;
                            while (sec > 0)
                            {
                                sec--;
                            }
                            break;
                            

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
            int randX5 = random.Next(10, 400);
            int randY5 = random.Next(10, 700);
            if (spawn > 1)
            {
                spawn = 0;
                if (PlayerStatus.Score >= 0 )
                {
                    if(ship.Position.X != randX1 && ship.Position.Y != randY1) { 
                        enemies.Add(new Enemies(Content.Load<Texture2D>("1"), new Vector2(randX1, randY1),1));
                    }
                    else
                    {
                        randX1 = randX1 + 200;
                        randY1 = randY1 + 200;
                    }
                }
                if (PlayerStatus.Score > 50 )
                {
                    if(ship.Position.X != randX2 && ship.Position.Y != randY2) { 
                        enemies.Add(new Enemies(Content.Load<Texture2D>("2"), new Vector2(randX2, randY2),2));
                    }
                    else
                    {
                        randX2 = randX2 + 200;
                        randY2 = randY2 + 200;
                    }
                }
                if (PlayerStatus.Score > 100)
                {
                    if(ship.Position.X != randY3 && ship.Position.Y != randY3) { 
                        enemies.Add(new Enemies(Content.Load<Texture2D>("3"), new Vector2(randX3, randY3),3));
                    }
                    else
                    {
                        randX3 = randX3 + 200;
                        randY3 = randY3 + 200;
                    }
                }
                if (PlayerStatus.Score > 200)
                {
                    if(ship.Position.X != randX4 && ship.Position.Y != randY4) { 
                        enemies.Add(new Enemies(Content.Load<Texture2D>("4"), new Vector2(randX4, randY4),4));
                    }
                    else
                    {
                        randX4 = randX4 + 200;
                        randY4 = randY4 + 200;
                    }
                }
                if (PlayerStatus.Score > scoboss)
                {
                    scoboss = scoboss * 2;
                    if (ship.Position.X != randX5 && ship.Position.Y != randY5) { 
                        boss.Add(new Boss(Content.Load<Texture2D>("boss"), new Vector2(randX4, randY4)));
                    }
                    else
                    {
                        randX5 = randX5 + 200;
                        randY5 = randY5 + 200;
                    }
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
                    foreach (Boss bos in boss)
                    {

                        bos.Draw(spriteBatch);
                    }
                    if (PlayerStatus.Lives == 4)
                        spriteBatch.DrawString(Content.Load<SpriteFont>("Font/Font"), "o", new Vector2(60, 0), Color.White);
                    if (PlayerStatus.Lives >= 3)
                        spriteBatch.DrawString(Content.Load<SpriteFont>("Font/Font"), "o", new Vector2(40, 0), Color.White);
                    if (PlayerStatus.Lives >= 2)
                        spriteBatch.DrawString(Content.Load<SpriteFont>("Font/Font"), "o", new Vector2(20, 0), Color.White);
                    if (PlayerStatus.Lives >= 1)
                        spriteBatch.DrawString(Content.Load<SpriteFont>("Font/Font"), "o", new Vector2(0, 0), Color.White);

                    DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
                    //       DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35);
                    // draw the custom mouse cursor
                    //    spriteBatch.Draw(Content.Load<Texture2D>("Art/Pointer"), Input.MousePosition, Color.White);

                    break;
                case GameState.GameOver:
                    spriteBatch.Draw(Content.Load<Texture2D>("Background"), new Rectangle(0, 0, 800, 600), Color.White);
                   var textWidth = Content.Load<SpriteFont>("Font/Font").MeasureString("Your score: " + PlayerStatus.Score).X;
                    spriteBatch.DrawString(Content.Load<SpriteFont>("Font/Font"), "Your score: " + PlayerStatus.Score, new Vector2(300, 300), Color.White);
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
