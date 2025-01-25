using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Final_Project_CIS_297
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Directions : Page
    {
        public Directions()
        {
            this.InitializeComponent();
            ControllerControlsBlock.Text = "Controls for Controller:\n - Move with the D pad\n - Attack enemies with the A button\n - Toggle fog with the Left Bumper";
            KeyboardControlsBlock.Text = "Controls for Keyboard:\n - Move with the arrow keys\n - Attack enemies with the spacebar\n - Toggle fog with the f key";
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Frame rootframe = Window.Current.Content as Frame;

            rootframe.Navigate(typeof(BlankPage1));
        }

        private void ControllerControlsBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
