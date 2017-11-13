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
    public class Ship
    {
        private Vector2 position;
        private Texture2D ship;
        private bool Isshoot = false;
        private KeyboardState keyState;
        private MouseState mouseState, lastMouseState;
        List<Bullet> BulletList = new List<Bullet>();
        ContentManager content;
        float spawn = 0;

        public Ship(ContentManager content)
        {

            ship = content.Load<Texture2D>("player");
            this.content = content;

        }
        //public Vector2 MousePosition() => new Vector2(mouseState.X, mouseState.Y);

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



        }

        public void turn()
        {
            if (position.X > 750) position.X -= 10;
            if (position.X < -10) position.X += 10;
            if (position.Y < -10) position.Y += 10;
            if (position.Y > 550) position.Y -= 10;
        }

        public void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            spriteBatch.Draw(ship, position);
            if (Isshoot)
                foreach (Bullet bullet in BulletList)
                    bullet.Draw(spriteBatch);
        }


    }
}
