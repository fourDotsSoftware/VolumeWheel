namespace VolumeWheel
{
    partial class ucWheelOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucWheelOption));
            this.chkShift = new System.Windows.Forms.CheckBox();
            this.chkAlt = new System.Windows.Forms.CheckBox();
            this.chkControl = new System.Windows.Forms.CheckBox();
            this.txtHotKey = new System.Windows.Forms.TextBox();
            this.lblShortcutKeys = new System.Windows.Forms.Label();
            this.cmbOption = new System.Windows.Forms.ComboBox();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.cmbChannels = new System.Windows.Forms.ComboBox();
            this.cmbStep = new System.Windows.Forms.ComboBox();
            this.lblEXE = new System.Windows.Forms.Label();
            this.txtEXEFilename = new System.Windows.Forms.TextBox();
            this.btnBrowseEXE = new System.Windows.Forms.Button();
            this.btnClearHotKey = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkShift
            // 
            resources.ApplyResources(this.chkShift, "chkShift");
            this.chkShift.BackColor = System.Drawing.Color.Transparent;
            this.chkShift.Checked = true;
            this.chkShift.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShift.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkShift.Name = "chkShift";
            this.chkShift.UseVisualStyleBackColor = false;
            // 
            // chkAlt
            // 
            resources.ApplyResources(this.chkAlt, "chkAlt");
            this.chkAlt.BackColor = System.Drawing.Color.Transparent;
            this.chkAlt.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkAlt.Name = "chkAlt";
            this.chkAlt.UseVisualStyleBackColor = false;
            // 
            // chkControl
            // 
            resources.ApplyResources(this.chkControl, "chkControl");
            this.chkControl.BackColor = System.Drawing.Color.Transparent;
            this.chkControl.Checked = true;
            this.chkControl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkControl.ForeColor = System.Drawing.Color.DarkBlue;
            this.chkControl.Name = "chkControl";
            this.chkControl.UseVisualStyleBackColor = false;
            // 
            // txtHotKey
            // 
            resources.ApplyResources(this.txtHotKey, "txtHotKey");
            this.txtHotKey.Name = "txtHotKey";
            this.txtHotKey.Enter += new System.EventHandler(this.txtHotKey_Enter);
            this.txtHotKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHotKey_KeyDown);
            this.txtHotKey.Leave += new System.EventHandler(this.txtHotKey_Leave);
            // 
            // lblShortcutKeys
            // 
            resources.ApplyResources(this.lblShortcutKeys, "lblShortcutKeys");
            this.lblShortcutKeys.BackColor = System.Drawing.Color.Transparent;
            this.lblShortcutKeys.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblShortcutKeys.Name = "lblShortcutKeys";
            // 
            // cmbOption
            // 
            this.cmbOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbOption, "cmbOption");
            this.cmbOption.FormattingEnabled = true;
            this.cmbOption.Name = "cmbOption";
            this.cmbOption.SelectedIndexChanged += new System.EventHandler(this.cmbOption_SelectedIndexChanged);
            // 
            // cmbDevice
            // 
            this.cmbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbDevice, "cmbDevice");
            this.cmbDevice.FormattingEnabled = true;
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.SelectedIndexChanged += new System.EventHandler(this.cmbDevice_SelectedIndexChanged);
            // 
            // cmbChannels
            // 
            this.cmbChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbChannels, "cmbChannels");
            this.cmbChannels.FormattingEnabled = true;
            this.cmbChannels.Name = "cmbChannels";
            // 
            // cmbStep
            // 
            this.cmbStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbStep, "cmbStep");
            this.cmbStep.FormattingEnabled = true;
            this.cmbStep.Name = "cmbStep";
            // 
            // lblEXE
            // 
            resources.ApplyResources(this.lblEXE, "lblEXE");
            this.lblEXE.BackColor = System.Drawing.Color.Transparent;
            this.lblEXE.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblEXE.Name = "lblEXE";
            // 
            // txtEXEFilename
            // 
            resources.ApplyResources(this.txtEXEFilename, "txtEXEFilename");
            this.txtEXEFilename.Name = "txtEXEFilename";
            // 
            // btnBrowseEXE
            // 
            resources.ApplyResources(this.btnBrowseEXE, "btnBrowseEXE");
            this.btnBrowseEXE.Name = "btnBrowseEXE";
            this.btnBrowseEXE.UseVisualStyleBackColor = true;
            this.btnBrowseEXE.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnClearHotKey
            // 
            resources.ApplyResources(this.btnClearHotKey, "btnClearHotKey");
            this.btnClearHotKey.Image = global::VolumeWheel.Properties.Resources.delete;
            this.btnClearHotKey.Name = "btnClearHotKey";
            this.btnClearHotKey.UseVisualStyleBackColor = true;
            this.btnClearHotKey.Click += new System.EventHandler(this.btnClearHotKey_Click);
            // 
            // ucWheelOption
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClearHotKey);
            this.Controls.Add(this.btnBrowseEXE);
            this.Controls.Add(this.txtEXEFilename);
            this.Controls.Add(this.lblEXE);
            this.Controls.Add(this.cmbStep);
            this.Controls.Add(this.cmbChannels);
            this.Controls.Add(this.cmbDevice);
            this.Controls.Add(this.cmbOption);
            this.Controls.Add(this.lblShortcutKeys);
            this.Controls.Add(this.chkShift);
            this.Controls.Add(this.chkAlt);
            this.Controls.Add(this.chkControl);
            this.Controls.Add(this.txtHotKey);
            this.Name = "ucWheelOption";
            this.Load += new System.EventHandler(this.ucWheelOption_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblShortcutKeys;
        private System.Windows.Forms.Label lblEXE;
        private System.Windows.Forms.Button btnBrowseEXE;
        private System.Windows.Forms.Button btnClearHotKey;
        public System.Windows.Forms.TextBox txtEXEFilename;
        public System.Windows.Forms.ComboBox cmbOption;
        public System.Windows.Forms.ComboBox cmbDevice;
        public System.Windows.Forms.ComboBox cmbChannels;
        public System.Windows.Forms.ComboBox cmbStep;
        public System.Windows.Forms.CheckBox chkShift;
        public System.Windows.Forms.CheckBox chkAlt;
        public System.Windows.Forms.CheckBox chkControl;
        public System.Windows.Forms.TextBox txtHotKey;
    }
}
