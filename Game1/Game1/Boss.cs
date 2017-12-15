using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    class Boss
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;
        public Rectangle bossBox;
        public int lifeboss;


        Random random = new Random();
        int randX, randY;

        public Boss(Texture2D newTexture, Vector2 newPosition)
        {

            texture = newTexture;
            position = newPosition;

            randX = random.Next(-4, 4);
            randY = random.Next(-4, -1);
            velocity = new Vector2(randX, randY);
            lifeboss = 40;
            //enemyBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update(GraphicsDevice graphics, Vector2 ShipPosition)
        {

            if (ShipPosition.X > position.X)
            {
                position.X += (1);
            }
            if (ShipPosition.X < position.X)
            {
                position.X -= (1);
            }
            if (ShipPosition.Y > position.Y)
            {
                position.Y += (1);
            }
            if (ShipPosition.Y < position.Y)
            {
                position.Y -= (1);
            }

            bossBox = new Rectangle((int)position.X, (int)position.Y, texture.Width / 2, texture.Height / 2);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
