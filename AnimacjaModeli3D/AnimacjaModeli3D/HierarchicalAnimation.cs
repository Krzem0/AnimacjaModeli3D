using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimacjaModeli3D
{
    class HierarchicalAnimation
    {
        #region private fields
        private Model _humvee;
        private KeyframedAnimation _keyframedAnimation;
        private Matrix[] _transformsOrigin;
        private Matrix[] _transforms;
        private float scale;
        private float _wheelTotalRot;
        private float _wheelRotSpeed;
        private float _turnDelta;
        private Vector3 _prevPos;
        private Vector3 _prevRot;
        private Vector3 _upDnDamperFront;
        private Vector3 _upDnDamperRear;
        private bool _countUPFront;
        private bool _countUPRear;
        private Random random;
        #endregion

        public HierarchicalAnimation(Model humveeModel)
        {
            _humvee = humveeModel;
            _keyframedAnimation = new KeyframedAnimation(Animation0(), true);
            _transforms = new Matrix[_humvee.Bones.Count];
            _humvee.CopyBoneTransformsTo(_transforms);
            _transformsOrigin = new Matrix[_humvee.Bones.Count];
            _humvee.CopyBoneTransformsTo(_transformsOrigin);
            scale = 0.2f;
            _prevPos = _prevRot = Vector3.Zero;
            random = new Random();
        }

        public void Update(TimeSpan elapsedTime)
        {
            _keyframedAnimation.Update(elapsedTime);
            DamperMove();
            WheelRotation(elapsedTime);
        }

        public void Draw(TimeSpan elapsedTime, Matrix View, Matrix Projection)
        {
            Vector3 position = _keyframedAnimation.Position;
            Vector3 rotation = _keyframedAnimation.Rotation;
            Matrix world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
                           Matrix.CreateTranslation(position);
            _humvee.Draw(world, View, Projection);
        }

        private static List<AnimationFrame> Animation8()
        {
            List<AnimationFrame> animationFrames = new List<AnimationFrame>();
            animationFrames.Add(new AnimationFrame(new Vector3(-200, -50, 0),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(0)));
            animationFrames.Add(new AnimationFrame(new Vector3(-200, -50, -60),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(2)));
            animationFrames.Add(new AnimationFrame(new Vector3(-120, -50, -140),
                new Vector3(0, MathHelper.ToRadians(90), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(4)));
            animationFrames.Add(new AnimationFrame(new Vector3(-40, -50, -60),
                new Vector3(0, MathHelper.ToRadians(35), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(8)));
            animationFrames.Add(new AnimationFrame(new Vector3(40, -50, 60),
                new Vector3(0, MathHelper.ToRadians(35), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(10.4))); //zmiana prędkości!!!
            animationFrames.Add(new AnimationFrame(new Vector3(120, -50, 140),
                new Vector3(0, MathHelper.ToRadians(90), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(12.4)));
            animationFrames.Add(new AnimationFrame(new Vector3(200, -50, 60),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(14.4)));
            animationFrames.Add(new AnimationFrame(new Vector3(200, -50, -60),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(16.4)));
            animationFrames.Add(new AnimationFrame(new Vector3(120, -50, -140),
                new Vector3(0, MathHelper.ToRadians(270), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(18.4)));
            animationFrames.Add(new AnimationFrame(new Vector3(40, -50, -60),
                new Vector3(0, MathHelper.ToRadians(320), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(20.4)));
            animationFrames.Add(new AnimationFrame(new Vector3(-40, -50, 60),
                new Vector3(0, MathHelper.ToRadians(320), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(21.8))); //zmiana prędkości!!!
            animationFrames.Add(new AnimationFrame(new Vector3(-120, -50, 140),
                new Vector3(0, MathHelper.ToRadians(270), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(23.8)));
            animationFrames.Add(new AnimationFrame(new Vector3(-200, -50, 60),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(25.8)));
            animationFrames.Add(new AnimationFrame(new Vector3(-200, -50, 0),
                new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(27.8)));
            return animationFrames;
        }

        private static List<AnimationFrame> Animation0()
        {
            List<AnimationFrame> animationFrames = new List<AnimationFrame>();
            animationFrames.Add(new AnimationFrame(new Vector3(0, -50, -100),
               new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(0)));
            animationFrames.Add(new AnimationFrame(new Vector3(100, -50, -200),
               new Vector3(0, MathHelper.ToRadians(90), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(2)));
            animationFrames.Add(new AnimationFrame(new Vector3(200, -50, -100),
               new Vector3(0, MathHelper.ToRadians(0), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(4)));
            animationFrames.Add(new AnimationFrame(new Vector3(100, -50, 0),
               new Vector3(0, MathHelper.ToRadians(-90), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(6)));
            animationFrames.Add(new AnimationFrame(new Vector3(-100, -50, 0),
               new Vector3(0, MathHelper.ToRadians(-90), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(7.5)));
            animationFrames.Add(new AnimationFrame(new Vector3(-200, -50, 100),
               new Vector3(0, MathHelper.ToRadians(0), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(9.5)));
            animationFrames.Add(new AnimationFrame(new Vector3(-100, -50, 200),
               new Vector3(0, MathHelper.ToRadians(90), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(11.5)));
            animationFrames.Add(new AnimationFrame(new Vector3(0, -50, 100),
               new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(13.5)));
            animationFrames.Add(new AnimationFrame(new Vector3(0, -50, -100),
               new Vector3(0, MathHelper.ToRadians(180), MathHelper.ToRadians(0)), TimeSpan.FromSeconds(15)));
            return animationFrames;
        }

        private void DamperMove()
        {
            if (_upDnDamperFront.Y > 5) _countUPFront = false;
            if (_upDnDamperFront.Y < -5) _countUPFront = true;
            if (_countUPFront) _upDnDamperFront.Y += (float)random.NextDouble();
            else _upDnDamperFront.Y -= (float)random.NextDouble();
            _transforms[7] = Matrix.CreateTranslation(_upDnDamperFront) * _transformsOrigin[7];

            if (_upDnDamperRear.Y > 5) _countUPRear = false;
            if (_upDnDamperRear.Y < -5) _countUPRear = true;
            if (_countUPRear) _upDnDamperRear.Y += (float)random.NextDouble();
            else _upDnDamperRear.Y -= (float)random.NextDouble();
            _transforms[10] = Matrix.CreateTranslation(_upDnDamperRear) * _transformsOrigin[10];
        }

        private void WheelRotation(TimeSpan elapsedTime)
        {
            CalculateRotationWheel(elapsedTime);

            _transforms[8] = Matrix.CreateFromYawPitchRoll(_turnDelta, _wheelTotalRot, 0) * _transformsOrigin[8];
            _transforms[9] = Matrix.CreateFromYawPitchRoll(_turnDelta, _wheelTotalRot, 0) * _transformsOrigin[9];
            _transforms[11] = Matrix.CreateRotationX(_wheelTotalRot) * _transformsOrigin[11];
            _transforms[12] = Matrix.CreateRotationX(_wheelTotalRot) * _transformsOrigin[12];
            _humvee.CopyBoneTransformsFrom(_transforms);
        }

        private void CalculateRotationWheel(TimeSpan elapsedTime)
        {
            _wheelRotSpeed = Vector3.Distance(_prevPos, _keyframedAnimation.Position) / (float)elapsedTime.TotalSeconds;
            _wheelTotalRot += MathHelper.ToRadians(_wheelRotSpeed / 10);
            _wheelTotalRot = MathHelper.WrapAngle(_wheelTotalRot);
            _prevPos = _keyframedAnimation.Position;

            _turnDelta = (_keyframedAnimation.Rotation.Y - _prevRot.Y) * 20;
            _prevRot = _keyframedAnimation.Rotation;
        }
    }
}
