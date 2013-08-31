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
    class Player
    {
        public Vector2 position;
        public Texture2D texture;
        public int mapaAtual;
        public Player(int posX, int posY)
        {
            mapaAtual = 0;
            position.X = posX;
            position.Y = posY;
        }
    }
}
