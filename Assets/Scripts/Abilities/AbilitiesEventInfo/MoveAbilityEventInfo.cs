using UnityEngine;

namespace PositionerDemo
{
    public class MoveAbilityEventInfo : AbilityEventInfo
    {
        public IOcuppy moveOccupy;// QUIEN SE MOVIO ID
        public Vector2 startPosition;// DESDE QUE POSX/POSY Vector2 posInicial
        public Vector2 endPosition;// HASTA QUE POSX/POSY Vector2 posFinal
        public int distance; // DISTANCIA QUE SE MOVIO == Vector2.Distance(posInicial, posFinal);

        public MoveAbilityEventInfo(IOcuppy moveOccupy, Vector2 startPosition, Vector2 endPosition)
        {
            this.moveOccupy = moveOccupy;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.distance = Mathf.FloorToInt(Vector2.Distance(startPosition, endPosition));
        }
    }
}
