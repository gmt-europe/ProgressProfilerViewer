namespace ProgressProfilerViewer
{
    partial class FunctionsUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.themedPanel1 = new SystemEx.Windows.Forms.ThemedPanel();
            this._grid = new SourceGrid.Grid();
            this.themedPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // themedPanel1
            // 
            this.themedPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.themedPanel1.Controls.Add(this._grid);
            this.themedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themedPanel1.Location = new System.Drawing.Point(0, 0);
            this.themedPanel1.Name = "themedPanel1";
            this.themedPanel1.Size = new System.Drawing.Size(631, 460);
            this.themedPanel1.TabIndex = 5;
            // 
            // _grid
            // 
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.EnableSort = true;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Name = "_grid";
            this._grid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this._grid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this._grid.Size = new System.Drawing.Size(629, 458);
            this._grid.TabIndex = 0;
            this._grid.TabStop = true;
            this._grid.ToolTipText = "";
            // 
            // FunctionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.themedPanel1);
            this.Name = "FunctionsUserControl";
            this.Size = new System.Drawing.Size(631, 460);
            this.themedPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SystemEx.Windows.Forms.ThemedPanel themedPanel1;
        private SourceGrid.Grid _grid;
    }
}
