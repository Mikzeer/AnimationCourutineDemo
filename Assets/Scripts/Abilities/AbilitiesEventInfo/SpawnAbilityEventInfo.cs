namespace PositionerDemo
{
    public class SpawnAbilityEventInfo : AbilityEventInfo
    {                       
        public Tile spawnTile { get; set; } // EN QUE TILE SPAWNEO
        public UNITTYPE spawnUnitType; // QUE UNIDAD SPAWNEO ... unit type????
        public Player spawnerPlayer; // QUIEN SPAWNEO ID

        public SpawnAbilityEventInfo(Player spawnerPlayer, UNITTYPE spawnUnitType, Tile spawnTile)
        {
            this.spawnerPlayer = spawnerPlayer;
            this.spawnUnitType = spawnUnitType;
            this.spawnTile = spawnTile;
        }

    }
}
