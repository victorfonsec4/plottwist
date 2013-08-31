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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D[] mapas;
        Player player;
        Objeto[] objetos;
        int numObjetos;
        int dt;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1366;
            graphics.PreferredBackBufferWidth = 768;
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            mapas = new Texture2D[3];
            player = new Player(0, graphics.GraphicsDevice.Viewport.Height * 3 / 4);
            numObjetos = 1;
            objetos = new Objeto[numObjetos];
            objetos[0] = new Objeto(graphics.GraphicsDevice.Viewport.Width/2, graphics.GraphicsDevice.Viewport.Height/2, 1, 3);
            dt = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mapas[0] = Content.Load<Texture2D>("mapa1");
            mapas[1] = Content.Load<Texture2D>("mapa2");
            mapas[2] = Content.Load<Texture2D>("mapa3");
            player.texture = Content.Load<Texture2D>("cara");
            objetos[0].texture = Content.Load<Texture2D>("objeto1");
            objetos[0].framesAnimacao[0] = Content.Load<Texture2D>("frame1");
            objetos[0].framesAnimacao[1] = Content.Load<Texture2D>("frame2");
            objetos[0].framesAnimacao[2] = Content.Load<Texture2D>("frame3");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            dt += gameTime.ElapsedGameTime.Milliseconds;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && player.position.X >= 0)
                player.position.X -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && player.position.X + player.texture.Width <= graphics.GraphicsDevice.Viewport.Width)
                player.position.X += 10;
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                for (int i = 0; i < numObjetos; i++)
                {
                    if (Math.Abs((player.position.X - objetos[i].position.X)) <= 20)
                        objetos[i].tocarAnimacao = true;
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
                for (int i = 0; i < numObjetos; i++)
                {
                    if (objetos[i].tocarAnimacao && objetos[i].currentFrameAnimacao < objetos[i].numFramesAnimacao - 1)
                    {
                        objetos[i].currentFrameAnimacao++;
                    }
                }
            }

        
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(mapas[player.mapaAtual], new Vector2(0, 0), Color.White);
            spriteBatch.Draw(player.texture, player.position, Color.White);
            for (int i = 0; i < numObjetos; i++)
            {
                if (player.mapaAtual == objetos[i].mapa && !objetos[i].tocarAnimacao)
                    spriteBatch.Draw(objetos[i].texture, objetos[i].position, Color.White);
                if (player.mapaAtual == objetos[i].mapa && objetos[i].tocarAnimacao)
                    spriteBatch.Draw(objetos[i].framesAnimacao[objetos[i].currentFrameAnimacao], objetos[i].position, Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
