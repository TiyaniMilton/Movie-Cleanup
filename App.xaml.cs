using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Movie_Cleanup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] OnLoadArgs;
        public Mutex mutex = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            OnLoadArgs = e.Args;
            if (e.Args.Length == 1) //make sure an argument is passed
            {
                OnLoadArgs = e.Args;
                FileInfo file = new FileInfo(e.Args[0]);
                if (file.Exists) //make sure it's actually a file
                {
                    //Do whatever
                    //MainWindow.DataContext = e.Args;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    // register single instance app. and check for existence of other process
        //    if (!ApplicationInstanceManager.CreateSingleInstance(
        //            Assembly.GetExecutingAssembly().GetName().Name,
        //            SingleInstanceCallback)) return; // exit, if same app. is running
        //    base.OnStartup(e);
        //}
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Activated"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            var win = MainWindow as MainWindow;
            var OnLoadArgsX = Environment.GetEnvironmentVariables().Values;//.Args;

            if (win == null) return;
            // add 1st args
            //win.ApendArgs(Environment.GetCommandLineArgs());
            win.Media_Drop();
        }
        ///// <summary>
        ///// Single instance callback handler.
        ///// </summary>
        ///// <param name="sender">The sender.</param>
        ///// <param name="args">The <see cref="SingleInstanceApplication.InstanceCallbackEventArgs"/> instance containing the event data.</param>
        //private void SingleInstanceCallback(object sender, InstanceCallbackEventArgs args)
        //{
        //    if (args == null || Dispatcher == null) return;
        //    Action<bool> d = (bool x) =>
        //    {
        //        var win = MainWindow as MainWindow;
        //        if (win == null) return;
        //        win.ApendArgs(args.CommandLineArgs);
        //        win.Activate(x);
        //    };
        //    Dispatcher.Invoke(d, true);
        //}
    }
}
