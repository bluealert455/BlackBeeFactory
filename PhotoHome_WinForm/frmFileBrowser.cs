using CommonLib;
using SuperControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoHome
{
    public partial class frmFileBrowser : frmFloatBase
    {
        
        private static ShellItem mDesktopItem = null;
        
        private string mCurFolder = null;
        private string mFilter = null;

        private CheckBox[] mTypeCheckBoxes = null;
        private List<ThumbBox> mRootThumbBoxes = null;
        private List<ThumbBox> mRecentsThumbBoxes = null;
        private bool mIsSplash = true;
        public frmFileBrowser()
        {
            InitializeComponent();
            
            this.filesList.IsFolderBrowser = true;
            this.Shown += FrmFileBrowser_Shown;
            this.Load += FrmFileBrowser_Load;
            this.FormClosed += FrmFileBrowser_FormClosed;
            this.treeFolder.OnBeforeExpand += TreeFolder_OnBeforeExpand;
            this.treeFolder.OnNodeSelected += TreeFolder_OnNodeSelected;
            this.treeFolder.OnAfterCollapse += TreeFolder_OnAfterCollapse;
            this.treeFolder.OnAfterExpand += TreeFolder_OnAfterExpand;
            
            //this.tvwFolders.BeforeExpand += TvwFolders_BeforeExpand;

            this.navCurFolder.OnFolderChanged += NavCurFolder_OnFolderChanged;

            mTypeCheckBoxes = new CheckBox[] { chkPhoto, chkAudio, chkVideo };
        }

        private void FrmFileBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            GC.Collect();
        }

        private void FrmFileBrowser_Load(object sender, EventArgs e)
        {
            if (mIsSplash)
            {
                this.btnClose.Text = "退出";
            }
            else
            {
                this.btnClose.Text = "取消";
            }
        }

        private void NavCurFolder_OnFolderChanged(string str)
        {
            if (str == Global.mMyComputerText)
            {
                filesList.LoadItems(mRootThumbBoxes);
                treeFolder.SelectedNode = null;
            }
            else if (str == Global.mRecentsText)
            {

            }
            else
            {
                mCurFolder = str;
                SetSelectedNodeByCurFolder(null);
                this.filesList.LoadFolder(str);
            }
        }

        private void SetSelectedNodeByCurFolder(TreePanelNode node)
        {
            TreePanelNode curNode = null;
            if(node==null)
                curNode=GetNodeByFolderName(mCurFolder);
            else
            {
                curNode = GetNodeByFolderName(node,mCurFolder);
            }
            if (curNode != null)
            {
                treeFolder.SelectedNode = curNode;
            }
        }
        private void TreeFolder_OnAfterExpand(TreePanelNode node)
        {
            SetSelectedNodeByCurFolder(node);
        }

        private void TreeFolder_OnAfterCollapse(TreePanelNode node)
        {
            SetSelectedNodeByCurFolder(node);
        }

        private void TreeFolder_OnNodeSelected(TreePanelNode node)
        {
            object objTag = node.Tag;
            if (objTag.ToString() == Global.mRecentNodeTag)
            {
                this.filesList.LoadItems(mRecentsThumbBoxes);
                navCurFolder.Path = Global.mRecentsText;
            }
            else
            {
                ShellItem shNode = (ShellItem)objTag;
                if (shNode != null)
                {
                    mCurFolder = shNode.Path;
                    this.filesList.LoadFolder(shNode.Path);
                    this.navCurFolder.Path = shNode.Path;
                }
            }
            
            
        }

        private void TreeFolder_OnBeforeExpand(TreePanelNode node)
        {
            node.Nodes.Clear();
            ShellItem shNode = (ShellItem)node.Tag;
            ArrayList arrSub = shNode.GetSubFolders();
            foreach (ShellItem shChild in arrSub)
            {
                TreePanelNode folderNode = CreateNode(shChild);
                node.Nodes.Add(folderNode);
            }

            treeFolder.Invalidate();
            
        }



        public bool IsSplash { get => mIsSplash; set => mIsSplash = value; }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            navCurFolder.IsEditing = false;
        }
        private void FrmFileBrowser_Shown(object sender, EventArgs e)
        {
            InitControls();
            InitTree();
            GetRecents();
            if (mRecentsThumbBoxes != null)
            {
                this.filesList.LoadItems(mRecentsThumbBoxes);
                this.treeFolder.SelectedNode = this.treeFolder.Nodes[0];
                this.navCurFolder.Path = Global.mRecentsText;
            }
            else
            {
                this.filesList.LoadItems(mRootThumbBoxes);
                this.treeFolder.SelectedNode = null;
                this.navCurFolder.Path = "";
            }
        }

        private bool mIsInitializing = false;
        private void InitControls()
        {
            AppConfig ac = AppConfig.Instance;
            mIsInitializing = true;
            this.chkPhoto.Checked = (Convert.ToBoolean(Global.CurrentSurpportedType & EnumFileType.Image));
            this.chkVideo.Checked = (Convert.ToBoolean(Global.CurrentSurpportedType & EnumFileType.Video));
            this.chkAudio.Checked = (Convert.ToBoolean(Global.CurrentSurpportedType & EnumFileType.Audio));

            SetAllTypesCheckState();
            mIsInitializing = false;

            this.chkFindInSubFolders.Checked = Global.FolderRecursion;


        }
        private void GetRecents()
        {
            if (mRecentsThumbBoxes != null) mRecentsThumbBoxes.Clear();
            List<RecentItem> recents = AppConfig.Instance.Recents;
            if (recents != null&&recents.Count>0)
            {
                mRecentsThumbBoxes = new List<ThumbBox>();
                for(int i = 0; i < recents.Count; i++)
                {
                    string path = recents[i].Path;
                    ThumbBox boxExisted = null;
                    for(int j = 0; j < mRootThumbBoxes.Count; j++)
                    {
                        if (path.ToLower() == mRootThumbBoxes[j].OriginalFileName.ToLower())
                        {
                            boxExisted = mRootThumbBoxes[j];
                            break;
                        }
                    }
                    if (boxExisted == null)
                    {
                        mRecentsThumbBoxes.Add(CreateThumbBoxSimple(path, Path.GetFileName(path), Directory.Exists(path)));
                    }
                    else
                    {
                        mRecentsThumbBoxes.Add(boxExisted);
                    }
                    
                }
            }
        }

        private ThumbBox CreateThumbBoxSimple(string path,string dispName,bool isFolder=true)
        {
            ThumbBox box = new ThumbBox();
            box.IsFolder = isFolder;
            box.OriginalFileName = path;
            box.ShortName = dispName;
            box.OriginalName = dispName;
            return box;
        }
        
        private void InitTree()
        {
            mRootThumbBoxes = new List<ThumbBox>();
            TreePanelNode nodeRecents = new TreePanelNode();
            nodeRecents.Text = Global.mRecentsText;
            nodeRecents.ImageKey = "quickaccess16.png";
            nodeRecents.Tag = Global.mRecentNodeTag;
            this.treeFolder.Nodes.Add(nodeRecents);
            mRootThumbBoxes.Add(CreateThumbBoxSimple(Global.mRecentsText, Global.mRecentsText));

            if(mDesktopItem==null) mDesktopItem = new ShellItem();
            IntPtr pComputerPIDL = IntPtr.Zero;
            ShellAPI.SHGetSpecialFolderLocation(IntPtr.Zero, ShellAPI.CSIDL.CSIDL_DRIVES, ref pComputerPIDL);

            ShellItem computerItem = new ShellItem(mDesktopItem.RootShellFolder, pComputerPIDL, mDesktopItem);
            ArrayList arrChildren = computerItem.GetSubFolders();
            foreach (ShellItem shChild in arrChildren)
            {
                if (shChild.HasSubFolder==false||IsHidden(shChild.Path)) continue;
                TreePanelNode tvwChild = CreateNode(shChild);
                this.treeFolder.Nodes.Add(tvwChild);

                mRootThumbBoxes.Add(CreateThumbBoxSimple(shChild.Path,shChild.DisplayName));

            }
            treeFolder.IsDirty = true;
            treeFolder.Invalidate();


        }

        private bool IsHidden(string path)
        {
            string[] pathHidden = new string[] { "downloads", "3d objects","documents" };
            for(int i = 0; i < pathHidden.Length; i++)
            {
                if (path.ToLower().IndexOf(pathHidden[i]) > 0)
                    return true;
            }
            return false;
        }
        private TreePanelNode CreateNode(ShellItem si)
        {
            TreePanelNode nodeChild = new TreePanelNode();
            nodeChild.Text = si.DisplayName;
            string imgKey = "icon_" + si.IconIndex.ToString();
            if (imgFolderTree.Images.ContainsKey(imgKey) == false)
            {
                imgFolderTree.Images.Add(imgKey, ShellAPI.GetIconByHandler(si.IconHandler));
            }

            nodeChild.ImageKey = imgKey;
            nodeChild.Tag = si;

            if (si.IsFolder && si.HasSubFolder)
                nodeChild.Nodes.Add(new TreePanelNode("PH"));
            return nodeChild;
        }


        private TreeNode CreateFolderNode(string path)
        {
            ShellItemSimple item = ShellAPI.GetPathSimpleItem(path);
            TreeNode folderNode = new TreeNode();
            folderNode.Text = item.DisplayName;
            string imgKey = "icon_" + item.IconIndex.ToString();
            if (imgFolderTree.Images.ContainsKey(imgKey) == false)
            {
                imgFolderTree.Images.Add(imgKey, item.Icon);
            }
            folderNode.ImageKey = imgKey;
            folderNode.SelectedImageKey = imgKey;
            folderNode.Tag = path;
            if (item.HasSubFolder)
            {
                folderNode.Nodes.Add("shit");
            }

            return folderNode;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {

            navCurFolder.IsEditing = false;
            this.DialogResult = DialogResult.Cancel;
            //if this window is splash screen,quit application

            this.Hide();
        }

        public string SelectedPath = null;
        private void btnOK_Click(object sender, EventArgs e)
        {
            navCurFolder.IsEditing = false;
            bool isFolder = true;
            string path = null;
            if (this.filesList.SelectedNode != null)
            {
                ThumbBox selectedBox = this.filesList.SelectedNode as ThumbBox;
                path = selectedBox.OriginalFileName;
                isFolder = selectedBox.IsFolder;
            }
            else
            {
                TreePanelNode selectedNode = this.treeFolder.SelectedNode as TreePanelNode;
                ShellItem si = selectedNode.Tag as ShellItem;
                if (si != null)
                {
                    path = si.Path;
                }
            }
            if (string.IsNullOrEmpty(path) == true)
            {
                Utils.MessageUIQueue.ShowMsgForm("selectpatherror", MessageBoxIcon.Error, "请选择文件夹或者文件。");
                return;
            }
            SelectedPath = path;

            AppConfig.Instance.AddRecent(path);
            AppConfig.Instance.Save();
            this.DialogResult = DialogResult.OK;
            this.Hide();
            
        }

        private void tvwFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private string GetPath(TreeNode node)
        {
            if (node.Tag == null) return null;
            string path = node.Tag.ToString();
            if (path == Global.mRecentNodeTag) return null;
            ShellItem si = node.Tag as ShellItem;
            if (si != null)
                return si.Path;
            return null;
        }

        private TreePanelNode GetNodeByFolderName(string folder)
        {
            folder = folder.ToLower();
            for(int i = 0; i < treeFolder.Nodes.Count; i++)
            {
                TreePanelNode node = GetNodeByFolderName(treeFolder.Nodes[i], folder);
                if (node == null)
                    continue;
                else
                    return node;
            }
            return null;
        }
        private TreePanelNode GetNodeByFolderName(TreePanelNode parentNode,string folder)
        {
            if (folder == null) return null;   
            if (parentNode.Tag == null || parentNode.Tag.ToString() == Global.mRecentNodeTag) return null;
            ShellItem si = parentNode.Tag as ShellItem;
            folder = folder.ToLower();
            if (folder.IndexOf(si.Path.ToLower()) == 0)
            {
                if (parentNode.IsExpanded==false||folder==si.Path.ToLower())
                {
                    return parentNode;
                }
                else
                {
                    for (int i = 0; i < parentNode.Nodes.Count; i++)
                    {
                        TreePanelNode node = GetNodeByFolderName(parentNode.Nodes[i], folder);
                        if (node == null)
                            continue;
                        else
                            return node;
                                
                    }
                }
            }
            
            return null;
        }
        private void filesList_OnFolderChanged(string str)
        {
            mCurFolder = str;
            this.navCurFolder.Path = str;
            TreePanelNode selectedNode = GetNodeByFolderName(str);
            treeFolder.SelectedNode = selectedNode;
        }

     
        private void OnMouseUpCommon(object sender, MouseEventArgs e)
        {
            navCurFolder.IsEditing = false;
        }

     
        private void SetFolderInputBoxDataSource(string path)
        {
            if (Directory.Exists(path))
            {
                string[] subDirs = Directory.GetDirectories(path);
                
                //txtCurFolder.AutoCompleteSource = subDirs;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                string keyword = this.txtSearch.Text;
                keyword = keyword.Trim();
                mFilter = keyword;
                this.filesList.LoadFolder(mCurFolder, keyword,true);
                
               
            }
        }

        private void SetCheckBoxesState(bool bChecked)
        {
            for(int i = 0; i < mTypeCheckBoxes.Length; i++)
            {
                mTypeCheckBoxes[i].Checked = bChecked;
            }
        }
        private void chkAllTypes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllTypes.Checked)
            {
                EnumFileType fileType = EnumFileType.Image | EnumFileType.Video | EnumFileType.Audio;
                Global.CurrentSurpportedType = fileType;
            }
            else
            {
                Global.ClearSurpportedTypes();
            }
            SetCheckBoxesState(this.chkAllTypes.Checked);
            filesList.LoadFolder(mCurFolder, mFilter, true);
        }

        private void chkPhoto_CheckedChanged(object sender, EventArgs e)
        {
            WhenTypeCheckChanged();
        }

        private void WhenTypeCheckChanged()
        {
            if (mIsInitializing) return;
            int checkedCnt = 0;
            EnumFileType fileType = EnumFileType.None;
            for (int i = 0; i < mTypeCheckBoxes.Length; i++)
            {
                if (mTypeCheckBoxes[i].Checked == true)
                {

                    object tag = mTypeCheckBoxes[i].Tag;

                    int val = int.Parse(tag.ToString());
                    fileType = fileType | (EnumFileType)val;
                    checkedCnt++;
                }
            }

            Global.CurrentSurpportedType = fileType;
            if (checkedCnt < mTypeCheckBoxes.Length && checkedCnt > 0)
            {
                chkAllTypes.CheckState = CheckState.Indeterminate;
            }
            else if (checkedCnt == 0)
            {
                chkAllTypes.CheckState = CheckState.Unchecked;
            }
            else if (checkedCnt == mTypeCheckBoxes.Length)
            {
                chkAllTypes.CheckState = CheckState.Checked;
            }
            if (mCurFolder != null)
            {
                filesList.LoadFolder(mCurFolder, mFilter, true);
            }
            
        }

        private void SetAllTypesCheckState()
        {
            int checkedCnt = 0;
            for (int i = 0; i < mTypeCheckBoxes.Length; i++)
            {
                if (mTypeCheckBoxes[i].Checked == true)
                {
                    checkedCnt++;
                }
            }
            if (checkedCnt < mTypeCheckBoxes.Length && checkedCnt > 0)
            {
                chkAllTypes.CheckState = CheckState.Indeterminate;
            }
            else if (checkedCnt == 0)
            {
                chkAllTypes.CheckState = CheckState.Unchecked;
            }
            else if (checkedCnt == mTypeCheckBoxes.Length)
            {
                chkAllTypes.CheckState = CheckState.Checked;
            }
        }
        private void chkVideo_CheckedChanged(object sender, EventArgs e)
        {
            
            WhenTypeCheckChanged();
        }

        private void chkAudio_CheckedChanged(object sender, EventArgs e)
        {
            WhenTypeCheckChanged();
        }

        private void chkFindInSubFolders_CheckedChanged(object sender, EventArgs e)
        {
            Global.FolderRecursion = chkFindInSubFolders.Checked;
        }
    }
}
