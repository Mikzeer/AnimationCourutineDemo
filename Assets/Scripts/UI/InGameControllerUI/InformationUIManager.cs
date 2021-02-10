using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;
using MikzeerGame.Animotion;

namespace MikzeerGame
{
    namespace UI
    {
        public class InformationUIManager : MonoBehaviour
        {
            [SerializeField] private InformationBanner informationBanner = default;

            public PositionerDemo.Motion ScaleUpAndDownBanner(RectTransform bannerRect)
            {
                List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();
                List<Configurable> configureAnimotion = new List<Configurable>();
                // ACTUAL SCALE / NEW SCALE
                Vector3 normalScale = bannerRect.localScale;
                Vector3 finalScale = new Vector3(1.2f, 1.2f, 1);
                // SE ACTIVA
                SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRect, 1);
                configureAnimotion.Add(KimbokoActiveConfigAnimotion);
                // REPRODUCE LA TWEEN
                PositionerDemo.Motion motionTweenScaleUp = new ScaleRectTweenMotion(this, bannerRect, 2, finalScale);
                motionsSpawn.Add(motionTweenScaleUp);
                PositionerDemo.Motion motionTweenScaleDown = new ScaleRectTweenMotion(this, bannerRect, 3, normalScale);
                motionsSpawn.Add(motionTweenScaleDown);
                // SE DESACTIVA
                SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveFalseConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRect, 4, true, false);
                configureAnimotion.Add(KimbokoActiveFalseConfigAnimotion);
                CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawn, configureAnimotion);
                return combinMoveMotion;
            }

            public PositionerDemo.Motion ShowBannerForAmountOfTimeThenClose(RectTransform bannerRectTimer, float duration)
            {
                List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();
                List<Configurable> configureAnimotion = new List<Configurable>();
                // SE ACTIVA
                SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRectTimer, 1);
                configureAnimotion.Add(KimbokoActiveConfigAnimotion);
                // REPRODUCE LA TWEEN
                PositionerDemo.Motion motionTimer = new TimeMotion(this, 2, duration);
                motionsSpawn.Add(motionTimer);
                // SE DESACTIVA
                SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveFalseConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRectTimer, 4, true, false);
                configureAnimotion.Add(KimbokoActiveFalseConfigAnimotion);
                CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawn, configureAnimotion);
                return combinMoveMotion;
            }

            public PositionerDemo.Motion ShowBannerScaleUpWaitForTimeScaleDownClose(RectTransform bannerRect, float duration)
            {
                List<PositionerDemo.Motion> motionsSpawn = new List<PositionerDemo.Motion>();
                List<Configurable> configureAnimotion = new List<Configurable>();
                // ACTUAL SCALE / NEW SCALE
                Vector3 normalScale = bannerRect.localScale;
                Vector3 finalScale = new Vector3(1.2f, 1.2f, 1);
                // SE ACTIVA ORDER 1
                SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRect, 1);
                configureAnimotion.Add(KimbokoActiveConfigAnimotion);
                // REPRODUCE LA TWEEN DE SCALE UP ORDER 2
                PositionerDemo.Motion motionTweenScaleUp = new ScaleRectTweenMotion(this, bannerRect, 2, finalScale, 1);
                motionsSpawn.Add(motionTweenScaleUp);
                // ESPERAMOS ORDER 3
                PositionerDemo.Motion motionTimer = new TimeMotion(this, 3, duration);
                motionsSpawn.Add(motionTimer);
                // REPRODUCE LA TWEEN DE SCALE DOWN ORDER 4
                PositionerDemo.Motion motionTweenScaleDown = new ScaleRectTweenMotion(this, bannerRect, 4, normalScale, 1);
                motionsSpawn.Add(motionTweenScaleDown);
                // SE DESACTIVA ORDER 5
                SetActiveConfigureAnimotion<Transform, Transform> KimbokoActiveFalseConfigAnimotion = new SetActiveConfigureAnimotion<Transform, Transform>(bannerRect, 5, true, false);
                configureAnimotion.Add(KimbokoActiveFalseConfigAnimotion);
                CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsSpawn, configureAnimotion);
                return combinMoveMotion;
            }

            public PositionerDemo.Motion SetAndShowBanner(string txtBannerInfo, float duration)
            {
                informationBanner.SetBannerTextInformation(txtBannerInfo);
                return ShowBannerScaleUpWaitForTimeScaleDownClose(informationBanner.GetRectTransform(), duration);
            }


            #region ANIMOTION

            public Animotion.Animotion ScaleUpAndDownBannerAnimotion(RectTransform bannerRect)
            {
                List<Animotion.Animotion> motionsSpawn = new List<Animotion.Animotion>();
                List<ConfigurableMotion> configureAnimotion = new List<ConfigurableMotion>();
                // ACTUAL SCALE / NEW SCALE
                Vector3 normalScale = bannerRect.localScale;
                Vector3 finalScale = new Vector3(1.2f, 1.2f, 1);

                // SE ACTIVA
                GameObjectSetActiveConfigureContainer setActiveTrueContainer = new GameObjectSetActiveConfigureContainer(bannerRect.gameObject, true);
                ConfigureAnimotion setActiveConfigure = new ConfigureAnimotion(setActiveTrueContainer, 1, true);
                configureAnimotion.Add(setActiveConfigure);

                // REPRODUCE LA TWEEN
                Animotion.Animotion motionTweenScaleUp = new ScaleRectTweenAnimotion(this, 2, bannerRect, finalScale);
                motionsSpawn.Add(motionTweenScaleUp);
                Animotion.Animotion motionTweenScaleDown = new ScaleRectTweenAnimotion(this, 3, bannerRect, normalScale);
                motionsSpawn.Add(motionTweenScaleDown);
                // SE DESACTIVA
                GameObjectSetActiveConfigureContainer setActiveFalseContainer = new GameObjectSetActiveConfigureContainer(bannerRect.gameObject, false);
                ConfigureAnimotion setActiveFalseConfigure = new ConfigureAnimotion(setActiveFalseContainer, 4, true);
                configureAnimotion.Add(setActiveFalseConfigure);

                CombineAnimotion combinMoveMotion = new CombineAnimotion(this, 1, motionsSpawn, configureAnimotion);
                return combinMoveMotion;
            }

            public Animotion.Animotion ShowBannerForAmountOfTimeThenCloseAnimotion(RectTransform bannerRectTimer, float duration)
            {
                List<Animotion.Animotion> motionsSpawn = new List<Animotion.Animotion>();
                List<ConfigurableMotion> configureAnimotion = new List<ConfigurableMotion>();

                // SE ACTIVA
                GameObjectSetActiveConfigureContainer setActiveTrueContainer = new GameObjectSetActiveConfigureContainer(bannerRectTimer.gameObject, true);
                ConfigureAnimotion setActiveConfigure = new ConfigureAnimotion(setActiveTrueContainer, 1, true);
                configureAnimotion.Add(setActiveConfigure);

                // REPRODUCE LA TWEEN
                Animotion.Animotion motionTimer = new TimeAnimotion(this, 2, duration);
                motionsSpawn.Add(motionTimer);

                // SE DESACTIVA
                GameObjectSetActiveConfigureContainer setActiveFalseContainer = new GameObjectSetActiveConfigureContainer(bannerRectTimer.gameObject, false);
                ConfigureAnimotion setActiveFalseConfigure = new ConfigureAnimotion(setActiveFalseContainer, 4, true);
                configureAnimotion.Add(setActiveFalseConfigure);

                CombineAnimotion combinMoveMotion = new CombineAnimotion(this, 1, motionsSpawn, configureAnimotion);
                return combinMoveMotion;
            }

            public Animotion.Animotion ShowBannerScaleUpWaitForTimeScaleDownCloseAnimotion(RectTransform bannerRect, float duration)
            {
                List<Animotion.Animotion> motionsSpawn = new List<Animotion.Animotion>();
                List<ConfigurableMotion> configureAnimotion = new List<ConfigurableMotion>();
                // ACTUAL SCALE / NEW SCALE
                Vector3 normalScale = bannerRect.localScale;
                Vector3 finalScale = new Vector3(1.2f, 1.2f, 1);

                // SE ACTIVA ORDER 1
                GameObjectSetActiveConfigureContainer setActiveTrueContainer = new GameObjectSetActiveConfigureContainer(bannerRect.gameObject, true);
                ConfigureAnimotion setActiveConfigure = new ConfigureAnimotion(setActiveTrueContainer, 1, true);
                configureAnimotion.Add(setActiveConfigure);

                // REPRODUCE LA TWEEN DE SCALE UP ORDER 2
                Animotion.Animotion motionTweenScaleUp = new ScaleRectTweenAnimotion(this, 2, bannerRect, finalScale, 1);
                motionsSpawn.Add(motionTweenScaleUp);

                // ESPERAMOS ORDER 3
                Animotion.Animotion motionTimer = new TimeAnimotion(this, 3, duration);
                motionsSpawn.Add(motionTimer);

                // REPRODUCE LA TWEEN DE SCALE DOWN ORDER 4
                Animotion.Animotion motionTweenScaleDown = new ScaleRectTweenAnimotion(this, 4, bannerRect, normalScale, 1);
                motionsSpawn.Add(motionTweenScaleDown);

                // SE DESACTIVA ORDER 5
                GameObjectSetActiveConfigureContainer setActiveFalseContainer = new GameObjectSetActiveConfigureContainer(bannerRect.gameObject, false);
                ConfigureAnimotion setActiveFalseConfigure = new ConfigureAnimotion(setActiveFalseContainer, 5, true);
                configureAnimotion.Add(setActiveFalseConfigure);

                CombineAnimotion combinMoveMotion = new CombineAnimotion(this, 1, motionsSpawn, configureAnimotion);
                return combinMoveMotion;
            }

            public Animotion.Animotion SetAndShowBannerAnimotion(string txtBannerInfo, float duration)
            {
                informationBanner.SetBannerTextInformation(txtBannerInfo);
                return ShowBannerScaleUpWaitForTimeScaleDownCloseAnimotion(informationBanner.GetRectTransform(), duration);
            }

            #endregion
        }
    }
}