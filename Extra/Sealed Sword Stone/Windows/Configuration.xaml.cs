using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sealed_Sword_Stone;

/// <summary>
/// Interaction logic for Configuration.xaml
/// </summary>
public partial class Configuration : Window
{
    // Misc
    string unplugPath = Path.Combine(App.ProgramPath, "LAUNCHER", "96-96-Unplug.png");

    // User Configuration
    UserConfig configFile;

    // All Mapping Elements
    Grid forwardMappingCtrl;
    Grid backwardMappingCtrl;
    Grid moveRightMappingCtrl;
    Grid moveLeftMappingCtrl;
    Grid turnRightMappingCtrl;
    Grid turnLeftMappingCtrl;
    Grid lookUpMappingCtrl;
    Grid lookDownMappingCtrl;
    Grid attackMappingCtrl;
    Grid magicMappingCtrl;
    Grid inspectMappingCtrl;
    Grid openMenuMappingCtrl;
    Grid closeMenuMappingCtrl;
    Grid acceptMenuMappingCtrl;


    // Selected Mapping Grid Element
    Grid mappingElement;

    public Configuration()
    {
        configFile = UserConfig.Load();

        InitializeComponent();

        PreviewKeyDown   += OnPreviewKeyDown;
        PreviewMouseDown += OnPreviewMouseDown;

        // MOVEMENT
        AddHeader();
        forwardMappingCtrl = AddMappingControl("Forward",      configFile.MovePlayerForward);
        backwardMappingCtrl = AddMappingControl("Backward",     configFile.MovePlayerBack);
        moveRightMappingCtrl = AddMappingControl("Strafe Right", configFile.MovePlayerRight);
        moveLeftMappingCtrl = AddMappingControl("Strafe Left",  configFile.MovePlayerLeft);
        turnRightMappingCtrl = AddMappingControl("Turn Right", configFile.TurnPlayerRight);
        turnLeftMappingCtrl = AddMappingControl("Turn Left", configFile.TurnPlayerLeft);
        lookUpMappingCtrl = AddMappingControl("Look Up", configFile.LookPlayerUp);
        lookDownMappingCtrl = AddMappingControl("Look Down", configFile.LookPlayerDown);
        attackMappingCtrl = AddMappingControl("Attack", configFile.ActionAttack);
        magicMappingCtrl = AddMappingControl("Cast", configFile.ActionMagicCast);
        inspectMappingCtrl = AddMappingControl("Inspect", configFile.ActionInspect);
        openMenuMappingCtrl = AddMappingControl("Open Menu", configFile.ActionOpenMenu);
        closeMenuMappingCtrl = AddMappingControl("Close Menu", configFile.ActionCloseMenu);
        acceptMenuMappingCtrl = AddMappingControl("Accept Menu", configFile.ActionAcceptMenu);

        // Other
        useMouseLookCB.IsChecked = configFile.UseMouseLook;
    }

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (mappingElement == null)
            return;
        
        // The current map value, minus the key and device setting
        uint mapValue = (uint)mappingElement.Tag;
        mapValue &= 0xF0FFFF00;

        // Set map value keyboard device
        mapValue |= 0x02000000;

        // Set map value key
        mapValue |= ((uint)e.ChangedButton & 0xFF);

        // Store value into the tag again
        mappingElement.Tag = mapValue;

        TextBlock mapCtrl = (mappingElement.Children[1] as TextBlock);
        mapCtrl.Text = GetKeyMouseFromValue(mapValue);
    }

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (mappingElement == null)
            return;

        // Get Key as Virtual Key
        int mappedKey = KeyInterop.VirtualKeyFromKey(e.Key);

        // The current map value, minus the key and device setting
        uint mapValue  = (uint)mappingElement.Tag;
             mapValue &= 0xF0FFFF00;

        // Set map value keyboard device
        mapValue |= 0x01000000;

        // Set map value key
        mapValue |= (uint)(mappedKey & 0xFF);

        // Store value into the tag again
        mappingElement.Tag = mapValue;

        TextBlock mapCtrl = (mappingElement.Children[1] as TextBlock);
        mapCtrl.Text = GetKeyMouseFromValue(mapValue);
    }

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnEnterMapping(object sender, MouseEventArgs e)
    {
        TextBlock textBlock = (sender as TextBlock);

        if (textBlock == null)
            return;

        textBlock.Background = new SolidColorBrush(Color.FromArgb(96, 0, 16, 48));
        mappingElement = (textBlock.Parent as Grid);
    }

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnLeaveMapping(object sender, MouseEventArgs e)
    {
        TextBlock textBlock = (sender as TextBlock);

        if (textBlock == null)
            return;

        textBlock.Background = new SolidColorBrush(Color.FromArgb(64, 0, 0, 0));
        mappingElement = null;
    }

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnUnbindKeyMapping(object sender, MouseEventArgs e)
    {
        // Get Parent of sender
        Grid mappingElement = ((sender as Image).Parent as Grid);

        // Wipe the value
        mappingElement.Tag = 0xFFFFFFFF;

        // Get the text block
        TextBlock mappingText = mappingElement.Children[1] as TextBlock;
        mappingText.Text = GetKeyMouseFromValue((uint)mappingElement.Tag);     
    }

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnClickCancel(object sender, RoutedEventArgs e) =>
        Close();

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnClickConfirm(object sender, RoutedEventArgs e)
    {
        // Copy to config file...
        configFile.MovePlayerForward = (uint)forwardMappingCtrl.Tag;
        configFile.MovePlayerBack = (uint)backwardMappingCtrl.Tag;
        configFile.MovePlayerRight = (uint)moveRightMappingCtrl.Tag;
        configFile.MovePlayerLeft = (uint)moveLeftMappingCtrl.Tag;
        configFile.TurnPlayerRight = (uint)turnRightMappingCtrl.Tag;
        configFile.TurnPlayerLeft = (uint)turnLeftMappingCtrl.Tag;
        configFile.LookPlayerUp = (uint)lookUpMappingCtrl.Tag;
        configFile.LookPlayerDown = (uint)lookDownMappingCtrl.Tag;
        configFile.ActionAttack = (uint)attackMappingCtrl.Tag;
        configFile.ActionMagicCast = (uint)magicMappingCtrl.Tag;
        configFile.ActionInspect = (uint)inspectMappingCtrl.Tag;
        configFile.ActionOpenMenu = (uint)openMenuMappingCtrl.Tag;
        configFile.ActionCloseMenu = (uint)closeMenuMappingCtrl.Tag;
        configFile.ActionAcceptMenu = (uint)acceptMenuMappingCtrl.Tag;

        // Other
        configFile.UseMouseLook = useMouseLookCB.IsChecked ?? false;

        // Save Config file
        configFile.Save();

        // Close Window
        Close();
    }

    /// <summary>
    /// Adds a mapping control
    /// </summary>
    Grid AddMappingControl(string controlName, uint value)
    {
        Grid borderGrid = new Grid
        {
            Background = Brushes.Cornsilk,
            Height = 32,
            Margin = new Thickness(1, 1, 1, 0),
            Tag = value
        };
        borderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2.0, GridUnitType.Star) });
        borderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
        borderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });
        borderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
        borderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });

        // Label
        borderGrid.Children.Add(new TextBlock
        {
            FontSize = 16,
            Text     = controlName,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment   = VerticalAlignment.Center
        });
        Grid.SetColumn(borderGrid.Children[0], 0);

        // Keyboard - Mouse Mapping
        borderGrid.Children.Add(new TextBlock
        {
            FontSize = 12,
            Text = GetKeyMouseFromValue(value),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = VerticalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(64, 0, 0, 0))
        });
        Grid.SetColumn(borderGrid.Children[1], 1);
        borderGrid.Children[1].MouseEnter += OnEnterMapping;
        borderGrid.Children[1].MouseLeave += OnLeaveMapping;

        // Keyboard-Mouse Unbind
        borderGrid.Children.Add(new Image
        {
            Source = new BitmapImage(new Uri(unplugPath)),
            Width  = 24, Height = 24,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Tag = borderGrid.Children[1]
        });
        Grid.SetColumn(borderGrid.Children[2], 2);
        borderGrid.Children[2].MouseDown += OnUnbindKeyMapping;

        // Gamepad Mapping
        borderGrid.Children.Add(new TextBlock
        {
            FontSize = 12,
            Text = "Unbound",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = VerticalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(64, 0, 0, 0))
        });
        Grid.SetColumn(borderGrid.Children[3], 3);

        // Gamepad Unbind
        borderGrid.Children.Add(new Image
        {
            Source = new BitmapImage(new Uri(unplugPath)),
            Width = 24,
            Height = 24,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        });
        Grid.SetColumn(borderGrid.Children[4], 4);

        bindingsList.Children.Add(borderGrid);

        return borderGrid;
    }

    /// <summary>
    /// Adds a header control
    /// </summary>
    void AddHeader()
    {
        Grid borderGrid = new Grid
        {
            Background = Brushes.Cornsilk,
            Height     = 32,
            Margin     = new Thickness(1, 1, 1, 0)
        };
        borderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2.0, GridUnitType.Star) });
        borderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
        borderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });
        borderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
        borderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });

        borderGrid.Children.Add(new TextBlock
        {
            Background = new SolidColorBrush(Color.FromArgb(96, 0, 32, 96)),
            FontSize = 20,
            Text = "Action",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Width = bindingsList.Width,
            Height = 32,
            Margin = new Thickness(1, 1, 1, 0),
        });
        Grid.SetColumn(borderGrid.Children[0], 0);

        borderGrid.Children.Add(new TextBlock
        {
            Background = new SolidColorBrush(Color.FromArgb(96, 0, 32, 96)),
            FontSize = 20,
            Text = "Key & Mouse Binding",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Width = bindingsList.Width,
            Height = 32,
            Margin = new Thickness(1, 1, 1, 0),
        });
        Grid.SetColumn(borderGrid.Children[1], 1);

        borderGrid.Children.Add(new TextBlock
        {
            Background = new SolidColorBrush(Color.FromArgb(96, 0, 32, 96)),
            FontSize = 20,
            Text = "Gamepad Binding",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Width = bindingsList.Width,
            Height = 32,
            Margin = new Thickness(1, 1, 1, 0),
        });
        Grid.SetColumn(borderGrid.Children[2], 3);

        bindingsList.Children.Add(borderGrid);
    }

    /// <summary>
    /// Turns a key-mouse mapping value into text
    /// </summary>
    string GetKeyMouseFromValue(uint value)
    {    
        // Keyboard or Mouse?
        return (value & 0x0F000000) switch
        {
            0x01000000 => $"{KeyInterop.KeyFromVirtualKey((int)value & 0xFF)}",
            0x02000000 => $"Mouse {((MouseButton)(value & 0xFF))}",
            _          => "Unbound"
        };
    }
}
