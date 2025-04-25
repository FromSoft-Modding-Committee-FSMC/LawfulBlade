using ParamPro.Control;
using ParamPro.Format;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ParamPro;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // Editor Roots
    TreeViewItem npcsRootNode;
    TreeViewItem enemiesRootNode;
    TreeViewItem itemsRootNode;
    TreeViewItem objectsRootNode;
    TreeViewItem tilesRootNode;

    // Project Roots

    // Controls
    PRMMapControl prmMapControl = new();
    PRFObjectControl prfObjectControl = new();
    PRFNpcControl prfNpcControl = new();

    // We use this enum to say what sort of item is inside a tree view tag
    enum TargetItemType
    {
        PrtMap,
        PrfObject,
        PrfNpc
    }

    // We use this struct to shove more data into the treeviewitem tag than we really should
    struct TargetItem
    {
        public TargetItemType type;
        public string path;
    }

    public MainWindow()
    {
        InitializeComponent();
        
        switch(App.TargetType)
        {
            case App.SomTargetType.Editor:
                LoadEditorTarget();
                break;

            default:
                throw new Exception();
        }
    }

    /// <summary>
    /// Loads files from an instance.
    /// </summary>
    public void LoadEditorTarget()
    {
        // Editors need the following...
        
        enemiesRootNode = new TreeViewItem { Header = "Enemies" };
        itemsRootNode = new TreeViewItem { Header = "Items" };
        fileView.Items.Add(enemiesRootNode);    
        fileView.Items.Add(itemsRootNode);

        // NPCs
        npcsRootNode = new TreeViewItem { Header = "NPCs" };
        fileView.Items.Add(npcsRootNode);
        LoadEditorNpcs();

        // Objects
        objectsRootNode = new TreeViewItem { Header = "Objects" };
        fileView.Items.Add(objectsRootNode);
        LoadEditorObjects();

        // Tiles
        tilesRootNode = new TreeViewItem { Header = "Tiles" };
        fileView.Items.Add(tilesRootNode);
        LoadEditorTiles();
    }

    /// <summary>
    /// Loads all tile prm files
    /// </summary>
    void LoadEditorTiles()
    {
        // Create a path to the tile content...
        string tilePrtsPath = Path.Combine(App.TargetPath, "DATA", "MAP", "PARTS");

        // Get all parts...
        foreach (FileInfo prtFile in (new DirectoryInfo(tilePrtsPath)).GetFiles())
        {
            TreeViewItem treeViewItem = new TreeViewItem
            {
                Header = prtFile.Name,
                Tag    = new TargetItem { type = TargetItemType.PrtMap, path = prtFile.FullName }
            };

            tilesRootNode.Items.Add(treeViewItem);
        }
    }

    /// <summary>
    /// Loads all object prf files
    /// </summary>
    void LoadEditorObjects()
    {
        string objPrfPath = Path.Combine(App.TargetPath, "DATA", "OBJ", "PROF");

        // Get all profiles
        foreach (FileInfo prfFile in (new DirectoryInfo(objPrfPath)).GetFiles())
        {
            TreeViewItem treeViewItem = new TreeViewItem
            {
                Header = prfFile.Name,
                Tag    = new TargetItem { type = TargetItemType.PrfObject, path = prfFile.FullName }
            };

            objectsRootNode.Items.Add(treeViewItem);
        }
    }

    void LoadEditorNpcs()
    {
        string npcPrfPath = Path.Combine(App.TargetPath, "DATA", "NPC", "PROF");

        // Get all profiles
        foreach (FileInfo prfFile in (new DirectoryInfo(npcPrfPath)).GetFiles())
        {
            TreeViewItem treeViewItem = new TreeViewItem
            {
                Header = prfFile.Name,
                Tag = new TargetItem { type = TargetItemType.PrfNpc, path = prfFile.FullName }
            };

            npcsRootNode.Items.Add(treeViewItem);
        }
    }

    /// <summary>
    /// Event Callback.<br/>
    /// Called when the tree view item is changed...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        // Get the selected item as a tree view item
        TreeViewItem treeItem = (e.NewValue as TreeViewItem);

        // If treeItem is null, we fuck off right now.
        if (treeItem == null)
            return;

        // Get the tag of the treeItem as a TargetItem
        TargetItem? targetItem = (treeItem.Tag as TargetItem?);

        // If targetItem is null, fuck off right now.
        if (targetItem == null)
            return;

        // TO-DO: Dirty Check...

        // Depending on the type of target item, we want a different editor...
        switch (targetItem.Value.type)
        {
            case TargetItemType.PrtMap:
                prmMapControl.LoadAsset(targetItem.Value.path); // Load Asset
                editorView.Child = prmMapControl;               // Set Control for this asset
                break;

            case TargetItemType.PrfNpc:
                prfNpcControl.LoadAsset(targetItem.Value.path);
                editorView.Child = prfNpcControl;
                break;

            case TargetItemType.PrfObject:
                prfObjectControl.LoadAsset(targetItem.Value.path);
                editorView.Child = prfObjectControl;
                break;

            default:
                return;
        }
    }

    void OnMenuClickNewNPC(object sender, RoutedEventArgs e)
    {
        prfNpcControl.NewAsset();
        editorView.Child = prfNpcControl;
    }
}