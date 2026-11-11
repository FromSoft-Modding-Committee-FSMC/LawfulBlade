using ImageMagick;
using LawfulBlade.Core;
using LawfulBlade.Core.Extensions;
using LawfulBlade.Dialog;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LawfulBlade.Control
{
    /// <summary>
    /// Interaction logic for InstanceControl.xaml
    /// </summary>
    public partial class InstanceControl : UserControl
    {
        public Instance Instance { get; private set; }

        public InstanceControl(Instance instance)
        {
            // Initialize the control
            InitializeComponent();

            // Now copy data from the instance...
            instIconField.Source = instance.IconImage.ToBitmapSource();
            instNameField.Text   = instance.Name;
            instDescField.Text   = instance.Description;

            // Load tags from the instance
            tagList.Children.Clear();

            foreach(string tag in instance.Tags)
            {
                // Add all letters of tag together
                int tagValue = 0;
                foreach (char c in tag)
                    tagValue += c;

                // Calculate the background colour of the tag...
                Color background;
                background = (tagValue % 7) switch
                {
                    0 => Color.FromRgb(0xE7, 0x4C, 0x3C),
                    1 => Color.FromRgb(0x2E, 0xCC, 0x71),
                    2 => Color.FromRgb(0x34, 0x98, 0xDB),
                    3 => Color.FromRgb(0xF1, 0xC4, 0x0F),
                    4 => Color.FromRgb(0x8E, 0x44, 0xAD),
                    5 => Color.FromRgb(0x1A, 0xBC, 0x9C),
                    _ => Color.FromRgb(0xBD, 0xC3, 0xC7)
                };

                background = background.Mix(((tagValue ^ 0x4) % 7) switch
                {
                    0 => Color.FromRgb(0xA9, 0x32, 0x26),
                    1 => Color.FromRgb(0x22, 0x99, 0x54),
                    2 => Color.FromRgb(0x24, 0x71, 0xA3),
                    3 => Color.FromRgb(0xD6, 0x89, 0x10),
                    4 => Color.FromRgb(0x5B, 0x2C, 0x6F),
                    5 => Color.FromRgb(0x13, 0x8D, 0x75),
                    _ => Color.FromRgb(0x70, 0x7B, 0x7C)
                }, 0.5f);

                // Calculate the foreground colour of the tag
                int b = (int)(255f * background.GetSaturation() + background.GetBrightness()) / 2;

                tagList.Children.Add(new TextBlock
                {
                    Text       = tag,
                    Foreground = new SolidColorBrush(Color.FromRgb((byte)b, (byte)b, (byte)b)),
                    Background = new SolidColorBrush(background),
                    Margin     = new Thickness(0, 0, 8, 0),
                    Padding    = new Thickness(4, 0, 4, 0)
                });
            }

            Instance = instance;

            RefreshActiveState();
        }

        /// <summary>
        /// Sets the active instance start visibility
        /// </summary>
        public void RefreshActiveState()
        {
            instActive.Visibility = Instance.IsActiveInstance ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when a user clicks the delete button.
        /// </summary>
        void OnClickDelete(object sender, RoutedEventArgs e)
        {
            // Don't allow when open
            if (Message.Warning("This instance (or a project using it) is currently open!", InstanceManager.CurrentOpenInstance == Instance))
                return;

            // Maybe move this UI code somewhere else...
            if (MessageBox.Show("Warning! This will remove the instance from Lawful Blade and the file system! Are you sure you want to continue?", "Lawful Blade", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                return;

            // Delete Instance Content...
            InstanceManager.RemoveInstance(Instance);
            Instance.Delete();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Creates a shortcut to the instance when pressed.
        /// </summary>
        void OnClickCreateShortcut(object sender, RoutedEventArgs e) =>
            Instance.CreateShortcut();

        /// <summary>
        /// Event Callback.<br/>
        /// Opens the package manager dialog with this instance as the target
        /// </summary>
        void OnClickManagePackages(object sender, RoutedEventArgs e)
        {
            // Don't allow when open
            if (Message.Warning("This instance (or a project using it) is currently open!", InstanceManager.CurrentOpenInstance == Instance))
                return;

            // Show the Package Manager Dialog
            (new PackageManagerDialog(Instance)).ShowDialog();

            Instance.Save();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Opens the instance in the explorer.
        /// </summary>
        void OnClickOpenExplorer(object sender, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"\"{Instance.Root}\"", UseShellExecute = true });

        /// <summary>
        /// Event Callback.<br/>
        /// Launches the instance.
        /// </summary>
        void OnClickLaunch(object sender, RoutedEventArgs e)
        {
            // Don't allow if _any_ instance is open.
            if (Message.Warning("Another instance or project is currently open!", InstanceManager.CurrentOpenInstance != null))
                return;

            Instance.Launch(null, false);
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the mouse enters the instance control to enable a highlight.
        /// </summary>
        void OnMouseEnter(object sender, MouseEventArgs e) =>
            Background = new SolidColorBrush(Color.FromRgb(32, 48, 64));

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the mouse leaves the instance control to disable highlight.
        /// </summary>
        void OnMouseLeave(object sender, MouseEventArgs e) =>
            Background = new SolidColorBrush(Color.FromRgb(32, 32, 32));

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the context menu is opened for this instance, filling it with data.
        /// </summary>
        void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // Fill the context menu with items...
            foreach (MenuItem item in contextMenu.Items)
                item.Click -= OnContextMenuItemClick;

            contextMenu.Items.Clear();

            if (Instance.Tools != null)
            {
                foreach (InstanceTool tool in Instance.Tools)
                {
                    MenuItem newItem = AddMenuItem(contextMenu, tool.MenuPath);

                    // Store the command inside the tag, for later use...
                    newItem.Tag = tool.Command;

                    if (newItem != null)
                        newItem.Click += OnContextMenuItemClick;
                }
                    
            }

            // If it is empty, cancel it by marking the event as handled
            if (contextMenu.Items.Count == 0)
                e.Handled = true;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when an owned context menu item is clicked, executing its command.
        /// </summary>
        void OnContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // First we need to load in variables for the current instance...
                Instance.LoadInstanceVariables();

                // Recover the command and arguments for execution...
                InstanceToolCommand command = (InstanceToolCommand)(sender as MenuItem).Tag;

                if (string.IsNullOrEmpty(command.Executable))
                    return;

                string expandedCommand = Instance.ExpandVariable(command.Executable, null);

                string argumentStrings = string.Empty;
                if (command.Arguments != null)
                {
                    foreach (string argument in command.Arguments)
                    {
                        if (string.IsNullOrEmpty(argument))
                            continue;

                        if (!string.IsNullOrEmpty(argumentStrings))
                            argumentStrings += ' ';

                        argumentStrings += Instance.ExpandVariable(argument, null);
                    }
                }

                // Now we have our expanded command and arguments, we can execute it...
                Process commandProcess = new();
                commandProcess.StartInfo = new ProcessStartInfo
                {
                    FileName = expandedCommand,
                    Arguments = argumentStrings
                };

                commandProcess.Start();
            }
            catch (Exception ex)
            {
                Core.Debug.Error($"Could not execute command: {ex.Message}");
                Core.Debug.Critical($"\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Helper to add items into the context menu
        /// </summary>
        static MenuItem AddMenuItem(ContextMenu menu, string path)
        {
            ItemCollection items = menu.Items;
            MenuItem current = null;

            foreach (string part in path.Split('\\'))
            {
                current = items.OfType<MenuItem>().FirstOrDefault(item => (string)item.Header == part);

                if (current == null)
                {
                    current = new MenuItem { Header = part };
                    items.Add(current);
                }

                items = current.Items;
            }

            return current;
        }
    }
}
