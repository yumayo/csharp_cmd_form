using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Dictionary<string, TabPage> tabPages = new Dictionary<string, TabPage>();
        Dictionary<string, CmdControl> cmdControlls = new Dictionary<string, CmdControl>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();

            for (int i = 2; i < 5; i++)
            {
                var tabpage = new TabPage();
                tabpage.Name = string.Format("tabPage{0}", i);
                tabpage.Text = string.Format("tabPage{0}", i);
                tabPages.Add(tabpage.Name, tabpage);

                var cmdControl = new CmdControl();
                cmdControl.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
                cmdControl.Location = new System.Drawing.Point(6, 7);
                cmdControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                cmdControl.Name = string.Format("cmdControl{0}", i);
                cmdControl.Size = new System.Drawing.Size(908, 519);
                cmdControl.TabIndex = 6;
                tabpage.Controls.Add(cmdControl);

                tabControl1.Controls.Add(tabpage);
            }

            this.ResumeLayout(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(cmdControl1.p == null)
            {
                Task.Run(cmdControl1.Task);
            }
            else
            {
                MessageBox.Show(
                    "同時に二つのプロセスを起動できません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cmdControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
