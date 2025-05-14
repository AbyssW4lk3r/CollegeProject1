namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 newForm2 = new Form2();
            this.Hide();
            newForm2.FormClosed += (s, args) => this.Show();
            newForm2.Show();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 newForm3 = new Form3();
            this.Hide();
            newForm3.FormClosed += (s, args) => this.Show();
            newForm3.Show();
        }
    }
}
