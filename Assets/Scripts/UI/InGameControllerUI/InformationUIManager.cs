using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;

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

        }
    }
}