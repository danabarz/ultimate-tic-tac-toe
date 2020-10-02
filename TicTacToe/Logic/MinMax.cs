﻿using System;
using TicTacToe.Logic;

namespace TicTacToe.Logic
{
    public class MinMax
    {
        readonly EvaluationFunction evaluateFunction = new EvaluationFunction();

        private int Minmax(Board gameBoard, int depth, bool isMax, PlayerMarker playerMarker)
        {
            int value = evaluateFunction.Evaluate(gameBoard, playerMarker);

            if (value == int.MaxValue)
            {
                return value - depth;
            }
            else if (value == int.MinValue)
            {
                return value + depth;
            }
            else if (gameBoard.CheckIfGameOver() == PlayerMarker.Tie)
            {
                return 0;
            }
            else if (isMax)
            {
                return AddMarkerAndCheckBoardValue(int.MinValue);
            }
            return AddMarkerAndCheckBoardValue(int.MaxValue);


            int AddMarkerAndCheckBoardValue(int defultValue)
            {
                int bestValue = defultValue;
                var (row, col) = GetEmptyCell(gameBoard);
                gameBoard.GameBoard[row, col] = playerMarker;
                if (defultValue == int.MinValue)
                {
                    bestValue = Math.Max(bestValue, Minmax(gameBoard, depth + 1, !isMax, gameBoard.GetOponenentPiece(playerMarker)));
                }
                else
                {
                    bestValue = Math.Min(bestValue, Minmax(gameBoard, depth + 1, !isMax, gameBoard.GetOponenentPiece(playerMarker)));
                }
                gameBoard.GameBoard[row, col] = null;
                return bestValue;
            }
        }

        private Tuple<int, int> GetEmptyCell(Board gameBoard)
        {
            for (int i = 0; i < Game.BoardDimensions; i++)
            {
                for (int j = 0; j < Game.BoardDimensions; j++)
                {
                    if (gameBoard.GameBoard[i, j] == null)
                    {
                        return Tuple.Create(i, j);
                    }
                }
            }
            return null;
        }

        public PlayerMove FindBestMove(Game game, PlayerMarker playerMarker)
        {
            var (boardRow, boardColumn) = UpdateBoardForMinMaxCheck(game._summaryBoard);
            var (cellRow, cellColumn) = UpdateBoardForMinMaxCheck(game.MainBoard[boardRow, boardColumn]);
            return new PlayerMove(game.MainBoard[boardRow, boardColumn], cellRow, cellColumn, playerMarker);


            Tuple<int, int> UpdateBoardForMinMaxCheck(Board gameBoard)
            {
                int bestValue = int.MinValue;
                int row = int.MinValue;
                int col =int.MinValue;

                for (int i = 0; i < Game.BoardDimensions; i++)
                {
                    for (int j = 0; j < Game.BoardDimensions; j++)
                    {
                        if (gameBoard.GameBoard[i, j] == null)
                        {
                            gameBoard.GameBoard[i, j] = playerMarker;
                            int moveValue = Minmax(gameBoard, 0, false, gameBoard.GetOponenentPiece(playerMarker));
                            gameBoard.GameBoard[i, j] = null;
                            if (moveValue > bestValue)
                            {
                                row = i;
                                col = j;
                                bestValue = moveValue;
                            }
                        }
                    }
                }
                return Tuple.Create(row, col);
            }
        }
    }
}
