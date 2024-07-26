using System.Diagnostics;

namespace LawfulBladeManager.Dialog
{
    public partial class BusyDialog : Form
    {
        readonly string[] waitMessages = new string[]
        {
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
        };

        // Singleton Implementation
        public static BusyDialog Instance { get; private set; } = new();

        // Cache an instance of random here to avoid fucking the GC'er
        static readonly Random random = new();

        // Busy state is used because async is annoying to think about.
        enum BusyState
        {
            Closed,
            WaitForOpen,
            Open
        }

        // The busy state holder...
        BusyState state = BusyState.Closed;

        BusyDialog()
        {
            InitializeComponent();

            // We need to know when the dialog is opened.
            Shown += BusyDialog_Shown;
        }

        public void ShowBusy()
        {
            // Set initial result to wait for open...
            Task.Run(ShowDialog);

            state = BusyState.WaitForOpen;

            // Set a random message to tell the user they're waiting an amount of time...
            Text = "Doing a thing...";
            tbMessage.Text = waitMessages[random.Next(waitMessages.Length)];
        }

        public void ShowBusy(string customTitle, string customMessage)
        {
            // Set initial result to wait for open...
            Task.Run(ShowDialog);

            state = BusyState.WaitForOpen;

            // Set the custom title
            Text = customTitle;
            tbMessage.Text  = customMessage;
        }

        void BusyDialog_Shown(object? sender, EventArgs e) =>
            state = BusyState.Open;

        public void HideBusy()
        {
            // Exit early if already closed...
            if (state == BusyState.Closed)
                return;

            // If state is currently wait for open, we need to pause for a while
            while (state == BusyState.WaitForOpen)
                continue;

            // Invoke an action to hide the busy dialog
            BeginInvoke(new Action(Hide));

            state = BusyState.Closed;
        }
    }
}
