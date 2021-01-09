using CommandPatternActions;
using UnityEngine;

namespace PositionerDemo
{
    public class SpawnManager
    {
        SpawnManagerUI spawnManagerUI;
        int spawnIndexID = 0;

        public SpawnManager(SpawnManagerUI spawnManagerUI)
        {
            this.spawnManagerUI = spawnManagerUI;
        }

        public void OnTrySpawn(Tile TileObject, Player player)
        {
            // SI ESTOY EN EL JUEGO NORMAL SPAWNEO NORMALMENTE
            // SI ESTOY EN EL JUEGO ONLINE ENTONCES EL SPAWN SE VA A ENCARGAR EL SERVER
            // YA QUE NO SOLO LO TENGO QUE HACER INTERNAMENTE, SINO QUE TAMBIEN LO TIENE QUE VER REFLEJADO 
            // EL OTRO JUGADOR

            // SI YA TENEMOS GUARDADA EL SPAWN Y ES UN SAPWN VALIDO, ENTONCES DEBERIAMOS PASARLE LA SPAWNEVENTINFO
            // AL METODO Spawn YA QUE SERIA LO MISMO... POR QUE EN EL SpawnEventInfo TENEMOS Player/UnitType/Tile
            // HASTA SERIA MEJOR, YA QUE SI ENTREMEDIO ME CAMBIARON LA TILE, O EL JUGADOR O EL TIPO DE LA UNIDAD,
            // ESTA SE VA A TERMINAR SPAWNEANDO EN OTRO LADO, O CON OTRO JUGADOR, O CON OTRO TIPO DE RANGO DE UNIDAD

            // El SpawnEventInfo tambien podria tener una List<Motion>... esas motions las agregaria cada uno de 
            // los modifiers que se le aplicaron a la unidad, y se ejecutarian en algun de la animacion del spawneo
            // o en el mismo momento que empieza indicando todas las modificaciones.
            // Cada AbilityModifier en si se encargaria de llenar la lista del SpawnEventInfo.List<Motions> ya que cada uno
            // sabria que animacion aplicar al momento de activar un efecto, al igual que el sonido.

            // 6- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            //    SINO CHEQUEO TODO NORMALMENTE
            // QUIEN QUIERE SPAWNEAR, Y EN DONDE QUIERE SPAWNEAR
            // SI EL PLAYER ES VALIDO Y ES SU TURNO
            // Y SI EL LUGAR PARA SPAWNEAR ES UN LUGAR VALIDO
            // ENTONCES EL SERVER TE DICE SI, PODES SPAWNEAR
            if (TileObject == null)
            {
                Debug.Log("No Tile Object");
                return;
            }


            if (!IsLegalSpawn(TileObject, player))
            {
                Debug.Log("Ilegal Spawn");
                return;
            }
            if (player.Abilities.ContainsKey(ABILITYTYPE.SPAWN) == false)
            {
                Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
                return;
            }
            SpawnAbility spw = (SpawnAbility)player.Abilities[ABILITYTYPE.SPAWN];
            if (spw == null)
            {
                Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
                return;
            }
            //spw.Set(TileObject);
            SpawnAbilityEventInfo spwInf = new SpawnAbilityEventInfo(player, UNITTYPE.X, TileObject);
            spw.SetRequireGameData(spwInf);
            if (spw.OnTryExecute() == false)
            {
                Debug.Log("ERROR EN TRY EXECUTE DE LA HABILIDAD ");
                return;
            }
            spw.Perform();
            if (spw.actionStatus == ABILITYEXECUTIONSTATUS.CANCELED)
            {
                Debug.Log("SPAWN ABILITY CANCELADA");
                return;
            }
            Spawn(TileObject, player, spawnIndexID);
        }

        public bool IsLegalSpawn(Tile TileObject, Player player)
        {
            return true;
        }

        public void Spawn(Tile TileObject, Player player, int spawnIndexID)
        {
            if (TileObject.IsOccupied())
            {
                CombineSpawn(TileObject, player, spawnIndexID);
            }
            else
            {
                NormalSpawn(TileObject, player, spawnIndexID);
            }
        }

        public void NormalSpawn(Tile TileObject, Player player, int spawnIndexID)
        {
            Kimboko kimboko = GetNewKimboko(player, spawnIndexID);
            GameObject goKimboko = spawnManagerUI.GetKimbokoPrefab();

            kimboko.SetGameObject(goKimboko);

            ISpawnCommand spawnCommand = new ISpawnCommand(TileObject, player, kimboko);
            Invoker.AddNewCommand(spawnCommand);

            Vector3 spawnPosition = TileObject.GetRealWorldLocation();
            Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
            InvokerMotion.AddNewMotion(normalSpawnMotion);
        }

        public void CombineSpawn(Tile TileObject, Player player, int spawnIndexID)
        {
            if (TileObject.IsOccupied())
            {
                if (TileObject.GetOccupier().OccupierType == OCUPPIERTYPE.UNIT)
                {
                    Kimboko unit = (Kimboko)TileObject.GetOccupier();

                    if (unit.OwnerPlayerID != player.PlayerID)
                    {
                        // SPAWN COMBINABLE SI SE PUEDE....
                    }
                }
            }
        }

        public Kimboko GetNewKimboko(Player player, int spawnIndexID)
        {
            // SPAWN NORMAL
            KimbokoXFactory kimbokoXFac = new KimbokoXFactory();
            Kimboko kimboko = kimbokoXFac.CreateKimboko(spawnIndexID, player);
            return kimboko;
        }
    }
}
