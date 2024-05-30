using System;
using System.Drawing;
using System.Windows.Forms;

namespace TetrisGame
{
    public partial class Form1 : Form
    {
        private const int GridWidth = 10;
        private const int GridHeight = 20;
        private const int CellSize = 30;

        private Timer gameTimer;
        private bool[,] grid;
        private Tetromino currentTetromino;
        private Point currentPosition;

        public Form1()
        {
            InitializeComponent();
            this.ClientSize = new Size(GridWidth * CellSize, GridHeight * CellSize);
            this.DoubleBuffered = true;

            grid = new bool[GridHeight, GridWidth];
            gameTimer = new Timer();
            gameTimer.Interval = 500;
            gameTimer.Tick += GameTick;
            gameTimer.Start();

            currentTetromino = Tetromino.GetRandomTetromino();
            currentPosition = new Point(GridWidth / 2 - 1, 0);

            this.KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    MoveTetromino(-1, 0);
                    break;
                case Keys.Right:
                    MoveTetromino(1, 0);
                    break;
                case Keys.Down:
                    MoveTetromino(0, 1);
                    break;  
                case Keys.Up:
                    RotateTetromino();
                    break;
            }
        }

        private void GameTick(object sender, EventArgs e)
        {
            if (!MoveTetromino(0, 1))
            {
                PlaceTetromino();
                ClearLines();
                currentTetromino = Tetromino.GetRandomTetromino();
                currentPosition = new Point(GridWidth / 2 - 1, 0);

                if (!IsPositionValid(currentTetromino, currentPosition))
                {
                    gameTimer.Stop();
                    MessageBox.Show("Game Over!");
                }
            }
            this.Invalidate();
        }

        private bool MoveTetromino(int dx, int dy)
        {
            Point newPosition = new Point(currentPosition.X + dx, currentPosition.Y + dy);
            if (IsPositionValid(currentTetromino, newPosition))
            {
                currentPosition = newPosition;
                return true;
            }
            return false;
        }

        private void RotateTetromino()
        {
            currentTetromino.Rotate();
            if (!IsPositionValid(currentTetromino, currentPosition))
            {
                currentTetromino.RotateBack();
            }
        }

        private bool IsPositionValid(Tetromino tetromino, Point position)
        {
            for (int y = 0; y < tetromino.Shape.GetLength(0); y++)
            {
                for (int x = 0; x < tetromino.Shape.GetLength(1); x++)
                {
                    if (tetromino.Shape[y, x] && (position.X + x < 0 || position.X + x >= GridWidth || position.Y + y >= GridHeight || grid[position.Y + y, position.X + x]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void PlaceTetromino()
        {
            for (int y = 0; y < currentTetromino.Shape.GetLength(0); y++)
            {
                for (int x = 0; x < currentTetromino.Shape.GetLength(1); x++)
                {
                    if (currentTetromino.Shape[y, x])
                    {
                        grid[currentPosition.Y + y, currentPosition.X + x] = true;
                    }
                }
            }
        }

        private void ClearLines()
        {
            for (int y = 0; y < GridHeight; y++)
            {
                bool fullLine = true;
                for (int x = 0; x < GridWidth; x++)
                {
                    if (!grid[y, x])
                    {
                        fullLine = false;
                        break;
                    }
                }
                if (fullLine)
                {
                    for (int row = y; row > 0; row--)
                    {
                        for (int col = 0; col < GridWidth; col++)
                        {
                            grid[row, col] = grid[row - 1, col];
                        }
                    }
                    for (int col = 0; col < GridWidth; col++)
                    {
                        grid[0, col] = false;
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    if (grid[y, x])
                    {
                        g.FillRectangle(Brushes.Blue, x * CellSize, y * CellSize, CellSize, CellSize);
                        g.DrawRectangle(Pens.Black, x * CellSize, y * CellSize, CellSize, CellSize);
                    }
                }
            }

            for (int y = 0; y < currentTetromino.Shape.GetLength(0); y++)
            {
                for (int x = 0; x < currentTetromino.Shape.GetLength(1); x++)
                {
                    if (currentTetromino.Shape[y, x])
                    {
                        g.FillRectangle(Brushes.Red, (currentPosition.X + x) * CellSize, (currentPosition.Y + y) * CellSize, CellSize, CellSize);
                        g.DrawRectangle(Pens.Black, (currentPosition.X + x) * CellSize, (currentPosition.Y + y) * CellSize, CellSize, CellSize);
                    }
                }
            }
        }
    }
}