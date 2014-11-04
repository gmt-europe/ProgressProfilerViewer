namespace ProgressProfilerViewer
{
    partial class ProfilerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this._callTreePage = new System.Windows.Forms.TabPage();
            this._callTree = new ProgressProfilerViewer.CallTreeUserControl();
            this._functionsPage = new System.Windows.Forms.TabPage();
            this._functions = new ProgressProfilerViewer.FunctionsUserControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._refresh = new System.Windows.Forms.ToolStripButton();
            this.tabControl1.SuspendLayout();
            this._callTreePage.SuspendLayout();
            this._functionsPage.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this._callTreePage);
            this.tabControl1.Controls.Add(this._functionsPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(966, 557);
            this.tabControl1.TabIndex = 0;
            // 
            // _callTreePage
            // 
            this._callTreePage.Controls.Add(this._callTree);
            this._callTreePage.Location = new System.Drawing.Point(4, 22);
            this._callTreePage.Name = "_callTreePage";
            this._callTreePage.Padding = new System.Windows.Forms.Padding(3);
            this._callTreePage.Size = new System.Drawing.Size(958, 531);
            this._callTreePage.TabIndex = 0;
            this._callTreePage.Text = "Call Tree";
            this._callTreePage.UseVisualStyleBackColor = true;
            // 
            // _callTree
            // 
            this._callTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._callTree.Location = new System.Drawing.Point(3, 3);
            this._callTree.Name = "_callTree";
            this._callTree.Size = new System.Drawing.Size(952, 525);
            this._callTree.TabIndex = 3;
            // 
            // _functionsPage
            // 
            this._functionsPage.Controls.Add(this._functions);
            this._functionsPage.Location = new System.Drawing.Point(4, 22);
            this._functionsPage.Name = "_functionsPage";
            this._functionsPage.Padding = new System.Windows.Forms.Padding(3);
            this._functionsPage.Size = new System.Drawing.Size(958, 531);
            this._functionsPage.TabIndex = 1;
            this._functionsPage.Text = "Functions";
            this._functionsPage.UseVisualStyleBackColor = true;
            // 
            // _functions
            // 
            this._functions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._functions.Location = new System.Drawing.Point(3, 3);
            this._functions.Name = "_functions";
            this._functions.Size = new System.Drawing.Size(952, 525);
            this._functions.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._refresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(966, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _refresh
            // 
            this._refresh.Image = global::ProgressProfilerViewer.Properties.Resources.refresh;
            this._refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._refresh.Name = "_refresh";
            this._refresh.Size = new System.Drawing.Size(66, 22);
            this._refresh.Text = "&Refresh";
            this._refresh.Click += new System.EventHandler(this._refresh_Click);
            // 
            // ProfilerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 582);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ProfilerForm";
            this.Text = "ProfilerForm";
            this.Shown += new System.EventHandler(this.ProfilerForm_Shown);
            this.tabControl1.ResumeLayout(false);
            this._callTreePage.ResumeLayout(false);
            this._functionsPage.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage _callTreePage;
        private System.Windows.Forms.TabPage _functionsPage;
        private FunctionsUserControl _functions;
        private CallTreeUserControl _callTree;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _refresh;


    }
}