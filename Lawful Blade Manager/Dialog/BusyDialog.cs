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
            "November 5th, 1955!",
            "Waiting for Blinge to stop arguing with random ding dongs.",
            "Tea, two sugars and a dash of milk please.",
            
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
        static BusyDialog busyDialog = new BusyDialog();
        static Random random = new Random();

        public static BusyDialog Instance => busyDialog;

        BusyDialog()
        {
            InitializeComponent();
        }

        public void ShowBusy()
        {
            // Set a random message to tell the user they're waiting an amount of time...
            tbMessage.Text = waitMessages[random.Next(waitMessages.Length)];

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
