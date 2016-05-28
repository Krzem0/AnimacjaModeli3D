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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Camera : DrawableGameComponent
    {
        // Typ wyliczeniowy okreœlaj¹cy wygl¹d kursora myszki
        private enum CursorState { Arrow, Hand }
        private CursorState cursorState = CursorState.Arrow;

        #region private fields
        private Matrix _world;
        private MouseState _mousePrevState;
        private readonly Vector3 _cameraDefaultPosition;
        private readonly Vector3 _cameraDefaultTarget;
        private readonly Vector3 _cameraDefaultUp;
        private Vector3 _cameraTempPosition;
        private Vector3 _cameraTempTarget;
        private Vector3 _cameraTempUp;
        private const float RotationSpeed = 0.3f;
        private float _updownRot;
        private float _leftrightRot;
        private bool _helpVisible;

        private SpriteBatch _spriteBatch;
        private Texture2D _arrowCrs, _handCrs;
        private Texture2D _shadow;
        private Texture2D _helpIcon, _helpText;
        private Rectangle _helpIconRectangle;
        private Texture2D _backButton;
        private Rectangle _backButtonRectangle;
        #endregion

        #region public properties
        public Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.PiOver4,
                    Game.Window.ClientBounds.Width / (float)Game.Window.ClientBounds.Height,
                    1, 10000);
            }
        }
        public Matrix View
        {
            get { return Matrix.CreateLookAt(_cameraTempPosition, _cameraTempTarget, _cameraTempUp); }
        }
        public Matrix World
        {
            get { return _world; }
        }
        #endregion

        public Camera(Game game, Vector3 cameraPosition, Vector3 cameraTarget)
            : base(game)
        {
            _cameraDefaultPosition = _cameraTempPosition = cameraPosition;
            _cameraDefaultTarget = _cameraTempTarget = cameraTarget;
            _cameraDefaultUp = _cameraTempUp = Vector3.Up;
            _world = Matrix.Identity;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _mousePrevState = Mouse.GetState();
            _helpVisible = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _arrowCrs = Game.Content.Load<Texture2D>("Sprites/arrowCrs");
            _handCrs = Game.Content.Load<Texture2D>("Sprites/handCrs");
            _shadow = Game.Content.Load<Texture2D>("Sprites/shadow");

            _helpIcon = Game.Content.Load<Texture2D>("Sprites/helpIcon");
            _helpIconRectangle = new Rectangle(10, 10, _helpIcon.Width, _helpIcon.Height);
            _helpText = Game.Content.Load<Texture2D>("Sprites/helpText");

            _backButton = Game.Content.Load<Texture2D>("Sprites/backButton");
            _backButtonRectangle = new Rectangle(5, Game.Window.ClientBounds.Height - _backButton.Height - 5,
                _backButton.Width, _backButton.Height);

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float timeDifference = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            MouseState mouseState = Mouse.GetState();
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                float xDifference = _mousePrevState.X - mouseState.X;
                float yDifference = _mousePrevState.Y - mouseState.Y;

                _leftrightRot += RotationSpeed * xDifference * timeDifference;
                float updownTempRot = RotationSpeed * yDifference * timeDifference;
                if (Math.Abs(_updownRot + updownTempRot) < MathHelper.PiOver2) _updownRot += updownTempRot;

                Matrix cameraRotation = Matrix.CreateRotationX(_updownRot) * Matrix.CreateRotationY(_leftrightRot);

                _cameraTempPosition = Vector3.Transform(_cameraDefaultPosition, cameraRotation);
            }

            _mousePrevState = mouseState;


            cursorState = CursorState.Arrow;
            if (Keyboard.GetState().IsKeyDown(Keys.R)) Reset();

            if (_backButtonRectangle.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1)))
            {
                cursorState = CursorState.Hand;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    ((Game1)Game).gameState = Game1.GameState.StartScreen;
                    ((Game1)Game).IsGameStateChanged = true;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                ((Game1)Game).gameState = Game1.GameState.StartScreen;
                ((Game1)Game).IsGameStateChanged = true;
            }

            _helpVisible = _helpIconRectangle.Intersects(new Rectangle(mouseState.X,mouseState.Y,1,1));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_shadow, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_backButton, _backButtonRectangle, Color.White);
            _spriteBatch.Draw(_helpVisible ? _helpText : _helpIcon, new Vector2(10, 10), Color.White);
            CursorDraw();
            _spriteBatch.End();
        }

        public void Reset()
        {
            _cameraTempPosition = _cameraDefaultPosition;
            _cameraTempTarget = _cameraDefaultTarget;
            _cameraTempUp = _cameraDefaultUp;
            _updownRot = 0;
            _leftrightRot = 0;
        }

        private void CursorDraw()
        {
            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            switch (cursorState)
            {
                case CursorState.Arrow:
                    _spriteBatch.Draw(_arrowCrs, mousePosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
                case CursorState.Hand:
                    _spriteBatch.Draw(_handCrs, mousePosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
                default:
                    _spriteBatch.Draw(_arrowCrs, mousePosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
            }
        }
    }
}
