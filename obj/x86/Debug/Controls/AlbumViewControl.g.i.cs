﻿#pragma checksum "..\..\..\..\Controls\AlbumViewControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B60FDEEDECF55B4EE923DE4BD9EDB6C9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Movie_Cleanup.Controls {
    
    
    /// <summary>
    /// AlbumViewControl
    /// </summary>
    public partial class AlbumViewControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 44 "..\..\..\..\Controls\AlbumViewControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblAlbumName;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Controls\AlbumViewControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lstvwAlbumTracks;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\Controls\AlbumViewControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContextMenu ZoneInformationList;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\Controls\AlbumViewControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem OpenContainingFolderContextMenu;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\Controls\AlbumViewControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem GetTagInfoContextMenu;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Movie Cleanup;component/controls/albumviewcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\AlbumViewControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.lblAlbumName = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.lstvwAlbumTracks = ((System.Windows.Controls.ListView)(target));
            
            #line 49 "..\..\..\..\Controls\AlbumViewControl.xaml"
            this.lstvwAlbumTracks.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.lstvwAlbumTracks_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ZoneInformationList = ((System.Windows.Controls.ContextMenu)(target));
            return;
            case 4:
            this.OpenContainingFolderContextMenu = ((System.Windows.Controls.MenuItem)(target));
            
            #line 56 "..\..\..\..\Controls\AlbumViewControl.xaml"
            this.OpenContainingFolderContextMenu.Click += new System.Windows.RoutedEventHandler(this.OpenContainingFolderContextMenu_OnClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.GetTagInfoContextMenu = ((System.Windows.Controls.MenuItem)(target));
            
            #line 57 "..\..\..\..\Controls\AlbumViewControl.xaml"
            this.GetTagInfoContextMenu.Click += new System.Windows.RoutedEventHandler(this.GetTagInfoContextMenuContextMenu_OnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 74 "..\..\..\..\Controls\AlbumViewControl.xaml"
            ((System.Windows.Controls.Image)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.imgAddToPlaylist_MouseLeftButtonDown);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

