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
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            mapas = new Texture2D[3];
            numObjetos = 1;
            player = new Player(60, (int)(screenHeight * 3.25 / 4));
            objetos = new Objeto[numObjetos];
            //depth: (0->1) 0 = back , 1 = front
            objetos[0] = new Objeto(0, 0, 0, 14, "Heat Milk", 5, 3, 1000, (int)(0.73 * graphics.GraphicsDevice.Viewport.Width), 1);
            dt = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mapas[0] = Content.Load<Texture2D>("QuartoCozinha");
            mapas[1] = Content.Load<Texture2D>("mapa2");
            mapas[2] = Content.Load<Texture2D>("mapa3");
            player.texture = Content.Load<Texture2D>("cara");
            objetos[0].spriteSheet = Content.Load<Texture2D>("microondas");
            objetos[0].som = Content.Load<SoundEffect>("microwavefinal");
            objetos[0].popupFont = Content.Load<SpriteFont>("FontePopups");
            objetos[0].popupTexture = Content.Load<Texture2D>("Popup");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            dt += gameTime.ElapsedGameTime.Milliseconds;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && player.position.X >= player.texture.Width / 2)
                player.position.X -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && player.position.X + player.texture.Width / 2 <= graphics.GraphicsDevice.Viewport.Width)
                player.position.X += 10;
            foreach (Objeto o in objetos)
            {
                if (Math.Abs(o.posicaoX - player.position.X) <= 70 && o.mapa == player.mapaAtual && o.currentFrameAnimacao == 0)
                {
                    o.popupActivated = true;
                    if (o.popupScale <= 0f)
                        o.popupScale += 0.2f;
                }
                else
                    o.popupActivated = false;
                if (o.currentFrameAnimacao == o.numFramesAnimacao - 1 && !o.animationStarted)
                {
                    o.finalPopup = true;
                    o.animationStarted = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && o.VerificarPosicao(player.position.X, screenWidth) && !o.tocarAnimacao && o.mapa == player.mapaAtual)
                {
                    o.tocarAnimacao = true;
                    o.som.Play(0.5f, 0.0f, 0.0f);
                }
            }
            if (player.position.X <= player.texture.Width / 2 && player.mapaAtual > 0)
            {
                player.mapaAtual--;
                player.position.X = graphics.GraphicsDevice.Viewport.Width - player.texture.Width / 2 - 5;
            }
            if (player.position.X + player.texture.Width / 2 >= graphics.GraphicsDevice.Viewport.Width && player.mapaAtual < 2)
            {
                player.mapaAtual++;
                player.position.X = player.texture.Width / 2 + 5;
            }
            foreach (Objeto o in objetos)
                if (o.tocarAnimacao)
                    o.Animation(gameTime.ElapsedGameTime.Milliseconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            spriteBatch.Draw(mapas[player.mapaAtual], screenRectangle, Color.White);

            foreach (Objeto o in objetos)
            {
                if (o.popupScale >= 0f)
                    o.DrawPopup(spriteBatch, screenRectangle);
                if (player.mapaAtual == o.mapa)
                    o.Draw(spriteBatch, screenRectangle);
                if (o.finalPopup)
                   o.DrawFinalPopup(spriteBatch, screenRectangle, (int)gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            if(player.mapaAtual == 0)
                spriteBatch.Draw(player.texture, player.position, null, Color.White, 0f, new Vector2(player.texture.Width / 2, player.texture.Height / 2), 1, SpriteEffects.None, 0.8f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}