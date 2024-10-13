using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CaffeineApp
{
    public partial class Form1 : Form
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

        public Form1()
        {
            // Initialize the context menu for the tray icon
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Activate", OnActivate);
            trayMenu.MenuItems.Add("Deactivate", OnDeactivate);
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Caffeine for Windows";
            trayIcon.Icon = SystemIcons.Application; // You can replace this with a custom icon
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            // Start by activating stay awake mode
            PreventSleep();
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

        // Handle the "Exit" menu item click
        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;  // Hide the tray icon
            trayIcon.Dispose();        // Dispose of the tray icon to release resources
            AllowSleep();              // Restore normal sleep behavior
            Application.Exit();
        }

        // Form closing event handler to clean up resources
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            trayIcon.Visible = false;  // Hide the tray icon when the form closes
            trayIcon.Dispose();        // Dispose of the tray icon
            AllowSleep();              // Ensure normal sleep behavior is restored
            base.OnFormClosing(e);
        }
    }
}
