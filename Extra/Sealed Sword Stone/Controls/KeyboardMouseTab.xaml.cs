using Sealed_Sword_Stone.Core;
using Sealed_Sword_Stone.Windows;
using System.Windows.Controls;

namespace Sealed_Sword_Stone.Controls
{
    /// <summary>
    /// Interaction logic for KeyboardMouseTab.xaml
    /// </summary>
    public partial class KeyboardMouseTab : UserControl
    {
        ConfigurationDialog tab;

        public KeyboardMouseTab(ConfigurationDialog confDialog)
        {
            InitializeComponent();

            tab = confDialog;

            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerMoveForward, "Move Fowards"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerMoveBackward, "Move Backwards"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerStrafeLeft, "Strafe Left"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerStrafeRight, "Strafe Right"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerTurnLeft, "Turn Left"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerTurnRight, "Turn Right"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerLookUp, "Look Up"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerLookDown, "Look Down"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerAttack, "Attack"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerCast, "Cast Magic"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerInspect, "Inspect"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.PlayerSprint, "Sprint"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.MenuOpen, "Open Menu"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.MenuConfirm, "Confirm Menu"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.MenuCancel, "Cancel Menu"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.MenuUp, "Cursor Up"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.MenuDown, "Cursor Down"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.MenuLeft, "Cursor Left"));
            mappingBox.Children.Add(new KeyboardMouseBinding(tab.MenuRight, "Cursor Right"));
        }
    }
}
