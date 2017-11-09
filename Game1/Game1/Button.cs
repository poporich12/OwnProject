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
    class Button
    {
        private bool _isClicked;
        public void Update(MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
                _isClicked = true;
            else
                _isClicked = false;

        }
        public void SetCliecked(bool isClicked)
        {
            _isClicked = isClicked;
        }
        public bool IsClicked()
        {
            return _isClicked;
        }

    }
}
