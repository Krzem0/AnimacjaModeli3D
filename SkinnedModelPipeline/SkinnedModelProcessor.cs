using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using SkinnedModel;

namespace SkinnedModelPipeline
{
    [ContentProcessor]
    [DisplayName("Skinned Model Processor")]
    public class SkinnedModelProcessor : ModelProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            // Znajduje szkielet w modelu.
            BoneContent skeleton = MeshHelper.FindSkeleton(input);

            // Wczytuje domyœlne ustawienia koœci i hierarchiê.
            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);
            
            // Deklaracja list
            List<Matrix> bindPose = new List<Matrix>();
            List<Matrix> inverseBindPose = new List<Matrix>();
            List<int> skeletonHierarchy = new List<int>();
            
            // Za³adowanie powy¿szych list
            foreach (BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }

            // Konwersja animacji do formatu dictionary
            Dictionary<string,AnimationClip> animationClips;
            animationClips = ProcessAnimations(skeleton.Animations, bones);

            ModelContent modelContent = base.Process(input, context);

            modelContent.Tag = new SkinningData(animationClips, bindPose, inverseBindPose, skeletonHierarchy);
            return modelContent;
        }

        private Dictionary<string, AnimationClip> ProcessAnimations(AnimationContentDictionary animations, IList<BoneContent> boneContents)
        {
            Dictionary<string,int> boneMap = new Dictionary<string, int>();

            for (int i = 0; i < boneContents.Count; i++)
            {
                boneMap.Add(boneContents[i].Name,i);
            }

            Dictionary<string,AnimationClip> animationClips = new Dictionary<string, AnimationClip>();

            foreach (KeyValuePair<string, AnimationContent> animation in animations)
            {
                AnimationClip processed = ProcessAnimation(animation.Value, boneMap);
                animationClips.Add(animation.Key,processed);
            }
            return animationClips;
        }

        private AnimationClip ProcessAnimation(AnimationContent value, Dictionary<string, int> boneMap)
        {
            List<Keyframe> keyframes = new List<Keyframe>();

            foreach (KeyValuePair<string, AnimationChannel> channel in value.Channels)
            {
                int boneIndex = boneMap[channel.Key];
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    keyframes.Add(new Keyframe(boneIndex,keyframe.Time,keyframe.Transform));
                }
            }

            keyframes.Sort(CompareKeyframeTimes);
            return new AnimationClip(value.Duration,keyframes);
        }

        static int CompareKeyframeTimes(Keyframe a, Keyframe b)
        {
            return a.Time.CompareTo(b.Time);
        }
    }
}