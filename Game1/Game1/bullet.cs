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
    class Bullet
    {
        private Vector2 position, speed;
        private Texture2D bullet;
        private MouseState mouseState, lastMouseState;
        SpriteBatch spriteBatch;
        ContentManager content;
        Rectangle rectangleBox;

        public Bullet(ContentManager content, Vector2 ShipPosition)
        {

            bullet = content.Load<Texture2D>("heal");
            mouseState = Mouse.GetState();
            position = ShipPosition;
            this.content = content;
            speed.X = (mouseState.X - ShipPosition.X) / 50;
            speed.Y = (mouseState.Y - ShipPosition.Y) / 50;
            if (Math.Abs(speed.X) < 2) speed.X *= 5;
            if (Math.Abs(speed.Y) < 2) speed.Y *= 5;
        }

        public void Move()
        {
            position.X += speed.X;
            position.Y += speed.Y;
            rectangleBox = new Rectangle((int)position.X, (int)position.Y, bullet.Width / 5, bullet.Height / 5);
        }
        public Rectangle rectanglebox()
        {

            return rectangleBox;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bullet, position);
        }

        public bool IsEnd()
        {
            if (position.X > 800 || position.X < -50 || position.Y < -50 || position.Y > 600) return true;
            return false;
        }

    }
}