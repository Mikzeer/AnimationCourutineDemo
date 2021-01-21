using PositionerDemo;
using UnityEngine;

namespace CommandPatternActions
{
    public class ISpawnCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }

        SpawnAbilityEventInfo spawnInfo;
        GameObject kimbokoGO;
        Kimboko kimboko;
        IGame game;

        public ISpawnCommand(SpawnAbilityEventInfo spawnInfo, GameObject kimbokoGO, IGame game)
        {
            logInsert = true;
            this.spawnInfo = spawnInfo;
            this.kimbokoGO = kimbokoGO;
            this.game = game;
        }

        public void Execute()
        {
            kimboko = GetNewKimboko(spawnInfo);
            kimboko.SetGoAnimContainer(new GameObjectAnimatorContainer(kimbokoGO, kimbokoGO.GetComponent<Animator>()));
            spawnInfo.spawnTile.OcupyTile(kimboko);
            spawnInfo.spawnerPlayer.AddUnit(kimboko);

            PositionerDemo.Position pos = new PositionerDemo.Position(spawnInfo.spawnTile.position.posX, spawnInfo.spawnTile.position.posY);
            game.board2DManager.AddModifyOccupierPosition(kimboko, pos);
            kimboko.SetPosition(pos);
            executionState = COMMANDEXECUTINSTATE.FINISH;
        }

        public void Unexecute()
        {
            spawnInfo.spawnTile.Vacate();
            spawnInfo.spawnerPlayer.RemoveUnit(kimboko);
            kimboko.goAnimContainer.DestroyPrefab();

            game.board2DManager.RemoveOccupierPosition(kimboko);
        }

        private Kimboko GetNewKimboko(SpawnAbilityEventInfo spawnInfo)
        {
            Kimboko kimboko = null;
            switch (spawnInfo.spawnUnitType)
            {
                case UNITTYPE.X:
                    KimbokoXFactory kimbokoXFac = new KimbokoXFactory();
                    kimboko = kimbokoXFac.CreateKimboko(spawnInfo.spawnIndexID, spawnInfo.spawnerPlayer);
                    break;
                case UNITTYPE.Y:
                    KimbokoYFactory kimbokoYFac = new KimbokoYFactory();
                    kimboko = kimbokoYFac.CreateKimboko(spawnInfo.spawnIndexID, spawnInfo.spawnerPlayer);
                    break;
                case UNITTYPE.Z:
                    KimbokoZFactory kimbokoZFac = new KimbokoZFactory();
                    kimboko = kimbokoZFac.CreateKimboko(spawnInfo.spawnIndexID, spawnInfo.spawnerPlayer);
                    break;
                case UNITTYPE.COMBINE:
                    break;
                case UNITTYPE.FUSION:
                    break;
                default:
                    break;
            }

            return kimboko;
        }
    }
}