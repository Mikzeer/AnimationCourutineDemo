using UnityEngine;
using AbilitySelectionUI;
namespace PositionerDemo
{
    public class AbilitySelectionManagerUI : MonoBehaviour
    {
        public void HighlightTile(Tile tileObject, HIGHLIGHTUITYPE highlightType)
        {
            switch (highlightType)
            {
                case HIGHLIGHTUITYPE.SPAWN:
                    HighlightSelection(tileObject, Color.blue);
                    break;
                case HIGHLIGHTUITYPE.ATTACK:
                    break;
                case HIGHLIGHTUITYPE.MOVE:
                    HighlightSelection(tileObject, Color.cyan);
                    break;
                case HIGHLIGHTUITYPE.COMBINE:
                    HighlightSelection(tileObject, Color.green);
                    break;
                case HIGHLIGHTUITYPE.DECOMBINE:
                    break;
                case HIGHLIGHTUITYPE.BUFF:
                    break;
                case HIGHLIGHTUITYPE.NERF:
                    break;
                case HIGHLIGHTUITYPE.NEUTRAL:
                    break;
                case HIGHLIGHTUITYPE.NONE:
                    HighlightSelection(tileObject, Color.white);
                    break;
                default:
                    break;
            }
        }

        private void HighlightSelection(Tile tileObject, Color color)
        {
            tileObject.goAnimContainer.GetGameObject().GetComponent<SpriteRenderer>().color = color;
        }
    }
}