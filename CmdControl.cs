using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class CmdControl : UserControl
    {
        private ProcessStartInfo startInfo = new ProcessStartInfo();
        public Process p;

        char[] buffer = new char[1024];

        delegate void ProcessDelegate(Process p);
        delegate void TextDelegate(string text);

        public CmdControl()
        {
            InitializeComponent();
        }

        public async Task Task()
        {
            p = Process.Start(startInfo);
            await Read(p);

            // 子プロセスの標準入力を閉じて書き込みを終了する
            p.StandardInput.Close();

            p.Dispose();
            p = null;
        }

        private void CmdControl_Load(object sender, EventArgs e)
        {
            startInfo.FileName = @"..\..\test.bat";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;

            textBox1.KeyDown += textBox1_KeyDown;
        }

        private void RichTextBox1TextAddText(string text)
        {
            richTextBox1.Text += text;
        }

        private async Task Read(Process p)
        {
            while (!p.StandardOutput.EndOfStream)
            {
                int count = await p.StandardOutput.ReadAsync(buffer, 0, 1024);
                string chunk = new string(buffer, 0, count);
                chunk = chunk.Replace("\r\n", "\n");
                TextDelegate d = new TextDelegate(RichTextBox1TextAddText);
                this.Invoke(d, chunk);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (p != null)
                {
                    p.StandardInput.WriteLine(textBox1.Text);
                    richTextBox1.Text += textBox1.Text;
                    textBox1.Text = string.Empty;
                    richTextBox1.Select(richTextBox1.TextLength, 0);
                }
            }
        }
    }
}
