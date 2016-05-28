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
using SkinnedModel;

namespace AnimacjaModeli3D
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Character : DrawableGameComponent
    {

        #region private fields
        private Model _character;
        private Camera _camera;
        private DepthStencilState _depthStencilState;
        private SkinningData skinningData;
        private DragonAnimation dragonAnimation;
        #endregion

        public Character(Game game)
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

            _camera = new Camera(Game, new Vector3(0, 0, 100), new Vector3(0, 0, 0));
            _camera.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //_character = Game.Content.Load<Model>("Models/Character_FBX/character_FBX");
            //_character = Game.Content.Load<Model>("Models/dude/dude");
            _character = Game.Content.Load<Model>("Models/creature_4d1");
            SetSkinnedEffect();
            skinningData = _character.Tag as SkinningData;
            dragonAnimation =  new DragonAnimation(skinningData);
            
            base.LoadContent();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            dragonAnimation.Update(Matrix.Identity,gameTime.ElapsedGameTime);
            _camera.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = _depthStencilState;
            //_character.Draw(Matrix.CreateTranslation(new Vector3(0,0,0)), _camera.View, _camera.Projection);
            foreach (ModelMesh mesh in _character.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(dragonAnimation.Transforms);
                    effect.World =Matrix.CreateScale(2)* Matrix.CreateRotationX(MathHelper.ToRadians(-90));
                    effect.View = _camera.View;
                    effect.Projection = _camera.Projection;
                }
                mesh.Draw();
            }
            _camera.Draw(gameTime);
            base.Draw(gameTime);
        }

        private void SetSkinnedEffect()
        {
            foreach (ModelMesh mesh in _character.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    SkinnedEffect skinnedEffect = new SkinnedEffect(Game.GraphicsDevice);
                    BasicEffect basicEffect = (BasicEffect)meshPart.Effect;

                    skinnedEffect.EnableDefaultLighting();
                    skinnedEffect.SpecularColor = Color.White.ToVector3();


                    skinnedEffect.AmbientLightColor = basicEffect.AmbientLightColor;
                    skinnedEffect.DiffuseColor = basicEffect.DiffuseColor;
                    skinnedEffect.Texture = basicEffect.Texture;

                    meshPart.Effect = skinnedEffect;
                }
            }
        }
    }
}
