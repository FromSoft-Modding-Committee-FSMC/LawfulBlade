using ParamPro.Format;
using System.Windows.Controls;

namespace ParamPro.Control
{
    /// <summary>
    /// Interaction logic for PRFObjectControl.xaml
    /// </summary>
    public partial class PRFObjectControl : UserControl
    {
        // The currently bound asset...
        public EditorObjPRF Asset { get; private set; }

        public PRFObjectControl()
        {
            InitializeComponent();

            // Load items into the type...
            objTypeField.ItemsSource = Enum.GetValues(typeof(EditorObjPRF.EditorObjContoller));
            objColliderField.ItemsSource = Enum.GetValues(typeof(EditorObjPRF.EditorObjCollider));
        }

        public void LoadAsset(string filepath)
        {
            // Load the file
            Asset = EditorObjPRF.LoadFromFile(filepath);

            objNameField.Text = Asset.Name;
            objTypeField.SelectedIndex = objTypeField.Items.IndexOf(Asset.ClassID);
            objBillboardField.IsChecked = Asset.IsBillboard;
            objLoopAnimField.IsChecked = Asset.LoopAnimation;
            objOpenableField.IsChecked = Asset.IsOpenable;
            objVisibleField.IsChecked = Asset.Visible;
            objXZRotField.IsChecked = Asset.FreeRotation;
            objColliderField.SelectedIndex = objColliderField.Items.IndexOf(Asset.ColliderShape);
            objf32x4c.Text = Asset.f32x4c.ToString();

            objColWidth.Text = Asset.ColliderRadiusWidth.ToString();
            objColHeight.Text = Asset.ColliderHeight.ToString();
            objColDepth.Text = Asset.ColliderDepth.ToString();
        }
    }
}
