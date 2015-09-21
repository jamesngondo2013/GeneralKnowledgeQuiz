using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
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
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void history_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(history));
        }

        private void geography_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(geography));
        }

        private void maths_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Maths));
        }

        private void play_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Games));
        }

        private void about_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(about));
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            ViewHandler();
        }


        private void ViewHandler()
        {
            ApplicationView current = ApplicationView.GetForCurrentView();
            if (current.IsFullScreen)
            {
                Snap.Visibility = Visibility.Collapsed;

            }

            else
            {
                Snap.Visibility = Visibility.Visible;

            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;

        } 
    }
}
