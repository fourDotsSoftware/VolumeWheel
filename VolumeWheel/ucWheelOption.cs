using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System.Media;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VolumeWheel
{
    public partial class ucWheelOption : UserControl
    {
        private MMDevice device;

        public MMDevice Device
        {
            get
            {
                return device;
            }
        }

        public int HotKey = -1;
        public string HotKeyStr = "";

        public bool Control = false;
        public bool Shift = false;
        public bool Alt = false;

        private bool InSpecifyHotKey = false;

        public KeyboardHook keyboardHook = new KeyboardHook();
        public MouseHook mouseHook = null;

        public bool PressedHotKey = false;

        private int OVER_THE_CORNERS_INDEX;
        private int OVER_THE_EDGES_INDEX;
        private int OVER_THE_TOP_BOTTOM_EDGES_INDEX;
        private int OVER_THE_LEFT_RIGHT_EDGES_INDEX;
        private int OVER_THE_TASKBAR;
        private int SPECIFIC_PROGRAM_FOCUSED;
        private int SPECIFIC_PROGRAM_FOCUSED_HOTKEY;
        private int SPECIFIC_PROGRAM;
        private int OVER_TITLEBAR_SPECIFIC_PROGRAM;
        public int DISABLE;

        public OptionEvaluator OptionEvaluator = new OptionEvaluator(0);

        private bool InitializedHotKey = false;

        public bool OverTitleBar = false;

        public int OverTitleBarProcessId = -1;

        public ucWheelOption()
        {
            InitializeComponent();            

            var deviceEnumerator = new MMDeviceEnumerator();
            device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            cmbDevice.Visible = true;

            foreach (var d in deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.All))
            {
                cmbDevice.Items.Add(d);

                if (d.ID == device.ID)
                {
                    cmbDevice.SelectedItem = d;
                    d.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                }
            }

            cmbDevice_SelectedIndexChanged(null, null);

            cmbStep.Items.Add("1");
            cmbStep.Items.Add("5");
            cmbStep.Items.Add("10");
            cmbStep.Items.Add("20");
            cmbStep.Items.Add("30");
            cmbStep.Items.Add("40");
            cmbStep.Items.Add("50");
            cmbStep.Items.Add("60");
            cmbStep.Items.Add("70");
            cmbStep.Items.Add("80");
            cmbStep.Items.Add("100");
            cmbStep.Items.Add(TranslateHelper.Translate("Mute / Unmute"));

            cmbStep.SelectedIndex = 2;                       

            cmbOption.Items.Add(TranslateHelper.Translate("Control + Shift Key is pressed"));
            cmbOption.Items.Add(TranslateHelper.Translate("Control + Alt Key is pressed"));
            cmbOption.Items.Add(TranslateHelper.Translate("Shift + Alt Key is pressed"));
            cmbOption.Items.Add(TranslateHelper.Translate("Control Key is pressed"));
            cmbOption.Items.Add(TranslateHelper.Translate("Shift Key is pressed"));
            cmbOption.Items.Add(TranslateHelper.Translate("Alt Key is pressed"));
            cmbOption.Items.Add(TranslateHelper.Translate("Custom Key Combination is pressed"));
            cmbOption.Items.Add(TranslateHelper.Translate("Mouse is at the Edges of Screen"));
            cmbOption.Items.Add(TranslateHelper.Translate("Mouse is at the Top, Bottom Edges of Screen"));
            cmbOption.Items.Add(TranslateHelper.Translate("Mouse is at the Left, Right Edges of Screen"));
            cmbOption.Items.Add(TranslateHelper.Translate("Mouse is at the Corners of Screen"));
            cmbOption.Items.Add(TranslateHelper.Translate("Mouse is above Taskbar"));
            cmbOption.Items.Add(TranslateHelper.Translate("Specific Program is focused"));
            cmbOption.Items.Add(TranslateHelper.Translate("Specific Program is focused and Hot Key is pressed"));
            cmbOption.Items.Add(TranslateHelper.Translate("For Specific Program and Hot Key is pressed"));
            cmbOption.Items.Add(TranslateHelper.Translate("Mouse is over the Title Bar"));
            cmbOption.Items.Add(TranslateHelper.Translate("Mouse is over the Title Bar, for Specific Program"));
            cmbOption.Items.Add(TranslateHelper.Translate("Disable"));

            cmbOption.SelectedIndex = cmbOption.Items.Count - 1;

            mouseHook = new MouseHook(this);

            keyboardHook.KeyDown += KeyboardHook_KeyDown;
            mouseHook.MouseWheel += MouseHook_MouseWheel;
            mouseHook.MouseDown += MouseHook_MouseDown;

            mouseHook.MouseMoveTitleBar += MouseHook_MouseMoveTitleBar;
            mouseHook.MouseLeaveTitleBar += MouseHook_MouseLeaveTitleBar;

        }

        private void MouseHook_MouseLeaveTitleBar(object sender, MouseEventArgs e)
        {
            OverTitleBar = false;
        }

        private void MouseHook_MouseMoveTitleBar(object sender, MouseEventArgs e)
        {
            OverTitleBar = true;
        }

        private void LoadOptions()
        {
            this.Height = cmbOption.Bottom + 5;

            OVER_THE_EDGES_INDEX = 7;
            OVER_THE_TOP_BOTTOM_EDGES_INDEX = 8;
            OVER_THE_LEFT_RIGHT_EDGES_INDEX = 9;
            OVER_THE_CORNERS_INDEX = 10;
            OVER_THE_TASKBAR = 11;
            SPECIFIC_PROGRAM_FOCUSED = 12;
            SPECIFIC_PROGRAM_FOCUSED_HOTKEY = 13;
            SPECIFIC_PROGRAM = 14;
            OVER_TITLEBAR_SPECIFIC_PROGRAM = 16;
            DISABLE = 17;

            lblShortcutKeys.Visible = false;
            txtHotKey.Visible = false;
            chkAlt.Visible = false;
            chkControl.Visible = false;
            chkShift.Visible = false;
            btnClearHotKey.Visible = false;

            txtEXEFilename.Visible = false;
            lblEXE.Visible = false;
            btnBrowseEXE.Visible = false;

            if (cmbOption.SelectedIndex == 0)
            {
                Control = true;
                Shift = true;
                Alt = false;
                HotKey = -1;
                HotKeyStr = "";
            }
            else if (cmbOption.SelectedIndex == 1)
            {
                Control = true;
                Shift = false;
                Alt = true;
                HotKey = -1;
                HotKeyStr = "";
            }
            else if (cmbOption.SelectedIndex == 2)
            {
                Control = false;
                Shift = true;
                Alt = true;
                HotKey = -1;
                HotKeyStr = "";
            }
            else if (cmbOption.SelectedIndex == 3)
            {
                Control = true;
                Shift = false;
                Alt = false;
                HotKey = -1;
                HotKeyStr = "";
            }
            else if (cmbOption.SelectedIndex == 4)
            {
                Control = false;
                Shift = true;
                Alt = false;
                HotKey = -1;
                HotKeyStr = "";
            }
            else if (cmbOption.SelectedIndex == 5)
            {
                Control = false;
                Shift = false;
                Alt = true;
                HotKey = -1;
                HotKeyStr = "";
            }
            else if (cmbOption.SelectedIndex == 6)
            {
                lblShortcutKeys.Visible = true;
                txtHotKey.Visible = true;
                chkAlt.Visible = true;
                chkControl.Visible = true;
                chkShift.Visible = true;
                btnClearHotKey.Visible = true;

                if (!InitializedHotKey)
                {
                    Control = true;
                    Alt = false;
                    Shift = false;

                    chkControl.Checked = true;
                    chkShift.Checked = false;
                    chkAlt.Checked = false;                    

                    InitializedHotKey = true;
                }

                this.Height = chkShift.Bottom + 5;
            }
            else if (cmbOption.SelectedIndex == SPECIFIC_PROGRAM_FOCUSED)
            {
                txtEXEFilename.Visible = true;
                lblEXE.Visible = true;
                btnBrowseEXE.Visible = true;

                txtEXEFilename.Top = txtHotKey.Top;
                lblEXE.Top = txtHotKey.Top;

                btnBrowseEXE.Top = txtEXEFilename.Top - 3;
                btnBrowseEXE.Left = txtEXEFilename.Right + 5;


                this.Height = txtEXEFilename.Bottom + 8;
            }
            else if (cmbOption.SelectedIndex == SPECIFIC_PROGRAM_FOCUSED_HOTKEY)
            {
                txtEXEFilename.Visible = true;
                lblEXE.Visible = true;
                btnBrowseEXE.Visible = true;

                lblShortcutKeys.Visible = true;
                txtHotKey.Visible = true;
                chkAlt.Visible = true;
                chkControl.Visible = true;
                chkShift.Visible = true;
                btnClearHotKey.Visible = true;
                btnClearHotKey.Top = txtHotKey.Top - 3;

                if (!InitializedHotKey)
                {
                    Control = true;
                    Alt = false;
                    Shift = false;

                    chkControl.Checked = true;
                    chkShift.Checked = false;
                    chkAlt.Checked = false;

                    InitializedHotKey = true;
                }

                txtEXEFilename.Top = chkShift.Bottom + 3;
                lblEXE.Top = chkShift.Bottom + 3;

                btnBrowseEXE.Top = txtEXEFilename.Top - 3;
                btnBrowseEXE.Left = txtEXEFilename.Right + 5;

                this.Height = txtEXEFilename.Bottom + 8;
            }
            else if (cmbOption.SelectedIndex == SPECIFIC_PROGRAM)
            {
                txtEXEFilename.Visible = true;
                lblEXE.Visible = true;
                btnBrowseEXE.Visible = true;

                lblShortcutKeys.Visible = true;
                txtHotKey.Visible = true;
                chkAlt.Visible = true;
                chkControl.Visible = true;
                chkShift.Visible = true;
                btnClearHotKey.Visible = true;
                btnClearHotKey.Top = txtHotKey.Top - 3;

                if (!InitializedHotKey)
                {
                    Control = true;
                    Alt = false;
                    Shift = false;

                    chkControl.Checked = true;
                    chkShift.Checked = false;
                    chkAlt.Checked = false;

                    InitializedHotKey = true;
                }

                txtEXEFilename.Top = chkShift.Bottom + 3;
                lblEXE.Top = chkShift.Bottom + 3;

                btnBrowseEXE.Top = txtEXEFilename.Top - 3;
                btnBrowseEXE.Left = txtEXEFilename.Right + 5;

                this.Height = txtEXEFilename.Bottom + 8;
            }

        }

        private void MouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            if (!PressedHotKey) return;
        }

        private void MouseHook_MouseWheel(object sender, MouseEventArgs e)
        {
            bool enable = false;

            bool specificProgram = false;

            if (cmbOption.SelectedIndex == DISABLE)
            {
                return;
            }
            if (OptionEvaluator.IsSpecificProgram(this))
            {
                enable = true;

                specificProgram = true;
            }
            else if (cmbOption.SelectedIndex == SPECIFIC_PROGRAM_FOCUSED_HOTKEY)
            {
                enable = (OptionEvaluator.IsCurrentlyFocusedApplication(this) && PressedHotKey);
            }
            else if (OptionEvaluator.IsOverTheCorners() ||
               OptionEvaluator.IsOverTheEdges() ||
               OptionEvaluator.IsOverTheTaskbar() ||
               OptionEvaluator.IsOverTheTitleBar(this) ||
               OptionEvaluator.IsOverTheTopBottomEdges() ||
               OptionEvaluator.IsOverTheLeftRightEdges() ||
               OptionEvaluator.IsCurrentlyFocusedApplication(this)
               )
            {
                enable = true;

                if (cmbOption.SelectedIndex==OVER_TITLEBAR_SPECIFIC_PROGRAM)
                {
                    specificProgram = true;
                }
            }
            else if ((cmbOption.SelectedIndex>=0 && cmbOption.SelectedIndex<=6) && PressedHotKey)
            {
                enable = true;
            }

            if (enable)
            {
                if (e.Delta > 0)
                {
                    IncreaseVolume(specificProgram);
                }
                else
                {
                    DecreaseVolume(specificProgram);
                }
            }
        }
        private void KeyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (InSpecifyHotKey) return;

            PressedHotKey = false;

            bool pressed = true;

            if (Control && ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) != Keys.Control))
            {
                pressed = false;

                return;
            }

            if (!Control && ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control))
            {
                pressed = false;

                return;
            }

            if (Shift && ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) != Keys.Shift))
            {
                pressed = false;

                return;
            }

            if (!Shift && ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift))
            {
                pressed = false;

                return;
            }

            if (Alt && ((System.Windows.Forms.Control.ModifierKeys & Keys.Alt) != Keys.Alt))
            {
                pressed = false;

                return;
            }

            if (!Alt && ((System.Windows.Forms.Control.ModifierKeys & Keys.Alt) == Keys.Alt))
            {
                pressed = false;

                return;
            }

            if (HotKey != -1)
            {
                if (HotKey == e.KeyValue)
                {
                    PressedHotKey = true;

                    return;
                }
            }
            else
            {
                if (!Control && !Alt && !Shift)
                {
                    PressedHotKey = false;

                    return;
                }

                PressedHotKey = true;


                return;
            }
        }



        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {

        }

        public void IncreaseVolume(bool specificProgram)
        {           
            try
            {
                float stepk = 0;

                if (cmbStep.SelectedItem.ToString() != TranslateHelper.Translate("Mute / Unmute"))
                {
                    stepk = int.Parse(cmbStep.SelectedItem.ToString());
                }
                
                float f100 = 100f;
                float step = stepk / f100;

                float newVol = 0;

                if (!specificProgram)
                {
                    if (cmbStep.SelectedItem.ToString() == TranslateHelper.Translate("Mute / Unmute"))
                    {
                        Device.AudioEndpointVolume.Mute = false;

                        newVol= Device.AudioEndpointVolume.MasterVolumeLevelScalar;
                    }
                    else if (cmbChannels.SelectedIndex == 0)
                    {
                        float vol = Device.AudioEndpointVolume.MasterVolumeLevelScalar;

                        decimal dvol = decimal.Round((decimal)vol, 2);

                        vol = (float)dvol;

                        Device.AudioEndpointVolume.MasterVolumeLevelScalar = Math.Min(1, vol + step);

                        newVol = Math.Min(1, vol + step);

                    }                    
                    else
                    {
                        float vol = Device.AudioEndpointVolume.Channels[cmbChannels.SelectedIndex - 1].VolumeLevelScalar;

                        decimal dvol = decimal.Round((decimal)vol, 2);

                        vol = (float)dvol;

                        Device.AudioEndpointVolume.Channels[cmbChannels.SelectedIndex - 1].VolumeLevelScalar = Math.Min(1, vol + step);

                        newVol = Math.Min(1, vol + step);
                    }
                }
                else
                {
                    var sessions = Device.AudioSessionManager.Sessions;

                    if (sessions == null) return;
                    /*                      
                    if (cmbOption.SelectedIndex==OVER_TITLEBAR_SPECIFIC_PROGRAM)
                    {
                        IntPtr hwnd = WindowFromPoint(System.Windows.Forms.Cursor.Position);

                        uint procid = GetWindowThreadProcessId(hwnd, IntPtr.Zero);

                        OptionEvaluator.LastSpecificProgramProcessId = (int)procid;
                    }
                    */

                    for (int i = 0; i < sessions.Count; i++)
                    {
                        var session = sessions[i];                        
                            
                            if (((cmbOption.SelectedIndex != OVER_TITLEBAR_SPECIFIC_PROGRAM) && ((int)session.GetProcessID == OptionEvaluator.LastSpecificProgramProcessId)
                            || ((cmbOption.SelectedIndex == OVER_TITLEBAR_SPECIFIC_PROGRAM) && ((int)session.GetProcessID == OverTitleBarProcessId))))
                            //3if (ProcessExists(session.GetProcessID))
                            {
                            /*
                            string filepath = ProcessFilenameRetriever.GetExecutablePath(System.Diagnostics.Process.GetProcessById((int)session.GetProcessID));

                            if (filepath == string.Empty) continue;

                            string fn = System.IO.Path.GetFileNameWithoutExtension(txtEXEFilename.Text.ToLower());

                            filepath = System.IO.Path.GetFileNameWithoutExtension(filepath).ToLower();

                            if (fn==filepath)
                            {*/
                            if (cmbStep.SelectedItem.ToString()== TranslateHelper.Translate("Mute / Unmute"))
                            {
                                newVol = session.SimpleAudioVolume.Volume;

                                session.SimpleAudioVolume.Mute = false;
                            }
                            else if (true)
                            {
                                float vol = session.SimpleAudioVolume.Volume;

                                decimal dvol = decimal.Round((decimal)vol, 2);

                                vol = (float)dvol;

                                newVol = Math.Min(1, vol + step);

                                session.SimpleAudioVolume.Volume = newVol;

                                break;
                            }
                        }
                    }
                }

                if (Properties.Settings.Default.ShowVolumeBar)
                {
                    Screen screen = Screen.FromPoint(System.Windows.Forms.Cursor.Position);
                    frmVoulme.Instance.Left = screen.Bounds.Width / 2 - frmVoulme.Instance.Width / 2;
                    frmVoulme.Instance.Top = screen.WorkingArea.Height - frmVoulme.Instance.Height - 100;
                    frmVoulme.Instance.Volume = newVol;
                    frmVoulme.Instance.TopMost = true;
                    frmVoulme.Instance.ShowVolume();
                }
            }
            catch { }
        }                        
        public void DecreaseVolume(bool specificProgram)
        {
            try
            {
                float stepk = 0;                

                if (cmbStep.SelectedItem.ToString() != TranslateHelper.Translate("Mute / Unmute"))
                {
                    stepk = int.Parse(cmbStep.SelectedItem.ToString());
                }                

                float f100 = 100f;
                float step = stepk / f100;
                float newVol = 0;                               

                if (!specificProgram)
                {
                    if (cmbStep.SelectedItem.ToString() == TranslateHelper.Translate("Mute / Unmute"))
                    {                        
                        Device.AudioEndpointVolume.Mute = true;
                    }
                    else if (cmbChannels.SelectedIndex == 0)
                    {
                        float vol = Device.AudioEndpointVolume.MasterVolumeLevelScalar;

                        decimal dvol = decimal.Round((decimal)vol, 2);

                        vol = (float)dvol;

                        Device.AudioEndpointVolume.MasterVolumeLevelScalar = Math.Max(0, vol - step);

                        newVol = Math.Max(0, vol - step);
                    }                    
                    else
                    {
                        float vol = Device.AudioEndpointVolume.Channels[cmbChannels.SelectedIndex - 1].VolumeLevelScalar;

                        decimal dvol = decimal.Round((decimal)vol, 2);

                        vol = (float)dvol;

                        Device.AudioEndpointVolume.Channels[cmbChannels.SelectedIndex - 1].VolumeLevelScalar = Math.Max(0, vol - step);

                        newVol = Math.Max(0, vol - step);
                    }
                }
                else
                {
                    var sessions = Device.AudioSessionManager.Sessions;

                    if (sessions == null) return;

                    for (int i = 0; i < sessions.Count; i++)
                    {
                        var session = sessions[i];                        

                        //3if (ProcessExists(session.GetProcessID))
                        if (
                            ((cmbOption.SelectedIndex != OVER_TITLEBAR_SPECIFIC_PROGRAM) && ((int)session.GetProcessID == OptionEvaluator.LastSpecificProgramProcessId))
                            || ((cmbOption.SelectedIndex==OVER_TITLEBAR_SPECIFIC_PROGRAM) && ((int)session.GetProcessID==OverTitleBarProcessId)))
                        {
                            /*
                            string filepath = ProcessFilenameRetriever.GetExecutablePath(System.Diagnostics.Process.GetProcessById((int)session.GetProcessID));

                            if (filepath == string.Empty) continue;

                            string fn = System.IO.Path.GetFileNameWithoutExtension(txtEXEFilename.Text.ToLower());

                            filepath = System.IO.Path.GetFileNameWithoutExtension(filepath).ToLower();

                            if (fn == filepath)*/

                            if (cmbStep.SelectedItem.ToString() == TranslateHelper.Translate("Mute / Unmute"))
                            {                               
                                session.SimpleAudioVolume.Mute = true;
                            }
                            else if (true)
                            {                               
                                float vol = session.SimpleAudioVolume.Volume;

                                decimal dvol = decimal.Round((decimal)vol, 2);

                                vol = (float)dvol;

                                newVol = Math.Max(0, vol - step);

                                session.SimpleAudioVolume.Volume = newVol;

                                break;
                            }
                        }
                    }
                }
                if (Properties.Settings.Default.ShowVolumeBar)
                {
                    Screen screen = Screen.FromPoint(System.Windows.Forms.Cursor.Position);
                    frmVoulme.Instance.Visible = false;
                    frmVoulme.Instance.Left = screen.Bounds.Width / 2 - frmVoulme.Instance.Width / 2;
                    frmVoulme.Instance.Top = screen.WorkingArea.Height - frmVoulme.Instance.Height - 100;
                    frmVoulme.Instance.Volume = newVol;
                    frmVoulme.Instance.TopMost = true;
                    frmVoulme.Instance.ShowVolume();
                }

            }
            catch { }
        }

        bool ProcessExists(uint processId)
        {
            try
            {
                var process = Process.GetProcessById((int)processId);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private void txtHotKey_KeyDown(object sender, KeyEventArgs e)
        {
            int if13 = (int)Keys.F13;
            int if14 = (int)Keys.F14;
            int if15 = (int)Keys.F15;
            int if16 = (int)Keys.F16;
            int if17 = (int)Keys.F17;

            int vkCode = (int)e.KeyData;

            if ((vkCode == if13) || (vkCode == if14) || (vkCode == if15) || (vkCode == if16) || (vkCode == if17))
            {
                return;
            }

                e.Handled = true;

            Control = e.Control;

            Alt = e.Alt;

            Shift = e.Shift;

            chkControl.Checked = Control;
            chkAlt.Checked = Alt;
            chkShift.Checked = Shift;

            HotKey = -1;
            HotKeyStr = "";


            if (!(e.KeyCode.ToString().Contains("Control") || e.KeyCode.ToString().Contains("Shift") || e.KeyCode.ToString().Contains("Alt") || e.KeyCode.ToString().Contains("Menu")))
            {
                txtHotKey.Text = e.KeyCode.ToString();
                
                HotKey = e.KeyValue;

                HotKeyStr = e.KeyCode.ToString();
            }
            else
            {
                txtHotKey.Text = "";
                HotKey = -1;
                HotKeyStr = "";
            }
        }

        public void cmbOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionEvaluator.ActionIndex = cmbOption.SelectedIndex;

            LoadOptions();
        }

        public void cmbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            var deviceEnumerator = new MMDeviceEnumerator();

            int selindex = cmbDevice.SelectedIndex;

            int k = 0;

            foreach (var d in deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.All))
            {
                if (k == selindex)
                {
                    device = d;

                    int cnt=d.AudioEndpointVolume.Channels.Count;

                    cmbChannels.Items.Clear();
                    cmbChannels.Items.Add(TranslateHelper.Translate("All Channels"));

                    if (cnt == 2)
                    {
                        cmbChannels.Items.Add(TranslateHelper.Translate("Left Channel"));
                        cmbChannels.Items.Add(TranslateHelper.Translate("Right Channel"));
                    }
                    else
                    {
                        for (int m = 1; m <= cnt; m++)
                        {
                            cmbChannels.Items.Add(TranslateHelper.Translate("Channel") + " " + m.ToString());
                        }
                    }

                    cmbChannels.SelectedIndex = 0;

                    break;
                }
                k++;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Executable Files (*.exe)|*.exe";

            if (fd.ShowDialog()==DialogResult.OK)
            {
                txtEXEFilename.Text = System.IO.Path.GetFileName(fd.FileName);
            }
        }

        private void txtHotKey_Enter(object sender, EventArgs e)
        {
            InSpecifyHotKey = true;
        }

        private void txtHotKey_Leave(object sender, EventArgs e)
        {
            InSpecifyHotKey = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
        }
        private void ucWheelOption_Load(object sender, EventArgs e)
        {
            
        }

        private void btnClearHotKey_Click(object sender, EventArgs e)
        {
            txtHotKey.Text = "";
            HotKey = -1;
        }
    }
}

