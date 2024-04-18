namespace LawfulBladeManager.Dialog
{
    public partial class BusyDialog : Form
    {
        readonly string[] waitMessages = new string[]
        {
            /* Thanks, me! */
            "Don't fall asleep just yet!",
            "Constructing Additional Pylons!",
            "I'm just too impatient to be patient!",
            "Better late than never!",
            "Did you know that (No. 65134): progress bars are annoying.",
            "99 BOTTLES OF BEER ON THE WALL, 99 BOTTLES OF BEER...",
            "Eeny meeny miny moe, why's this progress bar so slow?",
            "I'll be back in two shakes of a lambs tale.",
            "Your call is very important to us, so please continue to hold the line.",
            "Does anyone actually read these?",
            "November 5th, 1955!",
            "Waiting for Blinge to stop arguing with people.",
            "Tea, two sugars and a dash of milk please.",
            
            /* Thanks, meain! */
            "Don't worry - a few bits tried to escape, but we caught them",
            "Downloading more RAM...",
            "TODO: Insert elevator music...",
        };

        // Singleton Implementation
        static BusyDialog busyDialog = new BusyDialog();
        public static BusyDialog Instance => busyDialog;

        BusyDialog()
        {
            InitializeComponent();
        }

        public void ShowBusy()
        {
            // Set a random message to tell the user they're waiting an amount of time...
            Random rng = new();
            tbMessage.Text = waitMessages[rng.Next(waitMessages.Length)];

            // Start a task for show dialog, so it keeps updating even when the
            // main thread is frozen.
            Task.Run(() =>
            {
                ShowDialog();
            });
        }

        public void HideBusy()
        {
            BeginInvoke(new Action(Hide));
        }
    }
}
