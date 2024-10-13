namespace CaffeineApp
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Initialize the form components. In this case, we don't need much because the app runs in the tray.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Text = "Caffeine for Windows";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;  // Start minimized
            this.ShowInTaskbar = false;  // Hide the form from the taskbar
        }

        #endregion
    }
}