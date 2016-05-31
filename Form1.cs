using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Forms;


namespace AstronomerGuide
{
    public partial class Form1 : MetroForm
    {
        Sky mySky = new Sky();

        public Form1()
        {
            InitializeComponent();
            MetroStyleManager.Default.Style = MetroFramework.MetroColorStyle.Teal;
            
            mySky.ShowAllStars(comboBox1);
            mySky.PrintAllStars(dataGridView1);

            dataGridView3.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9F, FontStyle.Regular);

        }

     

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mySky.GetStarProp(
                comboBox1, 
                metroLabel8,
                metroLabel9,
                metroLabel10,
                metroLabel11,
                metroLabel12,
                metroLabel13,
                dataGridView2,
                metroLabel14,
                metroLabel16);

            metroLabel15.Visible = true;
        }



        private void metroLabel17_Click(object sender, EventArgs e)
        {
            mySky.Check1( maskedTextBox1,
                          maskedTextBox2,
                          maskedTextBox3,
                          maskedTextBox4,
                          maskedTextBox5,
                          maskedTextBox6,
                          dataGridView1 );

            dataGridView2.Rows.Clear();
            comboBox1.Items.Clear();
            
            mySky.PrintAllStars(dataGridView2);
            mySky.ShowAllStars(comboBox1);


        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            comboBox1.Items.Clear();            

            mySky.PrintAllStars(dataGridView1);
            mySky.PrintAllStars(dataGridView2);
            mySky.ShowAllStars(comboBox1);
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
           
            if (MessageBox.Show("Вы действительно хотите удалить эту звезду?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                mySky.Remove(e.Row.Cells[0].Value.ToString());
            }
            
        }

        private void metroLabel24_Click(object sender, EventArgs e)
        {
            mySky.Check2( maskedTextBox7,
                          maskedTextBox8,
                          maskedTextBox9,
                          maskedTextBox10,
                          maskedTextBox11,
                          maskedTextBox12,
                          dataGridView3 );

            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            comboBox1.Items.Clear();

            mySky.PrintAllStars(dataGridView1);
            mySky.PrintAllStars(dataGridView2);
            mySky.ShowAllStars(comboBox1);


        }

        private void metroLabel17_MouseHover(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(metroLabel17, "Заполните поля и кликните на \"Добавить звезду\" для добавления");
        }

        private void metroLabel24_MouseHover(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(metroLabel24, "Заполните поля и кликните на \"Поиск видимых звёзд\" для поиска");
        }

       
    }
}
