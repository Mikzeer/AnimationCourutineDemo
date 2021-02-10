using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public class GameObjectSetActiveConfigureContainer : ConfigureContainer
    {
        // SetActiveConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
        public GameObject gameObjectToSet;
        public bool isActive;
        public GameObjectSetActiveConfigureContainer(GameObject gameObjectToDestroy, bool isActive)
        {
            this.gameObjectToSet = gameObjectToDestroy;
            this.isActive = isActive;
        }

        public void Execute()
        {
            gameObjectToSet.SetActive(isActive);
        }
    }

    #endregion
}