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
    public class Eurofighter : DrawableGameComponent
    {
        #region private fields
        private Model eurofighter;
        private Model ground;
        private Camera camera;
        private KeyframedAnimation _animation;
        private DepthStencilState _depthStencilState;
        #endregion

        public Eurofighter(Game game)
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

            camera = new Camera(Game, new Vector3(0, 0, 500), new Vector3(0, 0, 0));
            camera.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            eurofighter = Game.Content.Load<Model>("Models/eurofighter");
            ground = Game.Content.Load<Model>("Models/ground");

            List<AnimationFrame> frames = MyAnimation();
            _animation = new KeyframedAnimation(frames, true);

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            camera.Update(gameTime);
            _animation.Update(gameTime.ElapsedGameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = _depthStencilState;
            DrawGround();
            DrawEurofighter(_animation.Rotation, _animation.Position);
            camera.Draw(gameTime);
            base.Draw(gameTime);
        }

        private static List<AnimationFrame> MyAnimation()
        {
            List<AnimationFrame> animationFrames = new List<AnimationFrame>();
            animationFrames.Add(new AnimationFrame(new Vector3(-200, -50, 0), 
                new Vector3(0,MathHelper.ToRadians(180),MathHelper.ToRadians(10)), TimeSpan.FromSeconds(0)));
            animationFrames.Add(new AnimationFrame(new Vector3(-200, -50, -60),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(20)), TimeSpan.FromSeconds(1)));
            animationFrames.Add(new AnimationFrame(new Vector3(-120, -50, -140),
                new Vector3(0, MathHelper.ToRadians(90), MathHelper.ToRadians(45)), TimeSpan.FromSeconds(2)));
            animationFrames.Add(new AnimationFrame(new Vector3(-40, -50, -60),
                new Vector3(0, MathHelper.ToRadians(45), MathHelper.ToRadians(20)), TimeSpan.FromSeconds(3)));
            animationFrames.Add(new AnimationFrame(new Vector3(40, -50, 60),
                new Vector3(0, MathHelper.ToRadians(45), MathHelper.ToRadians(-20)), TimeSpan.FromSeconds(4)));
            animationFrames.Add(new AnimationFrame(new Vector3(120, -50, 140),
                new Vector3(0, MathHelper.ToRadians(90), MathHelper.ToRadians(-45)), TimeSpan.FromSeconds(5)));
            animationFrames.Add(new AnimationFrame(new Vector3(200, -50, 60),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(-20)), TimeSpan.FromSeconds(6)));
            animationFrames.Add(new AnimationFrame(new Vector3(200, -50, -60),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(-20)), TimeSpan.FromSeconds(7)));
            animationFrames.Add(new AnimationFrame(new Vector3(120, -50, -140),
                new Vector3(0, MathHelper.ToRadians(270), MathHelper.ToRadians(-45)), TimeSpan.FromSeconds(8)));
            animationFrames.Add(new AnimationFrame(new Vector3(40, -50, -60),
                new Vector3(0, MathHelper.ToRadians(315), MathHelper.ToRadians(-20)), TimeSpan.FromSeconds(9)));
            animationFrames.Add(new AnimationFrame(new Vector3(-40, -50, 60),
                new Vector3(0, MathHelper.ToRadians(315), MathHelper.ToRadians(20)), TimeSpan.FromSeconds(10)));
            animationFrames.Add(new AnimationFrame(new Vector3(-120, -50, 140),
                new Vector3(0, MathHelper.ToRadians(270), MathHelper.ToRadians(45)), TimeSpan.FromSeconds(11)));
            animationFrames.Add(new AnimationFrame(new Vector3(-200, -50, 60),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(20)), TimeSpan.FromSeconds(12)));
            animationFrames.Add(new AnimationFrame(new Vector3(-200, -50, 0),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(10)), TimeSpan.FromSeconds(13)));
            return animationFrames;
        }

        private void DrawGround()
        {
            foreach (BasicEffect effect in ground.Meshes["Plane"].Effects)
            {
                effect.EnableDefaultLighting();
                effect.Projection = camera.Projection;
                effect.View = camera.View;
                effect.World = Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateScale(3000) * Matrix.CreateTranslation(0, -500, 0);
            }
            ground.Meshes["Plane"].Draw();
        }

        private void DrawEurofighter(Vector3 rotation, Vector3 position)
        {
            foreach (ModelMesh mesh in eurofighter.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = camera.Projection;
                    effect.View = camera.View;
                    effect.World = Matrix.CreateRotationX(-MathHelper.PiOver2) *
                        Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateTranslation(position);
                    // Matrix.CreateRotationY(rotation.Y)
                }
                mesh.Draw();
            }
        }
    }
}
