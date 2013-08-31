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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Popup[] popups;
        Texture2D[] mapas;
        Player player;
        Objeto[] objetos;
        Rectangle screenRectangle;
        int numObjetos;
        int dt;
        int screenHeight, screenWidth;

        public Game1()
        {
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            popups = new Popup[3];
            popups[1] = new Popup("Teste");
            mapas = new Texture2D[3];
            player = new Player(0, graphics.GraphicsDevice.Viewport.Height * 3 / 4);
            numObjetos = 1;
            objetos = new Objeto[numObjetos];
            objetos[0] = new Objeto(graphics.GraphicsDevice.Viewport.Width/2, graphics.GraphicsDevice.Viewport.Height/2, 1, 3);
            dt = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mapas[0] = Content.Load<Texture2D>("mapa1");
            mapas[1] = Content.Load<Texture2D>("mapa2");
            mapas[2] = Content.Load<Texture2D>("mapa3");
            player.texture = Content.Load<Texture2D>("cara");
            objetos[0].texture = Content.Load<Texture2D>("objeto1");
            objetos[0].framesAnimacao[0] = Content.Load<Texture2D>("frame1");
            objetos[0].framesAnimacao[1] = Content.Load<Texture2D>("frame2");
            objetos[0].framesAnimacao[2] = Content.Load<Texture2D>("frame3");
            objetos[0].som = Content.Load<SoundEffect>("microwavefinal");
            popups[1].font = Content.Load<SpriteFont>("FontePopups");
            popups[1].texture = Content.Load<Texture2D>("Popup");

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            dt += gameTime.ElapsedGameTime.Milliseconds;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && player.position.X >= 0)
                player.position.X -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && player.position.X + player.texture.Width <= graphics.GraphicsDevice.Viewport.Width)
                player.position.X += 10;
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                foreach (Objeto o in objetos)
                    if (o.VerificarPosicao(player.position.X, screenWidth) && !o.tocarAnimacao)
                    {
                        o.tocarAnimacao = true;
                        o.som.Play(0.5f,0.0f,0.0f);
                    }
                if (popups[1].scale >= 1f)
                {
                    popups[1].ended = true;
                }
            }
            foreach (Objeto o in objetos)
            {
                if (o.currentFrameAnimacao == o.numFramesAnimacao - 1 && o.mapa==player.mapaAtual)
                {
                    popups[player.mapaAtual].activated=true;
                }
            }
            if (player.position.X <= 0 && player.mapaAtual > 0)
            {
                player.mapaAtual--;
                player.position.X = graphics.GraphicsDevice.Viewport.Width - player.texture.Width - 30;
            }
            if (player.position.X + player.texture.Width >= graphics.GraphicsDevice.Viewport.Width && player.mapaAtual < 2)
            {
                player.mapaAtual++;
                player.position.X = 0 + 30;
            }
            if (dt >= 500)
            {
                dt = 0;
                foreach (Objeto o in objetos)
                    if(o.tocarAnimacao)
                        o.Animation();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(mapas[player.mapaAtual], screenRectangle, Color.White);
            spriteBatch.Draw(player.texture, player.position, Color.White);


            for (int i = 0; i < numObjetos; i++)
            {
                if (player.mapaAtual == objetos[i].mapa && !objetos[i].tocarAnimacao)
                    spriteBatch.Draw(objetos[i].texture, objetos[i].position, Color.White);
                if (player.mapaAtual == objetos[i].mapa && objetos[i].tocarAnimacao)
                    spriteBatch.Draw(objetos[i].framesAnimacao[objetos[i].currentFrameAnimacao], objetos[i].position, Color.White);
            }

            if (popups[1].activated && !popups[1].ended)
                 popups[1].Draw(spriteBatch,screenRectangle);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
