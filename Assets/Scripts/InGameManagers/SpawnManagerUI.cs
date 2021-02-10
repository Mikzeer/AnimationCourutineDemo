using System.Collections.Generic;
using UnityEngine;
using MikzeerGame.Animotion;
namespace PositionerDemo
{
    public class SpawnManagerUI : MonoBehaviour
    {
        int craneTweenSpeedVelocity = 1;
        int kimbokoTweenSpeedVelocity = 2;
        [SerializeField] private GameObject Crane = default;
        [SerializeField] private Transform CraneEnd = default;
        [SerializeField] private GameObject kimbokoPrefab = default;
        Animator craneAnimator;
        float craneStartPositionY;
        private void Start()
        {
            craneAnimator = Crane.GetComponent<Animator>();
            craneStartPositionY = Crane.transform.position.y;
        }

        public GameObject GetKimbokoPrefab()
        {
            GameObject goKimbok = Instantiate(kimbokoPrefab);
            return goKimbok;
        }

        public Motion NormalSpawn(Vector3 tileObjectRealWorldLocation, GameObject goKimbok)
        {
            List<Motion> motionsSpawn = new List<Motion>();
            List<Configurable> configureAnimotion = new List<Configurable>();

            // POSICION INICIAL Y FINAL DE LA GRUA PARA TWEENEAR
            Vector3 craneStartPosition;
            Vector3 craneEndPostion;

            //B TWEEN DESDE UNA POSICION ELEVADA SOBRE LA TILE DONDE SE INDICO SPAWNEAR HASTA MAS ABAJO ASI SE VE DESDE ARRIBA EN EL TABLERO SOBRE LA TILE
            craneStartPosition = new Vector3(tileObjectRealWorldLocation.x, craneStartPositionY, 0);
            ConfigurePositionAssistant cnfPosAssist = new ConfigurePositionAssistant(craneStartPosition);
            TransformPositioConfigureAnimotion<Transform, ConfigurePositionAssistant> CranePositionConfigAnimotion =
            new TransformPositioConfigureAnimotion<Transform, ConfigurePositionAssistant>(Crane.transform, cnfPosAssist, 0, true);
            configureAnimotion.Add(CranePositionConfigAnimotion);

            craneEndPostion = new Vector3(tileObjectRealWorldLocation.x, Helper.GetCameraTopBorderYWorldPostion().y);

            //A CRANE//GRUA SET ACTIVE = TRUE // INSTANCIAMOS KIMBOKO SET ACTIVE FALSE
            Crane.SetActive(true);

            goKimbok.transform.position = CraneEnd.position;
            // ACTIVAMOS AL KIMBOKO SINO NO PUEDE OBTENER EL CURRENT ANIMATOR STATE INFO
            goKimbok.SetActive(true);

            // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
            Motion motionTweenMove = new MoveTweenMotion(this, Crane.transform, 1, craneEndPostion, craneTweenSpeedVelocity);
            motionsSpawn.Add(motionTweenMove);

            ////C ANIMATION CRANESPAWNING
            Motion motionCraneSpawn = new SpawnMotion(this, craneAnimator, 2);
            motionsSpawn.Add(motionCraneSpawn);

            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    Motion motionSpawnSound = new SoundMotion(this, 2, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[3], false);
                    motionsSpawn.Add(motionSpawnSound);
                }
            }

            KimbokoPositioConfigureAnimotion<Transform, Transform> KimbokoPositionConfigAnimotion = 
                new KimbokoPositioConfigureAnimotion<Transform, Transform>(goKimbok.transform, CraneEnd, 3);
            configureAnimotion.Add(KimbokoPositionConfigAnimotion);
            SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = 
                new SetActiveConfigureAnimotion<Transform, Transform>(goKimbok.transform, 3);
            configureAnimotion.Add(KimbokoActiveConfigAnimotion);

            ////E TWEEN DESDE LA PUNTA DEL CRANE HASTA EL PISO, DE LA MISMA DURACION QUE LA ANIMACION DE SPAWN
            Motion motionKimbokoTweenMove = new MoveTweenMotion(this, goKimbok.transform, 4, tileObjectRealWorldLocation, kimbokoTweenSpeedVelocity);
            motionsSpawn.Add(motionKimbokoTweenMove);
            //G TWEEN DE LA CRANE PARA QUE SALGA DEL MAPA
            Motion motionTweenBackCraneMove = new MoveTweenMotion(this, Crane.transform, 5, craneStartPosition, craneTweenSpeedVelocity);
            motionsSpawn.Add(motionTweenBackCraneMove);

            // FINISH //
            KimbokoIdlleConfigureAnimotion<Animator, Transform> kimbokoIdlleConfigureAnimotion = 
                new KimbokoIdlleConfigureAnimotion<Animator, Transform>(goKimbok.GetComponent<Animator>(), 6);
            configureAnimotion.Add(kimbokoIdlleConfigureAnimotion);
            KimbokoIdlleConfigureAnimotion<Animator, Transform> craneIdlleConfigureAnimotion = 
                new KimbokoIdlleConfigureAnimotion<Animator, Transform>(craneAnimator, 6);
            configureAnimotion.Add(craneIdlleConfigureAnimotion);

            CombineMotion spawnMotion = new CombineMotion(this, 1, motionsSpawn, configureAnimotion);

            // LO DESACTIVO PARA QUE NO MOLESTE EN ESCENA ANTES DE "SPAWNEARSE"
            goKimbok.SetActive(false);

            return spawnMotion;
        }

        public Motion CombineSpawn(Vector3 tileObjectRealWorldLocation, GameObject goKimbok, List<GameObject> kimbokos, GameMachine game)
        {
            List<Motion> motionsSpawnCombine = new List<Motion>();
            // A - MOVEMOS A LOS KIMBOKOS QUE OCUPEN LA TILE A LOS COSTADOS
            motionsSpawnCombine.Add(game.combineManagerUI.MoveToSquarePosition(kimbokos, tileObjectRealWorldLocation));
            // B - Motion normalSpawnMotion
            motionsSpawnCombine.Add(NormalSpawn(tileObjectRealWorldLocation, goKimbok));
            // C - REPOSICIONAMOS A TODOS LOS KIMBOKO
            // AGREGO EL KIMBOKO SPAWNEADO A LA LISTA
            kimbokos.Add(goKimbok);
            motionsSpawnCombine.Add(game.combineManagerUI.RearangePositionAfterCombineMotion(kimbokos, tileObjectRealWorldLocation, 2));

            CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawnCombine);
            return combinMoveMotion;
        }

        #region ANIMOTION

        public Animotion NormalSpawnAnimotion(Vector3 tileObjectRealWorldLocation, GameObject goKimbok)
        {
            List<Animotion> motionsSpawn = new List<Animotion>();
            List<ConfigurableMotion> configureAnimotion = new List<ConfigurableMotion>();

            //A CRANE//GRUA SET ACTIVE = TRUE
            Crane.SetActive(true);

            // POSICION INICIAL Y FINAL DE LA GRUA PARA TWEENEAR
            Vector3 craneStartPosition;
            Vector3 craneEndPostion;

            //B TWEEN DESDE UNA POSICION ELEVADA SOBRE LA TILE DONDE SE INDICO SPAWNEAR HASTA MAS ABAJO ASI SE VE DESDE ARRIBA EN EL TABLERO SOBRE LA TILE
            craneStartPosition = new Vector3(tileObjectRealWorldLocation.x, craneStartPositionY, 0);
            PositionVector3ConfigureContainer posVectContainer = new PositionVector3ConfigureContainer(craneStartPosition, Crane.transform);
            ConfigureAnimotion posVectConfigure = new ConfigureAnimotion(posVectContainer, 1);
            configureAnimotion.Add(posVectConfigure);

            craneEndPostion = new Vector3(tileObjectRealWorldLocation.x, Helper.GetCameraTopBorderYWorldPostion().y);

            Vector3 craneSpawnPosition = CraneEnd.position;
            // ACTIVAMOS AL KIMBOKO SINO NO PUEDE OBTENER EL CURRENT ANIMATOR STATE INFO
            goKimbok.SetActive(true);

            goKimbok.transform.position = craneSpawnPosition;

            // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
            Animotion motionTweenMove = new MoveTweenAnimotion(this, 2, Crane.transform,  craneEndPostion, craneTweenSpeedVelocity);
            motionsSpawn.Add(motionTweenMove);

            ////C ANIMATION CRANESPAWNING
            Animotion motionCraneSpawn = new SpawnAnimatedAnimotion(this, 3, craneAnimator);
            motionsSpawn.Add(motionCraneSpawn);

            if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
            {
                AudioSourceParameter audioParameter = new AudioSourceParameter(true); // false
                Animotion motionSpawnSound = new SoundAnimotion(this, 3, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[3], audioParameter);
                motionsSpawn.Add(motionSpawnSound);
            }

            PositionTransformConfigureContainer posTransformContainer = new PositionTransformConfigureContainer(goKimbok.transform, CraneEnd);
            ConfigureAnimotion posTransformConfigure = new ConfigureAnimotion(posTransformContainer, 4);
            configureAnimotion.Add(posTransformConfigure);

            GameObjectSetActiveConfigureContainer setActiveContainer = new GameObjectSetActiveConfigureContainer(goKimbok, true);
            ConfigureAnimotion setActiveConfigure = new ConfigureAnimotion(setActiveContainer, 4, true);
            configureAnimotion.Add(setActiveConfigure);


            ////E TWEEN DESDE LA PUNTA DEL CRANE HASTA EL PISO, DE LA MISMA DURACION QUE LA ANIMACION DE SPAWN
            Animotion motionKimbokoTweenMove = new MoveTweenAnimotion(this, 5, goKimbok.transform,  tileObjectRealWorldLocation, kimbokoTweenSpeedVelocity);
            motionsSpawn.Add(motionKimbokoTweenMove);
            //G TWEEN DE LA CRANE PARA QUE SALGA DEL MAPA
            Animotion motionTweenBackCraneMove = new MoveTweenAnimotion(this, 6, Crane.transform,  craneStartPosition, craneTweenSpeedVelocity);
            motionsSpawn.Add(motionTweenBackCraneMove);

            // FINISH //



            SetTriggerAnimationCongifureContainer setIdlleKimbokoContainer = new SetTriggerAnimationCongifureContainer(goKimbok.GetComponent<Animator>(), "Idlle");
            ConfigureAnimotion idlleKimbokoConfigure = new ConfigureAnimotion(setIdlleKimbokoContainer, 7, true);
            configureAnimotion.Add(idlleKimbokoConfigure);
            SetTriggerAnimationCongifureContainer setIdlleCraneContainer = new SetTriggerAnimationCongifureContainer(craneAnimator, "Idlle");
            ConfigureAnimotion idlleCraneConfigure = new ConfigureAnimotion(setIdlleCraneContainer, 7, true);
            configureAnimotion.Add(idlleCraneConfigure);
            //Animotion motionIdlleKimboko = new IdlleAnimatedAnimotion(this, 7, goKimbok.GetComponent<Animator>());
            //motionsSpawn.Add(motionIdlleKimboko);

            //Animotion motionIdlleCrane = new IdlleAnimatedAnimotion(this, 7, craneAnimator);
            //motionsSpawn.Add(motionIdlleCrane);

            CombineAnimotion spawnMotion = new CombineAnimotion(this, 1, motionsSpawn, configureAnimotion);

            // LO DESACTIVO PARA QUE NO MOLESTE EN ESCENA ANTES DE "SPAWNEARSE"
            goKimbok.SetActive(false);

            return spawnMotion;
        }

        public Animotion CombineSpawnAnimotion(Vector3 tileObjectRealWorldLocation, GameObject goKimbok, List<GameObject> kimbokos, GameMachine game)
        {
            List<Animotion> motionsSpawnCombine = new List<Animotion>();
            // A - MOVEMOS A LOS KIMBOKOS QUE OCUPEN LA TILE A LOS COSTADOS
            motionsSpawnCombine.Add(game.combineManagerUI.MoveToSquarePositionAnimotion(kimbokos, tileObjectRealWorldLocation));
            // B - Motion normalSpawnMotion
            motionsSpawnCombine.Add(NormalSpawnAnimotion(tileObjectRealWorldLocation, goKimbok));
            // C - REPOSICIONAMOS A TODOS LOS KIMBOKO
            // AGREGO EL KIMBOKO SPAWNEADO A LA LISTA
            kimbokos.Add(goKimbok);
            motionsSpawnCombine.Add(game.combineManagerUI.RearangePositionAfterCombineAnimotion(kimbokos, tileObjectRealWorldLocation, 2));

            CombineAnimotion combinMoveMotion = new CombineAnimotion(this, 1, motionsSpawnCombine);
            return combinMoveMotion;
        }

        #endregion
    }
}
