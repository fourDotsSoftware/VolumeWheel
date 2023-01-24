using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System.Media;
using System.Runtime.InteropServices;

namespace VolumeWheel
{
    public partial class frmMain : VolumeWheel.CustomForm
    {                       
        public static frmMain Instance = null;

        public string CurrentProject = "";

        public VolumeWheelProject VolumeWheelProject = new VolumeWheelProject();                

        public CurrentlyFocusedApplication CurrentlyFocusedApplication = new CurrentlyFocusedApplication();

        public bool IsStarted = false;

        private bool Hide = false;

        private bool ShownOnce = false;
        public frmMain()
        {
            InitializeComponent();

            Instance = this;

            //3Properties.Settings.Default.Initialized = false;                        

            //if (Properties.Settings.Default.Initialized && Properties.Settings.Default.MinimizeToTray)

            if ((ArgsManager.IsHidden) || (ArgsManager.IsNovisible))
            {
                this.ShowInTaskbar = false;
                this.Visible = false;
                this.Hide();
                WindowState = FormWindowState.Minimized;                
            }
            else if (!ArgsManager.IsMinimized)
            {
                WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            else if (ArgsManager.IsMinimized)
            {
                this.ShowInTaskbar = true;
            }
        }

        private void Start()
        {
            VolumeWheelProject.SaveProject(CurrentProject);

            for (int k = 0; k < WheelOptions.Count; k++)
            {
                WheelOptions[k].keyboardHook.Start();
                WheelOptions[k].mouseHook.Start();
            }

            IsStarted = true;

            timSetHookEnabled = true;
        }

        private void Stop()
        {
            for (int k = 0; k < WheelOptions.Count; k++)
            {
                WheelOptions[k].keyboardHook.Stop();
                WheelOptions[k].mouseHook.Stop();
            }

            IsStarted = false;

            timSetHookEnabled = false;
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            //Properties.Settings.Default.Initialized = false;
            fplWheelOptions.Controls.Clear();

            for (int k = 0; k < 5; k++)
            {
                btnAdd_Click(null, null);
            }

            bool loadedProject = false;

            if (System.IO.File.Exists(VolumeWheelProject.DefaultProject))
            {
                bool suc = VolumeWheelProject.LoadProject();

                loadedProject = suc;                
            }
            else
            {
                VolumeWheelProject.LoadedOnce = true;

                loadedProject = true;
            }

            if (!loadedProject)
            {
                RestartApplication();
                return;
            }

            tslProject.Text = VolumeWheelProject.DefaultProject;

            CurrentProject = VolumeWheelProject.DefaultProject;

            ucWheelOption wo = fplWheelOptions.Controls[0] as ucWheelOption;

            lblOption.Left = wo.cmbOption.Left;
            lblDevice.Left = wo.cmbDevice.Left;
            lblChannels.Left = wo.cmbChannels.Left;
            lblStep.Left = wo.cmbStep.Left;                       

            if (Properties.Settings.Default.CheckWeek)
            {
                UpdateHelper.InitializeCheckVersionWeek();
            }

            checkForNewVersionEachWeekToolStripMenuItem.Checked = Properties.Settings.Default.CheckWeek;
            minimizeToWindowsSystemTrayToolStripMenuItem.Checked = Properties.Settings.Default.MinimizeToTray;

            if (!Properties.Settings.Default.Initialized)
            {
                RunAtWndowsStartupManager.RunAtWindowsStartup = true;
            
                //3frmMessageCheckbox fm = new frmMessageCheckbox();
                //3fm.Show(this);

                Properties.Settings.Default.Initialized = true;

                Properties.Settings.Default.Save();

            }

            runAtWindowsStartupToolStripMenuItem.Checked = RunAtWndowsStartupManager.RunAtWindowsStartup;

            //3if (Properties.Settings.Default.Initialized && Properties.Settings.Default.MinimizeToTray)            

            SetTitle();

            //if (Properties.Settings.Default.Initialized && Properties.Settings.Default.MinimizeToTray)
            if (!(ArgsManager.IsHidden || ArgsManager.IsNovisible))
            {                
                this.ShowInTaskbar = true;                

                if (!ArgsManager.IsMinimized)
                {
                    this.WindowState = FormWindowState.Normal;
                    Show();
                    this.Visible = true;
                    this.BringToFront();
                }
                else
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            }

            if (!Properties.Settings.Default.Initialized)
            {
                Properties.Settings.Default.Initialized = true;
                Properties.Settings.Default.Save();
            }

            SetupOnLoad();
        }

        BackgroundWorker bwTimSetHook = new BackgroundWorker();

        private void SetupOnLoad()
        {
            CurrentlyFocusedApplication.StartListeningForWindowChanges();            

            AdjustSizeLocation();

            AddLanguageMenuItems();
            
            bwTimSetHook.DoWork += BwTimSetHook_DoWork;
            bwTimSetHook.RunWorkerAsync();

            if (ArgsManager.IsHidden || ArgsManager.IsRestart || ArgsManager.IsNovisible)
            {
                btnOK_Click(null, null);
            }            
        }

        public bool timSetHookEnabled = false;

        private void BwTimSetHook_DoWork(object sender, DoWorkEventArgs e)
        {            
            while (true)
            {
                if (timSetHookEnabled)
                {
                    timSetHook_Tick(null, null);

                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private bool InTimSetHook = false;

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private Point LastPointCheck = Point.Empty;

        private int TickCount = 0;

        public void timSetHook_Tick(object sender, EventArgs e)
        {
            //return;

            if (InTimSetHook) return;

            TickCount++;

            try
            {
                if (CPUUsageMeter.CPUUsageHigh) return;

                InTimSetHook = true;

                KeyboardHook.CheckSetHookTimer = true;

                IntPtr cw = GetForegroundWindow();

                if (cw.Equals(IntPtr.Zero)) return;

                uint procid;

                GetWindowThreadProcessId(cw, out procid);

                bool suc = System.Diagnostics.Process.GetProcessById((int)procid).WaitForInputIdle(1000);

                if (suc && System.Diagnostics.Process.GetProcessById((int)procid).Responding)
                {
                    SendKeys.SendWait("{F14}");

                    System.Threading.Thread.Sleep(3000);

                    suc = System.Diagnostics.Process.GetProcessById((int)procid).WaitForInputIdle(1000);

                    if (!(suc && System.Diagnostics.Process.GetProcessById((int)procid).Responding)) return;

                    if (KeyboardHook.CheckSetHookTimer)
                    {
                        RestartApplication();
                    }
                }

                MouseHook.CheckSetHookTimer = true;

                LastPointCheck = System.Windows.Forms.Cursor.Position;

                System.Threading.Thread.Sleep(1000);

                if ((System.Windows.Forms.Cursor.Position.X!= LastPointCheck.X) ||
                    (System.Windows.Forms.Cursor.Position.Y != LastPointCheck.Y))
                {
                    if (MouseHook.CheckSetHookTimer)
                    {
                        RestartApplication();
                    }
                }
            }
            catch (Exception exm)
            {
                Console.WriteLine(exm.ToString());
            }
            finally
            {
                InTimSetHook = false;
            }
        }        
        
        public void RestartApplication()
        {
            try
            {
                if (CPUUsageMeter.CPUUsageHigh) return;

                //"Hook exited. Reinstalling hooks"

                SaveSizeLocation();

                string svisible = "";
                string sminimize = "";

                if (this.WindowState == FormWindowState.Minimized)
                {
                    sminimize = " /minimized";
                }

                if (!this.Visible)
                {
                    svisible = " /novisible";
                }

                string sstarted = "";

                if (IsStarted)
                {
                    sstarted = " /restart";
                }

                frmMain_FormClosing(null, null);

                Properties.Settings.Default.Save();

                //Module.WriteToLog("Starting Application");
                //Module.WriteToLog("\"" + Application.ExecutablePath + "\" /redisable /hide");

                System.Diagnostics.Process proc = new Process();
                proc.StartInfo.FileName = Application.ExecutablePath;
                proc.StartInfo.Arguments = sminimize + svisible + sstarted;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();

                while (true)
                {
                    try
                    {
                        var time = proc.StartTime;

                        System.Threading.Thread.Sleep(100);

                        break;
                    }
                    catch (Exception) { }
                }

                System.Threading.Thread.Sleep(300);

                //Application.Exit();
                Environment.Exit(0);
            }
            finally
            {

            }
        }

        #region Help Menu

        private void helpGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Module.HelpURL);
        }

        private void pleaseDonateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/donate.php");
        }

        private void dotsSoftwarePRODUCTCATALOGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/downloads/4dots-Software-PRODUCT-CATALOG.pdf");
        }

        private void checkForNewVersionEachWeekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CheckWeek = checkForNewVersionEachWeekToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void tiHelpFeedback_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.4dots-software.com/support/bugfeature.php?app=" + System.Web.HttpUtility.UrlEncode(Module.ShortApplicationTitle));
        }

        private void checkForNewVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateHelper.CheckVersion(false);
        }

        private void followUsOnTwitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.twitter.com/4dotsSoftware");
        }

        private void visit4dotsSoftwareWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.4dots-software.com");
        }

        private void youtubeChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCovA-lld9Q79l08K-V1QEng");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout f = new frmAbout();
            f.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        private void minimizeToWindowsSystemTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MinimizeToTray = minimizeToWindowsSystemTrayToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void notMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {            
            this.Visible = true;
            this.Show();
            this.BringToFront();
            /*
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.CenterToScreen();*/

            AdjustSizeLocation();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSizeLocation();

            this.Visible = !Properties.Settings.Default.MinimizeToTray;

            if (Properties.Settings.Default.MinimizeToTray)
            {
                this.Hide();
                WindowState = FormWindowState.Minimized;
            }

            this.notMain.Visible = Properties.Settings.Default.MinimizeToTray;

            Start();
        }        

        private void frmMain_Resize(object sender, EventArgs e)
        {            
            if (!ShownOnce)
            {
                return;
            }
            
            if (WindowState==FormWindowState.Minimized)
            {
                if (Properties.Settings.Default.MinimizeToTray)
                {
                    notMain.Visible = true;
                    this.Visible = false;
                }
            }
        }                

        private void txtHotKey_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notMain_MouseDoubleClick(null, null);
        }

        private void runAtWindowsStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunAtWndowsStartupManager.RunAtWindowsStartup = !runAtWindowsStartupToolStripMenuItem.Checked;

            runAtWindowsStartupToolStripMenuItem.Checked = RunAtWndowsStartupManager.RunAtWindowsStartup;

        }

        bool FreeForPersonalUse = false;
        bool FreeForPersonalAndCommercialUse = true;

        private void SetTitle()
        {
            string str = "";
                        
            if (FreeForPersonalUse)
            {
                str += " - " + TranslateHelper.Translate("Free for Personal Use Only - Please Donate !");
            }
            else if (FreeForPersonalAndCommercialUse)
            {
                str += " - " + TranslateHelper.Translate("Free for Personal and Commercial Use - Please Donate !");
            }

            this.Text = Module.ApplicationTitle + str.ToUpper();
        }

        public List<ucWheelOption> WheelOptions
        {
            get
            {
                List<ucWheelOption> lst = new List<ucWheelOption>();

                for (int k=0;k<fplWheelOptions.Controls.Count;k++)
                {
                    ucWheelOption opt = fplWheelOptions.Controls[k] as ucWheelOption;

                    lst.Add(opt);
                }

                return lst;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ucWheelOption opt = new ucWheelOption();
            fplWheelOptions.Controls.Add(opt);
            fplWheelOptions.SetFlowBreak(opt, true);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (fplWheelOptions.Controls.Count>1)
            {
                fplWheelOptions.Controls.RemoveAt(fplWheelOptions.Controls.Count - 1);
            }
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmOptions f = new frmOptions();

            f.ShowDialog(this);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSizeLocation();

            VolumeWheelProject.SaveProject();

            CurrentlyFocusedApplication.StopListeningForWindowChanges();

            Properties.Settings.Default.Save();
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = CurrentProject;
            ofd.Filter = "Volume Wheel Project (*.vwp)|*.vwp";

            if (ofd.ShowDialog()==DialogResult.OK)
            {
                bool suc = VolumeWheelProject.LoadProject(ofd.FileName);

                if (!suc)
                {
                    Module.ShowMessage("Error could not Load Proejct !");

                    fplWheelOptions.Controls.Clear();

                    for (int k=0;k<5;k++)
                    {
                        btnAdd_Click(null, null);
                    }
                }
                    
            }
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VolumeWheelProject.SaveProject(CurrentProject);
        }

        private void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.FileName = CurrentProject;
            ofd.Filter = "Volume Wheel Project (*.vwp)|*.vwp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                bool suc = VolumeWheelProject.SaveProject(ofd.FileName);

                if (!suc)
                {
                    Module.ShowMessage("Error could not Save Proejct !");                    
                }
            }
        }

        #region Size - Location

        private void AdjustSizeLocation()
        {
            if (ArgsManager.IsHidden || ArgsManager.IsNovisible)
            {
                return;
            }

            this.ShowInTaskbar = true;

            if (Properties.Settings.Default.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Show();
                this.Visible = true;
                this.BringToFront();

                if (Properties.Settings.Default.Width == -1)
                {
                    this.CenterToScreen();
                    return;
                }
                else
                {
                    this.Width = Properties.Settings.Default.Width;
                }
                if (Properties.Settings.Default.Height != 0)
                {
                    this.Height = Properties.Settings.Default.Height;
                }

                if (Properties.Settings.Default.Left != -1)
                {
                    this.Left = Properties.Settings.Default.Left;
                }

                if (Properties.Settings.Default.Top != -1)
                {
                    this.Top = Properties.Settings.Default.Top;
                }

                if (this.Width < 300)
                {
                    this.Width = 300;
                }

                if (this.Height < 300)
                {
                    this.Height = 300;
                }

                if (this.Left < 0)
                {
                    this.Left = 0;
                }

                if (this.Top < 0)
                {
                    this.Top = 0;
                }
            }

        }

        private void SaveSizeLocation()
        {
            Properties.Settings.Default.Maximized = (this.WindowState == FormWindowState.Maximized);

            Properties.Settings.Default.Save();

            if ((this.WindowState != FormWindowState.Normal) || !this.Visible) return;

            Properties.Settings.Default.Left = this.Left;
            Properties.Settings.Default.Top = this.Top;
            Properties.Settings.Default.Width = this.Width;
            Properties.Settings.Default.Height = this.Height;

            Properties.Settings.Default.Save();

        }

        #endregion

        #region Localization

        private void AddLanguageMenuItems()
        {
            for (int k = 0; k < frmLanguage.LangCodes.Count; k++)
            {
                ToolStripMenuItem ti = new ToolStripMenuItem();
                ti.Text = frmLanguage.LangDesc[k];
                ti.Tag = frmLanguage.LangCodes[k];
                ti.Image = frmLanguage.LangImg[k];

                if (Properties.Settings.Default.Language == frmLanguage.LangCodes[k])
                {
                    ti.Checked = true;
                }

                ti.Click += new EventHandler(tiLang_Click);

                if (k < 25)
                {
                    languages1ToolStripMenuItem.DropDownItems.Add(ti);
                }
                else
                {
                    languages2ToolStripMenuItem.DropDownItems.Add(ti);
                }

                //languageToolStripMenuItem.DropDownItems.Add(ti);
            }
        }

        void tiLang_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = (ToolStripMenuItem)sender;
            string langcode = ti.Tag.ToString();
            ChangeLanguage(langcode);

            //for (int k = 0; k < languageToolStripMenuItem.DropDownItems.Count; k++)
            for (int k = 0; k < languages1ToolStripMenuItem.DropDownItems.Count; k++)
            {
                ToolStripMenuItem til = (ToolStripMenuItem)languages1ToolStripMenuItem.DropDownItems[k];
                if (til == ti)
                {
                    til.Checked = true;
                }
                else
                {
                    til.Checked = false;
                }
            }

            for (int k = 0; k < languages2ToolStripMenuItem.DropDownItems.Count; k++)
            {
                ToolStripMenuItem til = (ToolStripMenuItem)languages2ToolStripMenuItem.DropDownItems[k];
                if (til == ti)
                {
                    til.Checked = true;
                }
                else
                {
                    til.Checked = false;
                }
            }
        }

        private bool InChangeLanguage = false;

        private void ChangeLanguage(string language_code)
        {
            try
            {
                InChangeLanguage = true;

                Properties.Settings.Default.Language = language_code;
                frmLanguage.SetLanguage();

                Module.ShowMessage("Please restart the application !");

                Properties.Settings.Default.Save();

                Application.Exit();

                return;

                bool maximized = (this.WindowState == FormWindowState.Maximized);
                this.WindowState = FormWindowState.Normal;

                /*
                RegistryKey key = Registry.CurrentUser;
                RegistryKey key2 = Registry.CurrentUser;

                try
                {
                    key = key.OpenSubKey("Software\\4dots Software", true);

                    if (key == null)
                    {
                        key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\4dots Software");
                    }

                    key2 = key.OpenSubKey(frmLanguage.RegKeyName, true);

                    if (key2 == null)
                    {
                        key2 = key.CreateSubKey(frmLanguage.RegKeyName);
                    }

                    key = key2;

                    //key.SetValue("Language", language_code);
                    key.SetValue("Menu Item Caption", TranslateHelper.Translate("Change PDF Properties"));
                }
                catch (Exception ex)
                {
                    Module.ShowError(ex);
                    return;
                }
                finally
                {
                    key.Close();
                    key2.Close();
                }
                */
                //1SaveSizeLocation();

                //3SavePositionSize();

                this.Controls.Clear();

                InitializeComponent();

                SetupOnLoad();

                if (maximized)
                {
                    this.WindowState = FormWindowState.Maximized;
                }

                this.ResumeLayout(true);
            }
            finally
            {
                InChangeLanguage = false;
            }
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult dres = Module.ShowQuestionDialogYesFocus(TranslateHelper.Translate("Are you are sure that you want to Reset Options?"), TranslateHelper.Translate("Are you sure you want to reset ?"));

            if (dres==DialogResult.Yes)
            {
                fplWheelOptions.Controls.Clear();

                for (int k=0;k<5;k++)
                {
                    btnAdd_Click(null, null);
                }
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            Stop();

            ShownOnce = true;
        }
    }
}
