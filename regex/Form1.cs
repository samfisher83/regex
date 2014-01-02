using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace WindowsFormsApplication1
{
    


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            inputText.Pasted += new EventHandler<ClipboardEventArgs>(inputText_Pasted);
            
        }

        void inputText_Pasted(object sender, ClipboardEventArgs e)
        {
            if (checkBox1.Checked)
            {
                string[] lines = e.ClipboardText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                int i = 0;
                StringWriter text = new StringWriter();
                foreach (var item in lines)
                {
                    text.WriteLine(++i + ": " + item);
                }
                inputText.Text = text.ToString(); ;
            }
            else
            {
                ((TextBox)sender).Text = e.ClipboardText;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void DoSearch()
        {
            string[] splitstring = Regex.Split(inputText.Text, Environment.NewLine);
            // Regex pattern = new Regex("Register(.*)=(.*)");
            Regex pattern = null;
            try
            {
                pattern = new Regex(inputSeach.Text);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return;
            }
            
            int lineCount = 0;

            StringWriter text = new StringWriter();
            foreach (string item in splitstring)
            {
                lineCount++;
                Match match = pattern.Match(item);
                //textBox2.Text += string.Format("{0} {1}",match.Groups[1],match.Groups[2].Value) + Environment.NewLine;


                List<string> values = new List<string>();
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    values.Add(match.Groups[i].Value);
                }
                //textBox2.Text += string.Format("{0} \r\n", string.Join(" ", values.ToArray()));
                try
                {
                    if (values.Count != 0)
                    {
                        if (checkBox1.Checked)
                            text.WriteLine(string.Format(lineCount + ": " + outputString.Text, values.ToArray()));
                        else
                            text.WriteLine(string.Format(outputString.Text, values.ToArray()));
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

            }

            outputText.Text = text.ToString();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DoSearch();
        }
    }
    public class ClipboardEventArgs : EventArgs
    {
        public string ClipboardText { get; set; }
        public ClipboardEventArgs(string clipboardText)
        {
            ClipboardText = clipboardText;
        }
    }

    class MyTextBox : TextBox
    {
        public event EventHandler<ClipboardEventArgs> Pasted;

        private const int WM_PASTE = 0x0302;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PASTE)
            {
                var evt = Pasted;
                if (evt != null)
                {
                    evt(this, new ClipboardEventArgs(Clipboard.GetText()));
                }
            }
            else
            {

                base.WndProc(ref m);
            }
        }
    }
}
