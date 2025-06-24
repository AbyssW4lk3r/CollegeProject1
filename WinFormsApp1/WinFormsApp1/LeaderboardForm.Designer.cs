namespace WinFormsApp1
{
    partial class LeaderboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LeaderboardView = new DataGridView();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)LeaderboardView).BeginInit();
            SuspendLayout();
            // 
            // LeaderboardView
            // 
            LeaderboardView.AllowUserToAddRows = false;
            LeaderboardView.AllowUserToDeleteRows = false;
            LeaderboardView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LeaderboardView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            LeaderboardView.Location = new Point(0, 39);
            LeaderboardView.Name = "LeaderboardView";
            LeaderboardView.ReadOnly = true;
            LeaderboardView.RowHeadersVisible = false;
            LeaderboardView.ScrollBars = ScrollBars.Vertical;
            LeaderboardView.Size = new Size(801, 411);
            LeaderboardView.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Showcard Gothic", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(194, 3);
            label1.Name = "label1";
            label1.Size = new Size(428, 33);
            label1.TabIndex = 1;
            label1.Text = "Таблица лидеров по победам";
            // 
            // LeaderboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(LeaderboardView);
            Name = "LeaderboardForm";
            Text = "LeaderboardForm";
            ((System.ComponentModel.ISupportInitialize)LeaderboardView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView LeaderboardView;
        private Label label1;
    }
}