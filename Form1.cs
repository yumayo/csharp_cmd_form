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
        ProcessStartInfo startInfo = new ProcessStartInfo();
        Process p;

        string output = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox2.KeyDown += richTextBox2_KeyDown;
            richTextBox2.KeyPress += richTextBox2_KeyPress;
            richTextBox2.GotFocus += richTextBox2_GotFocus;

            startInfo.FileName = @"..\..\test.bat";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
        }

        delegate void ProcessDelegate(Process p);
        delegate void TextDelegate(string text);

        private void button1_Click(object sender, EventArgs e)
        {
            if(p == null)
            {
                Task.Run(Test);
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async Task Test()
        {
            p = Process.Start(startInfo);
            await Read(p);

            // 子プロセスの標準入力を閉じて書き込みを終了する
            p.StandardInput.Close();

            p.Dispose();
            p = null;
        }

        public void RichTextBox2TextAddText(string text)
        {
            output = richTextBox2.Text + text;
            richTextBox2.Text += text;
        }

        private async Task Read(Process p)
        {
            while (!p.StandardOutput.EndOfStream)
            {
                char[] buffer = new char[1024];
                int count = await p.StandardOutput.ReadAsync(buffer, 0, 1024);
                string chunk = new string(buffer, 0, count);
                chunk = chunk.Replace("\r\n", "\n");
                TextDelegate d = new TextDelegate(RichTextBox2TextAddText);
                this.Invoke(d, chunk);
            }
            int a = 0;
        }

        private string input = string.Empty;

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            // Read以外で呼ばれた。
            if (output.Length != richTextBox2.Text.Length)
            {
                input = richTextBox2.Text.Substring(output.Length);
            }
        }

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                e.Handled = richTextBox2.Text.Length == output.Length;
            }
            if (e.KeyCode == Keys.Return)
            {
                if (p != null)
                {
                    p.StandardInput.WriteLine(input);
                }
            }
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete)
            {
                e.Handled = richTextBox2.Text.Length == output.Length;
            }
        }

        private void richTextBox2_GotFocus(object sender, EventArgs e)
        {
            richTextBox2.Select(0, 0);
        }
    }
}
