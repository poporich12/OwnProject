using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace Game1
{
    class Ship : Entity
    {
        private Vector2 position;
        private Texture2D ship;
        private bool Isshoot = false;
        private bool Istouch = false;
        private KeyboardState keyState;
        private MouseState mouseState, lastMouseState;
        List<Bullet> BulletList = new List<Bullet>();
        ContentManager content;
        Rectangle rectangleBox;
        PlayerStatus playerStatus=new PlayerStatus();
        
        float spawn = 0;

        private Ship()
        {
            

            image = Art.Player;
            // Position = NeonShooterGame.ScreenSize/ 2;
            Radius = 10;
        }


        public Ship(ContentManager content)
        {

            ship = content.Load<Texture2D>("player");
            this.content = content;
         //   playerStatus = new PlayerStatus();
        }
        //public Vector2 MousePosition() => new Vector2(mouseState.X, mouseState.Y);

        private static Ship instance;
        public static Ship Instance
        {
            get
            {
                if (instance == null)
                    instance = new Ship();

                return instance;
            }
        }

        public bool touch() { return Istouch = true; }

        public void Move(GameTime gametime)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.A) && keyState.IsKeyDown(Keys.W))
            {
                position.X -= 5;
                position.Y -= 5;
                ship = content.Load<Texture2D>("player09");

            }
            else if (keyState.IsKeyDown(Keys.A) && keyState.IsKeyDown(Keys.S))
            {
                position.X -= 5;
                position.Y += 5;
                ship = content.Load<Texture2D>("player07");

            }
            else if (keyState.IsKeyDown(Keys.D) && keyState.IsKeyDown(Keys.S))
            {
                position.X += 5;
                position.Y += 5;
                ship = content.Load<Texture2D>("player06");

            }
            else if (keyState.IsKeyDown(Keys.D) && keyState.IsKeyDown(Keys.W))
            {
                position.X += 5;
                position.Y -= 5;
                ship = content.Load<Texture2D>("player05");

            }
            else if (keyState.IsKeyDown(Keys.A))
            {
                position.X -= 10;
                ship = content.Load<Texture2D>("player04");

            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                position.X += 10;
                ship = content.Load<Texture2D>("player02");

            }
            else if (keyState.IsKeyDown(Keys.W))
            {
                position.Y -= 10;
                ship = content.Load<Texture2D>("player");
            }
            else if (keyState.IsKeyDown(Keys.S))
            {
                position.Y += 10;
                ship = content.Load<Texture2D>("player03");
            }

            turn();
            spawn += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Isshoot = true;
                if (spawn > 0.15)
                {
                    spawn = 0;
                    BulletList.Add(new Bullet(content, position));

                }
            }

            if (Isshoot)
                foreach (Bullet bullet in BulletList)
                    bullet.Move();
            for (int i = 0; i < BulletList.Count; i++)
            {
                Bullet bullet = BulletList[i];
                if (bullet.IsEnd())
                {
                    BulletList.Remove(bullet);
                    i--;
                }
            }
            rectangleBox = new Rectangle((int)position.X, (int)position.Y, ship.Width / 5, ship.Height / 5);



        }

        public void turn()
        {
            if (position.X > 750) position.X -= 10;
            if (position.X < -10) position.X += 10;
            if (position.Y < -10) position.Y += 10;
            if (position.Y > 550) position.Y -= 10;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ship, position);
            if (Isshoot)
                foreach (Bullet bullet in BulletList)
                    bullet.Draw(spriteBatch);
        }

        public bool KillEnemy(Enemies enemy)
        {            
            for (int i = 0; i < BulletList.Count; i++)
            {
                Bullet bullet = BulletList[i];
                if (bullet.rectanglebox().Intersects(enemy.enemyBox))
                {               
                    BulletList.Remove(bullet);
                    i--;
                    return true;
                }

            }
            return false;
        }
        public bool KillBoss(Boss boss)
        {
            for (int i = 0; i < BulletList.Count; i++)
            {
                Bullet bullet = BulletList[i];
                if (bullet.rectanglebox().Intersects(boss.bossBox))
                {
                    BulletList.Remove(bullet);
                    i--;
                    return true;
                }

            }
            return false;
        }
        public Vector2 realPosition()
        {
            return position;
        }

        public override void Update()
        {

        }

        public Rectangle rectanglebox()
        {

            return rectangleBox;
        }

        int framesUntilRespawn = 0;
        internal object sprite;

        public bool IsDead { get { return framesUntilRespawn > 0; } }
    }
}