﻿#pragma checksum "D:\School\CIS 297\Final Project (Dig Dug)\cis297-winter2024-finalproject-Cjoynes2004\Final Project CIS 297\GamePage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C3BA7ED6F950BC2331604F89EACFE12887DEB704A6E96846B5AB607C55D36412"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Final_Project_CIS_297
{
    partial class GamePage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // GamePage.xaml line 15
                {
                    this.canvas = (global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)(target);
                    ((global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)this.canvas).Draw += this.Canvas_Draw;
                    ((global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)this.canvas).Update += this.Canvas_Update;
                    ((global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)this.canvas).CreateResources += this.Canvas_CreateResources;
                }
                break;
            case 3: // GamePage.xaml line 16
                {
                    global::Windows.UI.Xaml.Controls.TextBlock element3 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBlock)element3).SelectionChanged += this.TextBlock_SelectionChanged;
                }
                break;
            case 4: // GamePage.xaml line 21
                {
                    this.LevelBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5: // GamePage.xaml line 22
                {
                    this.ScoreBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

