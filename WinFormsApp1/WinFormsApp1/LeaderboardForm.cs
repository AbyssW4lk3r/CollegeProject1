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
    public partial class LeaderboardForm : Form
    {
        public LeaderboardForm()
        {
            InitializeComponent();
            LoadLeaderboardToDataGridView(LeaderboardView);
        }

        private void LoadLeaderboardToDataGridView(DataGridView dataGridView)
        {
            string filePath = "stats.txt";

            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add("Position", "Место");
            dataGridView.Columns.Add("Name", "Игрок");
            dataGridView.Columns.Add("Wins", "Победы");

            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
            {
                return; 
            }

            var players = new List<(string Name, int Wins)>();
            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2 && int.TryParse(parts[1], out int wins))
                {
                    players.Add((parts[0], wins));
                }
            }

            players = players.OrderByDescending(p => p.Wins).ToList();

            for (int i = 0; i < players.Count; i++)
            {
                dataGridView.Rows.Add(i + 1, players[i].Name, players[i].Wins);
            }
        }
    }
}
