using System.Collections.Generic;
namespace PositionerDemo
{
    public class MatchStatus
    {
        /*
         * LLEVAR CONTEO PARA AL FINALIZAR LA PARTIDA MOSTRAR TODOS ESTOS STATS
         * 
         * JUGADOR GANADOR / JUGADOR PERDEDOR
         * TIEMPO DE PARTIDA
         * TURNOS TOTALES
         * PTS DE VIDA DEL JUGADOR GANADOR
         * UNIDADES SPAWENEADAS //  PLAYER / UNITTYPE / SPAWNPOSXPOSY
         * CARTAS LEVANTADAS //  PLAYER / CARD
         * CARTAS USADAS //  PLAYER / USECARD / TARGET
         * DANO TOTAL EFECTUADO A UNIT/OBJECT/PLAYER 
         * UNIDADES ATACADAS // ATTACKER/DAMAGER ATTACKER/DAMAGERPOSX/POSY AMOUNTOFDAMAGE DAMAGETYPE
         * UNIDADES MUERTAS // UNIT  UNITPOSX/POSY 
         * DISTANCIA RECORRIDA POR UNIDADES //  MOVEUNIT   MOVEUNITSTARTPOSXPOSY  MOVEUNITENDPOSXPOSY  DISTANCE
         * CANTIDAD DE DANO DEFENDIDO //   UNIT QUE SE DEFENDIO   AMOUNTOFDAMGEDEFENDED
         * UNIDADES COMBINADAS // UNIT  UNITPOSX/POSY
         * UNIDADES FUSIONADAS // UNIT  UNITPOSX/POSY 
         * UNIDADES EVOLUCIONADAS // UNIT  UNITPOSX/POSY
         * UNIDADES DESCOMBINADAS // UNIT  UNITPOSX/POSY
         * 
         */

        Dictionary<int, int> UnitsSpawnedByPlayer;
        public void OnUnitSpwan(int playerID)
        {
            if (UnitsSpawnedByPlayer.ContainsKey(playerID))
            {
                UnitsSpawnedByPlayer[playerID]++;
            }
            else
            {
                UnitsSpawnedByPlayer.Add(playerID, 1);
            }
        }

        Dictionary<int, List<DamageAbilityEventInfo>> UnitsAttacked;
        public void OnUnitAttacks(int playerID, DamageAbilityEventInfo damageEventInfo)
        {
            if (UnitsAttacked == null)
            {
                UnitsAttacked = new Dictionary<int, List<DamageAbilityEventInfo>>();
            }

            if (UnitsAttacked.ContainsKey(playerID))
            {
                for (int i = 0; i < UnitsAttacked[playerID].Count; i++)
                {
                    if (UnitsAttacked[playerID][i].damageType == damageEventInfo.damageType)
                    {
                        UnitsAttacked[playerID][i].damageAmountRecived += damageEventInfo.damageAmountRecived;
                    }
                }
            }
            else
            {
                List<DamageAbilityEventInfo> damageEventInfoList = new List<DamageAbilityEventInfo>();
                damageEventInfoList.Add(damageEventInfo);
                UnitsAttacked.Add(playerID, damageEventInfoList);
            }

        }
    }

}