using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ProgressProfilerViewer.Dto;

namespace ProgressProfilerViewer
{
    public partial class ProfilerForm : SystemEx.Windows.Forms.DockContent
    {
        private readonly string _fileName;

        public ProfilerForm(string fileName)
        {
            _fileName = fileName;

            InitializeComponent();

            Text = Path.GetFileName(fileName);

        }

        private void ProfilerForm_Shown(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void ReloadData()
        {
            var session = Session.Load(_fileName);

            _functions.LoadFunctions(session);
            _callTree.LoadFunctions(session);
        }

        private void _refresh_Click(object sender, EventArgs e)
        {
            ReloadData();
        }
    }
}
