using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace plottwist
{
    class Popup
    {
        private string text;
        public SpriteFont font;
        public bool activated;
        public bool ended;
        public float scale;
        public Texture2D texture;
        public Popup(string text)
        {
            this.text = text;
            this.activated = false;
            this.ended = false;

            scale=0;
        }
        public void Draw(SpriteBatch spriteBatch,Rectangle screen)
        {
            spriteBatch.Draw(texture,new Vector2(screen.Width/2,screen.Height/2),null,Color.White,0,new Vector2(texture.Width/2,texture.Height/2),scale*texture.Width*4/screen.Width,SpriteEffects.None,0);
            spriteBatch.DrawString(font,text,new Vector2(screen.Width/2,screen.Height/2),Color.White,0,new Vector2(texture.Width/2,texture.Height/2),scale,SpriteEffects.None,0);
            if (scale < 1f)
                scale += 0.01f;
        }
    }
}
