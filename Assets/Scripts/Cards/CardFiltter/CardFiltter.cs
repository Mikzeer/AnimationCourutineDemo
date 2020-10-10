namespace PositionerDemo
{
    [System.Serializable]
    public class CardFiltter
    {
        public virtual ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            return cardTarget;
            /*  REQUERIMIENTOS DE USO => PUNTOS OSCUROS / ALGUNA UNIDAD ALIADA-ENEMIGA EN CAMPO 
             *  TILE VACIA EN EL CAMPO / CARTAS EN EL MAZO /
             *  QUE SE NECESITA PARA PODER UTILIZAR ESA CARTA, ESTO ES COMO LA FASE 
             *  DE CHEQUEO PARA VER SI SE PUEDE UTILIZAR O NO
             *  EN EL CASO DE LAS AUTOMATICAS SI ESTA FASE NO SE PASA, ENTONCES LA 
             *  CARTA SE DESCARTA AL CEMENTERIO AUTOMATICAMENTE
             */
            //* FILTTER => UNA CLASE QUE TIENE UNA FUNCION PARA CHEQUEAR SI A 
            // UN OBJETIVO AL CUAL SE LE VA A APLICAR LA CARD O UN EFECTO CUMPLE CIERTO CRITERIO
            // *ISENEMY / ISALLY / < 10HP / ISSTUNNED / SELF / REQUERIES TARGET


            // CASI TODOS VAN A CHEQUEAR SI LA UNIDAD ES AMIGA / ENEMIGA
            // UNIT / PLAYER / BOARDOBJECT
            // CheckFriendOrFoe
            // Y GENERALMENTE SI ES ENEMIGA VAN A CHEQUEAR SI ESTA EN LA BASE O NO PARA SABER SI SE PUEDE USAR
            // UNIT / BOARDOBJECT
            // CheckOccupierTyleTipe

            // ALGUNAS CARTAS VAN A PODER USARSE IGUAL EN LA BASE ENEMIGA, ASI QUE VA A SER UN FILTTER MENOS EN TODO CASO
            // CheckFriendOrFoe => UNIT / PLAYER / BOARDOBJECT
            // CheckOccupierTyleTipe => UNIT / BOARDOBJECT
            // CheckOccupierPosition => UNIT / BOARDOBJECT
            // CheckCardAmount => PLAYER
            // Check Stats => UNIT / PLAYER / BOARDOBJECT
            // CHEQUEAR EL ESTADO DE UN STAT. EJ: ATK > 0 // ACTUALHP < MAXHP // ATKRANGE < 3 // MOVERANGE < 3
            // CheckStatsBuffID => UNIT / PLAYER / BOARDOBJECT
            // Check Action => UNIT / PLAYER / BOARDOBJECT
            // CheckActionModifierID => UNIT / PLAYER / BOARDOBJECT
            // CheckUnitBackSpace => UNIT / BOARDOBJECT

        }
    }

}
