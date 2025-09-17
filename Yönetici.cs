using EntityProject;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Project1_AdonetPCustomer
{
    public partial class Yönetici : Form
    {
        DbProjectEntities db = new DbProjectEntities();
        int targetX;
        int targetY;
        int errorSyc = 0;

        public Yönetici()
        {
            InitializeComponent();
        }

        // Rastgele doğrulama kodu üretir
        public string RandomStringNumber(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789*/?=!+-";
            Random random = new Random();
            string result = "";

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                result += chars[index];
            }

            return result;
        }

        private void Yönetici_Load(object sender, EventArgs e)
        {

            label5.Text = RandomStringNumber(5);


            txtname.Focus();


            targetX = button1.Left;
            targetY = button2.Top;

            button1.Left = -button1.Width;
            button2.Top = -button2.Height;


            Timer timer = new Timer();
            timer.Interval = 10;
            timer.Tick += (s, ev) =>
            {
                if (button1.Left < targetX)
                    button1.Left += 10;

                if (button2.Top < targetY)
                    button2.Top += 10;

                if (button1.Left >= targetX && button2.Top >= targetY)
                    timer.Stop();
            };
            timer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != label5.Text)
            {
                textBox1.BackColor = Color.DarkRed;
                label5.Text = RandomStringNumber(5);
                return;
            }
            else
            {
                textBox1.BackColor = Color.LightGreen;
                MessageBox.Show("Giriş Yapılıyor.", "Depoya Hoşgeldiniz", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }

            // 1 . Yöntem
            // /* var query =
            // (from x in db.SORUMLU where txtname.Text == x.UserName && x.Password == txtpassword.Text select x);
            // if (query.Any())
            // {
            // MessageBox.Show("Giriş Başarılı");
            // Form1 fr = new Form1();
            // fr.Show();
            // this.Hide();
            // }
            // else
            // {
            // MessageBox.Show("kullaanıcı Adı Veya Şİfre Yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // } */
            // 2. Yöntem
            var user = db.SORUMLU.FirstOrDefault(x => x.UserName == txtname.Text && x.Password == txtpassword.Text);
            if (user != null)
            {
                Form1 fr = new Form1();
                fr.Show();
                this.Hide();
            }
            else
            {
                errorSyc++;
                switch (errorSyc)
                {
                    case 1:
                        MessageBox.Show("1 Kez Hatalı Giriş Yaptınız. 2 Hakkınız Kaldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 2:
                        MessageBox.Show("2 Kez Hatalı Giriş Yaptınız. Dikkatli Olunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 3:
                        MessageBox.Show("3 Kez Hatalı Giriş Yaptınız. Program Kapanacaktır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        break;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            txtpassword.UseSystemPasswordChar = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Uygulamadan Çıkış Yapılıyor.");
            Application.Exit();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtpassword.UseSystemPasswordChar = true;
        }
    }
}
