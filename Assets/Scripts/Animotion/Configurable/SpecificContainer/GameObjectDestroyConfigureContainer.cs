using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public class GameObjectDestroyConfigureContainer : ConfigureContainer
    {
        // DestroyGOConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
        public GameObject gameObjectToDestroy;
        public GameObjectDestroyConfigureContainer(GameObject gameObjectToDestroy)
        {
            this.gameObjectToDestroy = gameObjectToDestroy;
        }

        public void Execute()
        {
            GameObject.Destroy(gameObjectToDestroy);
        }
    }

    #endregion
}