using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProgressProfilerViewer
{
    public partial class MainForm : SystemEx.Windows.Forms.Form
    {
        private readonly string[] _args;

        public MainForm(string[] args)
        {
            _args = args;

            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new OpenFileDialog())
            {
                form.CheckFileExists = true;
                form.Filter = "All Files (*.*)|*.*";
                form.Multiselect = false;
                form.RestoreDirectory = true;

                if (form.ShowDialog(this) == DialogResult.OK)
                    OpenFile(form.FileName);
            }
        }

        private void OpenFile(string fileName)
        {
            new ProfilerForm(fileName).Show(_dockPanel);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            foreach (string arg in _args)
            {
                if (File.Exists(arg))
                    OpenFile(arg);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
