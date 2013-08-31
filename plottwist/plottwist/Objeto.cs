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
    class Objeto
    {
        public Vector2 position;
        public int mapa;
        public Texture2D texture;
        public bool tocarAnimacao;
        public Texture2D[] framesAnimacao;
        public int numFramesAnimacao;
        public int currentFrameAnimacao;
        public SoundEffect som;
        public Objeto(int posX, int posY, int mapa, int numFrames)
        {
            position.X = posX;
            position.Y = posY;
            this.mapa = mapa;
            tocarAnimacao = false;
            this.numFramesAnimacao = numFrames;
            framesAnimacao = new Texture2D[numFrames];
            currentFrameAnimacao = 0;
        }
        public void Animation()
        {
            if(this.currentFrameAnimacao<numFramesAnimacao-1)
                this.currentFrameAnimacao++;
        }
        public bool VerificarPosicao(float pos, int width)
        {
            if(Math.Abs(pos-this.position.X)<=40)
                return true;
            return false;
        }
    }
}
