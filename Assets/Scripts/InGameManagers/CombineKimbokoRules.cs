using System.Collections.Generic;

namespace PositionerDemo
{
    public static class CombineKimbokoRules
    {
        #region VARIABLES

        private static int Xpoints = 1;
        private static int Ypoints = 17;
        private static int Zpoints = 115;
        private static int XCombPoints = 1;
        private static int YCombPoints = 2;
        private static int ZCombPoints = 3;

        #endregion

        #region NEW POINTS RULES

        public static bool CanICombine(Kimboko combiner, Kimboko toCombine)
        {
            if (combiner.UnitType == UNITTYPE.FUSION || toCombine.UnitType == UNITTYPE.FUSION) return false;

            int totalPoints;

            int combinerPoints = PuntuateKimboko(combiner);
            int toCombinePoints = PuntuateKimboko(toCombine);

            totalPoints = combinerPoints + toCombinePoints;

            switch (totalPoints)
            {
                case 2:
                case 5:
                case 7:
                case 8:
                case 18:
                case 34:
                case 39:
                case 55:
                case 116:
                case 132:
                case 136:
                case 138:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CanICobineAndEvolve(Kimboko combinerCharacter, Kimboko toCombineCharacter)
        {
            if (combinerCharacter.UnitType == UNITTYPE.FUSION || toCombineCharacter.UnitType == UNITTYPE.FUSION) return false;
            if (combinerCharacter.UnitType != UNITTYPE.X && toCombineCharacter.UnitType != UNITTYPE.X) return false;

            int totalPoints;
            int combinerPoints = PuntuateKimboko(combinerCharacter);
            int toCombinePoints = PuntuateKimboko(toCombineCharacter);

            totalPoints = combinerPoints + toCombinePoints;

            switch (totalPoints)
            {
                case 22:
                case 41:
                case 121:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CanIEvolve(Kimboko combinerCharacter, Kimboko toCombineCharacter)
        {
            if (combinerCharacter.UnitType == UNITTYPE.FUSION || toCombineCharacter.UnitType == UNITTYPE.FUSION)
            {
                return false;
            }

            int totalPoints;

            int combinerPoints = PuntuateKimboko(combinerCharacter);
            int toCombinePoints = PuntuateKimboko(toCombineCharacter);

            totalPoints = combinerPoints + toCombinePoints;

            switch (totalPoints)
            {
                case 4:
                case 6:
                case 8:
                case 38:
                case 40:
                case 57:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CanIEvolve(Kimboko toEvolveCharacter)
        {
            if (toEvolveCharacter.UnitType == UNITTYPE.FUSION)
            {
                return false;
            }

            int evolverPoints = PuntuateKimboko(toEvolveCharacter);

            switch (evolverPoints)
            {
                case 4:
                case 6:
                case 8:
                case 38:
                case 40:
                case 57:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CanIFusion(Kimboko toFusionCharacter)
        {
            if (toFusionCharacter.UnitType == UNITTYPE.FUSION)
            {
                return false;
            }

            int fusionPoints = PuntuateKimboko(toFusionCharacter);

            switch (fusionPoints)
            {
                case 4:
                case 6:
                case 8:
                case 38:
                case 57:
                    return true;
                default:
                    return false;
            }
        }

        public static int PuntuateKimboko(Kimboko characterToPuntuate)
        {
            int totalPoints = 0;

            switch (characterToPuntuate.UnitType)
            {
                case UNITTYPE.X:
                    totalPoints += Xpoints;
                    break;
                case UNITTYPE.Y:
                    totalPoints += Ypoints;
                    break;
                case UNITTYPE.Z:
                    totalPoints += Zpoints;
                    break;
                case UNITTYPE.COMBINE:
                    for (int i = 0; i < (characterToPuntuate as KimbokoCombine).kimbokos.Count; i++)
                    {
                        switch ((characterToPuntuate as KimbokoCombine).kimbokos[i].UnitType)
                        {
                            case UNITTYPE.X:
                                totalPoints += Xpoints + XCombPoints;
                                break;
                            case UNITTYPE.Y:
                                totalPoints += Ypoints + YCombPoints;
                                break;
                            case UNITTYPE.Z:
                                totalPoints += Zpoints + ZCombPoints;
                                break;
                            case UNITTYPE.COMBINE:
                                return 0;
                            case UNITTYPE.FUSION:
                                return 0;
                            default:
                                return 0;
                        }
                    }
                    break;
                case UNITTYPE.FUSION:
                    return 0;
                default:
                    return 0;
            }

            return totalPoints;
        }

        public static int PuntuateKimbokoList(List<Kimboko> charactersToPuntuate)
        {
            int total = 0;

            for (int i = 0; i < charactersToPuntuate.Count; i++)
            {
                total += PuntuateKimboko(charactersToPuntuate[i]);
            }

            return total;
        }

        public static Kimboko WhoIsTheStrong(Kimboko kimbokoOne, Kimboko kimbokoTwo)
        {
            Kimboko theStongest;

            int CharacterOnePoints = PuntuateKimboko(kimbokoOne);
            int CharacterTwoPoints = PuntuateKimboko(kimbokoTwo);

            if (CharacterOnePoints == CharacterTwoPoints || CharacterOnePoints > CharacterTwoPoints)
            {
                theStongest = kimbokoOne;
            }
            else
            {
                theStongest = kimbokoTwo;
            }

            return theStongest;
        }

        public static bool CanIDecombine(Kimboko characterToDecombine, Tile from)
        {
            //// ESTO TIENE QUE SER GLOBAL YA QUE VA A SER UN CONTADOR
            //int CharactersLeftToDecombine = 0;

            //if (characterToDecombine.UnitType != UNITTYPE.COMBINE) return false;

            //// Primero tengo que ver cuantos characters hay combinados
            //// Despues tengo que buscar en forma de cruz si tengo esta cantidad de lugares libres
            //int amountOfCharacters = 0;
            //for (int i = 0; i < (characterToDecombine as KimbokoCombine).kimbokos.Count; i++)
            //{
            //    amountOfCharacters++;
            //}
            //// Lista de posibles lugares para moverse

            ////CrossCombineDirectionCommand crossCombineDirectionCommand = new CrossCombineDirectionCommand(from, characterToDecombine, BoardManager.Instance.GetAllBoardTileList());
            ////List<Tile2D> posibleDecombinableTiles = crossCombineDirectionCommand.crossTiles;

            //int amountOfFreePosibleTiles = 0;
            ////for (int i = 0; i < posibleDecombinableTiles.Count; i++)
            ////{
            ////    if (!posibleDecombinableTiles[i].IsOcuppied)
            ////    {
            ////        amountOfFreePosibleTiles++;
            ////    }
            ////}

            ////// le pongo menos uno ya que siempre el que descombina se queda en el lugar
            ////if (amountOfFreePosibleTiles >= amountOfCharacters - 1)
            ////{
            ////    CharactersLeftToDecombine = amountOfCharacters - 1;
            ////    return true;
            ////}

            return false;
        }

        #endregion

    }
}
