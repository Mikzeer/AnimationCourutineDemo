using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class CombineManager : AbilityManager
    {
        bool debugOn = false;
        IGame game;

        public CombineManager(IGame game)
        {
            this.game = game;
        }

        public void OnTryCombine(CombineAbilityEventInfo cmbInfo)
        {
            if (!IsLegalCombine(cmbInfo))
            {
                if (debugOn) Debug.Log("Ilegal Spawn");
                return;
            }
            if (cmbInfo.combiner.Abilities.ContainsKey(ABILITYTYPE.COMBINE) == false)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD COMBINE NO ENCONTRADA");
                return;
            }
            CombineAbility cmb = (CombineAbility)cmbInfo.combiner.Abilities[ABILITYTYPE.COMBINE];
            if (cmb == null)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD COMBINE NULL");
                return;
            }

            cmb.SetRequireGameData(cmbInfo);
            StartPerform(cmb);

            if (cmb.CanIExecute() == false)
            {
                if (debugOn) Debug.Log("COMBINE ABILITY NO SE PUEDE EJECUTAR");
                return;
            }
            Combine(cmbInfo);
            EndPerform(cmb);
        }

        public bool IsLegalCombine(CombineAbilityEventInfo cmbInfo)
        {
            return true;
        }

        public void Combine(CombineAbilityEventInfo cmbInfo)
        {
            // TENGO QUE CREAR EL NUEVO KIMBOKO Y SU GAME OBJECT
            // TECNICAMENTE ESTO DEBERIA ESTAR CREADO COMO EL KIMBOKO TO COMBINE... 
            // NO DEBERIA CREAR NADA NUEVO TEEEECCCNIICAMENTE
            // SOLO EL COMBINE KIMBOKO, YA QUE SI ES UNA COMBINACION NO VOY A DESTRUR NINGUN GO

            List<Kimboko> kimbokosTocombine = new List<Kimboko>();
            kimbokosTocombine.Add(cmbInfo.combiner);
            kimbokosTocombine.Add(cmbInfo.kimbokoToCombine);

            Kimboko kimboko = null;
            KimbokoCombineFactory kimbokoCombFac = new KimbokoCombineFactory(kimbokosTocombine);
            kimboko = kimbokoCombFac.CreateKimboko(cmbInfo.IndexID , cmbInfo.player);

            // 1 - CREAMOS EL GO DEL COMBINE
            // 2 - CREAMOS EL COMBINE KIMBOKO
            // 3 - 
        }
    }
}
