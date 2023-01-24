using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VolumeWheel
{
    public class VolumeWheelProject
    {
        public string DefaultProject = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" +
                Module.ApplicationName, "project.vwp");

        public bool LoadedOnce = false;

        public VolumeWheelProject()
        {

        }

        public bool SaveProject()
        {
            if (!LoadedOnce) return false;            

            try
            {
                string filepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" +
                    Module.ApplicationName, "project.vwp");

                string dir = System.IO.Path.GetDirectoryName(filepath);

                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                return SaveProject(filepath);
            }
            catch {

                return false;
            }
        }

        public bool LoadProject()
        {            
            try
            {
                string filepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" +
                    Module.ApplicationName, "project.vwp");

                if (!System.IO.File.Exists(filepath))
                {
                    LoadedOnce = true;

                    return false;
                }

                bool suc=LoadProject(filepath);

                LoadedOnce = true;

                return suc; 

            }
            catch
            {
                return false;
            }
            finally
            {
                
            }
        }

        public bool LoadProject(string filepath)
        {            
            try
            {
                DataSet ds = new DataSet("ds");
                ds.ReadXml(filepath, XmlReadMode.ReadSchema);

                DataTable dt = ds.Tables[0];

                frmMain.Instance.fplWheelOptions.Controls.Clear();

                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    try
                    {
                        ucWheelOption wo = new ucWheelOption();

                        DataRow dr = dt.Rows[k];

                        int iopt = int.Parse(dr["option"].ToString());

                        wo.cmbOption.SelectedIndex = iopt;                        

                        iopt = int.Parse(dr["step"].ToString());

                        wo.cmbStep.SelectedIndex = iopt;

                        iopt = int.Parse(dr["hotkey"].ToString());

                        wo.HotKey = iopt;

                        bool bc = bool.Parse(dr["control"].ToString());

                        wo.Control = bc;

                        wo.chkControl.Checked = bc;

                        bc = bool.Parse(dr["shift"].ToString());

                        wo.Shift = bc;

                        wo.chkShift.Checked = bc;

                        bc = bool.Parse(dr["alt"].ToString());

                        wo.Alt = bc;

                        wo.chkAlt.Checked = bc;

                        wo.HotKeyStr = dr["hotkeystr"].ToString();
                        wo.txtHotKey.Text = dr["hotkeystr"].ToString();

                        wo.txtEXEFilename.Text = dr["exefilename"].ToString();

                        string dev = dr["device"].ToString();

                        bool found = false;

                        for (int j = 0; j < wo.cmbDevice.Items.Count; j++)
                        {
                            if (wo.cmbDevice.Items[j].ToString().ToLower() == dev.ToLower())
                            {
                                found = true;

                                wo.cmbDevice.SelectedIndex = j;
                                break;
                            }
                        }

                        if (!found)
                        {
                            Module.ShowMessage(TranslateHelper.Translate("Error could not find proper Device !") + " " + dev);
                        }
                        else
                        {
                            frmMain.Instance.fplWheelOptions.Controls.Add(wo);

                            frmMain.Instance.fplWheelOptions.SetFlowBreak(wo, true);

                            wo.cmbDevice_SelectedIndexChanged(null, null);
                            wo.cmbOption_SelectedIndexChanged(null, null);

                            iopt = int.Parse(dr["channel"].ToString());

                            wo.cmbChannels.SelectedIndex = iopt;
                        }
                    }
                    catch { }
                }

                if (frmMain.Instance.fplWheelOptions.Controls.Count == 0)
                {
                    return false;
                }
                else
                {
                    frmMain.Instance.tslProject.Text = filepath;
                    frmMain.Instance.CurrentProject = filepath;
                }

                LoadedOnce = true;

                return true;
            }
            catch {

                return false;
            }
            finally
            {
                
            }
        }

        public bool SaveProject(string filepath)
        {
            if (!LoadedOnce) return false;            

            try
            {
                string dir = System.IO.Path.GetDirectoryName(filepath);

                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                DataSet ds = new DataSet("ds");
                DataTable dt = new DataTable("table");

                ds.Tables.Add(dt);

                dt.Columns.Add("option", typeof(int));
                dt.Columns.Add("device", typeof(string));
                dt.Columns.Add("channel", typeof(int));
                dt.Columns.Add("step", typeof(int));
                dt.Columns.Add("control", typeof(bool));
                dt.Columns.Add("shift", typeof(bool));
                dt.Columns.Add("alt", typeof(bool));
                dt.Columns.Add("hotkey", typeof(int));
                dt.Columns.Add("hotkeystr", typeof(string));
                dt.Columns.Add("exefilename", typeof(string));

                for (int k = 0; k < frmMain.Instance.WheelOptions.Count; k++)
                {
                    DataRow dr = dt.NewRow();

                    ucWheelOption wo = frmMain.Instance.WheelOptions[k];

                    dr["option"] = wo.cmbOption.SelectedIndex;
                    dr["device"] = wo.cmbDevice.SelectedItem.ToString();
                    dr["channel"] = wo.cmbChannels.SelectedIndex;
                    dr["step"] = wo.cmbStep.SelectedIndex;
                    dr["control"] = wo.Control;
                    dr["shift"] = wo.Shift;
                    dr["alt"] = wo.Alt;
                    dr["hotkey"] = wo.HotKey;
                    dr["hotkeystr"] = wo.HotKeyStr;
                    dr["exefilename"] = wo.txtEXEFilename.Text;

                    dt.Rows.Add(dr);
                }

                ds.WriteXml(filepath, XmlWriteMode.WriteSchema);

                frmMain.Instance.tslProject.Text = filepath;
                frmMain.Instance.CurrentProject = filepath;

                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}
