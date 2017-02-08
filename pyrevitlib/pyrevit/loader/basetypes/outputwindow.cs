using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PyRevitBaseClasses
{
    public partial class ScriptOutput : Form
    {
        public delegate void CustomProtocolHandler(String url);
        public CustomProtocolHandler UrlHandler;

        public ScriptOutput()
        {
            Application.EnableVisualStyles();
            InitializeComponent();
            txtStdOut.DocumentText = "<html><body></body></html>";

            // Let's leave the WebBrowser control working alone.
            while (txtStdOut.Document.Body == null)
            {
                Application.DoEvents();
            }

            txtStdOut.Document.Body.Style = ExternalConfig.htmlstyle;
            // txtStdOut.Document.Body.ScrollIntoView(false);
        }

        private void ScriptOutput_Load(object sender, EventArgs e)
        {

        }

        public void ScrollToBottom()
        {
            // MOST IMP : processes all windows messages queue
            Application.DoEvents();

            if (txtStdOut.Document != null)
            {
                txtStdOut.Document.Window.ScrollTo(0, txtStdOut.Document.Body.ScrollRectangle.Height);
            }
        }

        private void txtStdOut_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!(e.Url.ToString().Equals("about:blank", StringComparison.InvariantCultureIgnoreCase)))
            {
                var commandStr = e.Url.ToString();
                if (commandStr.StartsWith("http")) {
                    System.Diagnostics.Process.Start(e.Url.ToString());
                }
                else {
                    UrlHandler(e.Url.OriginalString);
                }

                e.Cancel = true;
            }
        }
    }
}