using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AnimacjaModeli3D
{
    public class AnimationFrame
    {
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public TimeSpan Time { get; private set; }

        public AnimationFrame(Vector3 position, Vector3 rotation, TimeSpan time)
        {
            Position = position;
            Rotation = rotation;
            Time = time;
        }
    }
}
