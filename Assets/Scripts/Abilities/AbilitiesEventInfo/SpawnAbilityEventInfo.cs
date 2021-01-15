namespace PositionerDemo
{
    public class SpawnAbilityEventInfo : AbilityEventInfo
    {                       
        public Tile spawnTile { get; set; } // EN QUE TILE SPAWNEO
        public UNITTYPE spawnUnitType; // QUE UNIDAD SPAWNEO ... unit type????
        public Player spawnerPlayer; // QUIEN SPAWNEO ID
        public int spawnIndexID;
        public SpawnAbilityEventInfo(Player spawnerPlayer, UNITTYPE spawnUnitType, Tile spawnTile, int spawnIndexID)
        {
            this.spawnerPlayer = spawnerPlayer;
            this.spawnUnitType = spawnUnitType;
            this.spawnTile = spawnTile;
            this.spawnIndexID = spawnIndexID;
        }

    }
}
