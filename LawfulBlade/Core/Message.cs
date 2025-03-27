using System.Windows;

namespace LawfulBlade.Core
{
    public class Message
    {
        public static void SimpleError(string message) =>
            MessageBox.Show(message, "Lawful Blade", MessageBoxButton.OK, MessageBoxImage.Error);

        public static bool Warning(string message, bool condition)
        {
            if (condition)
                MessageBox.Show(message, "Lawful Blade", MessageBoxButton.OK, MessageBoxImage.Warning);

            return condition;
        }

        public static bool Info(string message, bool condition)
        {
            if (condition)
                MessageBox.Show(message, "Lawful Blade", MessageBoxButton.OK, MessageBoxImage.Information);

            return condition;
        }    
        
        /// <summary>
        /// Asks the user a Yes/No question
        /// </summary>
        /// <param name="message">The question</param>
        /// <param name="condition">The condition to ask on.</param>
        /// <returns><b>TRUE</b> if the user presses yes, <b>FALSE</b> if the user presses no. True otherwise.</returns>
        public static bool WarningYesNo(string message, bool condition)
        {
            if (condition)
                return (MessageBox.Show(message, "Lawful Blade", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes);

            return true;
        }
    }
}
