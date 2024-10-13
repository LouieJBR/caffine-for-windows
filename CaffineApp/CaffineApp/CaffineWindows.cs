using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Timers;

namespace CaffeineApp
{
    public partial class CaffineWindows : Form
    {
        // Import SetThreadExecutionState function from Windows API
        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(uint esFlags);

        // Flags for preventing sleep
        const uint ES_CONTINUOUS = 0x80000000;
        const uint ES_SYSTEM_REQUIRED = 0x00000001;
        const uint ES_DISPLAY_REQUIRED = 0x00000002;

        // NotifyIcon for the system tray
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private System.Timers.Timer awakeTimer;
    
        public CaffineWindows()
        {

            InitializeTrayIcon();
            // Initialize the context menu for the tray icon
            CreateCustomContextMenu();

            // Start by activating stay awake mode
            PreventSleep();
        }

        private void InitializeTrayIcon()
        {
            // Create a tray icon
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Caffeine for Windows";
            trayIcon.Icon = SystemIcons.Application; // You can replace this with a custom icon
            trayIcon.Visible = true;
        }

        private void CreateCustomContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.BackColor = Color.LightGray; // Background color
            contextMenu.ForeColor = Color.Black; // Text color

            ToolStripLabel titleLabel = new ToolStripLabel("Caffeine App")
            {
                Font = new Font("Arial", 10, FontStyle.Bold), // Set font style and size
                ForeColor = Color.Black, // Set the text color
            };

            ToolStripMenuItem activateItem = new ToolStripMenuItem("Activate")
            {
                CheckOnClick = true // Make this item checkable
            };
            activateItem.Click += OnActivate;

            ToolStripMenuItem deactivateItem = new ToolStripMenuItem("Deactivate");
            deactivateItem.Click += OnDeactivate;

            ToolStripMenuItem awake30 = new ToolStripMenuItem("Awake for 30 mins")
            {
                CheckOnClick = true // Make this item checkable
            };
            awake30.Click += (s, e) => ActivateForDuration(30);
            
            ToolStripMenuItem awake60 = new ToolStripMenuItem("Awake for 1 hour")
            {
                CheckOnClick = true // Make this item checkable
            };
            awake60.Click += (s, e) => ActivateForDuration(60);

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += OnExit;

            contextMenu.Items.AddRange(new ToolStripItem[] { titleLabel, activateItem, deactivateItem, new ToolStripSeparator(), awake30, awake60, new ToolStripSeparator(), exitItem });

            trayIcon.ContextMenuStrip = contextMenu;
        }

        // Prevent system sleep
        private void PreventSleep()
        {
            if (SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED) == 0)
            {
                MessageBox.Show("Failed to prevent system sleep.");
            }
        }

        // Allow system to sleep again
        private void AllowSleep()
        {
            if (SetThreadExecutionState(ES_CONTINUOUS) == 0)
            {
                MessageBox.Show("Failed to restore system sleep behavior.");
            }
        }

        private void ActivateForDuration(int minutes)
        {
            PreventSleep();
            MessageBox.Show($"System will stay awake for {minutes} minutes.");

            // Start a timer to allow sleep after the specified duration
            awakeTimer = new System.Timers.Timer(minutes * 60 * 1000); // Convert minutes to milliseconds
            awakeTimer.Elapsed += (s, e) =>
            {
                awakeTimer.Stop();
                AllowSleep();
                MessageBox.Show($"{minutes} minute system timer is now up. Your system can now sleep");

                trayIcon.ShowBalloonTip(1000, "Deactivated", "System can now sleep.", ToolTipIcon.Info);
            };
            awakeTimer.Start();
        }
        // Handle the "Activate" menu item click
        private void OnActivate(object sender, EventArgs e)
        {
            PreventSleep();
            MessageBox.Show("System will stay awake!");
        }

        // Handle the "Deactivate" menu item click
        private void OnDeactivate(object sender, EventArgs e)
        {
            AllowSleep();
            MessageBox.Show("System can now sleep.");
        }

        private void ExitApplication()
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
            AllowSleep();
            Application.Exit();
        }

        // Handle the "Exit" menu item click
        private void OnExit(object sender, EventArgs e)
        {
            ExitApplication();
        }

        // Form closing event handler to clean up resources
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ExitApplication();
            base.OnFormClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Visible = false; // Hide the main window
            this.ShowInTaskbar = false; // Do not show in the taskbar
        }
    }
}
