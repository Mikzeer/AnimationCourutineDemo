using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class SpawnController
    {
        MotionController motionControllerSpawn = new MotionController();
        int craneTweenSpeedVelocity = 1;
        int kimbokoTweenSpeedVelocity = 1;
        int spawnIndexID = 0;

        public void OnTrySpawn(Tile TileObject, Player player)
        {
            if (motionControllerSpawn == null)
            {
                Debug.Log("No Motion Controller");
                return;
            }

            if (motionControllerSpawn.IsPerforming == true)
            {
                Debug.Log("Is Performing animation");
                return;
            }

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
            if (player.Abilities.ContainsKey(ABILITYTYPE.SPAWN))
            {

                spawnAbility = (SpawnAbility)player.Abilities[ABILITYTYPE.SPAWN];
            }
            else
            {
                Debug.Log("El Player no tiene la Spawn Ability");
            }

            if (spawnAbility != null)
            {
                SpawnAbilityEventInfo spwInf = new SpawnAbilityEventInfo(player, UNITTYPE.X, TileObject, spawnIndexID);
                spawnAbility.SetRequireGameData(spwInf);
                //spawnAbility.Set(TileObject);
            }
            else
            {
                Debug.Log("La ID de la Spawn Ability puede estar mal no funciono el casteo");
                return;
            }

            if (spawnAbility.CanIExecute() == false)
            {
                Debug.Log("Fallo en el On Try Execte de la Spawn Ability");
                return;
            }
            else
            {
                //spawnAbility.Perform();
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

            Spawn(TileObject, player);
        }

        public void Spawn(Tile TileObject, Player player)
        {
            Vector3 spawnPosition = TileObject.GetRealWorldLocation();

            //KimbokoGenericFactory kimbokoFac = new KimbokoGenericFactory();
            //Kimboko kimboko = kimbokoFac.CreateKimboko(spawnIndexID, player, KimSO);
            KimbokoXFactory kimbokoXFac = new KimbokoXFactory();
            Kimboko kimboko = kimbokoXFac.CreateKimboko(spawnIndexID, player);
            spawnIndexID++;

            GameObject goKimbok = GameObject.Instantiate(GameCreator.Instance.kimbokoPrefab);
            kimboko.SetGoAnimContainer(new GameObjectAnimatorContainer(goKimbok, goKimbok.GetComponent<Animator>()));

            if (TileObject.IsOccupied())
            {
                if (TileObject.GetOcuppy().OccupierType == OCUPPIERTYPE.UNIT)
                {
                    Kimboko unit = (Kimboko)TileObject.GetOcuppy();

                    if (unit.OwnerPlayerID != player.PlayerID)
                    {
                        // SPAWN COMBINABLE SI SE PUEDE....
                    }
                }
            }
            else
            {
                // SPAWN NORMAL
                NormalSpawn(spawnPosition, player, goKimbok);
                TileObject.OcupyTile(kimboko);
                player.AddUnit(kimboko);
            }
        }

        public void NormalSpawn(Vector3 tileObjectRealWorldLocation, Player player, GameObject goKimbok)
        {
            //Tile TileObject = GameCreator.Instance.GetBoard().GetGridObject(spawnPosition);

            List<Motion> motionsSpawn = new List<Motion>();
            List<Configurable> configureAnimotion = new List<Configurable>();

            // POSICION INICIAL Y FINAL DE LA GRUA PARA TWEENEAR
            Vector3 craneStartPosition;
            Vector3 craneEndPostion;
            //B TWEEN DESDE UNA POSICION ELEVADA SOBRE LA TILE DONDE SE INDICO SPAWNEAR HASTA MAS ABAJO ASI SE VE DESDE ARRIBA EN EL TABLERO SOBRE LA TILE
            craneStartPosition = new Vector3(tileObjectRealWorldLocation.x, GameCreator.Instance.Crane.transform.position.y, 0);
            GameCreator.Instance.Crane.transform.position = craneStartPosition;
            craneEndPostion = new Vector3(tileObjectRealWorldLocation.x, Helper.GetCameraTopBorderYWorldPostion().y);

            //A CRANE//GRUA SET ACTIVE = TRUE // INSTANCIAMOS KIMBOKO SET ACTIVE FALSE
            GameCreator.Instance.Crane.SetActive(true);

            goKimbok.transform.position = GameCreator.Instance.CraneEnd.position;
            // ACTIVAMOS AL KIMBOKO SINO NO PUEDE OBTENER EL CURRENT ANIMATOR STATE INFO
            goKimbok.SetActive(true);

            // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
            Motion motionTweenMove = new MoveTweenMotion(GameCreator.Instance, GameCreator.Instance.Crane.transform, 1, craneEndPostion, craneTweenSpeedVelocity);
            motionsSpawn.Add(motionTweenMove);

            ////C ANIMATION CRANESPAWNING
            Motion motionCraneSpawn = new SpawnMotion(GameCreator.Instance, GameCreator.Instance.Crane.GetComponent<Animator>(), 2);
            motionsSpawn.Add(motionCraneSpawn);

            Motion motionSpawnSound = new SoundMotion(GameCreator.Instance, 2, GameCreator.Instance.audioSource, GameCreator.Instance.audioClips[3], false);
            motionsSpawn.Add(motionSpawnSound);

            KimbokoPositioConfigureAnimotion<Transform, Transform> KimbokoPositionConfigAnimotion = new KimbokoPositioConfigureAnimotion<Transform, Transform>(goKimbok.transform, GameCreator.Instance.CraneEnd, 3);
            configureAnimotion.Add(KimbokoPositionConfigAnimotion);
            SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(goKimbok.transform, 3);
            configureAnimotion.Add(KimbokoActiveConfigAnimotion);

            ////E TWEEN DESDE LA PUNTA DEL CRANE HASTA EL PISO, DE LA MISMA DURACION QUE LA ANIMACION DE SPAWN
            Motion motionKimbokoTweenMove = new MoveTweenMotion(GameCreator.Instance, goKimbok.transform, 4, tileObjectRealWorldLocation, kimbokoTweenSpeedVelocity);
            motionsSpawn.Add(motionKimbokoTweenMove);
            //G TWEEN DE LA CRANE PARA QUE SALGA DEL MAPA
            Motion motionTweenBackCraneMove = new MoveTweenMotion(GameCreator.Instance, GameCreator.Instance.Crane.transform, 5, craneStartPosition, craneTweenSpeedVelocity);
            motionsSpawn.Add(motionTweenBackCraneMove);

            // FINISH //
            KimbokoIdlleConfigureAnimotion<Animator, Transform> kimbokoIdlleConfigureAnimotion = new KimbokoIdlleConfigureAnimotion<Animator, Transform>(goKimbok.GetComponent<Animator>(), 6);
            configureAnimotion.Add(kimbokoIdlleConfigureAnimotion);
            KimbokoIdlleConfigureAnimotion<Animator, Transform> craneIdlleConfigureAnimotion = new KimbokoIdlleConfigureAnimotion<Animator, Transform>(GameCreator.Instance.Crane.GetComponent<Animator>(), 6);
            configureAnimotion.Add(craneIdlleConfigureAnimotion);
            AbilityActionStatusConfigureAnimotion<IAbility, Transform> abActionConfigureAnimotion = new AbilityActionStatusConfigureAnimotion<IAbility, Transform>(player.Abilities[ABILITYTYPE.SPAWN], 6);
            configureAnimotion.Add(abActionConfigureAnimotion);

            CombineMotion combinMoveMotion = new CombineMotion(GameCreator.Instance, 1, motionsSpawn, configureAnimotion);

            // LO DESACTIVO PARA QUE NO MOLESTE EN ESCENA ANTES DE "SPAWNEARSE"
            goKimbok.SetActive(false);

            motionControllerSpawn.SetUpMotion(combinMoveMotion);
            motionControllerSpawn.TryReproduceMotion();
        }

        public bool IsLegalSpawn(Tile TileObject, Player player)
        {
            return true;
        }
    }
}
