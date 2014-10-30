using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Commit
{
    public sealed partial class MainPage : Page
    {
        object defaultButtonPressedBackgroundThemeBrush;
        object defaultButtonPressedForegroundThemeBrush;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            var applicationView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            applicationView.SetDesiredBoundsMode(Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainContent.RenderTransform = new CompositeTransform();
            LeftMenu.RenderTransform = new CompositeTransform();
            VisualStateManager.GoToState(this, MenuClosedVisualState.Name, false);
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xF5, 0x65, 45));
            (sender as Button).Foreground = new SolidColorBrush(Colors.White);
        }

        private void NoButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            defaultButtonPressedBackgroundThemeBrush = App.Current.Resources["ButtonPressedBackgroundThemeBrush"];
            defaultButtonPressedForegroundThemeBrush = App.Current.Resources["ButtonPressedForegroundThemeBrush"];

            App.Current.Resources["ButtonPressedBackgroundThemeBrush"] = new SolidColorBrush(Color.FromArgb(0xFF, 0xF5, 0x65, 0x45));
            App.Current.Resources["ButtonPressedForegroundThemeBrush"] = new SolidColorBrush(Colors.White);//App.Current.Resources["AppBackgroundThemeBrush"];
        }

        private void YesButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            defaultButtonPressedBackgroundThemeBrush = App.Current.Resources["ButtonPressedBackgroundThemeBrush"];
            defaultButtonPressedForegroundThemeBrush = App.Current.Resources["ButtonPressedForegroundThemeBrush"];

            App.Current.Resources["ButtonPressedBackgroundThemeBrush"] = new SolidColorBrush(Color.FromArgb(0xFF, 0x77, 0xDD, 0xBB));
            App.Current.Resources["ButtonPressedForegroundThemeBrush"] = new SolidColorBrush(Colors.White);// App.Current.Resources["AppBackgroundThemeBrush"];
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            App.Current.Resources["ButtonPressedBackgroundThemeBrush"] = defaultButtonPressedBackgroundThemeBrush;
            App.Current.Resources["ButtonPressedForegroundThemeBrush"] = defaultButtonPressedForegroundThemeBrush;
        }

        private void MainContent_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var delta = Math.Round(e.Delta.Translation.X * 10) / 10;

            if ((MainContent.RenderTransform as CompositeTransform).TranslateX >= 300 && delta > 0)
                return;

            if ((MainContent.RenderTransform as CompositeTransform).TranslateX <= -35 && delta < 0)
                return;

            (MainContent.RenderTransform as CompositeTransform).TranslateX += delta;
            (LeftMenu.RenderTransform as CompositeTransform).TranslateX += delta / 10;
        }

        private void MainContent_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Debug.WriteLine("MainContent_ManipulationStarted");
        }

        private void Page_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Debug.WriteLine("Page_ManipulationCompleted");

            bool openMenu = e.Cumulative.Translation.X > 100;
            bool closeMenu = e.Cumulative.Translation.X < 100 || (MainContent.RenderTransform as CompositeTransform).TranslateX < 150;

            if(openMenu || !closeMenu)
            {
                VisualStateManager.GoToState(this, MenuClosedVisualState.Name, true);
                VisualStateManager.GoToState(this, MenuOpenedVisualState.Name, true);
            }
            else
            {
                VisualStateManager.GoToState(this, MenuOpenedVisualState.Name, true);
                VisualStateManager.GoToState(this, MenuClosedVisualState.Name, true);
            }
        }

        private void CommandBar_Opened(object sender, object e)
        {
            Debug.WriteLine("CommandBar_Opened");
            (sender as CommandBar).Background = App.Current.Resources["AppBackgroundThemeBrush"] as SolidColorBrush;
        }

        private void CommandBar_Closed(object sender, object e)
        {
            Debug.WriteLine("CommandBar_Closed");
            (sender as CommandBar).Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
