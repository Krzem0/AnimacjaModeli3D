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
    public class Humvee : DrawableGameComponent
    {

        #region private fields
        private Model ground;
        private Camera _camera;
        private DepthStencilState _depthStencilState;
        private HierarchicalAnimation _hAnimation;
        #endregion

        public Humvee(Game game)
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

            _camera = new Camera(Game, new Vector3(0, 0, 500), new Vector3(0, 0, 0));
            _camera.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Model Humvee = Game.Content.Load<Model>("Models/humvee");
            _hAnimation = new HierarchicalAnimation(Humvee);

            ground = Game.Content.Load<Model>("Models/ground");
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);
            _hAnimation.Update(gameTime.ElapsedGameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = _depthStencilState;
            _hAnimation.Draw(gameTime.ElapsedGameTime,_camera.View,_camera.Projection);
            DrawGround();
            _camera.Draw(gameTime);
            base.Draw(gameTime);
        }

        private void DrawGround()
        {
            foreach (BasicEffect effect in ground.Meshes["Plane"].Effects)
            {
                effect.EnableDefaultLighting();
                effect.Projection = _camera.Projection;
                effect.View = _camera.View;
                effect.World = Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateScale(3000) * Matrix.CreateTranslation(0, -67, 0);
            }
            ground.Meshes["Plane"].Draw();
        }
    }
}
