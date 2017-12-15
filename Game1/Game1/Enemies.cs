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
        public Rectangle enemyBox;
        public int lifenemy;
        public int level;
        

        Random random = new Random();
        int randX, randY;

        public Enemies(Texture2D newTexture, Vector2 newPosition,int level)
        {

            texture = newTexture;
            position = newPosition;

            randX = random.Next(-4, 4);
            randY = random.Next(-4, -1);
            velocity = new Vector2(randX, randY);
            this.level = level;

            if (level == 1) lifenemy = 1;
            else if (level == 2) lifenemy = 2;
            else if (level == 3) lifenemy = 3;
            else if (level == 4) lifenemy = 4;
            //enemyBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
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

            enemyBox = new Rectangle((int)position.X, (int)position.Y, texture.Width / 2, texture.Height / 2);
        } 
        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}