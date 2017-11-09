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
        private bool shoot = false;
        private KeyboardState keyState;
        private MouseState mouseState, lastMouseState;

        public Ship(ContentManager content)
        {

            ship = content.Load<Texture2D>("player");

        }
        public Vector2 MousePosition() => new Vector2(mouseState.X, mouseState.Y);

        public void Move(ContentManager content)
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



            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                shoot = true;
            }
            if (shoot)
            {

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ship, position);
        }
    }
}
