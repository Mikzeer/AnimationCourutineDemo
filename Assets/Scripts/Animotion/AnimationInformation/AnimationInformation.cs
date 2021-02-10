namespace MikzeerGame.Animotion
{
    #region ANIMATION INFORMATION

    public abstract class AnimationInformation
    {
        public string startParameter { get; private set; }
        public string endParameter { get; private set; }
        public string animationStateName { get; private set; }
        public string animatorLayer { get; private set; }
        public int animatorLayerIndex { get; private set; }
        public string fullAnimationPath { get; private set; }
        public AnimationInformation(string startPrm, string endPrm, string animationStateName, string animatorLayer = "Base Layer", int animatorLayerIndex = 0)
        {
            this.startParameter = startPrm;
            this.endParameter = endPrm;
            this.animationStateName = animationStateName;
            this.animatorLayer = animatorLayer;
            this.animatorLayerIndex = animatorLayerIndex;
            fullAnimationPath = animatorLayer + animationStateName;
        }
    }

    #endregion
}