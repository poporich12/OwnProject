using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1 
{
    class Enemies
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 direction;
       


        Random random = new Random();
        int randX, randY;

        public Enemies(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;

            randX = random.Next(-4, 4);
            randY = random.Next(-4, -1);
            velocity = new Vector2(randX, randY);
        }



        public void Update(GraphicsDevice graphics, Vector2 ShipPosition)
        {
            if (ShipPosition.X > position.X)
            {
                position.X++;
            }
            if (ShipPosition.X < position.X)
            {
                position.X--;
            }
            if (ShipPosition.Y > position.Y)
            {
                position.Y++;
            }
            if (ShipPosition.Y < position.Y)
            {
                position.Y--;
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}