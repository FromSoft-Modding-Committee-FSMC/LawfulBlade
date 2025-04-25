using ParamPro.Format;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParamPro.Control
{
    /// <summary>
    /// Interaction logic for PRFNpcControl.xaml
    /// </summary>
    public partial class PRFNpcControl : UserControl
    {
        // The currently bound asset...
        EditorNpcPRF Asset { get; set; }

        public PRFNpcControl()
        {
            InitializeComponent();

            // Preload content...
            npcFXTypeField.ItemsSource = Enum.GetValues(typeof(FX2DType));
            animationSelector.ItemsSource = Enum.GetValues(typeof(NpcAnimations));
            EnumerateNpcModel();
            EnumerateNpcTexture();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the animation type changes...
        /// </summary>
        void OnAnimationTypeChange(object sender, SelectionChangedEventArgs e)
        {
            EnumerateNpcEffects();
            EnumerateNpcSounds();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnClickAddEffect(object sender, RoutedEventArgs e)
        {
            Asset.SpecialEffectsPerAnim[(int)animationSelector.SelectedItem].Add(new PrfSfxItem());
            EnumerateNpcEffects();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnClickRemoveEffect(object sender, RoutedEventArgs e)
        {
            if (effectList.Items.Count == 0 || effectList.SelectedIndex < 0)
                return;

            Asset.SpecialEffectsPerAnim[(int)animationSelector.SelectedItem].RemoveAt(effectList.SelectedIndex);
            EnumerateNpcEffects();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnClickAddSound(object sender, RoutedEventArgs e)
        {
            Asset.SoundsPerAnim[(int)animationSelector.SelectedItem].Add(new PrfSndItem());
            EnumerateNpcSounds();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnClickRemoveSound(object sender, RoutedEventArgs e)
        {
            if (soundList.Items.Count == 0 || soundList.SelectedIndex < 0)
                return;

            Asset.SoundsPerAnim[(int)animationSelector.SelectedItem].RemoveAt(soundList.SelectedIndex);
            EnumerateNpcSounds();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnFX2DChange(object sender, SelectionChangedEventArgs e) =>
            Asset.FXType = (FX2DType)npcFXTypeField.SelectedItem;

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEndEditFxCp(object sender, RoutedEventArgs e) =>
            Asset.FXControlPointID = byte.Parse(npcFXcpid.Text);

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEndEditFxLength(object sender, RoutedEventArgs e) =>
            Asset.FXLength = byte.Parse(npcFXframes.Text);

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEditEndStandFwdFrames(object sender, RoutedEventArgs e) =>
            Asset.StandTalkForwardLength = byte.Parse(npcIntrStandFwd.Text);

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEditEndStandLR(object sender, RoutedEventArgs e) =>
            Asset.StandTalkLeftRightLength = byte.Parse(npcIntrStandLR.Text);

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEditEndSitFwdFrames(object sender, RoutedEventArgs e) =>
            Asset.SittingTalkForwardLength = byte.Parse(npcIntrSitFwd.Text);

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEndEditSitSideFrames(object sender, RoutedEventArgs e) =>
            Asset.SittingTalkLeftRightLength = byte.Parse(npcIntrSitLR.Text);

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEditEndCollRad(object sender, RoutedEventArgs e) =>
            Asset.ColliderRadius = float.Parse(npcColliderRadius.Text);

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEditEndCollHeight(object sender, RoutedEventArgs e) =>
            Asset.ColliderHeight = float.Parse(npcColliderHeight.Text);

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEditEndShadowRad(object sender, RoutedEventArgs e) =>
            Asset.ShadowRadius = float.Parse(npcShadowRadius.Text);

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnTextureSelected(object sender, SelectionChangedEventArgs e)
        {
            if (npcTextureField.SelectedIndex != 0)
            {
                Asset.ExternalTextureFile = (string)npcTextureField.SelectedItem;
                Asset.UseExternalTexture  = true;
                return;
            }

            Asset.ExternalTextureFile = string.Empty;
            Asset.UseExternalTexture  = false;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnModelChanged(object sender, SelectionChangedEventArgs e) =>
            Asset.ModelFile = (string)npcModelField.SelectedItem;

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEndEditTurnSpeed(object sender, RoutedEventArgs e) =>
            Asset.TurnSpeed = float.Parse(npcTurnSpeedField.Text);

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEndEditName(object sender, RoutedEventArgs e) =>
            Asset.Name = npcNameField.Text;

        /// <summary>
        /// Loads the NPC Profile
        /// </summary>
        public void LoadAsset(string filepath)
        {
            Asset = EditorNpcPRF.LoadFromFile(filepath);

            ApplyAsset();
        }

        public void NewAsset()
        {
            Asset = EditorNpcPRF.Create();

            ApplyAsset();
        }

        void ApplyAsset()
        {
            // Select Name
            npcNameField.Text = Asset.Name;

            // Select Turn Speed
            npcTurnSpeedField.Text = $"{Asset.TurnSpeed}";

            // Select Model
            npcModelField.SelectedIndex = npcModelField.Items.IndexOf(Asset.ModelFile);

            // Select Texture
            if (!Asset.UseExternalTexture || npcTextureField.Items.IndexOf(Asset.ExternalTextureFile) < 0)
                npcTextureField.SelectedIndex = 0;
            else
                npcTextureField.SelectedIndex = npcTextureField.Items.IndexOf(Asset.ExternalTextureFile);

            // Select FX
            npcFXTypeField.SelectedIndex = npcFXTypeField.Items.IndexOf(Asset.FXType);
            npcFXcpid.Text = $"{Asset.FXControlPointID}";
            npcFXframes.Text = $"{Asset.FXLength}";

            // Select Collider
            npcColliderRadius.Text = $"{Asset.ColliderRadius}";
            npcColliderHeight.Text = $"{Asset.ColliderHeight}";

            // Select Shadow
            npcShadowRadius.Text = $"{Asset.ShadowRadius}";

            // Select interactions
            npcIntrStandFwd.Text = $"{Asset.StandTalkForwardLength}";
            npcIntrStandLR.Text = $"{Asset.StandTalkLeftRightLength}";
            npcIntrSitFwd.Text = $"{Asset.SittingTalkForwardLength}";
            npcIntrSitLR.Text = $"{Asset.SittingTalkLeftRightLength}";

            // Reset SFX/SND lists
            animationSelector.SelectedIndex = 0;
            EnumerateNpcEffects();
            EnumerateNpcSounds();
        }

        /// <summary>
        /// Loads all avaliable NPC models
        /// </summary>
        void EnumerateNpcModel()
        {
            // Clear the current files...
            npcModelField.Items.Clear();

            // Get New Files...
            DirectoryInfo npcModelDir = new DirectoryInfo(Path.Combine(App.TargetPath, "DATA", "NPC", "MODEL"));

            foreach (FileInfo mdlFile in npcModelDir.GetFiles("*.mdl"))
                npcModelField.Items.Add(mdlFile.Name);
        }

        /// <summary>
        /// Loads all avaliable external textures
        /// </summary>
        void EnumerateNpcTexture()
        {
            npcTextureField.Items.Clear();
            npcTextureField.Items.Add("Internal Texture");

            // Get New Files...
            DirectoryInfo npcTextureDir = new DirectoryInfo(Path.Combine(App.TargetPath, "DATA", "NPC", "MODEL"));

            foreach (FileInfo txrFile in npcTextureDir.GetFiles("*.txr"))
                npcModelField.Items.Add(txrFile.Name);
        }

        void EnumerateNpcEffects()
        {
            // Clear out effects list
            effectList.Items.Clear();

            // enumerate effects for this animation
            foreach (PrfSfxItem effectItem in Asset.SpecialEffectsPerAnim[(int)animationSelector.SelectedItem])
                effectList.Items.Add(new SfxItemControl(effectItem));
        }

        void EnumerateNpcSounds()
        {
            // Clear out sound list
            soundList.Items.Clear();

            // Enumerate sounds for this animation
            foreach (PrfSndItem soundItem in Asset.SoundsPerAnim[(int)animationSelector.SelectedItem])
                soundList.Items.Add(new SndItemControl(soundItem));
        }

        void OnClickSave(object sender, RoutedEventArgs e)
        {
            Asset.Save(Path.Combine(App.TargetPath, "DATA", "NPC", "PROF"));
        }
    }
}
