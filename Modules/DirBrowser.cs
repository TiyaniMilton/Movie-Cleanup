using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Movie_Cleanup.Modules
{
    /// <summary>
    /// Directory Browser.
    /// </summary>
    public class DirBrowser
    {
        #region Fields
        private string _description = "Select Directory";
        private string _directory = string.Empty;
        #endregion

        #region Properties
        public string Description
        {
            set
            {
                _description = value;
            }
            get
            {
                return _description;
            }
        }

        public string DirectoryPath
        {
            get
            {
                return _directory;
            }
        }

        #endregion

        #region Methods
        public DialogResult ShowDialog()
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            //FolderBrowser browser = new FolderBrowser();
            browser.Description = _description;
            //browser.StartLocation = FolderNameEditor.FolderBrowserFolder.MyComputer;
            browser.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult result = browser.ShowDialog();
            if (result == DialogResult.OK)
                _directory = browser.SelectedPath;
            else
                _directory = String.Empty;
            return result;
        }
        #endregion
    }
}
