using System.Collections.Generic;
using UnityEngine;
namespace PositionerDemo
{
    public class UnitMovePositioner
    {
        private float offsetPosition;

        public UnitMovePositioner(float offsetPosition)
        {
            float triOffset = offsetPosition / 3;
            this.offsetPosition = triOffset;            
        }
        
        public POSITIONTYPE GetPositionType(int unitAmount)
        {
            switch (unitAmount)
            {
                case 1:
                    return POSITIONTYPE.CENTER;
                case 2:
                    return POSITIONTYPE.ARROW;
                case 3:
                    return POSITIONTYPE.BOW;
                case 4:
                    return POSITIONTYPE.DIAMOND;
                default:
                    return POSITIONTYPE.CENTER;
            }
        }

        /// <summary>
        /// Nos da las posiciones finales en donde vamos a mover a nuestras unidades
        /// Ahora las acomode de la mas debil a la mas fuerte en posiciones
        /// </summary>
        /// <param name="tilePosition">La tile a donde se van a mover</param>
        /// <param name="positionType">De que forma van a acomodar en esa tile</param>
        /// <returns></returns>
        public Vector3[] GetPositions(Vector3 tilePosition, POSITIONTYPE positionType)
        {            
            Vector3[] shapePosition;
            switch (positionType)
            {                
                case POSITIONTYPE.CENTER:
                    shapePosition = new Vector3[1];
                    shapePosition[0] = tilePosition + new Vector3(offsetPosition * CardinalPosition.CENTER.x, offsetPosition * CardinalPosition.CENTER.y, 0);
                    break;
                case POSITIONTYPE.ARROW:
                    shapePosition = new Vector3[2];
                    shapePosition[0] = tilePosition + new Vector3(offsetPosition * CardinalPosition.EAST.x, offsetPosition * CardinalPosition.EAST.y, 0);
                    shapePosition[1] = tilePosition + new Vector3(offsetPosition * CardinalPosition.WEST.x, offsetPosition * CardinalPosition.WEST.y, 0);
                    break;
                case POSITIONTYPE.BOW:
                    shapePosition = new Vector3[3];
                    shapePosition[0] = tilePosition + new Vector3(offsetPosition * CardinalPosition.EAST.x, offsetPosition * CardinalPosition.EAST.y, 0);
                    shapePosition[1] = tilePosition + new Vector3(offsetPosition * CardinalPosition.NORTH.x, offsetPosition * CardinalPosition.NORTH.y, 0);
                    shapePosition[2] = tilePosition + new Vector3(offsetPosition * CardinalPosition.SOUTH.x, offsetPosition * CardinalPosition.SOUTH.y, 0);
                    break;
                case POSITIONTYPE.DIAMOND:
                    shapePosition = new Vector3[4];
                    shapePosition[0] = tilePosition + new Vector3(offsetPosition * CardinalPosition.EAST.x, offsetPosition * CardinalPosition.EAST.y, 0);
                    shapePosition[1] = tilePosition + new Vector3(offsetPosition * CardinalPosition.NORTH.x, offsetPosition * CardinalPosition.NORTH.y, 0);
                    shapePosition[2] = tilePosition + new Vector3(offsetPosition * CardinalPosition.SOUTH.x, offsetPosition * CardinalPosition.SOUTH.y, 0);
                    shapePosition[3] = tilePosition + new Vector3(offsetPosition * CardinalPosition.WEST.x, offsetPosition * CardinalPosition.WEST.y, 0);
                    break;
                case POSITIONTYPE.SQUARE:
                    shapePosition = new Vector3[4];
                    shapePosition[0] = tilePosition + new Vector3(offsetPosition * CardinalPosition.NORTHEAST.x, offsetPosition * CardinalPosition.NORTHEAST.y, 0);
                    shapePosition[1] = tilePosition + new Vector3(offsetPosition * CardinalPosition.NORTHWEST.x, offsetPosition * CardinalPosition.NORTHWEST.y, 0);
                    shapePosition[2] = tilePosition + new Vector3(offsetPosition * CardinalPosition.SOUTHEAST.x, offsetPosition * CardinalPosition.SOUTHEAST.y, 0);
                    shapePosition[3] = tilePosition + new Vector3(offsetPosition * CardinalPosition.SOUTHWEST.x, offsetPosition * CardinalPosition.SOUTHWEST.y, 0);
                    break;
                default:
                    shapePosition = new Vector3[0];
                    break;
            }
            return shapePosition;
        }

        public Dictionary<Enemy, Vector3[]> GetRoutePositions (Enemy[] enemies, POSITIONTYPE positionType, Vector3 finalWorldDestination, Vector3 actualPosition)
        {
            int amountOfPositionsToVisit = 3;

            Dictionary<Enemy, Vector3[]> transformWithPositionToTween = new Dictionary<Enemy, Vector3[]>();

            Vector3[] finalDestinationForEach = GetPositions(finalWorldDestination, positionType);

            Vector3 pointOfExitFromActualTile = GetTileExitDirection(actualPosition, finalWorldDestination);

            pointOfExitFromActualTile = actualPosition + new Vector3(offsetPosition * pointOfExitFromActualTile.x, offsetPosition * pointOfExitFromActualTile.y, 0);


            // Position[0] = transformToPosition[i].position
            // Position[1] = El Borde por donde salga para ir hacia la unidad, entonces tengo que tener una funcion para saber para que lado estoy yendo
            //               Este borde va a ser igual para todas las unidades, asi que es una posicion que vamos a guardar al principio
            // Position[2] = finalDestinationForEach[n]

            for (int i = 0; i < enemies.Length; i++)
            {
                Vector3[] ListOfPositionsForDictionary = new Vector3[amountOfPositionsToVisit];

                ListOfPositionsForDictionary[0] = enemies[i].transform.position;
                ListOfPositionsForDictionary[1] = pointOfExitFromActualTile;
                ListOfPositionsForDictionary[2] = finalDestinationForEach[i];

                transformWithPositionToTween.Add(enemies[i], ListOfPositionsForDictionary);
            }

            return transformWithPositionToTween;
        }

        public Dictionary<GameObject, Vector3[]> GetRoutePositions(GameObject[] enemies, POSITIONTYPE positionType, Vector3 finalWorldDestination, Vector3 actualPosition)
        {
            int amountOfPositionsToVisit = 3;

            Dictionary<GameObject, Vector3[]> transformWithPositionToTween = new Dictionary<GameObject, Vector3[]>();

            Vector3[] finalDestinationForEach = GetPositions(finalWorldDestination, positionType);

            Vector3 pointOfExitFromActualTile = GetTileExitDirection(actualPosition, finalWorldDestination);

            pointOfExitFromActualTile = actualPosition + new Vector3(offsetPosition * pointOfExitFromActualTile.x, offsetPosition * pointOfExitFromActualTile.y, 0);


            // Position[0] = transformToPosition[i].position
            // Position[1] = El Borde por donde salga para ir hacia la unidad, entonces tengo que tener una funcion para saber para que lado estoy yendo
            //               Este borde va a ser igual para todas las unidades, asi que es una posicion que vamos a guardar al principio
            // Position[2] = finalDestinationForEach[n]

            for (int i = 0; i < enemies.Length; i++)
            {
                Vector3[] ListOfPositionsForDictionary = new Vector3[amountOfPositionsToVisit];

                ListOfPositionsForDictionary[0] = enemies[i].transform.position;
                ListOfPositionsForDictionary[1] = pointOfExitFromActualTile;
                ListOfPositionsForDictionary[2] = finalDestinationForEach[i];

                transformWithPositionToTween.Add(enemies[i], ListOfPositionsForDictionary);
            }

            return transformWithPositionToTween;
        }

        public Vector3 GetTileExitDirection(Vector3 actualPosition, Vector3 endPosition)
        {
            Vector3 tileExit = new Vector3(0,0,0);

            if (actualPosition.x == endPosition.x)
            {
                tileExit.x = 0;
            }
            else if (actualPosition.x > endPosition.x)
            {
                tileExit.x = -1;
            }
            else
            {
                tileExit.x = 1;
            }

            if (actualPosition.y == endPosition.y)
            {
                tileExit.y = 0;
            }
            else if (actualPosition.y > endPosition.y)
            {
                tileExit.y = -1;
            }
            else
            {
                tileExit.y = 1;
            }

            return tileExit;
        }
    }

}

