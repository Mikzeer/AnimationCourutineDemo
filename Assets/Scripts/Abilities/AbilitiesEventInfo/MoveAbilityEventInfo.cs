using UnityEngine;

namespace PositionerDemo
{
    public class MoveAbilityEventInfo : AbilityEventInfo
    {
        public IOcuppy moveOccupy;// QUIEN SE MOVIO ID
        public Tile fromTile;// DESDE QUE POSX/POSY Vector2 posInicial
        public Tile endPosition;// HASTA QUE POSX/POSY Vector2 posFinal
        public int distance; // DISTANCIA QUE SE MOVIO == Vector2.Distance(posInicial, posFinal);

        public MoveAbilityEventInfo(IOcuppy moveOccupy, Tile fromTile, Tile endPosition)
        {
            Position startPosition = new Position(fromTile.position.posX, fromTile.position.posY);
            Position finishPosition = new Position(endPosition.position.posX, endPosition.position.posY);
            this.moveOccupy = moveOccupy;
            this.fromTile = fromTile;
            this.endPosition = endPosition;
            Vector2 startPost = new Vector2(startPosition.posX, startPosition.posY);
            Vector2 endPost = new Vector2(finishPosition.posX, finishPosition.posY);
            distance = Mathf.FloorToInt(Vector2.Distance(startPost, endPost));
        }
    }
}
