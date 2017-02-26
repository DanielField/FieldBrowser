using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FieldBrowser
{
    public partial class FrmMain : Form
    {
        string home = "google.com";
        List<WebKit.WebKitBrowser> pages = new List<WebKit.WebKitBrowser>();

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            newBrowser();
            tabs.TabPages[0].Text = "New tab";
            tabs.TabPages[1].Text = "+";
        }

        private void LoadOptions()
        {
            // Load tab
            WebKit.WebKitBrowser browser = new WebKit.WebKitBrowser();
            pages.Insert(pages.Count - 1, browser);
            TabPage options = new TabPage("Options");
            tabs.TabPages.Insert(pages.Count - 1, options);
            tabs.SelectedTab = options;
            
            // Load content
                
        }

        private void newBrowser()
        {
            WebKit.WebKitBrowser browser = new WebKit.WebKitBrowser();
            browser.Show();
            browser.Dock = DockStyle.Fill;
            pages.Add(browser);
            tabs.SelectedTab.Controls.Add(pages[pages.Count - 1]);

            pages[pages.Count - 1].Navigate(home);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            GetBrowserControl().Navigate(txtAddress.Text);
        }

        private WebKit.WebKitBrowser GetBrowserControl()
        {
            try { return (pages[tabs.SelectedIndex]); }
            catch { return new WebKit.WebKitBrowser(); }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (GetBrowserControl().CanGoBack)
                GetBrowserControl().GoBack();
            txtAddress.Text = GetBrowserControl().Url.ToString();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            if(GetBrowserControl().CanGoForward)
                GetBrowserControl().GoForward();
            txtAddress.Text = GetBrowserControl().Url.ToString();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetBrowserControl().Reload(WebBrowserRefreshOption.Completely);
            txtAddress.Text = GetBrowserControl().Url.ToString();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            GetBrowserControl().Navigate(home);
            txtAddress.Text = GetBrowserControl().Url.ToString();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            GetBrowserControl().Stop();
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            LoadOptions();
            txtAddress.Text = "Options";
        }

        private void txtAddress_KeyUp(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;

            if (e.KeyCode == Keys.Enter)
            {
                GetBrowserControl().Navigate(txtAddress.Text);
                GetBrowserControl().Select();
            }
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabs.SelectedIndex == tabs.TabCount - 1)
            {
                tabs.TabPages.Add("+");
                tabs.SelectedTab.Text = "New tab";
                newBrowser();
                GetBrowserControl().Navigate(home);
            }
            else
            {
                txtAddress.Text = GetBrowserControl().Url.ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // For each tab, update the title to display the title of the document
            for (int i = 0; i < tabs.TabPages.Count - 1; i++)
            {
                if (!string.IsNullOrEmpty(pages[i].DocumentTitle) && tabs.TabPages[i].Text != "Options")
                {
                    if (pages[i].DocumentTitle.Length > 30)
                    {
                        tabs.TabPages[i].Text = pages[i].DocumentTitle.Substring(0, 30) + "...";
                    }
                    else
                    {
                        tabs.TabPages[i].Text = pages[i].DocumentTitle;
                    }
                }
            }

            // Grey out buttons where necessary
            if (!GetBrowserControl().CanGoBack)
                btnBack.Enabled = false;
            else
                btnBack.Enabled = true;

            if (!GetBrowserControl().CanGoForward)
                btnForward.Enabled = false;
            else
                btnForward.Enabled = true;
        }

        private void tabs_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Middle)
                {
                    tabs.SelectedIndex = tabs.SelectedIndex - 1;
                    tabs.TabPages.RemoveAt(tabs.SelectedIndex + 1);
                    pages.RemoveAt(tabs.SelectedIndex + 1);
                }
            }catch{}
        }
    }
}