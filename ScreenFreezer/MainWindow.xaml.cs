using System;
using System.Windows;
using System.Windows.Input;

namespace ScreenFreezer
{
    public partial class MainWindow : Window
    {
        private readonly TriggerSource _activeTSource;
        private readonly TriggerSource _clickTSource;

        public MainWindow()
        {
            InitializeComponent();
            MessageLabel.Content= "To show this application's UI, move mouse point to the top edge of the screen.\n" +
                "To freeze or unfreeze the screen, there are two ways.\n" +
                "1. Click on the top edge and the left edge of the screen in turn.\n" +
                "2. Double click here.";

            _activeTSource = new TriggerSource(pullImmed: false)
            {
                Interval = 3000,
                FinalAction = () => this.InvokeIfNeeded(() => ChangeActiveStatus(false)),
            };
            _clickTSource = new TriggerSource(pullImmed: false)
            {
                Interval = 2000,
            };
        }

        private void ExitLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            => Environment.Exit(0);

        private void MessageLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SwitchFreezeStatus();
            ChangeActiveStatus(false);
        }

        private void TopLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _clickTSource.CreateNewTrigger(pullImmed: true);
        }

        private void LeftLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_clickTSource.Trigger.IsAlive)
            {
                SwitchFreezeStatus();
                ChangeActiveStatus(false);
            }
        }

        private void TopLabel_MouseEnter(object sender, MouseEventArgs e)
        {
            _activeTSource.CreateNewTrigger(pullImmed: false);
            ChangeActiveStatus(true);
        }

        private void TopLabel_MouseLeave(object sender, MouseEventArgs e)
            => _activeTSource.Pull();
        
        private void ChangeActiveStatus(bool active)
        {
            double opacity = 0.01;  // active == false (default)
            Visibility visibility = Visibility.Hidden;
            if (active)
            {
                opacity = 1;
                visibility = Visibility.Visible;
            }

            TopLabel.Opacity = opacity;
            LeftLabel.Opacity = opacity;
            ExitLabel.Visibility = visibility;
            MessageLabel.Visibility = visibility;
        }

        private void SwitchFreezeStatus()
            => Background.Opacity = (Background.Opacity == 0.01) ? 0.00 : 0.01;
    }
}
