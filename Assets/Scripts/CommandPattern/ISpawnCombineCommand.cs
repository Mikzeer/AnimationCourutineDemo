using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPatternActions
{
    public class ISpawnCombineCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }

        SpawnAbilityEventInfo spawnInfo;
        IGame game;
        Kimboko spawnedKimboko;
        Kimboko combiner;
        Kimboko finalCombinedKimboko;

        public ISpawnCombineCommand(Kimboko spawnedKimboko, SpawnAbilityEventInfo spawnInfo, Kimboko combiner, IGame game)
        {
            logInsert = true;
            this.combiner = combiner;
            this.game = game;
            this.spawnInfo = spawnInfo;
            this.spawnedKimboko = spawnedKimboko;
        }

        public void Execute()
        {
            // Kimboko aCombinar

            // Kimboko combinador
            // TENEMOS QUE ELIMINAR LA UNIDAD ACTUAL DE LA LISTA DE UNIDADES DEL PLAYER
            spawnInfo.spawnerPlayer.RemoveUnit(combiner);
            // DEBERIAMOS DESOCUPAR LA TILE DONDE ESTA EL KIMBOKO ACTUAL YA QUE VA A PASAR A SER UNO NUEVO
            spawnInfo.spawnTile.Vacate();
            // DEBERIAMOS DESOCUPAR EL BOARD MANAGER ACTUAL 
            game.board2DManager.RemoveOccupierPosition(combiner);

            // Kimboko combinado
            // CREAMOS EL KIMBOKO COMBINADO CON SUS NUEVOS STATS
            CombineAbilityEventInfo cmbInfo = new CombineAbilityEventInfo(combiner, spawnedKimboko, spawnInfo.spawnerPlayer, spawnInfo.spawnIndexID);
            finalCombinedKimboko = GetCombinedKimboko(cmbInfo);

            // DEBEMOS OCUPAR LA TILE CON LA NUEVA UNIDAD COMBINADA
            spawnInfo.spawnTile.OcupyTile(finalCombinedKimboko);
            // DEBEMOS AGREGARLE AL PLAYER EL NUEVO KIMBOKO COMBINADO
            spawnInfo.spawnerPlayer.AddUnit(finalCombinedKimboko);
            // DEBEMOS AGREGARLE AL BOARD MANAGER LA NUEVA UNIDAD COMBINADA
            PositionerDemo.Position pos = new PositionerDemo.Position(spawnInfo.spawnTile.position.posX, spawnInfo.spawnTile.position.posY);
            game.board2DManager.AddModifyOccupierPosition(finalCombinedKimboko, pos);
            finalCombinedKimboko.SetPosition(pos);

            executionState = COMMANDEXECUTINSTATE.FINISH;
        }

        public void Unexecute()
        {
            spawnInfo.spawnTile.Vacate();
            spawnInfo.spawnerPlayer.RemoveUnit(finalCombinedKimboko);
            finalCombinedKimboko.goAnimContainer.DestroyPrefab();
            game.board2DManager.RemoveOccupierPosition(finalCombinedKimboko);
            spawnedKimboko.goAnimContainer.DestroyPrefab();


            spawnInfo.spawnTile.OcupyTile(combiner);
            spawnInfo.spawnerPlayer.AddUnit(combiner);
            PositionerDemo.Position pos = new PositionerDemo.Position(spawnInfo.spawnTile.position.posX, spawnInfo.spawnTile.position.posY);
            game.board2DManager.AddModifyOccupierPosition(combiner, pos);
            combiner.SetPosition(pos);
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