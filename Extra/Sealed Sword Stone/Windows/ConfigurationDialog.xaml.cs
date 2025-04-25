using Sealed_Sword_Stone.Controls;
using Sealed_Sword_Stone.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sealed_Sword_Stone.Windows
{
    /// <summary>
    /// Interaction logic for ConfigurationDialog.xaml
    /// </summary>
    public partial class ConfigurationDialog : Window
    {
        // All our control bindings...
        public ControlBinding PlayerMoveForward = new ControlBinding(App.UserConfig.PlayerMoveForward);
        public ControlBinding PlayerMoveBackward = new ControlBinding(App.UserConfig.PlayerMoveBackward);
        public ControlBinding PlayerStrafeLeft = new ControlBinding(App.UserConfig.PlayerStrafeLeft);
        public ControlBinding PlayerStrafeRight = new ControlBinding(App.UserConfig.PlayerStrafeRight);
        public ControlBinding PlayerTurnLeft = new ControlBinding(App.UserConfig.PlayerTurnLeft);
        public ControlBinding PlayerTurnRight = new ControlBinding(App.UserConfig.PlayerTurnRight);
        public ControlBinding PlayerLookUp = new ControlBinding(App.UserConfig.PlayerLookUp);
        public ControlBinding PlayerLookDown = new ControlBinding(App.UserConfig.PlayerLookDown);
        public ControlBinding PlayerAttack = new ControlBinding(App.UserConfig.ActionAttack);
        public ControlBinding PlayerCast = new ControlBinding(App.UserConfig.ActionCast);
        public ControlBinding PlayerInspect = new ControlBinding(App.UserConfig.ActionInspect);
        public ControlBinding PlayerSprint = new ControlBinding(App.UserConfig.ActionSprint);
        public ControlBinding MenuOpen = new ControlBinding(App.UserConfig.MenuOpen);
        public ControlBinding MenuConfirm = new ControlBinding(App.UserConfig.MenuConfirm);
        public ControlBinding MenuCancel = new ControlBinding(App.UserConfig.MenuCancel);
        public ControlBinding MenuUp = new ControlBinding(App.UserConfig.MenuUp);
        public ControlBinding MenuDown = new ControlBinding(App.UserConfig.MenuDown);
        public ControlBinding MenuLeft = new ControlBinding(App.UserConfig.MenuLeft);
        public ControlBinding MenuRight = new ControlBinding(App.UserConfig.MenuRight);

        public ConfigurationDialog()
        {
            InitializeComponent();

            // Set content for tabs..
            kbmControlTab.Content = new KeyboardMouseTab(this);
            padControlTab.Content = new ControllerTab(this);
        }
    }
}
