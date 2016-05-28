using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SkinnedModel
{
    public class Keyframe
    {
        // Indeks ko�ci kt�r� animuje ramka.
        [ContentSerializer]
        public int Bone { get; private set; }

        // Czas ramki.
        [ContentSerializer]
        public TimeSpan Time { get; private set; }

        // Macierz transformacji dla ko�ci ramki.
        [ContentSerializer]
        public Matrix Transform { get; private set; }

        public Keyframe(int bone, TimeSpan time, Matrix transform)
        {
            Bone = bone;
            Time = time;
            Transform = transform;
        }

        private Keyframe(){}
    }
}
