using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;


namespace SkinnedModel
{
    public class AnimationClip
    {
        // Czas trwania całej animacji.
        [ContentSerializer]
        public TimeSpan Duration { get; private set; }

        // Lista wszystkich ramek, dla wszystkich kości, posortowana według czasu.
        [ContentSerializer]
        public List<Keyframe> Keyframes { get; private set; }

        public AnimationClip(TimeSpan duration, List<Keyframe> keyframes)
        {
            Keyframes = keyframes;
            Duration = duration;
        }

        private AnimationClip() { }
    }
}
