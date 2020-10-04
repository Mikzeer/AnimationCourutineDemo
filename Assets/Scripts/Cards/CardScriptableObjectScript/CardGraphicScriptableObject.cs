using UnityEngine;

namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "New Card Graphic", menuName = "Cards/Graphics/ New Card Graphic")]
    public class CardGraphicScriptableObject : ScriptableObject
    {
        [SerializeField] private Sprite image;
        [SerializeField] private Sprite miniatureImage;

        public Sprite Image { get { return image; } protected set { image = value; } }
        public Sprite MiniatureImage { get { return miniatureImage; } protected set { miniatureImage = value; } }
    }


}