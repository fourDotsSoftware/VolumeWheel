namespace VolumeWheel
{
    partial class frmOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.label3 = new System.Windows.Forms.Label();
            this.chkVolumeBar = new System.Windows.Forms.CheckBox();
            this.btnVolumeBarColorFrom = new System.Windows.Forms.Button();
            this.btnVolumeBarColorTo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnVolumeBarNumbersColor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkVolumeValueNumbers = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.DarkBlue;
            this.label3.Name = "label3";
            // 
            // chkVolumeBar
            // 
            resources.ApplyResources(this.chkVolumeBar, "chkVolumeBar");
            this.chkVolumeBar.BackColor = System.Drawing.Color.Transparent;
            this.chkVolumeBar.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkVolumeBar.Name = "chkVolumeBar";
            this.chkVolumeBar.UseVisualStyleBackColor = false;
            // 
            // btnVolumeBarColorFrom
            // 
            this.btnVolumeBarColorFrom.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.btnVolumeBarColorFrom, "btnVolumeBarColorFrom");
            this.btnVolumeBarColorFrom.Name = "btnVolumeBarColorFrom";
            this.btnVolumeBarColorFrom.UseVisualStyleBackColor = true;
            this.btnVolumeBarColorFrom.Click += new System.EventHandler(this.btnVolumeBarColorFrom_Click);
            // 
            // btnVolumeBarColorTo
            // 
            this.btnVolumeBarColorTo.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.btnVolumeBarColorTo, "btnVolumeBarColorTo");
            this.btnVolumeBarColorTo.Name = "btnVolumeBarColorTo";
            this.btnVolumeBarColorTo.UseVisualStyleBackColor = true;
            this.btnVolumeBarColorTo.Click += new System.EventHandler(this.btnVolumeBarColorFrom_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Name = "label1";
            // 
            // btnVolumeBarNumbersColor
            // 
            this.btnVolumeBarNumbersColor.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.btnVolumeBarNumbersColor, "btnVolumeBarNumbersColor");
            this.btnVolumeBarNumbersColor.Name = "btnVolumeBarNumbersColor";
            this.btnVolumeBarNumbersColor.UseVisualStyleBackColor = true;
            this.btnVolumeBarNumbersColor.Click += new System.EventHandler(this.btnVolumeBarColorFrom_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Name = "label2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Image = global::VolumeWheel.Properties.Resources.check;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Image = global::VolumeWheel.Properties.Resources.exit;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkVolumeValueNumbers
            // 
            resources.ApplyResources(this.chkVolumeValueNumbers, "chkVolumeValueNumbers");
            this.chkVolumeValueNumbers.BackColor = System.Drawing.Color.Transparent;
            this.chkVolumeValueNumbers.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkVolumeValueNumbers.Name = "chkVolumeValueNumbers";
            this.chkVolumeValueNumbers.UseVisualStyleBackColor = false;
            // 
            // frmOptions
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.chkVolumeValueNumbers);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnVolumeBarNumbersColor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnVolumeBarColorTo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnVolumeBarColorFrom);
            this.Controls.Add(this.chkVolumeBar);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkVolumeBar;
        private System.Windows.Forms.Button btnVolumeBarColorFrom;
        private System.Windows.Forms.Button btnVolumeBarColorTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnVolumeBarNumbersColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkVolumeValueNumbers;
    }
}
