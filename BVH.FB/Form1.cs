using BVH.FB.Common;
using BVH.FB.Model;
using BVH.FB.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BVH.FB
{
    public partial class Form1 : Form
    {
        private static string ACCOUNT_FILE_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "accounts.json");
        private static bool _StopFlag = false;

        private List<AccountInfor> listAccountInfor;
        private FormConfig _config {
            get {
                Properties.Settings.Default.ImagePath = txtImagePath.Text;
                Properties.Settings.Default.ProfilePath = txtProfilePath.Text;
                Properties.Settings.Default.Save();

                return new FormConfig()
                {
                    ImagePath = txtImagePath.Text,
                    ProfilePath = txtProfilePath.Text,
                    Wait = int.Parse(txtWait.Text)
                };
            }
        }
        public Form1()
        {
            Utilities.CreateFile(ACCOUNT_FILE_PATH, "[]");
            InitializeComponent();
            InitializeDataGridView();
            InitConfigControl();
        }

        #region UI

        private void InitializeDataGridView()
        {
            listAccountInfor = new List<AccountInfor>();
            LoadFile();
            gridAccInfor.AllowUserToAddRows = false;
            gridAccInfor.Columns[(int)EnumColumnOrder.UID].Width = 120;
            gridAccInfor.Columns[(int)EnumColumnOrder.Password].Width = 120;
            gridAccInfor.Columns[(int)EnumColumnOrder.TwoFactor].Width = 120;
            gridAccInfor.Columns[(int)EnumColumnOrder.Others].Width = 100;
            gridAccInfor.Columns[(int)EnumColumnOrder.PageID].Width = 140;
            gridAccInfor.Columns[(int)EnumColumnOrder.Cookie].Width = 180;
            gridAccInfor.Columns[(int)EnumColumnOrder.State].Width = 210;
        }

        private void InitConfigControl()
        {
            txtImagePath.Text = Properties.Settings.Default.ImagePath;
            txtProfilePath.Text = Properties.Settings.Default.ProfilePath;
        }
        #endregion

        #region #Event
        private void grdAccount_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bool hasRowSelected = gridAccInfor.SelectedRows.Count > 0;
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Thêm acc (UID|Pass|2FA)", AddAccountFromClipboard));
                m.MenuItems.Add(new MenuItem("Tạo page", CreatePage));
                m.MenuItems.Add(new MenuItem("Xóa dòng đã chọn", RemoveAccount));

                m.Show(gridAccInfor, new Point(e.X, e.Y));
            }
        }

        private void grdAccount_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == (int)EnumColumnOrder.State && e.RowIndex >= 0)
            {
                e.CellStyle.ForeColor = Color.Black;
                var stateValue = gridAccInfor[e.ColumnIndex, e.RowIndex].Value;
                if (stateValue != null)
                {
                    var state = stateValue.ToString();
                    if (state.StartsWith("Success"))
                    {
                        e.CellStyle.ForeColor = Color.DarkGreen;
                    }
                    else if (state.StartsWith("Error"))
                    {
                        e.CellStyle.ForeColor = Color.DarkRed;
                    }
                }
            }
        }

        private void grdAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveAccount(sender, e);
                LoadFile();
            }
        }

        private void grdAccount_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();
            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void grdAccount_SelectionChanged(object sender, EventArgs e)
        {
            lbCountSelected.Text = gridAccInfor.SelectedRows.Count.ToString();
        }
        #endregion

        #region Business
        private void CreatePage(Object sender, System.EventArgs e)
        {
            if (gridAccInfor.SelectedRows.Count > 0)
            {
                _StopFlag = false;
                DialogResult result1 = MessageBox.Show("Bạn có chắc chắn muốn tạo page cho " + gridAccInfor.SelectedRows.Count + " dòng đã chọn không?",
                       "Xác nhận tạo page",
                       MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    int count = 0;
                    foreach (DataGridViewRow row in gridAccInfor.SelectedRows)
                    {
                        if(!_StopFlag)
                        {
                            var iAccount = (AccountInfor)row.DataBoundItem;
                            var bindingAccount = GetDataBinding(iAccount.UID);
                            var fbPageCreattor = new FbPageCreator(bindingAccount, _config);
                            fbPageCreattor.Execute();
                        }
                    }
                    SaveFile();
                    ReloadGrid();
                    MessageBox.Show("Đã tạo page " + count + " dòng.");
                }
            }
        }

        private void AddAccountFromClipboard(Object sender, System.EventArgs e)
        {
            try
            {
                string rawText;
                string[] rowText;
                if (Clipboard.ContainsText())
                {
                    rawText = Clipboard.GetText(TextDataFormat.Text);
                    if (rawText.Length > 1)
                    {
                        //  unify all line breaks to \r
                        rawText = rawText.Replace("\r\n", "\r").Replace("\n", "\r");
                        //  create an array of lines
                        rowText = rawText.Split('\r');
                        gridAccInfor.Rows.Clear();
                        for (int i = 0; i < rowText.Length; i++)
                        {
                            string row = rowText[i];
                            string[] rowSplit = row.Split('|');
                            // duplicate check
                            List<AccountInfor> duplicateList = listAccountInfor.FindAll(
                                delegate (AccountInfor acc)
                                {
                                    return acc.UID.Equals(rowSplit[0]);
                                });
                            if (duplicateList.Count > 0)
                            {
                                continue;
                            }
                            if (rowSplit.Length >= 2)
                            {
                                var iAccount = new AccountInfor()
                                {
                                    UID = rowSplit[0],
                                    Password = rowSplit[1],
                                };

                                if(rowSplit.Length > 2)
                                {
                                    iAccount.TwoFactor = rowSplit[2];
                                }

                                if (rowSplit.Length > 3)
                                {
                                    iAccount.Others = rowSplit[3];
                                }

                                listAccountInfor.Add(iAccount);
                            }
                        }
                    }
                }
                ReloadGrid();
                SaveFile();
            }
            catch (Exception)
            {
                MessageBox.Show("Clipboard không đúng định dạng!");
            }
        }

        private void RemoveAccount(Object sender, System.EventArgs e)
        {
            if (gridAccInfor.SelectedRows.Count > 0)
            {
                DialogResult result1 = MessageBox.Show("Bạn có chắc chắn muốn xóa " + gridAccInfor.SelectedRows.Count + " dòng đã chọn không?",
                       "Xác nhận xóa dòng",
                       MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    int count = 0;
                    foreach (DataGridViewRow row in gridAccInfor.SelectedRows)
                    {
                        listAccountInfor.Remove((AccountInfor)row.DataBoundItem);
                        gridAccInfor.Rows.RemoveAt(row.Index);
                        count++;
                    }
                    SaveFile();
                    LoadGridInfor();
                    MessageBox.Show("Đã xóa " + count + " dòng.");
                }
            }
        }

        private AccountInfor GetDataBinding(string uid)
        {
            return listAccountInfor.FirstOrDefault(_ => _.UID == uid);
        }

        private void ReloadGrid()
        {
            var bindingList = new SortableBindingList<AccountInfor>(listAccountInfor);
            var source = new BindingSource(bindingList, null);
            gridAccInfor.DataSource = source;
            gridAccInfor.Update();
            gridAccInfor.Refresh();
            LoadGridInfor();
        }

        private void LoadGridInfor()
        {
            listAccountInfor = listAccountInfor ?? new List<AccountInfor>();
            lbCountAll.Text = listAccountInfor.Count.ToString();
            lbCountSelected.Text = gridAccInfor.SelectedRows.Count.ToString();
        }

        private void LoadFile()
        {
            if (File.Exists(ACCOUNT_FILE_PATH))
            {
                var json = File.ReadAllText(ACCOUNT_FILE_PATH);
                if (!String.IsNullOrEmpty(json))
                {
                    listAccountInfor = JsonConvert.DeserializeObject<List<AccountInfor>>(json);
                    ReloadGrid();
                }
            }
        }
        private void SaveFile()
        {
            listAccountInfor = listAccountInfor ?? new List<AccountInfor>();
            string json = JsonConvert.SerializeObject(listAccountInfor);

            //write string to file
            File.WriteAllText(ACCOUNT_FILE_PATH, json);
        }
        #endregion

        private void btnStop_Click(object sender, EventArgs e)
        {
            _StopFlag = true;
        }
    }
}
