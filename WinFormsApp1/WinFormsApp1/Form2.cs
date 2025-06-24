using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        private GameController controller;
        private Button[,] buttons = new Button[3, 3];
        private Label statsLabel;
        private Button restartButton;
        private Button mainMenuButton;

        public Form2()
        {
            InitializeComponent();
            InitializeGame();
            InitializeUI();
        }

        private void InitializeGame()
        {
            controller = new GameController();
        }
    
        private void InitializeUI()
        {
            this.Text = "Крестики-нолики";
            this.ClientSize = new Size(600, 700);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            int buttonSize = 150;
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    buttons[r, c] = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(c * buttonSize + 50, r * buttonSize + 50),
                        Font = new Font("Arial", 40),
                        Tag = new Point(r, c)
                    };
                    buttons[r, c].Click += Cell_Click;
                    this.Controls.Add(buttons[r, c]);
                }
            }

            statsLabel = new Label
            {
                Location = new Point(50, 500),
                Size = new Size(500, 50),
                Font = new Font("Arial", 14),
                Text = "Победы X: 0 | Победы O: 0 | Ничьи: 0"
            };
            this.Controls.Add(statsLabel);

            restartButton = new Button
            {
                Text = "Новая игра",
                Location = new Point(200, 570),
                Size = new Size(200, 50),
                Font = new Font("Arial", 12)
            };
            restartButton.Click += RestartButton_Click;
            this.Controls.Add(restartButton);

            mainMenuButton = new Button
            {
                Text = "В главное меню",
                Location = new Point(200, 630),
                Size = new Size(200, 50),
                Font = new Font("Arial", 12)
            };
            mainMenuButton.Click += MainMenuButton_Click;
            this.Controls.Add(mainMenuButton);
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var position = (Point)button.Tag;

            if (string.IsNullOrEmpty(button.Text) && !controller.IsGameOver())
            {
                bool isWin = controller.MakeMove(position.X, position.Y);
                UpdateBoard();

                if (isWin)
                {
                    MessageBox.Show($"Игрок {controller.GetCurrentPlayer()} победил!", "Победа");
                    UpdateStats();
                    ResetBoard();
                }
                else if (controller.IsDraw())
                {
                    MessageBox.Show("Ничья!", "Игра окончена");
                    UpdateStats();
                    ResetBoard();
                }
            }
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            ResetBoard();
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateBoard()
        {
            var board = controller.GetBoardState();
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    buttons[r, c].Text = board[r, c] == '\0' ? "" : board[r, c].ToString();
                    buttons[r, c].ForeColor = board[r, c] == 'X' ? Color.Blue : Color.Red;
                }
            }
        }

        private void UpdateStats()
        {
            statsLabel.Text = $"Победы X: {controller.WinsO} | Победы O: {controller.WinsX} | Ничьи: {controller.Draws}";
        }

        private void ResetBoard()
        {
            controller.ResetGame();
            foreach (var button in buttons)
            {
                button.Text = "";
            }
        }
    }

    public class Game
    {
        private char[,] board;
        private char currentPlayer;
        private bool isGameOver;

        public char CurrentPlayer => currentPlayer;
        public bool IsGameOver => isGameOver;

        public Game()
        {
            board = new char[3, 3];
            currentPlayer = 'X';
            isGameOver = false;
        }

        public bool MakeMove(int row, int col)
        {
            if (isGameOver || row < 0 || row >= 3 || col < 0 || col >= 3 || board[row, col] != '\0')
                return false;

            board[row, col] = currentPlayer;

            if (CheckWin(row, col))
            {
                isGameOver = true;
                return true;
            }

            if (IsDraw())
            {
                isGameOver = true;
                return false;
            }

            SwitchPlayer();
            return false;
        }

        public bool IsDraw()
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (board[r, c] == '\0')
                        return false;
            return true;
        }

        public void Reset()
        {
            board = new char[3, 3];
            currentPlayer = 'X';
            isGameOver = false;
        }

        public char[,] GetBoardState()
        {
            return (char[,])board.Clone();
        }

        private bool CheckWin(int row, int col)
        {
            char symbol = board[row, col];

            if (board[row, 0] == symbol && board[row, 1] == symbol && board[row, 2] == symbol)
                return true;

            if (board[0, col] == symbol && board[1, col] == symbol && board[2, col] == symbol)
                return true;

            if (row == col && board[0, 0] == symbol && board[1, 1] == symbol && board[2, 2] == symbol)
                return true;

            if (row + col == 2 && board[0, 2] == symbol && board[1, 1] == symbol && board[2, 0] == symbol)
                return true;

            return false;
        }

        private void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
        }
    }
    public class GameController
    {
        private Game game;
        public int WinsX { get; private set; }
        public int WinsO { get; private set; }
        public int Draws { get; private set; }

        public GameController()
        {
            game = new Game();
        }

        public bool MakeMove(int row, int col)
        {
            bool isWin = game.MakeMove(row, col);

            if (isWin)
            {
                if (game.CurrentPlayer == 'X') WinsO++;
                else WinsX++;
            }
            else if (game.IsDraw())
            {
                Draws++;
            }

            return isWin;
        }

        public void ResetGame()
        {
            game.Reset();
        }

        public char[,] GetBoardState() => game.GetBoardState();
        public char GetCurrentPlayer() => game.CurrentPlayer;
        public bool IsGameOver() => game.IsGameOver;
        public bool IsDraw() => game.IsDraw();
    }
}
