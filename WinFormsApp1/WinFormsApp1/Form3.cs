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
    public partial class Form3 : Form
    {
        private GameController2 controller2;
        private Button[,] buttons = new Button[3, 3];
        private Label statsLabel;
        private Button restartButton;
        private Button mainMenuButton;

        public Form3()
        {
            InitializeComponent();
            InitializeGame();
            InitializeUI();
        }

        private void InitializeGame()
        {
            controller2 = new GameController2();
        }

        private void SaveStats(object sender, FormClosingEventArgs e)
        {
            string playerName = textBox1.Text;
            int playerWins = controller2.PlayerWins;
            string filePath = "stats.txt";

            var players = new List<(string Name, int Wins)>();

            if (File.Exists(filePath))
            {
                foreach (string line in File.ReadAllLines(filePath))
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int wins))
                    {
                        players.Add((parts[0], wins));
                    }
                }
            }

            bool playerExists = false;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Name.Equals(playerName, StringComparison.OrdinalIgnoreCase))
                {
                    if (playerWins > players[i].Wins)
                    {
                        players[i] = (playerName, playerWins);
                    }
                    playerExists = true;
                    break;
                }
            }

            if (!playerExists)
            {
                players.Add((playerName, playerWins));
            }

            var sortedPlayers = players.OrderByDescending(p => p.Wins).ToList();

            File.WriteAllLines(filePath, sortedPlayers.Select(p => $"{p.Name}|{p.Wins}"));
        }

        private void InitializeUI()
        {
            this.Text = "Крестики-нолики (vs Компьютер)";
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
                        Font = new Font("Arial", 30),
                        Tag = new Point(r, c),
                        BackColor = Color.White
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



            var timer = new System.Windows.Forms.Timer { Interval = 100 };
            timer.Tick += (s, e) => UpdateUI();
            timer.Start();
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var position = (Point)button.Tag;

            if (string.IsNullOrEmpty(button.Text) && !controller2.IsGameOver())
            {
                bool isWin = controller2.MakePlayerMove(position.X, position.Y);
                UpdateUI();

                if (isWin)
                {
                    MessageBox.Show("Вы победили!", "Победа");
                }
                else if (controller2.IsDraw())
                {
                    MessageBox.Show("Ничья!", "Игра окончена");
                }
            }
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            controller2.ResetGame();
            UpdateUI();
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateUI()
        {
            var board = controller2.GetBoardState();
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    buttons[r, c].Text = board[r, c] == '\0' ? "" : board[r, c].ToString();
                    buttons[r, c].ForeColor = board[r, c] == 'X' ? Color.Blue : Color.Red;
                }
            }

            statsLabel.Text = $"Ваши победы: {controller2.PlayerWins} | Победы компьютера: {controller2.ComputerWins} | Ничьи: {controller2.Draws}";

            foreach (var button in buttons)
            {
                button.Enabled = !controller2.IsGameOver() && controller2.GetCurrentPlayer() == 'X';
            }
        }
    }
    public class Game2
    {
        private char[,] board;
        private char currentPlayer;
        private bool isGameOver;
        private Random random;

        public char CurrentPlayer => currentPlayer;
        public bool IsGameOver => isGameOver;

        public Game2()
        {
            board = new char[3, 3];
            currentPlayer = 'X'; 
            isGameOver = false;
            random = new Random();
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

        public void MakeComputerMove()
        {
            if (isGameOver || currentPlayer != 'O') return;

            var move = FindWinningMove('O') ?? FindWinningMove('X') ?? GetRandomMove();

            if (move.HasValue)
            {
                MakeMove(move.Value.row, move.Value.col);
            }
        }

        private (int row, int col)? FindWinningMove(char player)
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (board[r, c] == '\0')
                    {
                        board[r, c] = player;
                        bool isWin = CheckWin(r, c);
                        board[r, c] = '\0';

                        if (isWin) return (r, c);
                    }
                }
            }
            return null;
        }

        private (int row, int col)? GetRandomMove()
        {
            var emptyCells = new List<(int, int)>();
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (board[r, c] == '\0')
                        emptyCells.Add((r, c));

            if (emptyCells.Count == 0) return null;
            return emptyCells[random.Next(emptyCells.Count)];
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
    public class GameController2
    {
        private Game2 game2;
        public int PlayerWins { get; private set; }
        public int ComputerWins { get; private set; }
        public int Draws { get; private set; }

        public GameController2()
        {
            game2 = new Game2();
        }

        public bool MakePlayerMove(int row, int col)
        {
            if (game2.CurrentPlayer != 'X' || game2.IsGameOver)
                return false;

            bool isWin = game2.MakeMove(row, col);

            if (isWin)
            {
                PlayerWins++;
                return true;
            }

            if (game2.IsDraw())
            {
                Draws++;
                return false;
            }

            game2.MakeComputerMove();

            if (game2.IsGameOver)
            {
                if (game2.CurrentPlayer == 'O') ComputerWins++;
                else PlayerWins++;
            }
            else if (game2.IsDraw())
            {
                Draws++;
            }

            return isWin;
        }

        public void ResetGame()
        {
            game2.Reset();
        }

        public char[,] GetBoardState() => game2.GetBoardState();
        public char GetCurrentPlayer() => game2.CurrentPlayer;
        public bool IsGameOver() => game2.IsGameOver;
        public bool IsDraw() => game2.IsDraw();
    }
}
