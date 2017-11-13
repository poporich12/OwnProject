using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class cButton : Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        private Vector2 size;

        Color colour = new Color(255, 255, 255, 255);
        public cButton(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;
            //ScreenW = 800,ScreenH =600
            // Imgw     =100,Imgh =2
           size = new Vector2(graphics.Viewport.Width/8, graphics.Viewport.Height/8);
        }
        bool down;
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y,
                (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if (colour.A == 250) down = false;
                if (colour.A == 0) down = true;
                if (down) colour.A += 3; else colour.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed) SetCliecked(true);

            }
            else if (colour.A < 255)
            {
                colour.A += 3;
                SetCliecked(false);
            }
        }

        public void Draw(SpriteBatch spriteBactch)
        {
            spriteBactch.Draw(texture,rectangle,Color.Black);
        }

        public void setPosition(Vector2 newPosotion)
        {
            position = newPosotion;

        }

    }
}
