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
    public class StartScreen : DrawableGameComponent
    {
        // Typ wyliczeniowy okreœlaj¹cy wygl¹d kursora myszki
        private enum CursorState { Arrow, Hand }
        private CursorState _cursorState = CursorState.Arrow;

        #region private fields

        private Texture2D _arrowCrs, _handCrs;
        private Texture2D _backGround;
        private Texture2D _title;
        private Texture2D[] _menuItemsTex;
        private Vector2 _titlePosition;
        private Vector2[] _menuItemsCoord;
        private Vector2 _mousePosition;
        private SpriteBatch _spriteBatch;

        #endregion

        public StartScreen(Game game)
            : base(game)
        {

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            _backGround = Game.Content.Load<Texture2D>("Sprites/backGround");
            _arrowCrs = Game.Content.Load<Texture2D>("Sprites/arrowCrs");
            _handCrs = Game.Content.Load<Texture2D>("Sprites/handCrs");
            _title = Game.Content.Load<Texture2D>("Sprites/title");
            _menuItemsTex = new Texture2D[]
                                {
                                    Game.Content.Load<Texture2D>("Sprites/menuItem1"),
                                    Game.Content.Load<Texture2D>("Sprites/menuItem2"),
                                    Game.Content.Load<Texture2D>("Sprites/menuItem3"),
                                    Game.Content.Load<Texture2D>("Sprites/menuItem4")
                                };

            _titlePosition = new Vector2(GraphicsDevice.Viewport.Width / 2f,
                                        GraphicsDevice.Viewport.Height - GraphicsDevice.Viewport.Height * 3 / 4);
            _menuItemsCoord = new Vector2[]
                                  {
                                     new Vector2(_titlePosition.X, _titlePosition.Y + _title.Height + 30),
                                     new Vector2(_titlePosition.X, _titlePosition.Y + _title.Height + 30 + 
                                         _menuItemsTex[0].Height + 30),
                                     new Vector2(_titlePosition.X, _titlePosition.Y + _title.Height + 30 + 
                                         _menuItemsTex[0].Height + 30 + _menuItemsTex[1].Height+30),
                                     new Vector2(_titlePosition.X, _titlePosition.Y + _title.Height + 30 + 
                                         _menuItemsTex[0].Height + 30 + _menuItemsTex[1].Height + 30 + _menuItemsTex[2].Height + 30)
                                  };

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            _mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            _cursorState = CursorState.Arrow;

            MenuItemClick(new Rectangle((int)_titlePosition.X - _menuItemsTex[0].Width / 2,
                (int)_titlePosition.Y - _menuItemsTex[0].Height / 2 + _title.Height + 30,
                _menuItemsTex[0].Width, _menuItemsTex[0].Height).Intersects(
                new Rectangle((int)_mousePosition.X, (int)_mousePosition.Y, 2, 2)), Game1.GameState.Animation1);

            MenuItemClick(new Rectangle((int)_titlePosition.X - _menuItemsTex[1].Width / 2,
                (int)_titlePosition.Y - _menuItemsTex[1].Height / 2 + _title.Height + 30 + _menuItemsTex[0].Height + 30,
                _menuItemsTex[1].Width, _menuItemsTex[1].Height).Intersects(
                new Rectangle((int)_mousePosition.X, (int)_mousePosition.Y, 2, 2)), Game1.GameState.Animation2);

            MenuItemClick(new Rectangle((int)_titlePosition.X - _menuItemsTex[2].Width / 2,
                (int)_titlePosition.Y - _menuItemsTex[2].Height / 2 + _title.Height + 30 + _menuItemsTex[0].Height + 30 +
                _menuItemsTex[1].Height + 30,
                _menuItemsTex[2].Width, _menuItemsTex[2].Height).Intersects(
                new Rectangle((int)_mousePosition.X, (int)_mousePosition.Y, 2, 2)), Game1.GameState.Animation3);

            MenuItemClick(new Rectangle((int)_titlePosition.X - _menuItemsTex[1].Width / 2,
                (int)_titlePosition.Y - _menuItemsTex[1].Height / 2 + _title.Height + 30 + _menuItemsTex[0].Height + 30 + 
                _menuItemsTex[1].Height + 30 + _menuItemsTex[2].Height + 30,
                _menuItemsTex[1].Width, _menuItemsTex[1].Height).Intersects(
                new Rectangle((int)_mousePosition.X, (int)_mousePosition.Y, 2, 2)), Game1.GameState.Animation4);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // Rysuje kursor: strza³kê, lub rêkê, w zale¿noœci od po³o¿enia myszy na ekranie
            // czy wspó³rzêdne koliduj¹ z tekstem
            CursorDraw();

            Vector2 titleOrigin = new Vector2(_title.Width, _title.Height) / 2;
            _spriteBatch.Draw(_title, _titlePosition, null, Color.White, 0, titleOrigin, 1, SpriteEffects.None, 0.5f);
            for (int i = 0; i < _menuItemsTex.Length; i++)
            {
                _spriteBatch.Draw(_menuItemsTex[i], _menuItemsCoord[i],
                    null, Color.White, 0, new Vector2(_menuItemsTex[i].Width / 2f, _menuItemsTex[i].Height / 2f), 1, SpriteEffects.None, 0.5f);
            }
            _spriteBatch.Draw(_backGround, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void CursorDraw()
        {
            switch (_cursorState)
            {
                case CursorState.Arrow:
                    _spriteBatch.Draw(_arrowCrs, _mousePosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
                case CursorState.Hand:
                    _spriteBatch.Draw(_handCrs, _mousePosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
                default:
                    _spriteBatch.Draw(_arrowCrs, _mousePosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    break;
            }
        }

        private void MenuItemClick(bool IsClick, Game1.GameState gameState)
        {
            if (IsClick)
            {
                _cursorState = CursorState.Hand;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    ((Game1)Game).gameState = gameState;
                    if (!((Game1)Game).IsGameStateChanged)
                    {
                        ((Game1)Game).IsGameStateChanged = true;
                    }
                }
            }
        }
    }
}
