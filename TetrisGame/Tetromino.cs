using System;

namespace TetrisGame
{
    public class Tetromino
    {
        public bool[,] Shape { get; private set; }
        private static readonly Random Random = new Random();

        private Tetromino(bool[,] shape)
        {
            Shape = shape;
        }

        public static Tetromino GetRandomTetromino()
        {
            switch (Random.Next(7))
            {
                case 0: return new Tetromino(new bool[,] { { true, true, true, true } }); // I
                case 1: return new Tetromino(new bool[,] { { true, true }, { true, true } }); // O
                case 2: return new Tetromino(new bool[,] { { false, true, false }, { true, true, true } }); // T
                case 3: return new Tetromino(new bool[,] { { true, true, false }, { false, true, true } }); // S
                case 4: return new Tetromino(new bool[,] { { false, true, true }, { true, true, false } }); // Z
                case 5: return new Tetromino(new bool[,] { { true, true, true }, { true, false, false } }); // L
                case 6: return new Tetromino(new bool[,] { { true, true, true }, { false, false, true } }); // J
                default: return new Tetromino(new bool[,] { { true, true, true, true } });
            }
        }

        public void Rotate()
        {
            int width = Shape.GetLength(1);
            int height = Shape.GetLength(0);
            bool[,] rotated = new bool[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    rotated[x, height - 1 - y] = Shape[y, x];
                }
            }
            Shape = rotated;
        }

        public void RotateBack()
        {
            int width = Shape.GetLength(1);
            int height = Shape.GetLength(0);
            bool[,] rotated = new bool[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    rotated[width - 1 - x, y] = Shape[y, x];
                }
            }
            Shape = rotated;
        }
    }
}
