using UnityEngine;

namespace PositionerDemo
{
    public class GameObjectAnimatorContainer
    {
        private Animator animator;
        private GameObject gameObject;
        public GameObjectAnimatorContainer(GameObject gameObject, Animator animator)
        {
            this.gameObject = gameObject;
            this.animator = animator;
        }
        public Animator GetAnimator()
        {
            return animator;
        }
        public GameObject GetGameObject()
        {
            return gameObject;
        }
        public Transform GetTransform()
        {
            return gameObject.transform;
        }
        public void DestroyPrefab()
        {
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
