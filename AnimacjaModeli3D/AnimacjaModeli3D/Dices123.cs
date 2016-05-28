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
    public class Dices123 : DrawableGameComponent
    {
        #region private fields
        private Model dices;
        private Matrix _diceWorld;
        private DiceAnimation _diceAnimation;
        private Camera camera;
        private DepthStencilState _depthStencilState;
        #endregion

        public Dices123(Game game)
            : base(game)
        {

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            _depthStencilState = new DepthStencilState { DepthBufferEnable = true, DepthBufferWriteEnable = true };

            camera = new Camera(Game, new Vector3(0, 0, 100), new Vector3(0, 0, 0));
            camera.Initialize();

            _diceAnimation = new DiceAnimation(new Vector3(-30, 0, 0), new Vector3(30, 0, 0),
                0, MathHelper.TwoPi,
                0.1f, 0.1f, true, new TimeSpan(0, 0, 0, 5));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            dices = Game.Content.Load<Model>("Models/dices123");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            camera.Update(gameTime);

            _diceAnimation.Update(gameTime.ElapsedGameTime);
            _diceWorld = Matrix.CreateScale(_diceAnimation.Scale) *
                Matrix.CreateRotationY(_diceAnimation.Rotation) *
                Matrix.CreateTranslation(_diceAnimation.Position);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = _depthStencilState;
            foreach (BasicEffect effect in dices.Meshes[0].Effects)
            {
                effect.EnableDefaultLighting();
                effect.Projection = camera.Projection;
                effect.View = camera.View;
                effect.World = _diceWorld;
            }
            dices.Meshes[0].Draw();

            camera.Draw(gameTime);
            base.Draw(gameTime);
        }

        private class DiceAnimation
        {
            private Vector3 _startPosition;
            private Vector3 _endPosition;
            private float _startRot;
            private float _endRot;
            private TimeSpan _duration;
            private TimeSpan _elapsedTimeUp;
            private TimeSpan _elapsedTimeDn;
            private bool _loop;
            private float _startScale;
            private float _endScale;

            public Vector3 Position { get; private set; }
            public float Rotation { get; private set; }
            public float Scale { get; private set; }

            public DiceAnimation(Vector3 startPosition, Vector3 endPosition,
                float startRotation, float endRotation,
                float startScale, float endScale,
                bool loop, TimeSpan duration)
            {
                _startPosition = startPosition;
                _endPosition = endPosition;
                _startRot = startRotation;
                _endRot = endRotation;
                _startScale = startScale;
                _endScale = endScale;
                _loop = loop;
                _duration = duration;
                Position = startPosition;
                Rotation = startRotation;
                Scale = startScale;
            }

            public void Update(TimeSpan elapsed)
            {
                float current;

                if (_loop)
                {
                    if (_elapsedTimeUp + elapsed < _duration)
                    {
                        _elapsedTimeUp += elapsed;
                        current = (float)_elapsedTimeUp.TotalSeconds / (float)_duration.TotalSeconds;
                        Position = Vector3.Lerp(_startPosition, _endPosition, current);
                        Rotation = MathHelper.Lerp(_startRot, _endRot, current);
                        Scale = current < 0.5 ? MathHelper.Lerp(_startScale, _endScale * 2, current) :
                            MathHelper.Lerp(_endScale * 2, _startScale, current);

                    }
                    else
                    {
                        if (_elapsedTimeDn + elapsed < _duration)
                        {
                            _elapsedTimeDn += elapsed;
                            current = (float)_elapsedTimeDn.TotalSeconds / (float)_duration.TotalSeconds;
                            Position = Vector3.Lerp(_endPosition, _startPosition, current);
                            Rotation = MathHelper.Lerp(_endRot, _startRot, current);
                            Scale = current < 0.5 ? MathHelper.Lerp(_endScale, _startScale * 2, current) :
                                MathHelper.Lerp(_endScale * 2, _startScale, current);
                        }
                        else
                        {
                            _elapsedTimeUp = new TimeSpan();
                            _elapsedTimeDn = new TimeSpan();
                        }
                    }
                }
                else
                {
                    _elapsedTimeUp += elapsed;
                    current = (float)_elapsedTimeUp.TotalSeconds / (float)_duration.TotalSeconds;
                    current = MathHelper.Clamp(current, 0, 1);
                    Position = Vector3.Lerp(_startPosition, _endPosition, current);
                    Rotation = MathHelper.Lerp(_startRot, _endRot, current);
                    Scale = MathHelper.Lerp(_startScale, _endScale, current);
                }
            }
        }
    }
}
