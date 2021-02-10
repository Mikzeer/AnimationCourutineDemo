using UnityEngine;

namespace MikzeerGame.Animotion
{
    public class NewAnimotionTester : MonoBehaviour
    {
        public GameObject goToAnimate;
        public Animator goAnimator;
        private AnimotionController animotionController;

        public bool isMoving;

        private void Start()
        {
            animotionController = new AnimotionController();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //AttackMotion attackMotion = new AttackMotion(this, 0, goAnimator);
                //DamageMotion damageMotion = new DamageMotion(this, 0, goAnimator);
                if (isMoving)
                {
                    MoveAnimatedAnimotion moveMotion = new MoveAnimatedAnimotion(this, 0, goAnimator);
                    animotionController.SetUpMotion(moveMotion);
                    animotionController.TryReproduceMotion();
                }
                else
                {
                    IdlleAnimatedAnimotion idlleMotion = new IdlleAnimatedAnimotion(this, 0, goAnimator);
                    animotionController.SetUpMotion(idlleMotion);
                    animotionController.TryReproduceMotion();
                }
            }
        }
    }
}