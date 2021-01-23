using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MikzeerGame
{
    namespace UI
    {
        public class AbilityButtonCreationUI : MonoBehaviour
        {
            [SerializeField] private Transform buttonParent = default;
            [SerializeField] private Button abilityButtonPrefab = default;
            List<AbilityButton> actualAbilityButtons;
            GameMachine gameMachine;
            IOcuppy actualOccupier;
            public bool isCreated { get; protected set; }

            private void Awake()
            {
                gameMachine = FindObjectOfType<GameMachine>();
            }

            public void SetTile(Tile selectedTile)
            {
                if (selectedTile == null)
                {
                    ClearAbilityButtons();
                    return;
                }
                IOcuppy actualOccupier = selectedTile.GetOcuppy();
                SetUnit(actualOccupier);
            }

            public void SetUnit(IOcuppy actualOccupier)
            {
                if (actualOccupier == null)
                {
                    ClearAbilityButtons();
                    return;
                }
                if (this.actualOccupier == actualOccupier) return;
                ClearAbilityButtons();
                this.actualOccupier = actualOccupier;
                buttonParent.gameObject.SetActive(true);
                StartCreationOfButtons(actualOccupier);
            }

            public void ClearAbilityButtons()
            {
                buttonParent.gameObject.SetActive(false);
                actualOccupier = null;
                if (actualAbilityButtons != null && actualAbilityButtons.Count > 0)
                {
                    for (int i = 0; i < actualAbilityButtons.Count; i++)
                    {
                        actualAbilityButtons[i].Unsuscribe();
                        Destroy(actualAbilityButtons[i].btnAbility.gameObject);
                    }
                    actualAbilityButtons.Clear();
                }
                isCreated = false;
            }

            public void StartCreationOfButtons(IOcuppy actualOccupier)
            {
                actualAbilityButtons = new List<AbilityButton>();
                foreach (KeyValuePair<ABILITYTYPE, IAbility> ab in actualOccupier.Abilities)
                {
                    switch (ab.Key)
                    {
                        case ABILITYTYPE.SPAWN:
                            CreateButton(actualOccupier, ABILITYBUTTONTYPE.SPAWN);
                            break;
                        case ABILITYTYPE.TAKEACARD:
                            break;
                        case ABILITYTYPE.MOVE:
                            CreateButton(actualOccupier, ABILITYBUTTONTYPE.MOVE);
                            break;
                        case ABILITYTYPE.ATTACK:
                            CreateButton(actualOccupier, ABILITYBUTTONTYPE.ATTACK);
                            break;
                        case ABILITYTYPE.DEFEND:
                            CreateButton(actualOccupier, ABILITYBUTTONTYPE.DEFEND);
                            break;
                        case ABILITYTYPE.COMBINE:
                            break;
                        case ABILITYTYPE.DECOMBINE:
                            break;
                        case ABILITYTYPE.EVOLVE:
                            break;
                        case ABILITYTYPE.FUSION:
                            break;
                        default:
                            break;
                    }
                }
                isCreated = true;
            }

            public void CreateButton(IOcuppy actualOccupier, ABILITYBUTTONTYPE abilityButtonType)
            {
                Button btnAbi = Instantiate(abilityButtonPrefab);
                btnAbi.transform.SetParent(buttonParent, false);
                switch (abilityButtonType)
                {
                    case ABILITYBUTTONTYPE.MOVE:
                        MoveAbilityButtonExecution moveAbilityBtnExe = new MoveAbilityButtonExecution(actualOccupier, gameMachine, this);
                        AbilityButton btnMove = new AbilityButton(btnAbi, moveAbilityBtnExe, null);
                        btnMove.Suscribe();
                        actualAbilityButtons.Add(btnMove);
                        break;
                    case ABILITYBUTTONTYPE.ATTACK:
                        AttackAbilityButtonExecution attackAbilityBtnExe = new AttackAbilityButtonExecution(actualOccupier, gameMachine, this);
                        AbilityButton btnAttack = new AbilityButton(btnAbi, attackAbilityBtnExe, null);
                        btnAttack.Suscribe();
                        actualAbilityButtons.Add(btnAttack);
                        break;
                    case ABILITYBUTTONTYPE.DEFEND:
                        DefenseAbilityButtonExecution defenseAbilityBtnExe = new DefenseAbilityButtonExecution(actualOccupier, gameMachine, this);
                        AbilityButton btnDefense = new AbilityButton(btnAbi, defenseAbilityBtnExe, null);
                        btnDefense.Suscribe();
                        actualAbilityButtons.Add(btnDefense);
                        break;
                    case ABILITYBUTTONTYPE.COMBINE:
                        break;
                    case ABILITYBUTTONTYPE.DECOMBINE:
                        break;
                    case ABILITYBUTTONTYPE.EVOLVE:
                        break;
                    case ABILITYBUTTONTYPE.FUSION:
                        break;
                    case ABILITYBUTTONTYPE.SPAWN:
                        SpawnAbilityButtonExecution spawnAbilityBtnExe = new SpawnAbilityButtonExecution(actualOccupier, gameMachine, this);
                        AbilityButton btnSpawn = new AbilityButton(btnAbi, spawnAbilityBtnExe, null);
                        btnSpawn.Suscribe();
                        actualAbilityButtons.Add(btnSpawn);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}