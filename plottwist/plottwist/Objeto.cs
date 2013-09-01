using System;
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
        private string finalPopupText;
        public SpriteFont popupFont;
        public bool popupActivated;
        public bool finalPopup;
        public bool animationStarted;
        public float popupScale;
        public float finalPopupScale;
        private float depth;
        public Texture2D popupTexture;
        public int frameTime;
        int dt;
        int dt2;
        public int posicaoX;
        public Objeto(int posX, int posY, int mapa, int numFrames, string popupText, string finalPopupText,int spriteSheetHeight, int spriteSheetWidth, int frameTime, int posicaoX, float depth)
        {
            dt = 1000;
            position.X = posX;
            position.Y = posY;
            this.mapa = mapa;
            tocarAnimacao = false;
            this.numFramesAnimacao = numFrames;
            currentFrameAnimacao = 0;
            this.spriteSheetHeight = spriteSheetHeight;
            this.spriteSheetWidth = spriteSheetWidth;
            this.posicaoX = posicaoX;
            this.depth = depth;

            this.popupText = popupText;
            this.finalPopupText = finalPopupText;
            popupActivated = false;
            finalPopup = false;
            animationStarted = false;
            popupScale = 0;
            finalPopupScale = 0;
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
            if (Math.Abs(pos - this.posicaoX) <= 70)
                return true;
            return false;
        }
        public void DrawPopup(SpriteBatch spriteBatch, Rectangle screen)
        {
            spriteBatch.DrawString(popupFont, popupText, new Vector2(screen.Width / 2, screen.Height / 5), Color.White, 0, popupFont.MeasureString(popupText) / 2, popupScale * screen.Width / (popupTexture.Width * 4), SpriteEffects.None, 1);
            spriteBatch.Draw(popupTexture, new Vector2(screen.Width / 2, screen.Height / 5), null, Color.White, 0, new Vector2(popupTexture.Width / 2, popupTexture.Height / 2), popupScale * screen.Width / (popupTexture.Width * 4), SpriteEffects.None, 0.8f);
            if (popupActivated && popupScale < 1f)
                popupScale += 0.07f;
            if (!popupActivated && popupScale > 0f)
                popupScale -= 0.07f;
        }

        public void DrawFinalPopup(SpriteBatch spriteBatch, Rectangle screen,int elapsedGameTime)
        {
            dt2 += elapsedGameTime;
            spriteBatch.DrawString(popupFont, finalPopupText, new Vector2(screen.Width / 2, screen.Height / 5), Color.White, 0, popupFont.MeasureString(finalPopupText) / 2, finalPopupScale * screen.Width / (popupTexture.Width * 4), SpriteEffects.None, 1);
            spriteBatch.Draw(popupTexture, new Vector2(screen.Width / 2, screen.Height / 5), null, Color.White, 0, new Vector2(popupTexture.Width / 2, popupTexture.Height / 2), finalPopupScale * screen.Width / (popupTexture.Width * 4), SpriteEffects.None, 0.8f);
            if (finalPopupScale <= 0 && dt2 > 1000)
                finalPopup = false;
            if (finalPopup && finalPopupScale < 1f && dt2<1000)
                finalPopupScale += 0.07f;
            if (finalPopup && finalPopupScale > 0f && dt2>1500)
                finalPopupScale -= 0.07f;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle screen)
        {
            spriteBatch.Draw(spriteSheet, screen, new Rectangle((int)((spriteSheet.Width / spriteSheetWidth) * (currentFrameAnimacao % spriteSheetWidth)), (int)((spriteSheet.Height / spriteSheetHeight) * (currentFrameAnimacao / spriteSheetWidth)), (int)spriteSheet.Width / spriteSheetWidth, (int)spriteSheet.Height / spriteSheetHeight), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, depth);
        }
    }
}
