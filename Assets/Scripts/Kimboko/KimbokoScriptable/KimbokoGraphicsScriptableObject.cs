using System.Collections.Generic;
using UnityEngine;


namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "New Kimboko", menuName = "Kimboko/New Kimboko Graphics")]
    public class KimbokoGraphicsScriptableObject : ScriptableObject
    {
        [SerializeField] private Sprite actualSprite;
        [SerializeField] private Dictionary<int, List<Sprite>> animationSprites;

        public Sprite ActualSprite { get { return actualSprite; } protected set { actualSprite = value; } }
        public Dictionary<int, List<Sprite>> AnimationSprites { get { return animationSprites; } protected set { animationSprites = value; } }
    }

}