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
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            mapas = new Texture2D[3];
            player = new Player(0, graphics.GraphicsDevice.Viewport.Height * 3 / 4);
            numObjetos = 1;
            objetos = new Objeto[numObjetos];
            objetos[0] = new Objeto(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2, 1, 3, "Teste", 1, 3, 500);
            //objetos[1] = new Objeto(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2, 0, 10, "Heat Milk");
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
            objetos[0].spriteSheet= Content.Load<Texture2D>("sheet");
            objetos[0].popupFont = Content.Load<SpriteFont>("FontePopups");
            objetos[0].popupTexture = Content.Load<Texture2D>("Popup");
            objetos[0].som = Content.Load<SoundEffect>("microwavefinal");
            /*objetos[1].texture = Content.Load<Texture2D>("microwave/microwaveFrame10");
            objetos[1].som = Content.Load<SoundEffect>("microwavefinal");
            objetos[1].popupFont = Content.Load<SpriteFont>("FontePopups");
            objetos[1].popupTexture = Content.Load<Texture2D>("Popup");
            objetos[1].framesAnimacao[0] = Content.Load<Texture2D>("microwave/microwaveFrame1");
            objetos[1].framesAnimacao[1] = Content.Load<Texture2D>("microwave/microwaveFrame2");
            objetos[1].framesAnimacao[2] = Content.Load<Texture2D>("microwave/microwaveFrame3");
            objetos[1].framesAnimacao[3] = Content.Load<Texture2D>("microwave/microwaveFrame4");
            objetos[1].framesAnimacao[4] = Content.Load<Texture2D>("microwave/microwaveFrame5");
            objetos[1].framesAnimacao[5] = Content.Load<Texture2D>("microwave/microwaveFrame6");
            objetos[1].framesAnimacao[6] = Content.Load<Texture2D>("microwave/microwaveFrame7");
            objetos[1].framesAnimacao[7] = Content.Load<Texture2D>("microwave/microwaveFrame8");
            objetos[1].framesAnimacao[8] = Content.Load<Texture2D>("microwave/microwaveFrame9");
            objetos[1].framesAnimacao[9] = Content.Load<Texture2D>("microwave/microwaveFrame10");*/
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
            foreach (Objeto o in objetos)
            {
                if (Math.Abs(o.position.X - player.position.X) <= 70 && o.mapa==player.mapaAtual)
                {
                    o.popupActivated = true;
                    if (o.popupScale <= 0f)
                        o.popupScale += 0.2f;
                }
                else
                    o.popupActivated = false;
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && o.VerificarPosicao(player.position.X, screenWidth) && !o.tocarAnimacao && o.mapa==player.mapaAtual)
                {
                    o.tocarAnimacao = true;
                    o.som.Play(0.5f, 0.0f, 0.0f);
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
            foreach (Objeto o in objetos)
                if(o.tocarAnimacao)
                    o.Animation(gameTime.ElapsedGameTime.Milliseconds);

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
                if (player.mapaAtual == objetos[i].mapa)
                    objetos[i].Draw(spriteBatch, screenRectangle);
                if (objetos[i].popupScale>=0f)
                    objetos[i].DrawPopup(spriteBatch, screenRectangle);
            }



            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
