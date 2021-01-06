using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class GameTestController : MonoBehaviour
    {
        MotionController motionController = new MotionController();

        [SerializeField] private SpawnManagerUI spawnManagerUI;
        SpawnManager spawnManager;

        [SerializeField] private Board2DManagerUI board2DManagerUI;
        Board2DManager board2DManager;

        [SerializeField] private TileSelectionManagerUI tileSelectionManagerUI;

        private void Start()
        {
            board2DManager = new Board2DManager(board2DManagerUI, 5, 7);

            Player playerOne = new Player(0, PLAYERTYPE.PLAYER);
            Player playerTwo = new Player(1, PLAYERTYPE.PLAYER);
            Player[] players = new Player[2];
            players[0] = playerOne;
            players[1] = playerTwo;
            Motion motion = board2DManager.CreateBoard(players, OnBoardComplete);
            ReproduceMotion(motion);
            tileSelectionManagerUI.SetController(board2DManager);
        }

        private void OnBoardComplete()
        {
            Debug.Log("BOARD SE TERMINO DE CREAR");
        }

        private void ReproduceMotion(Motion motion)
        {
            MotionController motionController = new MotionController();
            motionController.SetUpMotion(motion);
            motionController.TryReproduceMotion();
        }

        private void Update()
        {
            
        }
    }
}
