using System.Windows;
using System.Windows.Threading;

namespace LawfulBlade.Dialog
{
    /// <summary>
    /// Interaction logic for BusyDialog.xaml
    /// </summary>
    public partial class BusyDialog : Window
    {
        // Array of various waiting messages
        static readonly string[] waitMessages =
        [
            /* Thanks, me! */
            "Don't fall asleep just yet!",
            "You must construct additional pylons!",
            "I'm just too impatient to be patient!",
            "Better late than never!",
            "Did you know that (No. 65134): progress bars are annoying.",
            "99 BOTTLES OF BEER ON THE WALL, 99 BOTTLES OF BEER...",
            "Eeny meeny miny moe, why's this progress bar so slow?",
            "We'll be back in two shakes of a lambs tail.",
            "Your call is very important to us, so please continue to hold the line...",
            "November 5th, 1955!",
            "Waiting for Blinge to stop arguing with random internet ding dongs.",
            "Tea, two sugars and a dash of milk please.",
            "A solar flare caused a bit flip, and we're trying to recover...",
            "Grinding Lvl 3 weapons at the fire-arrow faces",
            "Waiting for the King's Field V announcement",
            "Comparing equipment stats in Shadow Tower",
            "Waiting for Prince Devian Rosberg to turn around\r\n(He'll get there, someday...)",

            /* Thanks, Kurobake! */
            "Appears to be dead (just loading, though)",
            "Placing skeletons in boxes...",
            "Seamlessly streaming in data without load screens...",
            "Waiting for Dragon Tree Fruit to regenerate...",
            "Testing for illusory walls...",
            "Getting lost in the mines...",
            "Scrolling through inventory for the correct key...",
            "Waiting for poison mist to dissipate...",

            /* Thanks, meain! */
            "Don't worry - a few bits tried to escape, but we caught them",
            "Downloading more RAM...",
            "TODO: Insert elevator music...",
        ];

        // Cache an instance of random here to avoid fucking the GC'er
        static readonly Random random = new();

        // We store the instance of busy to make sure only one exists...
        static BusyDialog instance;
        static Thread busyThread;


        /// <summary>
        /// Private Constructor...
        /// </summary>
        BusyDialog() =>
            InitializeComponent();

        /// <summary>
        /// Shows the busy dialog with a specific title and message
        /// </summary>
        public static void ShowBusy(string title, string message)
        {
            // Don't show busy if it is not null...
            if (instance != null || busyThread != null)
                return;

            busyThread = new(() =>
            {
                instance = new BusyDialog();

                // Set the title and message...
                instance.busyWindow.Title = title;
                instance.busyMessage.Text = message;

                // Show the busy dialog
                instance.ShowDialog();
            });
  
            busyThread.SetApartmentState(ApartmentState.STA);
            busyThread.Start();
        }

        /// <summary>
        /// Shows the busy dialog with a specific message
        /// </summary>
        public static void ShowBusy(string message) =>
            ShowBusy("Lawful Blade - Busy...", message);

        /// <summary>
        /// Shows the busy dialog with a random message
        /// </summary>
        public static void ShowBusy() =>
            ShowBusy("Lawful Blade - Busy...", waitMessages[random.Next(waitMessages.Length)]);

        /// <summary>
        /// Stops the busy dialog...
        /// </summary>
        public static void HideBusy()
        {
            // Get the dispatcher
            Dispatcher busyDispatcher = Dispatcher.FromThread(busyThread);
            busyDispatcher.Invoke(instance.Close);
            busyThread.Join(1000);  // Do we really want this?..

            // Now we can clear the dispatcher...
            busyDispatcher = null;
            busyThread = null;
            instance = null;
        }
    }
}

/*


        public void HideBusy()
        {
            // Exit early if already closed...
            if (state == BusyState.Closed)
                return;

            // Invoke an action to hide the busy dialog
            Application.Current.Dispatcher.BeginInvoke(Hide);

            state = BusyState.Closed;
        }
            */