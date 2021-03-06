﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace watchmen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const int MINIMUM_SPLASH_TIME = 0;    // Miliseconds (eredetileg 3000)
        private const int SPLASH_FADE_TIME = 0;       // Miliseconds (eredetileg 500)

        protected override void OnStartup(StartupEventArgs e)
        {
            //splash screen implementáció
            //http://www.wpfsharp.com/2012/02/14/how-to-display-a-splash-screen-for-at-specific-minimum-length-of-time-in-wpf/
            // Step 1 - Load the splash screen
            SplashScreen splash = new SplashScreen("images/splashScreen.png");
            splash.Show(false, true);

            // Step 2 - Start a stop watch
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Step 3 - Load your windows but don't show it yet
            base.OnStartup(e);            

            // Step 4 - Make sure that the splash screen lasts at least two seconds
            timer.Stop();
            int remainingTimeToShowSplash = MINIMUM_SPLASH_TIME - (int)timer.ElapsedMilliseconds;
            if (remainingTimeToShowSplash > 0)
                Thread.Sleep(remainingTimeToShowSplash);
            
            splash.Close(TimeSpan.FromMilliseconds(SPLASH_FADE_TIME));
            //MainWindow main = new MainWindow();           //ez nem kell, mert app configból tudja, hogy melyik a MainWindow, és azt elindítja az OnStartup
            //main.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            this.Shutdown();
        }
    }
}
