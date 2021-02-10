using System.Collections.Generic;
using UnityEngine;
using MikzeerGame.Animotion;
namespace PositionerDemo
{
    public class CombineManagerUI : MonoBehaviour
    {
        public Motion MoveToSquarePosition(List<GameObject> kimbokos, Vector3 actualPosition)
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
                    Animator animatorAux = kimbokos[i].GetComponent<Animator>();


                    List<Motion> motionsCombineSpawnMoveSquare = new List<Motion>();
                    Motion motionMove = new MoveMotion(this, animatorAux, 1);
                    motionsCombineSpawnMoveSquare.Add(motionMove);

                    List<Motion> motionsCombineSpawnStopMoveSquare = new List<Motion>();
                    Motion motionTwMove = new MoveTweenMotion(this, kimbokos[i].transform, 1, squarePositions[i], 1);
                    Motion motionIdlle = new IdlleMotion(this, animatorAux, 2, true);
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
                                StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform> stopSoundConfigureAnimotion 
                                    = new StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform>(audioContainer, 2);
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

        public Motion RearangePositionAfterCombineMotion(List<GameObject> kimbokos, Vector3 actualPosition, int reproductionOrder = 1)
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
                    Animator animatorAux = kimbokos[i].GetComponent<Animator>();
                    // LOS PONGO EN ESTADO MOVE PARA LA ANIMACION
                    Motion motionMove = new MoveMotion(this, animatorAux, 1, false, shortNameHash);
                    //Motion motionMove = new MoveMotion(this, animatorAux, 1);
                    motionsSpawnCombine.Add(motionMove);

                    List<Motion> motionsCombineSpawnStopMoveSquare = new List<Motion>();
                    // LOS MUEVO DESDE DONDE ESTAN HASTA LA POSICION FINAL
                    Motion motionTwMove = new MoveTweenMotion(this, kimbokos[i].transform, 1, finalRearrangePositions[i], 1);
                    motionsCombineSpawnStopMoveSquare.Add(motionTwMove);
                    // LOS PONGO EN ESTADO IDLLE
                    Motion motionIdlle = new IdlleMotion(this, animatorAux, 2, true);
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
                                StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform> stopSoundConfigureAnimotion 
                                    = new StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform>(audioContainer, 2);
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
            CombineMotion combinMoveMotion = new CombineMotion(this, reproductionOrder, motionsSpawnCombine);
            return combinMoveMotion;
        }

        public Motion NormalCombineMotion(Vector3 tileObjectRealWorldLocation, GameObject goKimbok, List<GameObject> kimbokos, GameMachine game)
        {
            List<Motion> motionsSpawnCombine = new List<Motion>();
            // A - MOVEMOS A LOS KIMBOKOS QUE OCUPEN LA TILE A LOS COSTADOS
            motionsSpawnCombine.Add(MoveToSquarePosition(kimbokos, tileObjectRealWorldLocation));

            // B - Motion normal Move Motion
            motionsSpawnCombine.Add(game.moveManagerUI.MoveMotion(goKimbok, tileObjectRealWorldLocation));

            // C - REPOSICIONAMOS A TODOS LOS KIMBOKO
            // AGREGO EL KIMBOKO SPAWNEADO A LA LISTA
            kimbokos.Add(goKimbok);
            motionsSpawnCombine.Add(RearangePositionAfterCombineMotion(kimbokos, tileObjectRealWorldLocation, 2));

            CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawnCombine);
            return combinMoveMotion;
        }

        #region ANIMOTION

        public Animotion MoveToSquarePositionAnimotion(List<GameObject> kimbokos, Vector3 actualPosition)
        {
            List<Animotion> motionsSpawnCombine = new List<Animotion>();
            UnitMovePositioner movePositioner = new UnitMovePositioner(4f);
            // 2 - DESPUES ME DEBERIA FIJAR SI TENGO ENEMIGOS EN LA QUE SELECCIONE, Y SI TENGO DEBERIA AGREGAR EL COMANDO DE REPOSICIONARLOS
            if (kimbokos.Count > 0)
            {
                // AHORA TENGO QUE GENERAR EL CUADRADO DE POSICIONES PARA CADA UNA
                Vector3[] squarePositions = movePositioner.GetPositions(actualPosition, POSITIONTYPE.SQUARE);

                // SONIDO DE CUANDO SE MUEVEN PARA REPOSICIONARSE
                if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
                {
                    AudioSourceParameter audioParameter = new AudioSourceParameter(false, true);
                    Animotion motionMoveSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[4], audioParameter);
                    motionsSpawnCombine.Add(motionMoveSound);
                }

                // deberia recorrer la lista de unidades, y generar una move comand para posicionarse en el lugar que tiene cada una del cuadrado
                for (int i = 0; i < kimbokos.Count; i++)
                {
                    Animator animatorAux = kimbokos[i].GetComponent<Animator>();


                    List<Animotion> motionsCombineSpawnMoveSquare = new List<Animotion>();
                    Animotion motionMove = new MoveAnimatedAnimotion(this, 1, animatorAux);
                    motionsCombineSpawnMoveSquare.Add(motionMove);

                    List<Animotion> motionsCombineSpawnStopMoveSquare = new List<Animotion>();
                    Animotion motionTwMove = new MoveTweenAnimotion(this, 1, kimbokos[i].transform, squarePositions[i], 1);
                    Animotion motionIdlle = new IdlleAnimatedAnimotion(this, 2, animatorAux);
                    motionsCombineSpawnStopMoveSquare.Add(motionTwMove);
                    motionsCombineSpawnStopMoveSquare.Add(motionIdlle);

                    // esto solo lo hago para detener el sonido de los pasos
                    if (kimbokos.Count - 1 == i)
                    {
                        List<ConfigurableMotion> configurables = new List<ConfigurableMotion>();

                        if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
                        {
                            StopSoundConfigureContainer stopSoundContainer = new StopSoundConfigureContainer(GameSoundManager.Instance.audioSource);
                            ConfigureAnimotion stopSoundConfigure = new ConfigureAnimotion(stopSoundContainer, 2, true);
                            configurables.Add(stopSoundConfigure);
                        }
                        CombineAnimotion combineStopMotion = new CombineAnimotion(this, 1, motionsCombineSpawnStopMoveSquare, configurables);
                        motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                    }
                    else
                    {
                        CombineAnimotion combineStopMotion = new CombineAnimotion(this, 1, motionsCombineSpawnStopMoveSquare);
                        motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                    }
                    CombineAnimotion combinSquarePositionMotion = new CombineAnimotion(this, 1, motionsCombineSpawnMoveSquare);
                    motionsSpawnCombine.Add(combinSquarePositionMotion);
                }
            }
            CombineAnimotion repositionInSquareMotion = new CombineAnimotion(this, 1, motionsSpawnCombine);
            return repositionInSquareMotion;
        }

        public Animotion RearangePositionAfterCombineAnimotion(List<GameObject> kimbokos, Vector3 actualPosition, int reproductionOrder = 1)
        {
            List<Animotion> motionsSpawnCombine = new List<Animotion>();
            UnitMovePositioner movePositioner = new UnitMovePositioner(4f);
            //  - DESPUES ME DEBERIA FIJAR SI TENGO ENEMIGOS EN LA QUE SELECCIONE, Y SI TENGO DEBERIA AGREGAR EL COMANDO DE REPOSICIONARLOS
            if (kimbokos.Count > 1)
            {
                POSITIONTYPE positionTypeToRearrange = movePositioner.GetPositionType(kimbokos.Count);
                Vector3[] finalRearrangePositions = movePositioner.GetPositions(actualPosition, positionTypeToRearrange);

                // SONIDO DE CUANDO SE MUEVEN PARA REPOSICIONARSE
                if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
                {
                    AudioSourceParameter audioParameter = new AudioSourceParameter(false, true);
                    Animotion motionMoveSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[4], audioParameter);
                    motionsSpawnCombine.Add(motionMoveSound);
                }


                // deberia recorrer la lista de unidades, y generar una move comand para posicionarse en el lugar que tiene cada una del cuadrado
                for (int i = 0; i < kimbokos.Count; i++)
                {
                    List<Animotion> motionsCombineSpawnMoveSquare = new List<Animotion>();
                    Animator animatorAux = kimbokos[i].GetComponent<Animator>();
                    // LOS PONGO EN ESTADO MOVE PARA LA ANIMACION
                    Animotion motionMove = new MoveAnimatedAnimotion(this, 1, animatorAux);
                    motionsSpawnCombine.Add(motionMove);

                    List<Animotion> motionsCombineSpawnStopMoveSquare = new List<Animotion>();
                    // LOS MUEVO DESDE DONDE ESTAN HASTA LA POSICION FINAL
                    Animotion motionTwMove = new MoveTweenAnimotion(this, 1, kimbokos[i].transform,  finalRearrangePositions[i], 1);
                    motionsCombineSpawnStopMoveSquare.Add(motionTwMove);
                    // LOS PONGO EN ESTADO IDLLE
                    Animotion motionIdlle = new IdlleAnimatedAnimotion(this, 2, animatorAux);
                    motionsCombineSpawnStopMoveSquare.Add(motionIdlle);

                    // esto solo lo hago para detener el sonido de los pasos
                    if (kimbokos.Count - 1 == i)
                    {
                        List<ConfigurableMotion> configurables = new List<ConfigurableMotion>();

                        if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
                        {
                            StopSoundConfigureContainer stopSoundContainer = new StopSoundConfigureContainer(GameSoundManager.Instance.audioSource);
                            ConfigureAnimotion stopSoundConfigure = new ConfigureAnimotion(stopSoundContainer, 2, true);
                            configurables.Add(stopSoundConfigure);
                        }
                        CombineAnimotion combineStopMotion = new CombineAnimotion(this, 1, motionsCombineSpawnStopMoveSquare, configurables);
                        motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                    }
                    else
                    {
                        CombineAnimotion combineStopMotion = new CombineAnimotion(this, 1, motionsCombineSpawnStopMoveSquare);
                        motionsCombineSpawnMoveSquare.Add(combineStopMotion);
                    }
                    CombineAnimotion combinSquarePositionMotion = new CombineAnimotion(this, 1, motionsCombineSpawnMoveSquare);
                    motionsSpawnCombine.Add(combinSquarePositionMotion);
                }
            }
            CombineAnimotion combinMoveMotion = new CombineAnimotion(this, reproductionOrder, motionsSpawnCombine);
            return combinMoveMotion;
        }

        public Animotion NormalCombineAnimotion(Vector3 tileObjectRealWorldLocation, GameObject goKimbok, List<GameObject> kimbokos, GameMachine game)
        {
            List<Animotion> motionsSpawnCombine = new List<Animotion>();
            // A - MOVEMOS A LOS KIMBOKOS QUE OCUPEN LA TILE A LOS COSTADOS
            motionsSpawnCombine.Add(MoveToSquarePositionAnimotion(kimbokos, tileObjectRealWorldLocation));

            // B - Motion normal Move Motion
            motionsSpawnCombine.Add(game.moveManagerUI.MoveAnimotion(goKimbok, tileObjectRealWorldLocation));

            // C - REPOSICIONAMOS A TODOS LOS KIMBOKO
            // AGREGO EL KIMBOKO SPAWNEADO A LA LISTA
            kimbokos.Add(goKimbok);
            motionsSpawnCombine.Add(RearangePositionAfterCombineAnimotion(kimbokos, tileObjectRealWorldLocation, 2));

            CombineAnimotion combinMoveMotion = new CombineAnimotion(this, 1, motionsSpawnCombine);
            return combinMoveMotion;
        }

        #endregion
    }
}
