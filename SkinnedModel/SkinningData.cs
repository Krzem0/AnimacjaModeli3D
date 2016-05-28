using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SkinnedModel
{
    public class SkinningData
    {
        // Pobiera kolekcje klipów animacji posortowaną po nazwie.
        [ContentSerializer]
        public Dictionary<string, AnimationClip> AnimationClips { get; private set; }

        // Lista przechowująca domyślne ustawienia macierzy kości,
        // relatywnie do ich węzłów przodków.
        [ContentSerializer]
        public List<Matrix> BindPose { get; private set; }

        // Macierze wierzchołków znajdujących się w przestrzeni animacji 
        // danej kości, dla wszystkich kości w modelu.
        [ContentSerializer]
        public List<Matrix> InverseBindPose { get; private set; }

        // Dla każdej kości w modelu przechowuje numer jej przodka.
        [ContentSerializer]
        public List<int> SkeletonHierarchy { get; private set; }

        public SkinningData(Dictionary<string, AnimationClip> animationClips,
            List<Matrix> bindPose, List<Matrix> inverseBintPose, List<int> skeletonHierarchy)
        {
            AnimationClips = animationClips;
            BindPose = bindPose;
            InverseBindPose = inverseBintPose;
            SkeletonHierarchy = skeletonHierarchy;
        }

        private SkinningData() { }
    }
}
