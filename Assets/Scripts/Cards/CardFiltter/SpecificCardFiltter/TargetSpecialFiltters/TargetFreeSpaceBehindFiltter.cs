namespace PositionerDemo
{
    public class TargetFreeSpaceBehindFiltter : CardFiltter
    {
        CARDTARGETTYPE cardTargetType = CARDTARGETTYPE.BATTLEFIELD;
        Tile[,] GridArray;
        private const int FILTTER_ID = 43;
        public TargetFreeSpaceBehindFiltter(Tile[,] GridArray) : base(FILTTER_ID)
        {
            this.GridArray = GridArray;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != cardTargetType) return null;
            Tile tile = (Tile)cardTarget; // POSICION DE LA UNIT EN EL CAMPO DE BATALLA
            if (tile == null) return null;

            CardFiltter targetTeamEnemyFiltter = new TargetEnemyTeamFiltter();
            CardFiltter targetTeamAllyFiltter = new TargetAllyTeamFiltter();

            // SI ES ALIADO TENEMOS QUE CHEQUEAR LA POSICION EN X - 1
            // SI ES ENEMIGO TENEMOS QUE CHEQUEAR LA POSICION EN X + 1
            int positionToCheckX = (int)tile.GetGridPosition().x;
            if (targetTeamEnemyFiltter.CheckTarget(cardTarget) == null)
            {
                positionToCheckX += 1;
            }
            else if (targetTeamAllyFiltter.CheckTarget(cardTarget) == null)
            {
                positionToCheckX -= 1;
            }

            if (GridArray == null)
            {
                return null;
            }

            // ACA CHEQUEAMOS QUE EN NUESTRO GRID ARRAY EXISTA ESA TILE
            // NO ESTAMOS VERIFICANDO QUE NO LO MANDEMOS A UNA TILE DEL NEXO... JEJE
            // ESO YA SE DEBERIA HABER CHEQUEADO ANTES
            if (GridArray.GetLength(0) <= positionToCheckX)
            {
                // SI EXISTE ENTONCES VAMOS A VERIFICAR SI ESTA OCUPADA
                if (GridArray[positionToCheckX, tile.PosY].IsOccupied() == true)
                {
                    return null;
                }
            }

            return base.CheckTarget(cardTarget);
        }
    }
}