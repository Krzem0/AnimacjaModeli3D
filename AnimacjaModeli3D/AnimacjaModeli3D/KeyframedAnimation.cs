using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AnimacjaModeli3D
{
    public class KeyframedAnimation
    {
        private List<AnimationFrame> _frameList = new List<AnimationFrame>();
        private bool _loop;
        private TimeSpan _elapsedTime = TimeSpan.FromSeconds(0);

        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }

        public KeyframedAnimation(List<AnimationFrame> frames, bool loop)
        {
            _frameList = frames;
            _loop = loop;
            Position = _frameList[0].Position;
            Rotation = _frameList[0].Rotation;
        }

        public void Update(TimeSpan elapsedTime)
        {
            _elapsedTime += elapsedTime;

            TimeSpan totalTime = _elapsedTime;
            TimeSpan end = _frameList[_frameList.Count - 1].Time;

            if (_loop)
            {
                while (totalTime > end)
                {
                    totalTime -= end;
                }
            }
            else
            {
                Position = _frameList[_frameList.Count - 1].Position;
                Rotation = _frameList[_frameList.Count - 1].Position;
                return;
            }

            int i = 0;

            // Znajduje indeks bierzącej ramki
            while (_frameList[i + 1].Time < totalTime) i++;

            // Czas bierzącej ramki
            totalTime -= _frameList[i].Time;

            // Procentowa wartość upływu ramki
            float amount;

            amount = (float)(totalTime.TotalSeconds /
                     (_frameList[i + 1].Time - _frameList[i].Time).TotalSeconds);

            // Interpolacja liniowa
            //Position = Vector3.Lerp(_frameList[i].Position, _frameList[i + 1].Position, amount);

            // Interpolacja krzywych
            Position = CatmullRom3D(
                _frameList[Wrap(i - 1, _frameList.Count - 1)].Position,
                _frameList[Wrap(i, _frameList.Count - 1)].Position,
                _frameList[Wrap(i + 1, _frameList.Count - 1)].Position,
                _frameList[Wrap(i + 2, _frameList.Count - 1)].Position,
                amount);

            Rotation = Vector3.Lerp(_frameList[i].Rotation, _frameList[i + 1].Rotation, amount);
        }

        private Vector3 CatmullRom3D(Vector3 vector1, Vector3 vector2, Vector3 vector3, Vector3 vector4, float amount)
        {
            return new Vector3(
                MathHelper.CatmullRom(vector1.X, vector2.X, vector3.X, vector4.X, amount),
                MathHelper.CatmullRom(vector1.Y, vector2.Y, vector3.Y, vector4.Y, amount),
                MathHelper.CatmullRom(vector1.Z, vector2.Z, vector3.Z, vector4.Z, amount));
        }

        private int Wrap(int value, int max)
        {
            if (value > max) value -= max;
            if (value < 0) value += max;
            return value;
        }
    }
}
