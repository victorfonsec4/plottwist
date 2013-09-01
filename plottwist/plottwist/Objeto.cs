﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace plottwist
{
    class Objeto : Object
    {
        public Vector2 position;
        public int mapa;
        public bool tocarAnimacao;
        public Texture2D spriteSheet;
        public int spriteSheetHeight;
        public int spriteSheetWidth;
        public int numFramesAnimacao;
        public int currentFrameAnimacao;
        public SoundEffect som;
        private string popupText;
        public SpriteFont popupFont;
        public bool popupActivated;
        public float popupScale;
        public Texture2D popupTexture;
        public int frameTime;
        int dt;
        public Objeto(int posX, int posY, int mapa, int numFrames, string popupText, int spriteSheetHeight, int spriteSheetWidth, int frameTime)
        {
            dt = 0;
            position.X = posX;
            position.Y = posY;
            this.mapa = mapa;
            tocarAnimacao = false;
            this.numFramesAnimacao = numFrames;
            currentFrameAnimacao = 0;
            this.spriteSheetHeight = spriteSheetHeight;
            this.spriteSheetWidth = spriteSheetWidth;

            this.popupText = popupText;
            popupActivated = false;
            popupScale = 0;
            this.frameTime = frameTime;
        }
        public void Animation(int elapsedGameTime)
        {
            if (currentFrameAnimacao < numFramesAnimacao - 1)
            {
                dt += elapsedGameTime;
                if (dt >= frameTime)
                {
                    dt = 0;
                    this.currentFrameAnimacao++;
                }
            }
        }
        public bool VerificarPosicao(float pos, int width)
        {
            if (Math.Abs(pos - this.position.X) <= 40)
                return true;
            return false;
        }
        public void DrawPopup(SpriteBatch spriteBatch, Rectangle screen)
        {
            spriteBatch.Draw(popupTexture, new Vector2(screen.Width / 2, screen.Height / 2 - 80), null, Color.White, 0, new Vector2(popupTexture.Width / 2, popupTexture.Height / 2), popupScale * screen.Width / (popupTexture.Width * 4), SpriteEffects.None, 0);
            spriteBatch.DrawString(popupFont, popupText, new Vector2(screen.Width / 2, screen.Height / 2 - 80), Color.White, 0, popupFont.MeasureString(popupText) / 2, popupScale * screen.Width / (popupTexture.Width * 4), SpriteEffects.None, 0);
            if (popupActivated && popupScale < 1f)
                popupScale += 0.07f;
            if (!popupActivated && popupScale > 0f)
                popupScale -= 0.07f;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle screen)
        {
            spriteBatch.Draw(spriteSheet, new Rectangle((int)position.X, (int)position.Y, (int)(spriteSheet.Width / spriteSheetWidth), (int)(spriteSheet.Height / spriteSheetHeight) ), new Rectangle( (int) ((spriteSheet.Width/spriteSheetWidth)*(currentFrameAnimacao%spriteSheetWidth )), (int)( (spriteSheet.Height/spriteSheetHeight)*(currentFrameAnimacao/spriteSheetWidth+1) ), (int)spriteSheet.Width/spriteSheetWidth, (int)spriteSheet.Height/spriteSheetHeight), Color.White);
        }
    }
}
