using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VolumeWheel
{
    public partial class frmOptions : VolumeWheel.CustomForm
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            chkVolumeBar.Checked = Properties.Settings.Default.ShowVolumeBar;

            btnVolumeBarColorFrom.BackColor = Properties.Settings.Default.VolumeColorFrom;

            btnVolumeBarColorTo.BackColor = Properties.Settings.Default.VolumeColorTo;

            btnVolumeBarNumbersColor.BackColor = Properties.Settings.Default.VolumeColorNumbers;

            chkVolumeValueNumbers.Checked = Properties.Settings.Default.ShowVolumeValues;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowVolumeBar = chkVolumeBar.Checked;

            Properties.Settings.Default.VolumeColorFrom = btnVolumeBarColorFrom.BackColor;

            Properties.Settings.Default.VolumeColorTo = btnVolumeBarColorTo.BackColor;

            Properties.Settings.Default.VolumeColorNumbers = btnVolumeBarNumbersColor.BackColor;

            Properties.Settings.Default.ShowVolumeValues = chkVolumeValueNumbers.Checked;

            this.DialogResult = DialogResult.OK;
        }

        private void btnVolumeBarColorFrom_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            cd.FullOpen = true;

            if (sender==btnVolumeBarColorFrom)
            {
                cd.Color = btnVolumeBarColorFrom.BackColor;
            }
            else if (sender==btnVolumeBarColorTo)
            {
                cd.Color = btnVolumeBarColorTo.BackColor;
            }
            else if (sender==btnVolumeBarNumbersColor)
            {
                cd.Color = btnVolumeBarNumbersColor.BackColor;
            }

            if (cd.ShowDialog()==DialogResult.OK)
            {
                if (sender == btnVolumeBarColorFrom)
                {
                    btnVolumeBarColorFrom.BackColor = cd.Color;
                }
                else if (sender == btnVolumeBarColorTo)
                {
                    btnVolumeBarColorTo.BackColor = cd.Color;
                }
                else if (sender == btnVolumeBarNumbersColor)
                {
                    btnVolumeBarNumbersColor.BackColor = cd.Color;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
