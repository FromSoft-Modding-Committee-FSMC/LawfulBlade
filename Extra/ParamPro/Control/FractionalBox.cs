using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParamPro.Control
{
    public class FractionalBox : TextBox
    {
        public bool AllowFractions { get; set; } = true;
        public bool Unsigned { get; set; } = false;
        public long MinValue { get; set; } = long.MinValue;
        public long MaxValue { get; set; } = long.MaxValue;

        public FractionalBox() : base()
        {
            PreviewTextInput += OnPreviewTextInput;
            PreviewKeyDown += OnPreviewKeyDown;
            LostFocus += OnFocusLost;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Listens for the preview text event, and makes sure only numerics and the decimal place is included
        /// </summary>
        void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Get the new character as char
            char newChar = e.Text[0];

            // Is the character '-'? Only allow it at the start of the text.
            if (newChar == '-')
            {
                if (Text.Length > 0 || Unsigned)
                    e.Handled = true;

                return;
            }

            // Is the character '.'? Only allow one.
            if (newChar == '.')
            {
                if (Text.Contains('.') || !AllowFractions)
                    e.Handled = true;

                return;
            }

            // Any other char must be decimal
            e.Handled = !char.IsDigit(newChar);
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Pharses the input as a decimal, then turns it back into a string - to ensure correct format
        /// </summary>
        void OnFocusLost(object sender, RoutedEventArgs e)
        {
            // If the string starts with a dot, add a zero to the start.
            if (Text.StartsWith('.'))
                Text = '0' + Text;

            // Trim any . at the end.
            Text = Text.TrimEnd('.');

            // Now attempt to parse the text as a decimal, and immediately shove it back in
            if (decimal.TryParse(Text, out decimal result))
            {
                // Clamp the result to the required range
                result = Math.Clamp(result, MinValue, MaxValue);

                // Set the text to the final parsed result
                Text = $"{result}";
            }
            else
            {
                MessageBox.Show("Invalid Number!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                Text = AllowFractions ? "0.0" : "0";
            }
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Handles the user pressing enter to confirm input.
        /// </summary>
        void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            // Focus the main window to remove focus from this control...
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), null);
            Keyboard.ClearFocus();

            // Set the event as handled.
            e.Handled = true;
        }
    }
}
