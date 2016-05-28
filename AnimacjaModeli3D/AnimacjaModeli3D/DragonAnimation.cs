using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SkinnedModel;

namespace AnimacjaModeli3D
{
    class DragonAnimation
    {
        #region private fields
        private Matrix[] _boneTransforms;
        private Matrix[] _worldTransforms;
        private Matrix[] _skinTransforms;
        private SkinningData _skinningData;
        private KeyframedAnimation _cheek;
        #endregion

        #region Spinal column fields
        private KeyframedAnimation _spinalColumn1;
        private KeyframedAnimation _spinalColumn2;
        private KeyframedAnimation _spinalColumn3;
        private KeyframedAnimation _spinalColumn4;
        private KeyframedAnimation _spinalColumn5;
        private KeyframedAnimation _spinalColumn6;
        private KeyframedAnimation _spinalColumn7;
        #endregion

        #region Legs fields
        private KeyframedAnimation _frontLeftLeg1;
        private KeyframedAnimation _frontLeftLeg2;
        private KeyframedAnimation _frontLeftLeg4;

        private KeyframedAnimation _frontRightLeg1;
        private KeyframedAnimation _frontRightLeg2;
        private KeyframedAnimation _frontRightLeg4;

        private KeyframedAnimation _backLeftLeg1;
        private KeyframedAnimation _backLeftLeg3;

        private KeyframedAnimation _backRightLeg1;
        private KeyframedAnimation _backRightLeg3;
        #endregion

        #region Tail fields
        private KeyframedAnimation _tail1;
        private KeyframedAnimation _tail2;
        private KeyframedAnimation _tail3;
        private KeyframedAnimation _tail4;
        private KeyframedAnimation _tail5;
        private KeyframedAnimation _tail6;
        private KeyframedAnimation _tail7;
        private KeyframedAnimation _tail8;
        #endregion

        public DragonAnimation(SkinningData skinningData)
        {
            _skinningData = skinningData;
            _boneTransforms = new Matrix[skinningData.BindPose.Count];
            _worldTransforms = new Matrix[skinningData.BindPose.Count];
            _skinTransforms = new Matrix[skinningData.BindPose.Count];
            _boneTransforms = skinningData.BindPose.ToArray();
            _cheek = new KeyframedAnimation(CheekAnimation(), true);

            LegsInitialization();
            TailInitialization();
            SpinalColumnInitialization();
        }

        public void Update(Matrix root, TimeSpan elapsed)
        {
            _cheek.Update(elapsed);
            //_boneTransforms[34] = Matrix.CreateRotationX(MathHelper.ToRadians(5)) * _boneTransforms[34];
            //_boneTransforms[7] = Matrix.CreateTranslation(new Vector3(5,0,0)) * _skinningData.BindPose[7];
            _boneTransforms[10] = Matrix.CreateTranslation(_cheek.Position) * _skinningData.BindPose[10];

            SpinalColumnUpdate(elapsed);
            SpinalColumnAnimation();

            LegsUpdate(elapsed);
            LegsAnimation();

            TailUpdate(elapsed);
            TailAnimation();

            UpdateWorldTransforms(root);
            UpdateSkinTransforms();
        }

        private void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Kość korzenia.
            _worldTransforms[0] = _boneTransforms[0] * rootTransform;

            // Kosci potomkowie.
            for (int bone = 1; bone < _worldTransforms.Length; bone++)
            {
                int parentBone = _skinningData.SkeletonHierarchy[bone];

                _worldTransforms[bone] = _boneTransforms[bone] *
                                             _worldTransforms[parentBone];
            }
        }

        private void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < _skinTransforms.Length; bone++)
            {
                _skinTransforms[bone] = _skinningData.InverseBindPose[bone] *
                                            _worldTransforms[bone];
            }
        }

        public Matrix[] Transforms { get { return _skinTransforms; } }

        private List<AnimationFrame> CheekAnimation()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0.01f, 0.1f, 0.1f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0.02f, 0.15f, 0.3f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0.03f, 0.2f, 0.6f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0.03f, 0.2f, 0.6f), Vector3.Zero, TimeSpan.FromSeconds(2.5)));
            frames.Add(new AnimationFrame(new Vector3(0.02f, 0.15f, 0.3f), Vector3.Zero, TimeSpan.FromSeconds(3)));
            frames.Add(new AnimationFrame(new Vector3(0.01f, 0.1f, 0.1f), Vector3.Zero, TimeSpan.FromSeconds(3.5)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(4)));
            return frames;
        }

        #region Spinal column
        private void SpinalColumnInitialization()
        {
            _spinalColumn1 = new KeyframedAnimation(SpinalColumn1(),true);
            _spinalColumn2 = new KeyframedAnimation(SpinalColumn2(), true);
            _spinalColumn3 = new KeyframedAnimation(SpinalColumn3(), true);
            _spinalColumn4 = new KeyframedAnimation(SpinalColumn4(), true);
            _spinalColumn5 = new KeyframedAnimation(SpinalColumn5(), true);
            _spinalColumn6 = new KeyframedAnimation(SpinalColumn6(), true);
            _spinalColumn7 = new KeyframedAnimation(SpinalColumn7(), true);
        }

        private void SpinalColumnUpdate(TimeSpan elapsed)
        {
            _spinalColumn1.Update(elapsed);
            _spinalColumn2.Update(elapsed);
            _spinalColumn3.Update(elapsed);
            _spinalColumn4.Update(elapsed);
            _spinalColumn5.Update(elapsed);
            _spinalColumn6.Update(elapsed);
            _spinalColumn7.Update(elapsed);
        }

        private void SpinalColumnAnimation()
        {
            _boneTransforms[0] = Matrix.CreateTranslation(_spinalColumn1.Position) * _skinningData.BindPose[0];
            _boneTransforms[1] = Matrix.CreateTranslation(_spinalColumn2.Position) * _skinningData.BindPose[1];
            _boneTransforms[2] = Matrix.CreateTranslation(_spinalColumn3.Position) * _skinningData.BindPose[2];
            _boneTransforms[3] = Matrix.CreateTranslation(_spinalColumn4.Position) * _skinningData.BindPose[3];
            _boneTransforms[5] = Matrix.CreateTranslation(_spinalColumn5.Position) * _skinningData.BindPose[5];
            _boneTransforms[6] = Matrix.CreateTranslation(_spinalColumn6.Position) * _skinningData.BindPose[6];
            _boneTransforms[8] = Matrix.CreateTranslation(_spinalColumn7.Position) * _skinningData.BindPose[8];
        }

        private List<AnimationFrame> SpinalColumn1()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0f, 0.25f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, -0.25f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0.25f, 0f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> SpinalColumn2()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0.25f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0f, -0.25f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> SpinalColumn3()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0f, 0.25f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0f, -0.25f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0f, 0.25f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> SpinalColumn4()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> SpinalColumn5()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(-0.25f, 0.125f, 0.25f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0.125f, -0.25f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(-0.25f, 0.125f, 0.25f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> SpinalColumn6()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0f, -0.5f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(-0.1f, 0f, 0.2f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, -0.5f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0.1f, 0f, -0.2f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, -0.5f, 0f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> SpinalColumn7()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0f, 0.2f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0.2f, 0f, 0.2f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, -0.2f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(-0.2f, 0f, -0.2f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0.2f, 0f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }
        #endregion Spinal column

        #region Legs
        private void LegsInitialization()
        {
            _frontLeftLeg1 = new KeyframedAnimation(FrontLeftLeg1(), true);
            _frontLeftLeg2 = new KeyframedAnimation(FrontLeftLeg2(), true);
            _frontLeftLeg4 = new KeyframedAnimation(FrontLeftLeg4(), true);

            _frontRightLeg1 = new KeyframedAnimation(FrontRightLeg1(), true);
            _frontRightLeg2 = new KeyframedAnimation(FrontRightLeg2(), true);
            _frontRightLeg4 = new KeyframedAnimation(FrontRightLeg4(), true);

            _backLeftLeg1 = new KeyframedAnimation(BackLeftLeg1(), true);
            _backLeftLeg3 = new KeyframedAnimation(BackLeftLeg3(), true);

            _backRightLeg1 = new KeyframedAnimation(BackRightLeg1(), true);
            _backRightLeg3 = new KeyframedAnimation(BackRightLeg3(), true);
        }

        private void LegsUpdate(TimeSpan elapsed)
        {
            _frontLeftLeg1.Update(elapsed);
            _frontLeftLeg2.Update(elapsed);
            _frontLeftLeg4.Update(elapsed);

            _frontRightLeg1.Update(elapsed);
            _frontRightLeg2.Update(elapsed);
            _frontRightLeg4.Update(elapsed);

            _backLeftLeg1.Update(elapsed);
            _backLeftLeg3.Update(elapsed);

            _backRightLeg1.Update(elapsed);
            _backRightLeg3.Update(elapsed);
        }

        private void LegsAnimation()
        {
            _boneTransforms[11] = Matrix.CreateTranslation(_frontLeftLeg1.Position) * _skinningData.BindPose[11];
            _boneTransforms[12] = Matrix.CreateTranslation(_frontLeftLeg2.Position) * _skinningData.BindPose[12];
            _boneTransforms[14] = Matrix.CreateTranslation(_frontLeftLeg4.Position) * _skinningData.BindPose[14];

            _boneTransforms[21] = Matrix.CreateTranslation(_frontRightLeg1.Position) * _skinningData.BindPose[21];
            _boneTransforms[22] = Matrix.CreateTranslation(_frontRightLeg2.Position) * _skinningData.BindPose[22];
            _boneTransforms[24] = Matrix.CreateTranslation(_frontRightLeg4.Position) * _skinningData.BindPose[24];

            _boneTransforms[31] = Matrix.CreateTranslation(_backLeftLeg1.Position) * _skinningData.BindPose[31];
            _boneTransforms[33] = Matrix.CreateTranslation(_backLeftLeg3.Position) * _skinningData.BindPose[33];

            _boneTransforms[43] = Matrix.CreateTranslation(_backRightLeg1.Position) * _skinningData.BindPose[43];
            _boneTransforms[45] = Matrix.CreateTranslation(_backRightLeg3.Position) * _skinningData.BindPose[45];
        }

        private List<AnimationFrame> FrontLeftLeg1()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0.5f, 0), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, -0.5f, 0), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0.5f, 0), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> FrontLeftLeg2()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, -0.5f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, -0.5f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> FrontLeftLeg4()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0.5f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> FrontRightLeg1()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> FrontRightLeg2()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(-0.3f, -0.5f, 0.2f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(-0.3f, -0.5f, 0.2f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> FrontRightLeg4()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0.5f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> BackLeftLeg1()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, -0.5f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> BackLeftLeg3()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0.2f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0.2f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> BackRightLeg1()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0f, -0.5f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0f, 0.5f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }

        private List<AnimationFrame> BackRightLeg3()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0.2f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(0.5)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0f, 0.2f, 0f), Vector3.Zero, TimeSpan.FromSeconds(1.5)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0.2f, 0f), Vector3.Zero, TimeSpan.FromSeconds(2)));
            return frames;
        }
        #endregion Legs

        #region Tail
        private void TailInitialization()
        {
            _tail1 = new KeyframedAnimation(Tail1Animation(), true);
            _tail2 = new KeyframedAnimation(Tail2Animation(), true);
            _tail3 = new KeyframedAnimation(Tail3Animation(), true);
            _tail4 = new KeyframedAnimation(Tail4Animation(), true);
            _tail5 = new KeyframedAnimation(Tail5Animation(), true);
            _tail6 = new KeyframedAnimation(Tail6Animation(), true);
            _tail7 = new KeyframedAnimation(Tail7Animation(), true);
            _tail8 = new KeyframedAnimation(Tail8Animation(), true);
        }

        private void TailUpdate(TimeSpan elapsed)
        {
            _tail1.Update(elapsed);
            _tail2.Update(elapsed);
            _tail3.Update(elapsed);
            _tail4.Update(elapsed);
            _tail5.Update(elapsed);
            _tail6.Update(elapsed);
            _tail7.Update(elapsed);
            _tail8.Update(elapsed);
        }

        private void TailAnimation()
        {
            _boneTransforms[35] = Matrix.CreateTranslation(_tail1.Position) * _skinningData.BindPose[35];
            _boneTransforms[36] = Matrix.CreateTranslation(_tail2.Position) * _skinningData.BindPose[36];
            _boneTransforms[37] = Matrix.CreateTranslation(_tail3.Position) * _skinningData.BindPose[37];
            _boneTransforms[38] = Matrix.CreateTranslation(_tail4.Position) * _skinningData.BindPose[38];
            _boneTransforms[39] = Matrix.CreateTranslation(_tail5.Position) * _skinningData.BindPose[39];
            _boneTransforms[40] = Matrix.CreateTranslation(_tail6.Position) * _skinningData.BindPose[40];
            _boneTransforms[41] = Matrix.CreateTranslation(_tail7.Position) * _skinningData.BindPose[41];
            _boneTransforms[42] = Matrix.CreateTranslation(_tail8.Position) * _skinningData.BindPose[42];
        }

        private List<AnimationFrame> Tail1Animation()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0, 0.2f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(2)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0, -0.2f), Vector3.Zero, TimeSpan.FromSeconds(3)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(4)));
            frames.Add(new AnimationFrame(new Vector3(0.5f, 0, 0.2f), Vector3.Zero, TimeSpan.FromSeconds(5)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(6)));
            frames.Add(new AnimationFrame(new Vector3(-0.5f, 0, -0.2f), Vector3.Zero, TimeSpan.FromSeconds(7)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(8)));
            return frames;
        }

        private List<AnimationFrame> Tail2Animation()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0, -0.25f, 0), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(2)));
            frames.Add(new AnimationFrame(new Vector3(0, -0.25f, 0), Vector3.Zero, TimeSpan.FromSeconds(3)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(4)));
            frames.Add(new AnimationFrame(new Vector3(0, -0.25f, 0), Vector3.Zero, TimeSpan.FromSeconds(5)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(6)));
            return frames;
        }

        private List<AnimationFrame> Tail3Animation()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(-0.25f, 0, 0.1f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(2)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0, -0.15f), Vector3.Zero, TimeSpan.FromSeconds(3)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(4)));
            frames.Add(new AnimationFrame(new Vector3(-0.25f, 0, 0.1f), Vector3.Zero, TimeSpan.FromSeconds(5)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(6)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0, -0.15f), Vector3.Zero, TimeSpan.FromSeconds(7)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(8)));
            return frames;
        }

        private List<AnimationFrame> Tail4Animation()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(-0.01f, -0.25f, 0.05f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(2)));
            frames.Add(new AnimationFrame(new Vector3(-0.01f, -0.25f, -0.05f), Vector3.Zero, TimeSpan.FromSeconds(3)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(4)));
            frames.Add(new AnimationFrame(new Vector3(-0.01f, -0.25f, 0.05f), Vector3.Zero, TimeSpan.FromSeconds(5)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(6)));
            frames.Add(new AnimationFrame(new Vector3(-0.01f, -0.25f, -0.05f), Vector3.Zero, TimeSpan.FromSeconds(7)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(8)));

            return frames;
        }

        private List<AnimationFrame> Tail5Animation()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(-0.25f, 0, 0.15f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(2)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0, -0.15f), Vector3.Zero, TimeSpan.FromSeconds(3)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(4)));
            frames.Add(new AnimationFrame(new Vector3(-0.25f, 0, 0.15f), Vector3.Zero, TimeSpan.FromSeconds(5)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(6)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0, -0.15f), Vector3.Zero, TimeSpan.FromSeconds(7)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(8)));
            return frames;
        }

        private List<AnimationFrame> Tail6Animation()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0, -0.25f, 0), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(2)));
            frames.Add(new AnimationFrame(new Vector3(0, -0.25f, 0), Vector3.Zero, TimeSpan.FromSeconds(3)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(4)));
            frames.Add(new AnimationFrame(new Vector3(0, -0.25f, 0), Vector3.Zero, TimeSpan.FromSeconds(5)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(6)));
            return frames;
        }

        private List<AnimationFrame> Tail7Animation()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0, -0.25f), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(2)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0, 0.25f), Vector3.Zero, TimeSpan.FromSeconds(3)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(4)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0, -0.25f), Vector3.Zero, TimeSpan.FromSeconds(5)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(6)));
            frames.Add(new AnimationFrame(new Vector3(0.25f, 0, 0.25f), Vector3.Zero, TimeSpan.FromSeconds(7)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(8)));
            return frames;
        }

        private List<AnimationFrame> Tail8Animation()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(0)));
            frames.Add(new AnimationFrame(new Vector3(0, -0.25f, 0), Vector3.Zero, TimeSpan.FromSeconds(1)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(2)));
            frames.Add(new AnimationFrame(new Vector3(0, -0.25f, 0), Vector3.Zero, TimeSpan.FromSeconds(3)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(4)));
            frames.Add(new AnimationFrame(new Vector3(0, -0.25f, 0), Vector3.Zero, TimeSpan.FromSeconds(5)));
            frames.Add(new AnimationFrame(new Vector3(0, 0, 0), Vector3.Zero, TimeSpan.FromSeconds(6)));
            return frames;
        }
        #endregion Tail
    }
}
