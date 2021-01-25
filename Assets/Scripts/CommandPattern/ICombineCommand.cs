using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPatternActions
{
    public class ICombineCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }

        IGame game;
        Kimboko finalCombinedKimboko;
        CombineAbilityEventInfo combInfo;
        Tile combineTile;
        Tile fromTile;
        public ICombineCommand(CombineAbilityEventInfo combInfo, IGame game)
        {
            logInsert = true;
            this.combInfo = combInfo;
            this.game = game;
        }

        public void Execute()
        {
            // Kimboko aCombinar
            // TENEMOS QUE ELIMINAR LA UNIDAD ACTUAL DE LA LISTA DE UNIDADES DEL PLAYER QUE SE VA A COMBINAR
            combInfo.player.RemoveUnit(combInfo.kimbokoToCombine);

            // TENEMOS QUE SACARLO DE SU TILE Y DESOCUPARLA
            fromTile = game.board2DManager.GetUnitPosition(combInfo.kimbokoToCombine);
            fromTile.Vacate();
            game.board2DManager.RemoveOccupierPosition(combInfo.kimbokoToCombine);

            // Kimboko combinador
            // TENEMOS QUE ELIMINAR LA UNIDAD ACTUAL DE LA LISTA DE UNIDADES DEL PLAYER
            combInfo.player.RemoveUnit(combInfo.combiner);

            // DEBERIAMOS DESOCUPAR LA TILE DONDE ESTA EL KIMBOKO ACTUAL YA QUE VA A PASAR A SER UNO NUEVO
            // DONDE VOY A COMBINAR
            combineTile = game.board2DManager.GetUnitPosition(combInfo.combiner);
            combineTile.Vacate();

            // DEBERIAMOS DESOCUPAR EL BOARD MANAGER ACTUAL 
            game.board2DManager.RemoveOccupierPosition(combInfo.combiner);

            // Kimboko combinado
            // CREAMOS EL KIMBOKO COMBINADO CON SUS NUEVOS STATS
            finalCombinedKimboko = GetCombinedKimboko(combInfo);

            // DEBEMOS OCUPAR LA TILE CON LA NUEVA UNIDAD COMBINADA
            combineTile.OcupyTile(finalCombinedKimboko);
            // DEBEMOS AGREGARLE AL PLAYER EL NUEVO KIMBOKO COMBINADO
            combInfo.player.AddUnit(finalCombinedKimboko);
            // DEBEMOS AGREGARLE AL BOARD MANAGER LA NUEVA UNIDAD COMBINADA
            PositionerDemo.Position pos = new PositionerDemo.Position(combineTile.position.posX, combineTile.position.posY);
            game.board2DManager.AddModifyOccupierPosition(finalCombinedKimboko, pos);
            finalCombinedKimboko.SetPosition(pos);

            executionState = COMMANDEXECUTINSTATE.FINISH;
        }

        public void Unexecute()
        {
            combineTile.Vacate();
            combInfo.player.RemoveUnit(finalCombinedKimboko);
            finalCombinedKimboko.goAnimContainer.DestroyPrefab();
            game.board2DManager.RemoveOccupierPosition(finalCombinedKimboko);

            combineTile.OcupyTile(combInfo.combiner);
            combInfo.player.AddUnit(combInfo.combiner);
            PositionerDemo.Position pos = new PositionerDemo.Position(combineTile.position.posX, combineTile.position.posY);
            game.board2DManager.AddModifyOccupierPosition(combInfo.combiner, pos);
            combInfo.combiner.SetPosition(pos);

            fromTile.OcupyTile(combInfo.kimbokoToCombine); ;
            combInfo.player.AddUnit(combInfo.kimbokoToCombine);
            PositionerDemo.Position fromPos = new PositionerDemo.Position(fromTile.position.posX, fromTile.position.posY);
            game.board2DManager.AddModifyOccupierPosition(combInfo.kimbokoToCombine, fromPos);
            combInfo.kimbokoToCombine.SetPosition(fromPos);
        }

        private Kimboko GetCombinedKimboko(CombineAbilityEventInfo cmbInfo)
        {
            List<Kimboko> kimbokosTocombine = new List<Kimboko>();
            kimbokosTocombine.Add(cmbInfo.combiner);
            kimbokosTocombine.Add(cmbInfo.kimbokoToCombine);
            Kimboko kimboko = null;
            KimbokoCombineFactory kimbokoCombFac = new KimbokoCombineFactory(kimbokosTocombine);
            kimboko = kimbokoCombFac.CreateKimboko(cmbInfo.IndexID, cmbInfo.player);
            return kimboko;
        }
    }
}