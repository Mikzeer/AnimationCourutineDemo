namespace MikzeerGame.Animotion
{
    #region ANIMATION INFORMATION

    public class MoveAnimationInformation : AnimationInformation
    {
        private const string START_PARAMETER_STRING = "Move";
        private const string END_PARAMETER_STRING = "Idlle";
        private const string ANIMATION_STATE_NAME = ".Move";
        private const string ANIMATOR_LAYER = "Base Layer";
        private const int ANIMATOR_LAYER_INDEX = 0;

        public MoveAnimationInformation() : base(START_PARAMETER_STRING, END_PARAMETER_STRING, ANIMATION_STATE_NAME)
        {
        }
    }

    #endregion
}