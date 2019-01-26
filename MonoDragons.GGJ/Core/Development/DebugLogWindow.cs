using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MonoDragons.Core.Development
{
    public static class DebugLogWindow
    {
        private static Form _window;
        private static List<Func<string, bool>> _filters = new List<Func<string, bool>>();

        public static void Show()
        {
            _window?.BringToFront();
        }

        public static void Exclude(Func<string, bool> predicate)
        {
            _filters.Add(predicate);
        }
        
        public static void Launch()
        {
#if DEBUG
            if (_window != null)
                return;
            
            _window = new Form
            {
                StartPosition = FormStartPosition.Manual,
                Top = 0,
                Left = 0,
                Width = 1280,
                Height = 1000,
            };
            var logBox = new RichTextBox
            {
                Width = 1280,
                Height = 1000,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };
            Logger.AddSink(x =>
            {
                if (_filters.Any(f => f(x)))
                    return;
                
                logBox.AppendText($"{DateTime.Now.TimeOfDay} - {x}{Environment.NewLine}");
            });
            _window.Controls.Add(logBox);
            _window.Show();
            _window.SendToBack();
 #endif
        }
    }
}