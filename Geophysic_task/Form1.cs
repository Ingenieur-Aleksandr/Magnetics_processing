using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geophysic_task
{
    public partial class Form1 : Form
    {
        int keyf = 0;
        MagFile MFV;
        MagFile MVR;
        MagFile VAR;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            keyf = 1;
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
                try
                {
                   MFV = new MagFile(openFileDialog1.FileName, keyf);
                }

                catch (Exception ex)
                {
                    MessageBox.Show("ОШИБКА " + ex.Message);
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MFV.DrawData(zedGraphControl1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            keyf = 2;
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            try
                {
                   MVR = new MagFile(openFileDialog2.FileName, keyf);
                }

                catch (Exception ex)
                {
                    MessageBox.Show("ОШИБКА " + ex.Message);
                }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MVR.DrawData(zedGraphControl2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            VAR = MagFile.Variations(MFV, 20000);
            VAR.DrawData(zedGraphControl3);
        }      
    }
}
