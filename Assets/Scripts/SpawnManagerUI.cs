﻿using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class SpawnManagerUI : MonoBehaviour
    {
        int craneTweenSpeedVelocity = 2;
        int kimbokoTweenSpeedVelocity = 1;
        [SerializeField] private GameObject Crane;
        [SerializeField] private Transform CraneEnd;
        [SerializeField] private GameObject kimbokoPrefab;
        Animator craneAnimator;

        private void Start()
        {
            craneAnimator = Crane.GetComponent<Animator>();
        }

        public Motion NormalSpawn(Vector3 tileObjectRealWorldLocation, Kimboko kimboko)
        {
            GameObject goKimbok = Instantiate(kimbokoPrefab);
            kimboko.SetGameObject(goKimbok);

            List<Motion> motionsSpawn = new List<Motion>();
            List<Configurable> configureAnimotion = new List<Configurable>();

            // POSICION INICIAL Y FINAL DE LA GRUA PARA TWEENEAR
            Vector3 craneStartPosition;
            Vector3 craneEndPostion;

            //B TWEEN DESDE UNA POSICION ELEVADA SOBRE LA TILE DONDE SE INDICO SPAWNEAR HASTA MAS ABAJO ASI SE VE DESDE ARRIBA EN EL TABLERO SOBRE LA TILE
            craneStartPosition = new Vector3(tileObjectRealWorldLocation.x, Crane.transform.position.y, 0);
            Crane.transform.position = craneStartPosition;
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

            Motion motionSpawnSound = new SoundMotion(this, 2, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[3], false);
            motionsSpawn.Add(motionSpawnSound);

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
    }
}