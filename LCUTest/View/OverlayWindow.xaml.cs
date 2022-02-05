using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LCUTest.View
{
    /// <summary>
    /// OverlayWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();

            InitializeBaseOnWindowSettings();

            this.Deactivated += (s, e) => Window_Deactivated(s, e);

            frameTest.Navigate(new LayoutPage());
        }

        private void InitializeBaseOnWindowSettings()
        {
            this.Topmost = true;

            SetWindowPrimaryLocation(this);
            //SetWindowPos(windowHandle, 0, secondMonitor.Left, secondMonitor.Top, secondMonitor.Width, secondMonitor.Height, 0);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        protected T SetWindowLocation<T>(T window) where T : Window
        {
            //This function will set a window to appear in the center of the user's primary monitor.
            //Size will be set dynamically based on resoulution but will not shrink below a certain size nor grow above a certain size

            //Desired size constraints.  Makes sure window isn't too small if the users monitor doesn't meet the minimum, but also not too big on large monitors
            //Min: 1024w x 768h
            //Max: 1400w x 900h

            const int absoluteMinWidth = 1024;
            const int absoluteMinHeight = 768;
            const int absoluteMaxWidth = 2560;
            const int absoluteMaxHeight = 1440;

            var maxHeightForMonitor = System.Windows.SystemParameters.WorkArea.Height - 100;
            var maxWidthForMonitor = System.Windows.SystemParameters.WorkArea.Width - 100;

            var height = Math.Min(Math.Max(maxHeightForMonitor, absoluteMinHeight), absoluteMaxHeight);
            var width = Math.Min(Math.Max(maxWidthForMonitor, absoluteMinWidth), absoluteMaxWidth);

            window.Height = height;
            window.Width = width;
            window.Left = (System.Windows.SystemParameters.FullPrimaryScreenWidth - width) / 2;
            window.Top = (System.Windows.SystemParameters.FullPrimaryScreenHeight - height) / 2;
            window.WindowStartupLocation = WindowStartupLocation.Manual;

            return window;
        }

        protected void SetWindowPrimaryLocation<T>(T window) where T : Window
        {
            //This function will set a window to appear in the center of the user's primary monitor.
            //Size will be set dynamically based on resoulution but will not shrink below a certain size nor grow above a certain size
            window.Height = SystemParameters.PrimaryScreenHeight;
            window.Width = SystemParameters.PrimaryScreenWidth;
            window.Left = 0;
            window.Top = 0;
            window.WindowStartupLocation = WindowStartupLocation.Manual;
        }
    }
}
