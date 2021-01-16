using System.Collections.Generic;
using UnityEngine;

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

        public Motion CombineSpawn(Vector3 tileObjectRealWorldLocation, GameObject goKimbok)
        {
            List<Motion> motionsSpawnCombine = new List<Motion>();
            List<Configurable> configureAnimotion = new List<Configurable>();

            // ACA TENEMOS QUE VER SI HAY MAS DE UNA UNIDAD EN LA TILE
            // MIENTRAS BAJA EL CRANE // UNIDADES EN LA TILE SE REPOSICIONAN A LOS COSTADOS
            // ACA DEBERIA TENER UNA REFERENCIA AL COMBINE MANAGER

            // ENTONCES CREAMOS UNA COMBINE MOTION CON ESTOS TRES 
            // A - MOVEMOS A LOS KIMBOKOS QUE OCUPEN LA TILE A LOS COSTADOS
            // B - Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
            // C - REPOSICIONAMOS A LOS KIMBOKO




            return null;
        }
    }

    public class CombineManagerUI : MonoBehaviour
    {
        public Motion MoveToSquarePosition(List<Kimboko> kimbokos, Vector3 actualPosition)
        {
            List<Motion> motionsSpawnCombine = new List<Motion>();
            UnitMovePositioner movePositioner = new UnitMovePositioner(4f);
            // 2 - DESPUES ME DEBERIA FIJAR SI TENGO ENEMIGOS EN LA QUE SELECCIONE, Y SI TENGO DEBERIA AGREGAR EL COMANDO DE REPOSICIONARLOS
            if (kimbokos.Count > 0)
            {
                // AHORA TENGO QUE GENERAR EL CUADRADO DE POSICIONES PARA CADA UNA
                Vector3[] squarePositions = movePositioner.GetPositions(actualPosition, POSITIONTYPE.SQUARE);

                // SONIDO DE CUANDO SE MUEVEN PARA REPOSICIONARSE
                if (GameSoundManager.Instance != null)
                {
                    if (GameSoundManager.Instance.audioSource != null)
                    {
                        Motion motionMoveSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[4], false, true);
                        motionsSpawnCombine.Add(motionMoveSound);
                    }
                }
                // deberia recorrer la lista de unidades, y generar una move comand para posicionarse en el lugar que tiene cada una del cuadrado
                for (int i = 0; i < kimbokos.Count; i++)
                {
                    List<Motion> motionsCombineSpawnMoveSquare = new List<Motion>();
                    Motion motionMove = new MoveMotion(this, kimbokos[i].goAnimContainer.GetAnimator(), 1);
                    motionsCombineSpawnMoveSquare.Add(motionMove);

                    List<Motion> motionsCombineSpawnStopMoveSquare = new List<Motion>();
                    Motion motionTwMove = new MoveTweenMotion(this, kimbokos[i].goAnimContainer.GetTransform(), 1, squarePositions[i], 1);
                    Motion motionIdlle = new IdlleMotion(this, kimbokos[i].goAnimContainer.GetAnimator(), 2, true);
                    motionsCombineSpawnStopMoveSquare.Add(motionTwMove);
                    motionsCombineSpawnStopMoveSquare.Add(motionIdlle);

                    // esto solo lo hago para detener el sonido de los pasos
                    if (kimbokos.Count - 1 == i)
                    {
                        List<Configurable> configurables = new List<Configurable>();

                        if (GameSoundManager.Instance != null)
                        {
                            if (GameSoundManager.Instance.audioSource != null)
                            {
                                AudioSourceGenericContainer audioContainer = new AudioSourceGenericContainer(GameSoundManager.Instance.audioSource);
                                StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform> stopSoundConfigureAnimotion = new StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform>(audioContainer, 2);
                                configurables.Add(stopSoundConfigureAnimotion);
                            }
                        }
                        CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare, configurables);
                        motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                    }
                    else
                    {
                        CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                        motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                    }
                    CombineMotion combinSquarePositionMotion = new CombineMotion(this, 1, motionsCombineSpawnMoveSquare);
                    motionsSpawnCombine.Add(combinSquarePositionMotion);
                }
            }
            CombineMotion repositionInSquareMotion = new CombineMotion(this, 1, motionsSpawnCombine);
            return repositionInSquareMotion;
        }

        public Motion RearangePositionAfterCombineMotion(List<Kimboko> kimbokos, Vector3 actualPosition)
        {
            List<Motion> motionsSpawnCombine = new List<Motion>();
            UnitMovePositioner movePositioner = new UnitMovePositioner(4f);
            //  - DESPUES ME DEBERIA FIJAR SI TENGO ENEMIGOS EN LA QUE SELECCIONE, Y SI TENGO DEBERIA AGREGAR EL COMANDO DE REPOSICIONARLOS
            if (kimbokos.Count > 1)
            {
                POSITIONTYPE positionTypeToRearrange = movePositioner.GetPositionType(kimbokos.Count);
                Vector3[] finalRearrangePositions = movePositioner.GetPositions(actualPosition, positionTypeToRearrange);
                // SONIDO DE CUANDO SE MUEVEN PARA REPOSICIONARSE
                if (GameSoundManager.Instance != null)
                {
                    if (GameSoundManager.Instance.audioSource != null)
                    {
                        Motion motionMoveSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[4], false, true);
                        motionsSpawnCombine.Add(motionMoveSound);
                    }
                }
                // deberia recorrer la lista de unidades, y generar una move comand para posicionarse en el lugar que tiene cada una del cuadrado
                for (int i = 0; i < kimbokos.Count; i++)
                {
                    List<Motion> motionsCombineSpawnMoveSquare = new List<Motion>();
                    int shortNameHash = Animator.StringToHash("Base Layer" + ".Idlle");

                    Motion motionMove = new MoveMotion(this, kimbokos[i].goAnimContainer.GetAnimator(), 1, false, shortNameHash);
                    motionsCombineSpawnMoveSquare.Add(motionMove);

                    List<Motion> motionsCombineSpawnStopMoveSquare = new List<Motion>();
                    Motion motionTwMove = new MoveTweenMotion(this, kimbokos[i].goAnimContainer.GetTransform(), 1, finalRearrangePositions[i], 1);
                    motionsCombineSpawnStopMoveSquare.Add(motionTwMove);
                    Motion motionIdlle = new IdlleMotion(this, kimbokos[i].goAnimContainer.GetAnimator(), 2, true);
                    motionsCombineSpawnStopMoveSquare.Add(motionIdlle);

                    // esto solo lo hago para detener el sonido de los pasos
                    if (kimbokos.Count - 1 == i)
                    {
                        List<Configurable> configurables = new List<Configurable>();

                        if (GameSoundManager.Instance != null)
                        {
                            if (GameSoundManager.Instance.audioSource != null)
                            {
                                AudioSourceGenericContainer audioContainer = new AudioSourceGenericContainer(GameSoundManager.Instance.audioSource);
                                StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform> stopSoundConfigureAnimotion = new StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform>(audioContainer, 2);
                                configurables.Add(stopSoundConfigureAnimotion);
                            }
                        }
                        CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare, configurables);
                        motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                    }
                    else
                    {
                        CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsCombineSpawnStopMoveSquare);
                        motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                    }
                    CombineMotion combinSquarePositionMotion = new CombineMotion(this, 6, motionsCombineSpawnMoveSquare);
                    motionsSpawnCombine.Add(combinSquarePositionMotion);
                }
            }
            CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawnCombine);
            return combinMoveMotion;
        }
    }
}
