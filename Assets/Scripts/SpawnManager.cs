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
            // 6- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            //    SINO CHEQUEO TODO NORMALMENTE
            // QUIEN QUIERE SPAWNEAR, Y EN DONDE QUIERE SPAWNEAR
            // SI EL PLAYER ES VALIDO Y ES SU TURNO
            // Y SI EL LUGAR PARA SPAWNEAR ES UN LUGAR VALIDO
            // ENTONCES EL SERVER TE DICE SI, PODES SPAWNEAR
            if (!IsLegalSpawn(TileObject, player))
            {
                Debug.Log("Ilegal Spawn");
                return;
            }

            SpawnAbility spawnAbility = null;
            if (player.Abilities.ContainsKey(0))
            {
                spawnAbility = (SpawnAbility)player.Abilities[0];
            }
            else
            {
                Debug.Log("El Player no tiene la Spawn Ability");
            }

            if (spawnAbility != null)
            {
                spawnAbility.Set(TileObject);
            }
            else
            {
                Debug.Log("La ID de la Spawn Ability puede estar mal no funciono el casteo");
                return;
            }

            if (spawnAbility.OnTryExecute() == false)
            {
                Debug.Log("Fallo en el On Try Execte de la Spawn Ability");
                return;
            }
            else
            {
                spawnAbility.Perform();
            }

            if (spawnAbility.actionStatus == ABILITYEXECUTIONSTATUS.CANCELED)
            {
                Debug.Log("Se Cancelo desde la Ability la Spawn Ability");
                return;
            }

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

            Spawn(TileObject, player, spawnIndexID);
        }

        public bool IsLegalSpawn(Tile TileObject, Player player)
        {
            return true;
        }

        public void Spawn(Tile TileObject, Player player, int spawnIndexID)
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

        public void CombineSpawn(Tile TileObject, Player player)
        {
            if (TileObject.IsOccupied())
            {
                if (TileObject.GetOccupier().OccupierType == OCUPPIERTYPE.UNIT)
                {
                    Kimboko unit = (Kimboko)TileObject.GetOccupier();

                    if (unit.ownerPlayer != player)
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
