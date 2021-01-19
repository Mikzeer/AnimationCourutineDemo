using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class MoveManagerUI : MonoBehaviour
    {
        public Motion MoveMotion(GameObject goKimbok, Vector3 endPosition)
        {
            List<Motion> motionsMove = new List<Motion>();

            Animator animator = goKimbok.GetComponent<Animator>();

            Motion motionMove = new MoveMotion(this, animator, 1);
            motionsMove.Add(motionMove);

            List<Configurable> configurables = new List<Configurable>();

            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    Motion motionMoveSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[4], false, true);
                    motionsMove.Add(motionMoveSound);


                    AudioSourceGenericContainer audioContainer = new AudioSourceGenericContainer(GameSoundManager.Instance.audioSource);
                    StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform> stopSoundConfigureAnimotion =
                        new StopSoundConfigureAnimotion<AudioSourceGenericContainer, Transform>(audioContainer, 2);
                    configurables.Add(stopSoundConfigureAnimotion);
                }
            }

            List<Motion> motionsStopMove = new List<Motion>();

            Motion motionTweenMove = new MoveTweenMotion(this, goKimbok.transform, 1, endPosition);
            Motion motionIdlle = new IdlleMotion(this, animator, 2, true);
            motionsStopMove.Add(motionTweenMove);
            motionsStopMove.Add(motionIdlle);

            CombineMotion combineStopMotion = new CombineMotion(this, 1, motionsStopMove, configurables);
            motionsMove.Add(combineStopMotion);

            CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsMove);

            return combinMoveMotion;
        }

        public Motion CombineMoveMotion(Vector3 actualPosition, Vector3 endPosition, GameObject[] enemies)
        {
            UnitMovePositioner movePositioner = new UnitMovePositioner(4f);
            Dictionary<GameObject, Vector3[]> enmiesAndPathToMove = movePositioner.GetRoutePositions(enemies, movePositioner.GetPositionType(enemies.Length), endPosition, actualPosition);
            List<Motion> motionsCombineMove = new List<Motion>();
            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    Motion motionMoveSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[4], false, true);
                    motionsCombineMove.Add(motionMoveSound);
                }
            }

            int index = 0;
            foreach (KeyValuePair<GameObject, Vector3[]> entry in enmiesAndPathToMove)
            {
                AnimatedMotion motionMove = new MoveMotion(this, entry.Key.GetComponent<Animator>(), 1);
                motionsCombineMove.Add(motionMove);

                List<Motion> extraMotionsCombineStopMove = new List<Motion>();
                Motion motionTweenMove = new MovePathTweenMotion(this, entry.Key.transform, 1, entry.Value);
                Motion motionIdlle = new IdlleMotion(this, entry.Key.GetComponent<Animator>(), 2, true);
                extraMotionsCombineStopMove.Add(motionTweenMove);
                extraMotionsCombineStopMove.Add(motionIdlle);

                // esto solo lo hago para detener el sonido de los pasos
                if (enmiesAndPathToMove.Count - 1 == index)
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
                    CombineMotion extraCombineMoveStopMotion = new CombineMotion(this, 1, extraMotionsCombineStopMove, configurables);
                    motionsCombineMove.Add(extraCombineMoveStopMotion);
                }
                else
                {
                    CombineMotion extraCombineMoveStopMotion = new CombineMotion(this, 1, extraMotionsCombineStopMove);
                    motionsCombineMove.Add(extraCombineMoveStopMotion);
                }

                index++;
            }

            CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsCombineMove);
            return combinMoveMotion;
        }

    }
}
