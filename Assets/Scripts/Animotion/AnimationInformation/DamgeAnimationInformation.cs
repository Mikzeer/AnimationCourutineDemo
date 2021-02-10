namespace MikzeerGame.Animotion
{
    #region ANIMATION INFORMATION

    public class DamgeAnimationInformation : AnimationInformation
    {
        private const string START_PARAMETER_STRING = "Damage";
        private const string END_PARAMETER_STRING = "Idlle";
        private const string ANIMATION_STATE_NAME = ".Damage";
        private const string ANIMATOR_LAYER = "Base Layer";
        private const int ANIMATOR_LAYER_INDEX = 0;

        public DamgeAnimationInformation() : base(START_PARAMETER_STRING, END_PARAMETER_STRING, ANIMATION_STATE_NAME)
        {
        }
    }

    #endregion
}