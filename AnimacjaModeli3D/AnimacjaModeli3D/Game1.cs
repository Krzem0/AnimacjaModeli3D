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

namespace AnimacjaModeli3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public enum GameState { StartScreen, Animation1, Animation2, Animation3, Animation4 }
        public GameState gameState { get; set; }
        public bool IsGameStateChanged { get; set; }

        #region GameComponents
        private StartScreen _startScreen;
        private Dices123 _dices;
        private Eurofighter _eurofighter;
        private Humvee _humvee;
        private Character _character;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            Window.Title = "Animacja modeli 3D, Marcin Krzemiñski";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            gameState = GameState.StartScreen;
            IsGameStateChanged = false;


            _startScreen = new StartScreen(this);
            Components.Add(_startScreen);

            _dices = new Dices123(this) { Visible = false, Enabled = false };
            Components.Add(_dices);

            _eurofighter = new Eurofighter(this) { Visible = false, Enabled = false };
            Components.Add(_eurofighter);

            _humvee = new Humvee(this) { Visible = false, Enabled = false };
            Components.Add(_humvee);

            _character = new Character(this) { Visible = false, Enabled = false };
            Components.Add(_character);

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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (IsGameStateChanged)
            {
                IsGameStateChanged = false;

                _startScreen.Visible = false;
                _startScreen.Enabled = false;
                _dices.Visible = false;
                _dices.Enabled = false;
                _eurofighter.Visible = false;
                _eurofighter.Enabled = false;
                _humvee.Visible = false;
                _humvee.Enabled = false;
                _character.Visible = false;
                _character.Enabled = false;
                switch (gameState)
                {
                    case GameState.StartScreen:
                        _startScreen.Visible = true;
                        _startScreen.Enabled = true;
                        break;
                    case GameState.Animation1:
                        _dices.Visible = true;
                        _dices.Enabled = true;
                        break;
                    case GameState.Animation2:
                        _eurofighter.Visible = true;
                        _eurofighter.Enabled = true;
                        break;
                    case GameState.Animation3:
                        _humvee.Visible = true;
                        _humvee.Enabled = true;
                        break;
                    case GameState.Animation4:
                        _character.Visible = true;
                        _character.Enabled = true;
                        break;
                    default:
                        break;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SteelBlue);
            base.Draw(gameTime);
        }
    }
}
