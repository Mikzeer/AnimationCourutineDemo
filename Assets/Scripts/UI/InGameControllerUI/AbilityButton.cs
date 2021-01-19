using UnityEngine;
using UnityEngine.UI;

namespace MikzeerGame
{
    namespace UI
    {
        public class AbilityButton
        {
            Sprite btnSprite;
            public Button btnAbility;
            SpecificAbilityExecution abilityExecution;

            public AbilityButton(Button btnAbility, SpecificAbilityExecution abilityExecution, Sprite btnSprite)
            {
                this.btnSprite = btnSprite;
                this.btnAbility = btnAbility;
                btnAbility.GetComponentInChildren<Text>().text = abilityExecution.name;
                this.abilityExecution = abilityExecution;
            }

            public void Suscribe()
            {
                btnAbility.onClick.AddListener(Execute);
            }

            public void Unsuscribe()
            {
                btnAbility.onClick.RemoveAllListeners();
            }

            public void Execute()
            {
                abilityExecution.Execute();
            }

        }

    }
}