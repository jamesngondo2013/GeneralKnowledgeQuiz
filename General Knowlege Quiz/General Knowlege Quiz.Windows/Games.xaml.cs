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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace General_Knowlege_Quiz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Games : Page
    {
        public Games()
        {
            this.InitializeComponent();
        }

        private void geography_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(geography));
        }

        private void history_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(history));
        }

        private void maths_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Maths));
        }

        private void exit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
