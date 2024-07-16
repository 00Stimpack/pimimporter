using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using ZFPimImporter.DataTypes;
using ZFPimImporter.Extensions;
using ZFPimImporter.Helpes;
using ZFPimImporter.IO;
using ZFPimImporter.Modal;
using ZFPimImporter.Project;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ZFPimImporter;

public partial class Tabs : Window
{
    
    public event PropertyChangedEventHandler PropertyChanged;

    
    private void Segments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        // Notify UI to refresh when the Segments collection changes
        OnPropertyChanged(nameof(Segments));
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Refresh items to apply new width calculations based on window size, if necessary
        OnPropertyChanged(nameof(Segments));
       // StackPanelSegementList.Width = e.NewSize.Width;
       // SegementList.Width = e.NewSize.Width;
       
       var newValue =Math.Max(100, e.NewSize.Width-80 ) ;
       var newValueSegment =Math.Max(1215, e.NewSize.Width/2 ) ;
       //StackPanelSegementList.Width = newValueSegment;
       /*//Console.WriteLine($"Size changed: {e.NewSize.Width} x {e.NewSize.Height} newValue:{newValue}");

       SegementList.Width = newValueSegment;*/
       //StackPanelSegementList.Width = newValue;
       //SegementList.Dispatcher.InvokeAsync(() => {  SegementList.UpdateLayout(); });
       //StackPanelSegementList.Dispatcher.InvokeAsync(() => {  StackPanelSegementList.UpdateLayout(); });

       /*SegementList.Width = e.NewSize.Width;
       StackPanelSegementList.Width = e.NewSize.Width;*/

    }
    
    public enum ScreenSaverMode : byte
    {
        Disabled,
        Menu,
        Video,
        Product
    }

    public static bool IsDirty;
    public static bool IsDirtyApplication;
    public static bool IsDirtyProject;

    public static ScreenSaverMode ScreenSaverModeDisplay = ScreenSaverMode.Menu;

    public static bool VideoScreenSaverActive = false;
    public static bool MenuScreenSaverActive = false;
    public static bool ProductScreenSaverActive = false;

    public string Description = "EN";

    private static bool OptionsLoaded = false;
    /*public static OptionImporter OptionImporter { get; set; } = new();
    public static Option Option = new();*/

    public static RoutedCommand PublishCommand = new();
    public static RoutedCommand ExitCommand = new();

    public static RoutedCommand SaveCommand = new();
    public static RoutedCommand SaveAs = new();

    public static RoutedCommand DevModus = new();

    private bool optionsLoaded = false;
    public static ObservableCollection<PimJson> Solutions = new();
    public static ObservableCollection<PimJson> Products = new();
    public static ObservableCollection<SegmentJson> Segments { get; set; } = new ObservableCollection<SegmentJson>();

    public static bool isLoaded;

    public static string TitleStart = "ZF Pim Importer - ";
    /*
     private string ChoosenSegment
    {
        get
        {
            try
            {
                var data = (SegmentJson)SegementList.SelectedItem;
                if (data != null) DataSave.CurrentProject.OptionImporter.SegmentsID.ToString() = data.path;
            }
            catch (Exception e)
            {
            }

            return DataSave.CurrentProject.OptionImporter.SegmentsID.ToString();
        }
        set { }
    }*/

    public override void BeginInit()
    {
        base.BeginInit();
    }

    public void CreateBackgroundMenu()
    {
        try
        {
            if (true||File.Exists(Path.Combine(FixedStrings.FOCUS_APPPATH, "FocusApp.exe")))
            {
                if (!Directory.Exists(FixedStrings.FOCUS_APPPATH_BACKGROUND))
                    Directory.CreateDirectory(FixedStrings.FOCUS_APPPATH_BACKGROUND);
            }
            else
            {
                
            }

        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
          
        }
        
        Backgrounds.Items.Clear();
        Backgrounds.Icon = new PackIcon { Kind =  PackIconKind.ImageAreaClose };
        var defaultBackground = CreateMenuItem("", "Default", PackIconKind.ImageFrame, SetBackground, _ => "Standard background ");
        defaultBackground.StaysOpenOnClick = true;
        defaultBackground.IsCheckable = false;
        
        

        List<FileInfo> foundFiles = new List<FileInfo>();

        if (Directory.Exists(FixedStrings.FOCUS_APPPATH_BACKGROUND))
        {
            var di = new DirectoryInfo(FixedStrings.FOCUS_APPPATH_BACKGROUND);
            foreach (var file in di.GetFiles("*"))
            {
                if (file.Extension.ToLower() == ".mp4" || file.Extension.ToLower() == ".avi" || file.Extension.ToLower() == ".mpeg" || file.Extension.ToLower() == ".png" || file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".jpeg")
                {
                    foundFiles.Add(file);
                }
            }
        }

        bool foundIcon = false;

        Backgrounds.Items.Add(defaultBackground);
        if (foundFiles.Any())
        {

            Backgrounds.Items.Add(new Separator());
            foreach (var file in foundFiles)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name).Replace("_"," ").Replace("-"," ");
                if(string.IsNullOrEmpty(name))continue;
                var icon = new PackIcon { Kind =  PackIconKind.ImageOutline };
            
                var background = CreateMenuItem(file.Name, name , PackIconKind.FileExcel, SetBackground, _ => $"{name} background ");
                background.StaysOpenOnClick = true;
                background.IsCheckable = false;
                if (file.Name == DataSave.CurrentProject.Option.BackgroundImage)
                {
                    if (file.Extension.ToLower() == ".mp4" || file.Extension.ToLower() == ".avi" || file.Extension.ToLower() == ".mpeg" )
                    {
                        icon    =  new PackIcon { Kind =  PackIconKind.Video};
                    }
                    else
                    {
                        icon   =  new PackIcon { Kind =  PackIconKind.Image};
                    }

                    foundIcon = true;

                }
                else
                {
                    if (file.Extension.ToLower() == ".mp4" || file.Extension.ToLower() == ".avi" || file.Extension.ToLower() == ".mpeg" )
                    {
                        icon   = new PackIcon { Kind = PackIconKind.VideoOutline};
                        
                    }
                    else
                    {
                        icon   =new PackIcon { Kind =  PackIconKind.ImageOutline};
                    }
                    
                }

                background.Icon = icon;
                Backgrounds.Items.Add(background);
            }
            
        }
        
        defaultBackground.Icon = foundIcon ? new PackIcon { Kind =  PackIconKind.ImageFrame} : new PackIcon { Kind =  PackIconKind.ImageFilterFrames};
        
    }

    public void CreateExcelMenu()
    {
        try
        {
            if (!Directory.Exists(FixedStrings.EXCELSTORAGE))
                Directory.CreateDirectory(FixedStrings.EXCELSTORAGE);
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }

        Excels.Items.Clear();

        var importExcelButton = CreateMenuItem(ExcelImportType.Normal, "Import Excel", PackIconKind.FileExcel, ImportExcel, _ => "Import a standard Excel file");
        //var importExcelAsProjectButton = CreateMenuItem(ExcelImportType.ExcelToProject, "Import Excel As Project", PackIconKind.FileExcelOutline, ImportExcel, _ => "Import Excel as a new project");
        var importExcelConfidentialButton = CreateMenuItem(ExcelImportType.ExcelToConfidential, "Import Excel Confidential", PackIconKind.FileExcelBox, ImportExcelConfidential, _ => "Import confidential Excel");
        //var importExcelCustomButton = CreateMenuItem(ExcelImportType.ExcelToCustom, "Import Excel Custom Table", PackIconKind.FileExcelBoxOutline, ImportExcel, _ => "Import custom Excel table\nPath: Not applicable");

        Excels.Items.Add(importExcelButton);
        //Excels.Items.Add(importExcelAsProjectButton);
        Excels.Items.Add(importExcelConfidentialButton);
        //Excels.Items.Add(importExcelCustomButton);

        Excels.Items.Add(new Separator());

        var di = new DirectoryInfo(FixedStrings.EXCELSTORAGE);
        int index = 0;

        var excelList = CreateMenuItem(null, "Excel Storage", PackIconKind.FileExcelBoxOutline, null, _ => $"Stored Excel files\nPath: {FixedStrings.EXCELSTORAGE}");

        var deleteAllExcels = CreateMenuItem(ExcelImportType.ExcelToCustom, "Delete All Excels", PackIconKind.Delete, DeleteAllExcelsOnClick, _ => $"Delete all stored Excel files\nPath: {FixedStrings.EXCELSTORAGE}");
        excelList.Items.Add(deleteAllExcels);
        excelList.Items.Add(new Separator());

        foreach (var file in di.GetFiles("*.xlsx"))
        {
            string filePath = file.FullName;
            string fileName = Path.GetFileNameWithoutExtension(file.Name);
            var dynamicMenuItem = CreateMenuItem(file.Name, fileName, PackIconKind.ContentSaveAllOutline, null, context => $"Excel file options for {context}\nLocated at: {filePath}");
            var singleOpenExcelButton = CreateMenuItem(filePath, "Open Excel", PackIconKind.FileExcelOutline, SingleOpenExcelButtonOnClick, context => $"Open file\nFile: {context}\nPath: {filePath}");

            var singleimportExcelButton = CreateMenuItem(filePath, "Import Excel", PackIconKind.FileExcel, SingleImportExcelButtonOnClick, context => $"Import standard Excel file\nFile: {context}\nPath: {filePath}");
            //var singleimportExcelAsProjectButton = CreateMenuItem(filePath, "Import Excel As Project", PackIconKind.FileExcelOutline, SingleImportExcelAsProjectButtonOnClick, context => $"Import Excel as new project\nFile: {context}\nPath: {filePath}");
            var singleimportExcelConfidentialButton = CreateMenuItem(filePath, "Import Excel Confidential", PackIconKind.FileExcelBox, SingleImportConfidentialExcelButtonOnClick, context => $"Import confidential Excel file\nFile: {context}\nPath: {filePath}");
            //var singleimportExcelCustomButton = CreateMenuItem(filePath, "Import Excel Custom Table", PackIconKind.FileExcelBoxOutline, SingleImportExcelCustomButtonOnClick, context => $"Import custom Excel table\nFile: {context}\nPath: {filePath}");
            dynamicMenuItem.Items.Add(singleOpenExcelButton);

            dynamicMenuItem.Items.Add(singleimportExcelButton);
            //dynamicMenuItem.Items.Add(singleimportExcelAsProjectButton);
            dynamicMenuItem.Items.Add(singleimportExcelConfidentialButton);
            //  dynamicMenuItem.Items.Add(singleimportExcelCustomButton);

            index++;
            excelList.Items.Add(dynamicMenuItem);
        }

        Excels.Items.Add(excelList);
    }


    private void SingleOpenExcelButtonOnClick(object sender, RoutedEventArgs e)
    {
        string excelPath = GetPathDataContextMenuItem(sender);
        if (File.Exists(excelPath))
        {
            try
            {
                Process.Start(new ProcessStartInfo(excelPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open the Excel file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("The Excel file does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void SingleImportConfidentialExcelButtonOnClick(object sender, RoutedEventArgs e)
    {
        
        string path = GetPathDataContextMenuItem(sender);
        
        if (File.Exists(path))
        {

            ImportExcel(path,useConfidentialParser:true);
        }
    }
    private void SingleImportExcelButtonOnClick(object sender, RoutedEventArgs e)
    {
        
        string path = GetPathDataContextMenuItem(sender);
        
        if (File.Exists(path))
        {
            //Console.WriteLine($"ASASD\n \nPATH:{path} \n \n");

            ImportExcel(path);
        }
    }



    public void  ToggleConfidential(bool confidential, int id)
    {
        
    }
    

    private void DeleteAllExcelsOnClick(object sender, RoutedEventArgs e)
    {
        //Console.WriteLine("need to delete all Excels");
        DeleteAllExcels();
    }

    private void ImportExcel(string pathToProjectFile = "", Action OnSuccess = null,bool useConfidentialParser = false)
    {
        try
        {
            
            bool standardImport = string.IsNullOrEmpty(pathToProjectFile) || !File.Exists(pathToProjectFile);
            if (standardImport)
            {
                GetExcelPath(out pathToProjectFile);
            }

            if (!File.Exists(pathToProjectFile))
            {
                MessageBox.Show($"The Excel file does not exist:\n{pathToProjectFile}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var needRefresh = new List<PimJson>();
            var tempList = ExcelReader.LoadPimDataFromExcel(pathToProjectFile);
            Dictionary<string, List<PimRawData>> rawDataDict = new Dictionary<string,  List<PimRawData>>();
            foreach (var raw in tempList)
            {
                if (rawDataDict.TryGetValue(raw.ID, out var found))
                {
                    rawDataDict[raw.ID].Add(raw);
                }
                else
                {
                    rawDataDict.Add(raw.ID,new List<PimRawData>(){raw});
                }
            }

            var rawList = new List<PimRawData>();
            foreach (var raw in rawDataDict)
            {
                bool isConf = false;
                foreach (var valueFound in raw.Value)
                {
                    if (valueFound.IsConfidential)
                    {
                        isConf = true;
                        break;
                    }
                }
                if (isConf)
                {
                    for (int i = 0; i < rawDataDict[raw.Key].Count; i++)
                    {
                        rawDataDict[raw.Key][i].IsConfidential = true;
                    }
                }
                for (int i = 0; i < rawDataDict[raw.Key].Count; i++)
                {
                    rawList.Add(rawDataDict[raw.Key][i]);
                }
            }
            
            var pims = ExcelReader.ConvertPimRaw(rawList,useConfidentialParser);
            //PathGenerator.OverriderLangJson(DataSave.CurrentProject.LanguageData);
            for (var i = 0; i < pims.Count; i++)
            {
                if (!DataSave.CurrentProject.Products.Exists(x => x.id == pims[i].id))
                {
                    DataSave.CurrentProject.Products.Add(pims[i]);
                }
                else
                {
                    var productIndex = DataSave.CurrentProject.Products.FindIndex(x => x.id == pims[i].id);
                    DataSave.CurrentProject.Products[productIndex] = pims[i];
                    
                    needRefresh.Add(pims[i]);
                }

                foreach (var keys in DataSave.CurrentProject.AllSolutions.Keys)
                {
                    var productsKeys = DataSave.CurrentProject.AllSolutions[keys].Keys;
                    foreach (var productkey in productsKeys)
                    {
                        for (int j = 0; j < DataSave.CurrentProject.AllSolutions[keys][productkey].Count; j++)
                        {
                            

                            if (DataSave.CurrentProject.AllSolutions[keys][productkey][j].id == pims[i].id)
                            {
                                DataSave.CurrentProject.AllSolutions[keys][productkey][j] = pims[i];
                            }
                        }
                    }
        
                }
            }

            for (var i = 0; i < DataSave.CurrentProject.Products.Count; i++)
            {
                DataSave.CurrentProject.Products[i].path = DataSave.CurrentProject.Products[i].id.ToString();
            }

            Products = new ObservableCollection<PimJson>(DataSave.CurrentProject.Products.OrderBy(x => x.id));
            PimDataList.ItemsSource = Products;
            Solutions = new ObservableCollection<PimJson>(
                DataSave.CurrentProject.AllSolutions[DataSave.CurrentProject.Option.StartScreenMode][
                    DataSave.CurrentProject.OptionImporter.SegmentIndex]);
            SolutionsList.ItemsSource = Solutions;
            //JsonHandler.WriteJson(PathGenerator.GetProductJsonPath(), pims);
            LanguageDataTools.LanguageMenuSwitch(MenuLanguageButton.Header.ToString(), false);

            IsDirtyProject = true;
            IsDirty = true;
         
            OnSuccess?.Invoke();
 
            CreateMenus();
    
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Failed to import the Excel file: \n{pathToProjectFile} \n \nplease close it if its still open", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public enum ExcelImportType
    {
        Normal,
        ExcelToProject,
        ExcelToConfidential,
        ExcelToCustom,
    }
    private void ImportExcelAsProjectButtonOnClick(object sender, RoutedEventArgs e)
    {
        
        
    }

    private void ImportExcelConfidentialButtonOnClick(object sender, RoutedEventArgs e)
    {
        
    }

    private void ImportExcelCustomButtonOnClick(object sender, RoutedEventArgs e)
    {
        
    }

    private MenuItem CreateMenuItem(object dataContext, string header, PackIconKind iconKind, RoutedEventHandler clickEvent, Func<object, string> toolTipFunc)
    {
        var menuItem = new MenuItem
        {
            DataContext = dataContext,
            Header = header,
            Icon = new PackIcon { Kind = iconKind },
            ToolTip = toolTipFunc(dataContext)
        };
        if (clickEvent != null)
            menuItem.Click += clickEvent;
        return menuItem;
    }

    public void CreateProjectMenus()
    {
        try
        {
            if (!Directory.Exists(FixedStrings.SAVEDPROJECTS))
                Directory.CreateDirectory(FixedStrings.SAVEDPROJECTS);
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }

        Projects.Items.Clear();

        var newProjectMenuItem = CreateMenuItem(0, "New Project", PackIconKind.ContentSaveAllOutline, NewProject, _ => "Create a new project");
        var openProjectMenuItem = CreateMenuItem(1, "Open Project", PackIconKind.ContentSaveAllOutline, OpenProject, _ => "Open an existing project");
        var saveProjectMenuItem = CreateMenuItem(2, "Save Project", PackIconKind.ContentSaveAll, MenuSave, _ => $"Save the current project: {DataSave.ProjectName}");
        var saveProjectAsMenuItem = CreateMenuItem(3, "Save Project As", PackIconKind.ContentSaveAll, MenuSaveAs, _ => $"Save the current project ({DataSave.ProjectName}) with a new name");

        var projectName = new MenuItem
        {
            Margin = new Thickness(0, 0, 0, 0),
            FontWeight = FontWeight.FromOpenTypeWeight(700),
            Name = "ProjectTitle",
            Header = DataSave.ProjectName,
            ToolTip = $"Currently opened project: {DataSave.ProjectName}"
        };
        projectName.Items.Add(saveProjectMenuItem);
        projectName.Items.Add(saveProjectAsMenuItem);

        var projectListName = CreateTextBlock("Projects under saved", -55, 700, "List of saved projects");

        Projects.Items.Add(newProjectMenuItem);
        Projects.Items.Add(openProjectMenuItem);
        Projects.Items.Add(new Separator());
        Projects.Items.Add(projectName);
        Projects.Items.Add(new Separator());
        Projects.Items.Add(projectListName);

        PopulateSavedProjects();
    }

    private void PopulateSavedProjects()
    {
        var di = new DirectoryInfo(FixedStrings.SAVEDPROJECTS);
        int index = 0;
        foreach (var file in di.GetFiles("*.zfpimi"))
        {
            string projectname = Path.GetFileNameWithoutExtension(file.Name);
            var dynamicMenuItem = CreateMenuItem(index, projectname, PackIconKind.ContentSaveAllOutline, null, _ => $"Project options for {projectname}");
            if (projectname != DataSave.ProjectName)
            {
                dynamicMenuItem.Items.Add(CreateMenuItem(file.FullName, "Open", PackIconKind.ContentSaveAllOutline, OpenSingleProject, context => $"Open project: {context}"));
                dynamicMenuItem.Items.Add(CreateMenuItem(file.FullName, "Open Publish", PackIconKind.ContentSaveAllOutline, OpenPublishProject, context => $"Open published version of project: {context}"));
                //  dynamicMenuItem.Items.Add(CreateMenuItem(file.FullName, "Overwrite", PackIconKind.ContentCopy, OverwriteprojectMenuItemFileOnClick, context => $"Overwrite project: {context}"));
                dynamicMenuItem.Items.Add(CreateMenuItem(file.FullName, "Delete", PackIconKind.Remove, DeleteProject, context => $"Delete project: {context}"));
            }
            Projects.Items.Add(dynamicMenuItem);
            index++;
        }
    }

    private TextBlock CreateTextBlock(string text, int marginLeft, int fontWeight, string toolTip)
    {
        return new TextBlock
        {
            Margin = new Thickness(marginLeft, 0, 0, 0),
            FontWeight = FontWeight.FromOpenTypeWeight(fontWeight),
            Text = text,
            ToolTip = toolTip
        };
    }

    public void CreateMenus()
    {
        CreateProjectMenus();
        CreateExcelMenu();
        CreateBackgroundMenu();

    }

    private void OverwriteprojectMenuItemFileOnClick(object sender, RoutedEventArgs e)
    {
        //Console.WriteLine("OverwriteprojectMenuItemFileOnClick:"+sender.ToString());
    }

    public void CheckForCommands()
    {
        return;
        switch (ProjectPage.Command)
        {
            case CommandLineHelper.Commands.CheckData:
                DataSave.CurrentProjectToJson();

                break;
            case CommandLineHelper.Commands.StartApplication:
                DataSave.CurrentProjectToJson();
                break;
            case CommandLineHelper.Commands.UpdateData:
                DataSave.CurrentProjectToJson();
                break;
            case CommandLineHelper.Commands.UpdateAndStart:
                break;
        }

        if (CommandLineHelper.ProjectLoaded)
            DataSave.CurrentProjectToJson();
    }
    public void ReloadSegments()
    {
        var segments = new List<SegmentJson>();
        var pims = new List<PimJson>();
        segments = Starter.CheckSegments(DataSave.CurrentProject.Option.StartScreenMode);
        try
        {
            pims = DataSave.CurrentProject.Products;
            if (pims == null || pims == default)
            {
                pims = new List<PimJson>();
                JsonHandler.WriteJson(PathGenerator.GetProductJsonPath(), pims);
            }
        }
        catch (Exception e)
        {
            pims = new List<PimJson>();
        }

        Segments = new ObservableCollection<SegmentJson>(segments);
        SegementList.ItemsSource = Segments;

        Products = pims != null ? new ObservableCollection<PimJson>(pims) : new ObservableCollection<PimJson>();
        PimDataList.ItemsSource = Products;

        Solutions = new ObservableCollection<PimJson>();
        SolutionsList.ItemsSource = Solutions;

        RefreshSegmentSelection();
        CreateMenus();
    }

   
  
    

    private void RefreshSegmentSelection()
    {
        
        var index = 0;
        try
        {
            
            SegementList.SelectedIndex = DataSave.CurrentProject.OptionImporter.SegmentsID;
            txtFilePath.Content = "select RefreshTopic";
            if (!string.IsNullOrEmpty(DataSave.CurrentProject.OptionImporter.RefreshPimsFull[DataSave.CurrentProject.Option.StartScreenMode][DataSave.CurrentProject.OptionImporter.SegmentsID]))
                txtFilePath.Content = DataSave.CurrentProject.OptionImporter.RefreshPimsFull[DataSave.CurrentProject.Option.StartScreenMode][DataSave.CurrentProject.OptionImporter.SegmentsID];
        }
        catch (Exception e)
        {
            //Console.WriteLine("RefreshSegmentSelection:"+e);
        }
    }
   
    public Tabs()
    {
        isLoaded = false;
        InitializeComponent();
        PathGenerator.CreateStandardPath();
        DataSave.RefreshProjectPath();

        StarterTab();
        
   
        
        Loaded += (s, e) => {
            scrollViewer = FindVisualChild<ScrollViewer>(SolutionsList);
        };

    }

   
    public static void GogoTabs()
    {
    }

    public void StarterTab(Action onComplete = null)
    {

        PathGenerator.CreateStandardPath();
        DataSave.RefreshProjectPath();
        DataSave.CheckLastProject();
        
        //DataSave.RefreshTitle();
        
        this.Title = "ZF Pim Importer - "+ DataSave.ProjectName;
        
        /*List<PimJson>
        Solutions = new ObservableCollection<PimJson>();
        SolutionsList.ItemsSource = Solutions;*/
        
        
        Starter.CheckApplications();
        var fromFile = false;
        CheckForCommands();
        //DataSave.CurrentProject.Option = Starter.CheckOptions();
        //DataSave.CurrentProject.OptionImporter = Starter.CheckImporterOptions();

        var segments = new List<SegmentJson>();
        var pims = new List<PimJson>();

     

        segments = Starter.CheckSegments(DataSave.CurrentProject.Option.StartScreenMode);
        try
        {
            Segments = new ObservableCollection<SegmentJson>(segments);
            SegementList.ItemsSource = Segments;
        }
        catch (Exception e)
        {
            //Console.WriteLine("SEGMENTS FAILED:"+e);
        }
        SwitchStartScreenMenu(DataSave.CurrentProject.Option.StartScreenMode, () =>
        {
              
            LanguageDataTools.LanguageDataInit();

        
        
        
        
        
            /*SwitchOptionMenu(0, DataSave.CurrentProject.Option.AllMoreActive);
            SwitchOptionMenu(1, DataSave.CurrentProject.Option.AllDetailsActive);
            SwitchOptionMenu(2, DataSave.Option.AutoPublish);
            SwitchOptionMenu(3, DataSave.Option.DarkMode);
            SwitchOptionMenu(5, DataSave.CurrentProject.Option.UseConverter);*/

            setVideoScreenSaver_btn();


            option0.IsChecked = DataSave.CurrentProject.Option.AllDetailsActive;
            option1.IsChecked = DataSave.CurrentProject.Option.AllMoreActive;
#if DEVELOPER
#endif
            option2.IsChecked = DataSave.Option.AutoPublish;
            option3.IsChecked = DataSave.Option.DarkMode;
            option8.IsChecked = DataSave.CurrentProject.Option.AllSpecsActive;
            option9.IsChecked = DataSave.CurrentProject.Option.AllImagesActive;
            option10.IsChecked = DataSave.CurrentProject.Option.AllVideosActive;
            option11.IsChecked = DataSave.CurrentProject.Option.AllPresentationsActive;

            option6.IsChecked = DataSave.CurrentProject.Option.UseConverter;

            ScreenSaverCountdown.Text = DataSave.CurrentProject.Option.RefreshTimer.ToString();
            RadioButton1.IsChecked = false;
            RadioButton2.IsChecked = false;
            RadioButton3.IsChecked = false;

            switch (DataSave.CurrentProject.Option.ScreenSaverMode)
            {
                case ScreenSaverMode.Disabled:
                    break;
                case ScreenSaverMode.Menu:
                    RadioButton1.IsChecked = true;
                    break;
                case ScreenSaverMode.Video:
                    RadioButton2.IsChecked = true;
                    break;

                case ScreenSaverMode.Product:
                    RadioButton3.IsChecked = true;
                    break;
            }

        

            ToogleTheme(DataSave.Option.DarkMode);

            deleteAllSelectedBtn.Content = "< Remove";
            addAllSelectedBtn.Content = "Add >";

            int minWidth = 128;
            MenuFileHeader.Icon = new PackIcon { Kind = PackIconKind.Application, RenderTransform = new TranslateTransform(22, -1) };
            //MenuFileHeader.Icon = new PackIcon { Kind = PackIconKind.Application  };
            MenuFileHeader.Width = minWidth;
            
            MenuEditHeader.Icon = new PackIcon { Kind = PackIconKind.Edit, RenderTransform = new TranslateTransform(22, -1)  };
            MenuEditHeader.Width = minWidth;

            MenuItemStartscreen.Icon = new PackIcon { Kind = PackIconKind.Monitor , RenderTransform = new TranslateTransform(22, -1)  };
            MenuItemStartscreen.Width = minWidth;

            MenuOptionsHeader.Icon = new PackIcon { Kind = PackIconKind.Cog , RenderTransform = new TranslateTransform(22, -1)  };
            MenuOptionsHeader.Width = minWidth;

            Projects.Icon = new PackIcon { Kind = PackIconKind.ContentSaveMove , RenderTransform = new TranslateTransform(22, -1)  };
            Projects.Width = minWidth;

            Excels.Icon = new PackIcon { Kind = PackIconKind.MicrosoftExcel  , RenderTransform = new TranslateTransform(22, -1) };
            Excels.Width = minWidth;

            MenuOptionsHeader.Icon = new PackIcon { Kind = PackIconKind.Cog , RenderTransform = new TranslateTransform(22, -1) };
            MenuOptionsHeader.Width = minWidth;

            MenuLanguageButton.Icon = new PackIcon { Kind = PackIconKind.WebBox, RenderTransform = new TranslateTransform(22, -1)  };
            MenuLanguageButton.Width = minWidth;

            MenuHelpHeader.Icon = new PackIcon { Kind = PackIconKind.WebBox, RenderTransform = new TranslateTransform(22, -1) };
            MenuHelpHeader.Width = minWidth;

            
            MenuFileHeader.Padding = new Thickness(-22, 0, 0, 0);
            MenuEditHeader.Padding = new Thickness(-22, 0, 0, 0);
            MenuItemStartscreen.Padding = new Thickness(-22, 0, 0, 0);
            MenuOptionsHeader.Padding = new Thickness(-22, 0, 0, 0);
            Projects.Padding = new Thickness(-22, 0, 0, 0);
            Excels.Padding = new Thickness(-22, 0, 0, 0);
            MenuOptionsHeader.Padding = new Thickness(-22, 0, 0, 0);
            MenuLanguageButton.Padding = new Thickness(-22, 0, 0, 0);
            MenuHelpHeader.Padding = new Thickness(-22, 0, 0, 0);
            
            /*
            MenuFileHeader.Icon = new PackIcon { Kind = PackIconKind.Application, RenderTransform = new TranslateTransform(22, -1) };
            MenuEditHeader.Icon = new PackIcon { Kind = PackIconKind.Edit, RenderTransform = new TranslateTransform(22, -1) };
            MenuItemStartscreen.Icon = new PackIcon { Kind = PackIconKind.Monitor, RenderTransform = new TranslateTransform(22, -1) };
            MenuOptionsHeader.Icon = new PackIcon { Kind = PackIconKind.Cog, RenderTransform = new TranslateTransform(22, -1) };
            Projects.Icon = new PackIcon { Kind = PackIconKind.ContentSaveMove, RenderTransform = new TranslateTransform(22, -1) };
            Excels.Icon = new PackIcon { Kind = PackIconKind.MicrosoftExcel, RenderTransform = new TranslateTransform(22, -1) };
            MenuOptionsHeader.Icon = new PackIcon { Kind = PackIconKind.Cog, RenderTransform = new TranslateTransform(22, -1) };
            MenuLanguageButton.Icon = new PackIcon { Kind = PackIconKind.WebBox, RenderTransform = new TranslateTransform(22, -1) };
            MenuHelpHeader.Icon = new PackIcon { Kind = PackIconKind.WebBox, RenderTransform = new TranslateTransform(22, -1) };
            */
            
   
            /*
            MenuFileHeader.Icon = new PackIcon
                { Kind = PackIconKind.Application, RenderTransform = new TranslateTransform(22, -1) };
            MenuFileHeader.Padding = new Thickness(-22, 0, 0, 0);
            MenuFileHeader.Margin = new Thickness(0, 0, 4, 0);

            MenuEditHeader.Icon = new PackIcon
                { Kind = PackIconKind.Edit, RenderTransform = new TranslateTransform(22, -1) };
            MenuEditHeader.Padding = new Thickness(-22, 0, 0, 0);
            MenuEditHeader.Margin = new Thickness(0, 0, 4, 0);

            MenuItemStartscreen.Icon =    new PackIcon { Kind = PackIconKind.Monitor, RenderTransform = new TranslateTransform(22, -1) };
            MenuItemStartscreen.Padding = new Thickness(-22, 0, 0, 0);
            MenuItemStartscreen.Margin =   new Thickness(250, 0, 4, 0);

            MenuOptionsHeader.Icon = new PackIcon
                { Kind = PackIconKind.Cog, RenderTransform = new TranslateTransform(22, -1) };
            MenuOptionsHeader.Padding = new Thickness(-22, 0, 0, 0);
            MenuOptionsHeader.Margin = new Thickness(0, 0, 4, 0);

        
            Projects.Icon = new PackIcon
                { Kind = PackIconKind.ContentSaveMove, RenderTransform = new TranslateTransform(22, -1) };
            Projects.Padding = new Thickness(-22, 0, 0, 0);
            Projects.Margin = new Thickness(0, 0, 4, 0);
        
        
            Excels.Icon = new PackIcon
                { Kind = PackIconKind.MicrosoftExcel, RenderTransform = new TranslateTransform(22, -1) };
            Excels.Padding = new Thickness(-22, 0, 0, 0);
            Excels.Margin = new Thickness(0, 0, 4, 0);
        
        
        
            MenuOptionsHeader.Icon = new PackIcon
                { Kind = PackIconKind.Cog, RenderTransform = new TranslateTransform(22, -1) };
            MenuOptionsHeader.Padding = new Thickness(-22, 0, 0, 0);
            MenuOptionsHeader.Margin = new Thickness(0, 0, -80, 0);
        
        
        
            MenuLanguageButton.Icon = new PackIcon
                { Kind = PackIconKind.WebBox, RenderTransform = new TranslateTransform(22, -1) };
            MenuLanguageButton.Padding = new Thickness(-22, 0, 0, 0);
            MenuLanguageButton.Margin = new Thickness(440, 0, 4, 0);

            MenuHelpHeader.Icon = new PackIcon
                { Kind = PackIconKind.WebBox, RenderTransform = new TranslateTransform(22, -1) };
            MenuHelpHeader.Padding = new Thickness(-22, 0, 0, 0);
            MenuHelpHeader.Margin = new Thickness(0, 0, 4, 0);*/

            SkipMenu.Icon = DataSave.CurrentProject.Option.SkipStartScreen
                ? new PackIcon { Kind = PackIconKind.PageNext }
                : new PackIcon { Kind = PackIconKind.PageNextOutline };



            if (string.IsNullOrEmpty(DataSave.CurrentProject.Option.SecondLanguage))
                MenuLanguageButton.Header = " EN ";
            else
                MenuLanguageButton.Header = " " + DataSave.CurrentProject.Option.SecondLanguage + " ";

            MenuLanguageButton.Items.Clear();
            for (var i = 0; i < DataSave.CurrentProject.LanguageData.Count; i++)
            {
                var menuItem = new MenuItem();
                menuItem.Header =
                    $"{DataSave.CurrentProject.LanguageData[i].Abbreviation} - {DataSave.CurrentProject.LanguageData[i].Name} ";
                menuItem.Click += MenuItem_LanguageClick;
                MenuLanguageButton.Items.Add(menuItem);
            }
            IsDirty = false;

            RegisterEvents();
        
            isLoaded = true;
            RefreshSegmentSelection();
            CreateMenus();
            ToggleConfidential(DataSave.CurrentProject.Option.AllConfidentialActive);
            onComplete?.Invoke();

        });


      
        
    }

    private void RegisterEvents()
    {

        if (isLoaded) return;
        PublishCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
        ExitCommand.InputGestures.Add(new KeyGesture(Key.Q, ModifierKeys.Control));
        PimDataList.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(pimDataList_OnColumnClick));

        PublishCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
        ExitCommand.InputGestures.Add(new KeyGesture(Key.Q, ModifierKeys.Control));

        
        Window window = this;
        window.Closing += (sender, args) =>
        {
            if (SaveReminderProject())
            {
                SaveReminder(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString(),lastSegmentIndex);
            }
            //SaveReminder(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString());
            //SaveReminderProject();
            //SaveReminderApplication();
        };
    }

    private void MenuItem_LanguageClick(object sender, RoutedEventArgs e)
    {
        var item = (MenuItem)sender;
        var langsplit = item.Header.ToString().Split('-');
        var lang = langsplit[0].Replace(" ", "");
        MenuLanguageButton.Header = " " + lang + " ";
        LanguageDataTools.LanguageMenuSwitch(lang);
    }

    protected override void OnContextMenuOpening(ContextMenuEventArgs e)
    {
        base.OnContextMenuOpening(e);
    }

    private void solutionList_OnColumnClick(object sender, RoutedEventArgs routedEventArgs)
    {
        MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        ;
        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

        try
        {
            var header = (GridViewColumnHeader)routedEventArgs.OriginalSource;
            if (header == null) return;
            ChangeOrder(ref Solutions, SolutionsList, header);
        }
        catch (Exception e)
        {
        }
    }

    private void pimDataList_OnColumnClick(object sender, RoutedEventArgs routedEventArgs)
    {
        try
        {
            var header = (GridViewColumnHeader)routedEventArgs.OriginalSource;
            if (header == null) return;
            ChangeOrder(ref Products, PimDataList, header);
        }
        catch (Exception e)
        {
        }
    }

    private void ChangeOrder(ref ObservableCollection<PimJson> collection, ListView view, GridViewColumnHeader header)
    {
        if (header.DataContext == null)
            header.DataContext = false;
        switch (header.Column.Header)
        {
            case "ID":
                if ((bool)header.DataContext)
                    collection = new ObservableCollection<PimJson>(collection.OrderBy(x => x.id));
                else
                    collection = new ObservableCollection<PimJson>(collection.OrderByDescending(x => x.id));
                break;
            case "ProductName":
            case "Solutions":
                if ((bool)header.DataContext)
                    collection = new ObservableCollection<PimJson>(collection.OrderBy(x => x.en.ProductName));
                else
                    collection = new ObservableCollection<PimJson>(collection.OrderByDescending(x => x.en.ProductName));
                break;
        }

        header.DataContext = !(bool)header.DataContext;
        view.ItemsSource = collection;
    }

    private bool GetExcelPath(out string excelPath)
    {
        OpenFileDialog openfile = new OpenFileDialog();

        try
        {
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "Excel Files (.xlsx)|*.xlsx";

            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
      
                excelPath =  openfile.FileName;
                string destinationPath = openfile.FileName;
                if (DataSave.Option.UseExcelStorage)
                {
                    destinationPath = Path.Combine(FixedStrings.EXCELSTORAGE, Path.GetFileName(openfile.FileName));

                    FileInfo info = new FileInfo(openfile.FileName);

                    bool needCopy = false;
                    if (File.Exists(destinationPath))
                    {
                        string sourceHash = ComputeFileHash(openfile.FileName);
                        string destinationHash = ComputeFileHash(destinationPath);

                        if (sourceHash != destinationHash)
                        {
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(openfile.FileName);
                            string extension = Path.GetExtension(openfile.FileName);
                            int counter = 1;
                            string newDestinationPath = destinationPath;

                            while (File.Exists(newDestinationPath))
                            {
                                newDestinationPath = Path.Combine(FixedStrings.EXCELSTORAGE, $"{fileNameWithoutExtension}_{counter}{extension}");
                                counter++;
                            }
                            destinationPath = newDestinationPath;
                            needCopy = true;
                        }
                    }
                    else
                    {
                        needCopy = true;
                    }

                    if (needCopy)
                    {
                        info.CopyTo(destinationPath, true);
                    }


                    CreateExcelMenu();
                }

                excelPath = destinationPath;
                return true;
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine("EXEPT GetExcelPath:" + ex);
            //excelPath = null;
            excelPath =  openfile.FileName;

            return false;
        }
        excelPath =  openfile.FileName;

        return false;
    }
    private string ComputeFileHash(string filePath)
    {
        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var algorithm = new SHA256Managed())
            {
                byte[] hashBytes = algorithm.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }

    public void OpenConfidentialEditInstance(object sender, RoutedEventArgs routedEventArgs)
    {
        OpenConfidentialEdit();
    }

    public static void OpenConfidentialEdit()
    {
        ModalWindow.Text = FixedStrings.EditConfidentialMenuInfo;
        ModalWindow.inputActive = true;
        var modalWindow = new ModalWindow();
        modalWindow.Title = "Import Excel";
        modalWindow.ShowDialog();
        if (ModalWindow.success)
        {
   
            ModalWindow.inputActive = false;

        }
        else
        {
            ModalWindow.inputActive = false;

        }
        
    }

    private void ImportExcel( ExcelImportType excelType , Action OnSuccess = null,bool useConfidentialParser = false)
    {
        
        //Console.WriteLine("GexcelType:"+excelType.ToString());

        switch (excelType)
        {
            case ExcelImportType.Normal:
                ImportExcel(OnSuccess:OnSuccess,useConfidentialParser:useConfidentialParser);

                break;
            case ExcelImportType.ExcelToProject:
            case ExcelImportType.ExcelToConfidential:
                string execelPath = "";
                if (GetExcelPath(out execelPath))
                {
                    
                    ImportExcel(execelPath, OnSuccess,useConfidentialParser:useConfidentialParser);
                }
                break;
            
            /*case ExcelImportType.ExcelToConfidential:
                ModalWindow.inputActive = true;
                if (GetExcelPath(out ModalWindow.DataPath))
                {
                    ModalWindow.Text = "Enter the Tablename which should be used for the ordering";
                    var modalWindow = new ModalWindow();
                    modalWindow.Title = "Import Excel";
                    modalWindow.ShowDialog();
                    if (ModalWindow.success)
                    {

                        ModalWindow.inputActive = false;
                        //Console.WriteLine("COUNTER:" + ModalWindow.PimJson.Count);
                        ImportExcel(ModalWindow.DataPath, ModalWindow.TextInput, OnSuccess);
                        //SingleImportExcelCustomButtonOnClick(ModalWindow.DataPath);
                    }
                    else
                    {
                        ModalWindow.inputActive = false;
                        //Console.WriteLine("UNSECUSSFULL COUNTER:" + ModalWindow.PimJson.Count);

                    }

                }


                break;*/
            default:
                ImportExcel(OnSuccess:OnSuccess,useConfidentialParser:useConfidentialParser);
                break;
            
        }
   
    }

    private void SetBackground(object sender, RoutedEventArgs e)
    {
        var menuItem = (MenuItem)sender;
        string backgroundName = (string)menuItem.DataContext ?? "";
        if (DataSave.CurrentProject.Option.BackgroundImage != backgroundName)
        {
            //Console.WriteLine($"set Background:{backgroundName}");
            DataSave.CurrentProject.Option.BackgroundImage = backgroundName;
            PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
        }
        CreateBackgroundMenu();
    }

    private void ImportExcelConfidential(object sender, RoutedEventArgs e)
    {
        var menuItem = (MenuItem)sender;
        ExcelImportType excelType = (ExcelImportType)menuItem.DataContext;
        ImportExcel(excelType, null, true);

    }
    private void ImportExcel(object sender, RoutedEventArgs e)
    {
        var menuItem = (MenuItem)sender;
        ExcelImportType excelType = (ExcelImportType)menuItem.DataContext;
        ImportExcel(excelType);

    }

    private void ClearSolutionData(string segmentpath)
    {
        var jsons = new List<PimJson>();
        Solutions = new ObservableCollection<PimJson>();
        SolutionsList.ItemsSource = Solutions;
        PathGenerator.OverriderSegmentJson(segmentpath, jsons);
    }

    private void ClearSolutionData(object sender, RoutedEventArgs e)
    {
        try
        {
            var segment = (SegmentJson)SegementList.SelectedItem;
            ModalHelper.StartModal($"Do you really want clear all Products in {segment.name}", FixedStrings.ClearSolutionDataModalTitle, () =>
            {
                ClearSolutionData(segment.path);
                IsDirty = false;
                IsDirtyProject = false;
                SaveSolutionData(true);
                Products.Refresh();
                Solutions.Refresh();
                PimDataList.Items.Refresh();


            }, () => { }, FixedStrings.ClearSolutionDataModalOkayText,FixedStrings.ClearSolutionDataModalCancelText);
        }
        catch (Exception exception)
        {
        }
    }

    private void PublishSolutionData(string json,int currentID = -2, int currentScreenIndex = -2)
    {
        var data = Solutions.ToList();
        //if (currentScreenIndex == 0) currentScreenIndex = DataSave.CurrentProject.Option.StartScreenMode;
        if (!string.IsNullOrEmpty(json))
        {
            int index = currentScreenIndex;
            int ids = currentID;

            JsonHandler.WriteJson(PathGenerator.GetSegmentJsonPath(json), data);
            if (currentScreenIndex < 0)
                index =   DataSave.CurrentProject.OptionImporter.SegmentIndex;
            if (currentScreenIndex < 0)
                ids =   DataSave.CurrentProject.OptionImporter.SegmentsID;
            
            if (!DataSave.CurrentProject.AllSolutions[DataSave.CurrentProject.Option.StartScreenMode].TryGetValue(ids, out var pimjsons))
            {
                DataSave.CurrentProject.AllSolutions[DataSave.CurrentProject.Option.StartScreenMode][ids] = new List<PimJson>();
            }
            
            DataSave.CurrentProject.AllSolutions[DataSave.CurrentProject.Option.StartScreenMode][ids].Clear();
            DataSave.CurrentProject.AllSolutions[DataSave.CurrentProject.Option.StartScreenMode][ids] =   new List<PimJson>(data);
            foreach (var startscreen in DataSave.CurrentProject.AllSolutions[DataSave.CurrentProject.Option.StartScreenMode])
            {
                if (startscreen.Key == ids) continue;
                if (DataSave.CurrentProject.AllSolutions[DataSave.CurrentProject.Option.StartScreenMode].TryGetValue(startscreen.Key, out var pims))
                {
                    JsonHandler.WriteJson(PathGenerator.GetSegmentJsonPath(startscreen.Key.ToString()), pims);
                }
                else
                {
                    pims = new List<PimJson>();
                    JsonHandler.WriteJson(PathGenerator.GetSegmentJsonPath(startscreen.Key.ToString()), pims);

                }
            }
            //JsonHandler.WriteJson(PathGenerator.GetSegmentJsonPath(Path.Combine(FixedStrings.APPLICATIONSEGMENTPATH, currentScreenIndex.ToString()), json), data);
        }
        else
        {
            //Console.WriteLine("JSON IS NULL!!!!!!!!");
        }
    }


    private void SaveSolutionData(bool force = false, int currentScreenIndex = -2)
    {
        var data = Solutions.ToList();
        // force = true;
        // if (currentScreenIndex == 0) currentScreenIndex = DataSave.CurrentProject.Option.StartScreenMode;
        var segment = (SegmentJson)SegementList.SelectedItem;
        if (segment != null && (data.Any() || force))
            PublishSolutionData(segment.name, currentScreenIndex:currentScreenIndex,currentID:segment.id);
    }

    
    private void AddEmptyTile(object sender, RoutedEventArgs e)
    {
        PimJson item = null;
        var index = -1;
        if (SolutionsList.SelectedItems.Count != 1) return;
        item = (PimJson)SolutionsList.SelectedItems[SolutionsList.SelectedItems.Count - 1];
        index = Solutions.IndexOf(item) + 1;

        Solutions.Insert(index,
            new PimJson
            {
                path = "-1", id = -1,
                en = new PimData
                {
                    ProductName = "EmptyTile", Details = "", Subheadline = "", InformationHeader1 = "",
                    InformationHeader2 = "", InformationText1 = "", InformationText2 = "", KeyFact1 = "", KeyFact2 = "",
                    KeyFact3 = "", KeyFact1Description = "", KeyFact2Description = "", KeyFact3Description = ""
                }
            });

        Solutions.Refresh();

        if (DataSave.Option.AutoPublish)
            SaveSolutionData();
        else
        {
            IsDirty = true;
            IsDirtyProject = true;

        }
    }

    private void ClearExcelData(object sender, RoutedEventArgs e)
    {

        ModalHelper.StartModal(FixedStrings.ClearExcelDataModalText, FixedStrings.ClearExcelDataModalTitle,
            () =>
            {
                

                Products = new ObservableCollection<PimJson>();
                PimDataList.ItemsSource = Products;

                PimDataList.Items.Refresh();
                
                Solutions = new ObservableCollection<PimJson>();
                SolutionsList.ItemsSource = Solutions;
                
                DataSave.CurrentProject = new ProjectData();
                DataSave.CheckProject(DataSave.CurrentProject);
                Starter.ResetAllData();

                LanguageDataTools.LanguageDataInit();

                IsDirty = false;
                IsDirtyApplication = false;
                IsDirtyProject = false;

    
            }, () => { },FixedStrings.ClearExcelDataModalOkayText,FixedStrings.ClearExcelDataModalCancelText);
    }

    private string tempname = "";

    private int lastSegmentIndex = -1;


    private  async Task  SegementList_OnSelectionChanged(int appID,int segmentIndex = -2)
    {

        if(DataSave.CurrentProject.AllSegments.TryGetValue(appID,out var segmentJsons))
        {

            if (segmentIndex < 0)
            {
                segmentIndex = 0;

            }
            //Console.WriteLine("semgmentID: "+ segmentIndex +  "appID:"+DataSave.CurrentProject.Option.StartScreenMode);

            for (int i = 0; i < segmentJsons.Count; i++)
            {
                if (segmentJsons[i].id == segmentIndex)
                {
                    DataSave.CurrentProject.Option.StartScreenMode = appID;
                    await SegementList_OnSelectionChanged(segmentJsons[i]);
                    return;
                }
            }

      

        }
    }
    private async Task SegementList_OnSelectionChanged(SegmentJson data)
    {
        if (data == null)
            return;
  
        
        if (!IsDirty)
        {
            RefreshSolutions(data.id, data.path);
        }

        else if (DataSave.Option.AutoPublish)
        {
   
        }
        else if (Segments.Count - 1 < lastSegmentIndex)
        {
            lastSegmentIndex = 0;
            
        }

        else if (IsDirty)
        {
            var segmentremind = Segments[DataSave.CurrentProject.OptionImporter.SegmentsID];
            bool save = SaveReminder(segmentremind.path, DataSave.CurrentProject.OptionImporter.SegmentsID, true);
        
            //Console.WriteLine($"SaveReminder:{save}");
            lastSegmentIndex = data.id;
            //lastSegmentIndex = data.id;
            ////Console.WriteLine("lastSegmentIndex:"+lastSegmentIndex);
            RefreshSolutions(lastSegmentIndex,data.path);
        }

        if (int.TryParse(data.path, out var id))
        {
            DataSave.CurrentProject.OptionImporter.SegmentsID = id;

        }
        lastSegmentIndex = id;

        try
        {
            //DataSave.CurrentProject.OptionImporter.ChoosenSegments[DataSave.CurrentProject.Option.StartScreenMode] = data.path;
            txtFilePath.Content = DataSave.CurrentProject.OptionImporter.RefreshPimsFull[DataSave.CurrentProject.Option.StartScreenMode][lastSegmentIndex];
            DataSave.CurrentProject.Option.RefreshPimId = DataSave.CurrentProject.OptionImporter.RefreshPimsIds[DataSave.CurrentProject.Option.StartScreenMode][lastSegmentIndex];
        }
        catch (Exception ec)
        {
        }

        //DataSave.CurrentProject.OptionImporter.SegmentsID = lastSegmentIndex;
        DataSave.CurrentProject.OptionImporter.SegmentIndex = lastSegmentIndex;

        PathGenerator.SaveOptions();
        Products.Refresh();
        //PimDataList.Items.Refresh();
        SegementList.Items.Refresh();

    }

    private void SegementList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
        SegmentJson json = (SegmentJson)SegementList.SelectedItem;
        if (json != null)
        {
            //DataSave.CurrentProject.OptionImporter.SegmentsID = json.id;
            //Console.WriteLine("json.id:"+json.id+ "DataSave.CurrentProject.Option.StartScreenMode:"+DataSave.CurrentProject.Option.StartScreenMode);
            DataSave.CurrentProject.Option.ChoosenSegment = json.id.ToString();
            Application.Current.Dispatcher.InvokeAsync(() => SegementList_OnSelectionChanged(appID: DataSave.CurrentProject.Option.StartScreenMode,  segmentIndex:json.id));

        }
    
    }

    public bool SaveReminderProject()
    {
        if (!IsDirtyProject && !IsDirty) return true;

        //if (DataSave.Option.AutoPublish) return false;
        var add = " in "+DataSave.ProjectName;
        var title =  "Save Changes" + add;
        var text = $"There are unsaved changes{add}.\n\nDo you want to save them?";
        SegmentJson lastselect = new SegmentJson();
        if (IsDirty)
        {
            lastselect = Segments[lastSegmentIndex];
            if (!string.IsNullOrEmpty(lastselect.name))
                add += " and Publish " + lastselect.name;
            text = $"There are unsaved changes and unpublished changes {add}.\n\nWould you like to save them before exiting?";
            title =  "Save Changes" + add;

        }
        ModalWindow.inputActive = false;
        ModalWindow.Text = text;
        var modalWindow = new ModalWindow();
        modalWindow.Title = title;
        modalWindow.btnSaveData.Content = "Publish";

        modalWindow.ShowDialog();
        if (ModalWindow.success)
        {
            if(IsDirty)
            {
                Publish(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString());
                //Publish(lastselect.path, lastselect.id);
            }
            if(IsDirtyProject)
            {
                //Console.WriteLine(" DataSave.Option.LastProjectFile:"+ DataSave.Option.LastProjectFile);
                MenuSave(true);
            }

            IsDirty = false;
            IsDirtyProject = false;
            return true;
        }
        else
        {
            
        }

        IsDirty = false;
        IsDirtyProject = false;
        return false;
    }

    public bool SaveReminderApplication()
    {
        if (!IsDirtyApplication && !IsDirty) return true;
        if (DataSave.Option.AutoPublish) return true;

        var add = " in ";
        switch (DataSave.CurrentProject.Option.StartScreenMode)
        {
            case 1:
                add += "Smart Transportation";
                break;
            case 2:
                add += "Truck and Trailer";
                break;
            case 3:
                add = "Templates";
                break;
            case 4:
                add = "SdV Panel";
                break;
            case 5:
                add = "Shuttle Panel";
                break;
            case 6:
                add = "Commercial Vehicles";
                break;
            case 7:
                add = "Araiv";
                break;
            default:
                add = "";
                break;
        }

        ModalWindow.Text = $"There are unpublished changes{add}.\n\nDo you want to publish them?";
        ModalWindow.inputActive = false;

        var modalWindow = new ModalWindow();

        modalWindow.Title = "Publish Changes" + add;
        modalWindow.ShowDialog();
        if (ModalWindow.success)
        {
            Publish(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString());
            return true;
        }

        IsDirtyApplication = false;
        IsDirty = false;
        return false;
    }

    public bool SaveReminder(string path,int index, bool dontRefresh = false, bool dontPublish = false,bool dontSave = false)
    {
        if (!IsDirty) return true;
        if (DataSave.Option.AutoPublish) return true;

        var lastselect = new SegmentJson();
        lastselect = Segments[index];

        var add = "";
        if (!string.IsNullOrEmpty(lastselect.name))
            add += " in " + lastselect.name;
        ModalWindow.Text = $"There are unpublished changes{add}.\n\nDo you want to publish them?";
        ModalWindow.inputActive = false;

        var modalWindow = new ModalWindow();
        modalWindow.Title = $"Publish Changes{add}";
        modalWindow.btnSaveData.Content = "Publish";
        modalWindow.ShowDialog();
        if (ModalWindow.success)
        {
            //lastselect = Segments[index];
            IsDirty = false;
            if(!dontPublish)
                Publish(path,index);
            if(!dontRefresh) 
                RefreshSolutions(index,path);
            if(!dontSave)
                DataSave.SaveProject(DataSave.Option.LastProjectFile,DataSave.CurrentProject);
            return true;
        }

        IsDirty = false;
        if(!dontRefresh)
            RefreshSolutions(index,path);
        return false;
    }

    public void RefreshSolutions(int id,string path)
    {
        if (DataSave.CurrentProject.AllSolutions.TryGetValue(DataSave.CurrentProject.Option.StartScreenMode, out var dictSolution))
        {
            
            if (dictSolution.TryGetValue(id, out var segmentJsons))
            {
            
                
                Solutions = new ObservableCollection<PimJson>(segmentJsons);
                SolutionsList.ItemsSource = Solutions;
                Products.Refresh(); 
            }
            else
            {
                //Console.WriteLine("NOOOOOOOOOOOO SOLUTIOSN!!  ID:"+id);

            }
   
        }
        else
        {
            //Console.WriteLine("NOOOOOOOOOOOO APPPPP!!  ID:"+DataSave.CurrentProject.Option.StartScreenMode);

        }
        PathGenerator.RefreshSegmentJson(path);

        // Solutions = new ObservableCollection<PimJson>(PathGenerator.RefreshSegmentJson(path));
    
    }
    public void RefreshSolutionss(string path)
    {

        /*
        Solutions = new ObservableCollection<PimJson>(PathGenerator.RefreshSegmentJson(path));
        SolutionsList.ItemsSource = Solutions;
        Products.Refresh();*/
    }

    private void Solutions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    private void Products_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    private ScrollViewer scrollViewer;
    private Point startPoint;
    private int startIndex = -1;

    private void lstView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        startPoint = e.GetPosition(null);
    }
    
    private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child != null && child is T)
                return (T)child;
            else
            {
                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
        }
        return null;
    }


    private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
    {
        do
        {
            if (current is T) return (T)current;
            current = VisualTreeHelper.GetParent(current);
        } while (current != null);
        return null;
    }
    
    private void solutions_MouseMove(object sender, MouseEventArgs e)
    {
        var mousePos = e.GetPosition(null);
        var diff = startPoint - mousePos;

        if (e.LeftButton == MouseButtonState.Pressed &&
            (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
             Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
        {
            var listView = sender as ListView;
            var listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
            if (listViewItem == null) return;

            // Assuming PimJson is a class representing your data
            var item = (PimJson)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
            if (item == null) return;

            startIndex = SolutionsList.SelectedIndex; // Assuming lstView is the name of your ListView
            var dragData = new DataObject("PimJson", item);
            DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Copy | DragDropEffects.Move);

            // Automatic scrolling
            if (scrollViewer != null)
            {
                if (mousePos.Y < 50)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 2);
                }
                else if (mousePos.Y > SolutionsList.ActualHeight - 50)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 2);
                }
            }
            else
            {
                //Console.WriteLine("scrollview is null");
            }
        }
    }
    private void solutions_DragEnter(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent("PimJson") || sender != e.Source) e.Effects = DragDropEffects.None;
    }

    private void solutions_Drop(object sender, DragEventArgs e)
    {
        var index = -1;

        if (e.Data.GetDataPresent("PimJson") && sender == e.Source)
        {
            var listView = sender as ListView;
            var listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
            if (listViewItem == null)
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            var item = (PimJson)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

            e.Effects = DragDropEffects.Move;
            index = Solutions.IndexOf(item);
            if (startIndex >= 0 && index >= 0)
            {
                Solutions.Move(startIndex, index);
                Solutions.Refresh();
                IsDirty = true;
                IsDirtyProject = true;
            }

            startIndex = -1;
            if (DataSave.Option.AutoPublish)
            {
                IsDirty = false;
                SaveSolutionData(true);
            }
        }
    }

    private void ClearEverything()
    {
    }

    private void ClearApplication(string choosenSegment = "")
    {
        if (string.IsNullOrEmpty(choosenSegment)) choosenSegment = DataSave.CurrentProject.OptionImporter.SegmentsID.ToString();

        for (var i = 0; i < Segments.Count; i++)
        {
            ClearAllSubheadlines(Segments[i]);
            Segments[i].enabled = true;
        }
        DataSave.CurrentProject = new ProjectData();
        DataSave.CheckProject(DataSave.CurrentProject);
        Solutions.Clear();
        SolutionsList.ItemsSource = new ObservableCollection<PimJson>(Solutions);

        Products.Clear();
        PimDataList.ItemsSource = new ObservableCollection<PimJson>(Products);

        if (DataSave.Option.AutoPublish)
            Publish(choosenSegment);
        else
        {
            IsDirty = true;
            IsDirtyProject = true;
        }
        
    }

    private void ClearAll(object sender, RoutedEventArgs e)
    {
        ClearApplication();
    }

    private void ClearAllSubheadlines(object sender, RoutedEventArgs e)
    {
        try
        {
            var button = (Button)sender;
            var segmentjson = (SegmentJson)button.DataContext;
            

            ClearAllSubheadlines(segmentjson);
        }
        catch (Exception exception)
        {
        }
    }

    private void ClearAllSubheadlines(SegmentJson segmentjson)
    {
        var index = -1;

        try
        {
            var path = segmentjson.path;
            var segment = Segments.FirstOrDefault(x => x.path == path);
            if (segment == null) return;

            index = Segments.IndexOf(segment);
        }
        catch (Exception ex)
        {
            return;
        }

        if (index < 0)
            return;

        if (segmentjson.enabledlanguage)
        {
            Segments[index].subheadline = new[] { "", "", "", "", "" };

        }
        else
        {
            Segments[index].subheadlinelanguage = new[] { "", "", "", "", "" };
        }

        
        Segments = new ObservableCollection<SegmentJson>(Segments);
        SegementList.ItemsSource = Segments;
        /*IsDirty = true;
        IsDirtyProject = true;*/

    }

    private void TryTogglePresentation(PimJson data)
    {
        try
        {
            if (data.id > 0)
            {
                if (!DataSave.CurrentProject.Option.PresentationPimIds.Contains(data.id))
                    DataSave.CurrentProject.Option.PresentationPimIds.Add(data.id);
                else
                    DataSave.CurrentProject.Option.PresentationPimIds.Remove(data.id);
                Solutions.Refresh();
            }
        }
        catch 
        {
            // ignored
        }
    }

    private void setPresentation_btn(object sender, RoutedEventArgs e)
    {
        var datasegment = (SegmentJson)SegementList.SelectedItem;
        if (datasegment == null)
            return;
        
        var item = SolutionsList.SelectedItem;

        if (item == null)
        {
            ModalWarning.WarningMessage = "Please select at least on product from the right table";
            var warning = new ModalWarning();
            warning.ShowDialog();
            return;
        }
        var items = SolutionsList.SelectedItems;

        for (int i = 0; i < items.Count; i++)
        {
            try
            {
                var dataTemp = (PimJson)items[i];
                TryTogglePresentation(dataTemp);
            }
            catch
            {
                // ignored
            }
        }
        PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
    }

    private void setVideoScreenSaver_btn()
    {
    }

    private void setVideoScreenSaver_btn(object sender, RoutedEventArgs e)
    {
        setVideoScreenSaver_btn();
    }

    private void SetRefreshTopic(SegmentJson datasegment)
    {
        
    }

    private void SetRefreshTopic(int segmentIndex)
    {

        var item = SolutionsList.SelectedItem;
        if (item == null)
        {
            ModalWarning.WarningMessage = "Please select a product from the right table";
            var warning = new ModalWarning();
            warning.ShowDialog();
            return;
        }

        var data = (PimJson)item;
        txtFilePath.Content = data.en.ProductName + " " + data.id;
        try
        {
            DataSave.CurrentProject.OptionImporter.RefreshPimsFull[DataSave.CurrentProject.Option.StartScreenMode][segmentIndex] = data.id + " " + data.en.ProductName;
            DataSave.CurrentProject.Option.RefreshPimId = data.id;
            DataSave.CurrentProject.OptionImporter.RefreshPimsIds[DataSave.CurrentProject.Option.StartScreenMode][segmentIndex] = data.id;
        }
        catch (Exception exception)
        {
        }

        PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
    }

    private void setRefreshTopic_btn(object sender, RoutedEventArgs e)
    {
        
        var datasegment = (SegmentJson)SegementList.SelectedItem;
        if (datasegment == null)
            return;
        var segmentIndex = SegementList.SelectedIndex;

        var item = SolutionsList.SelectedItem;

        if (item == null)
        {
            ModalWarning.WarningMessage = "Please select a product from the right table";
            var warning = new ModalWarning();
            warning.ShowDialog();
            return;
        }

        var data = (PimJson)item;
        txtFilePath.Content = data.en.ProductName + " " + data.id;
        try
        {
            DataSave.CurrentProject.OptionImporter.RefreshPimsFull[DataSave.CurrentProject.Option.StartScreenMode][segmentIndex] = data.id + " " + data.en.ProductName;
            DataSave.CurrentProject.Option.RefreshPimId = data.id;
            DataSave.CurrentProject.OptionImporter.RefreshPimsIds[DataSave.CurrentProject.Option.StartScreenMode][segmentIndex] = data.id;
        }
        catch (Exception exception)
        {
        }

        PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
    }

    private void deleteSelectedSolution_btn(object sender, RoutedEventArgs e)
    {
    

        try
        {
            var items = SolutionsList.SelectedItems;
            for (var i = 0; i < items.Count; i++)
            {
                DataSave.CurrentProject.AllSolutions[DataSave.CurrentProject.Option.StartScreenMode][DataSave.CurrentProject.OptionImporter.SegmentsID].Remove((PimJson)items[i]);
                Solutions.Remove((PimJson)items[i]);
                i--;
            }
            
        }
        catch (Exception exception)
        {
            //Console.WriteLine("exception:"+exception);
        }
        

        if (DataSave.Option.AutoPublish)
            SaveSolutionData(true);
        else
        {
            IsDirty = true;
            IsDirtyProject = true;

        }
        Products.Refresh();
        Solutions.Refresh();
    }

    private void addSelectedSolution_btn(object sender, RoutedEventArgs e)
    {
        var items = PimDataList.SelectedItems;
        addSolutions(items);
    }

    private void addSolutions(IList items, int index = -1)
    {
        var solutions = Solutions.ToList();
        if (!solutions.Any())
        {
            
        }

        var isEmptyTile = false;
        for (var i = 0; i < items.Count; i++)
        {
            var pim = (PimJson)items[i];
            if (!isEmptyTile && pim.id == -1)
                isEmptyTile = true;
            if (pim.id == -1 || !solutions.Exists(x => x.id == pim.id))
            {
                if (index >= 0)
                    Solutions.Insert(index, pim);
                else
                    Solutions.Add(pim);
                Products.Refresh();
                IsDirty = true;
                IsDirtyProject = true;

            }
        }

        if (IsDirty)
            if (DataSave.Option.AutoPublish)
            {
                IsDirty = false;
                SaveSolutionData(true);
            }

        if (!isEmptyTile)
            PimDataList.UnselectAll();

        Solutions.Refresh();
    }

    private void SubHeadline_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var segment = (SegmentJson)SegementList.SelectedItem;
        if (segment == null)
            return;

        if (DataSave.Option.AutoPublish)
        {
            JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH, "segments.json"), Segments.ToList());
            //JsonHandler.WriteJson(Path.Combine(FixedStrings.APPLICATIONSEGMENTPATH, DataSave.CurrentProject.Option.StartScreenMode.ToString(), "segments.json"), Segments.ToList());
        }
        else
        {
            IsDirtyApplication = true;
        }
    }

    private readonly PaletteHelper _paletteHelper = new();

    public static bool IsDark;

    private void ToogleTheme(bool isDark)
    {
        var theme = _paletteHelper.GetTheme();
        IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : new MaterialDesignLightTheme();
        IsDark = isDark;
        theme.SetBaseTheme(baseTheme);
        _paletteHelper.SetTheme(theme);

        Products.Refresh();
    }



    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        var context = (ButtonBase)e.OriginalSource;

        var type = (string)context.DataContext;
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = (sender as ListBox).SelectedIndex;
        DataSave.CurrentProject.Option.StartScreenMode = index;
        PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
    }

    private void NameChanged(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;

        SaveName(json.Text, json.DataContext);
    }

    private void NameChangedLanguage(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;


        SaveName(json.Text, json.DataContext, true);
    }

    #region Subheadline

    private void SubeheadlineLanguageChanged0(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;

        SaveSubeheadline(json.Text, json.DataContext, 0, true);
    }

    private void SubeheadlineChanged0(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;
        //Console.WriteLine($"SubeheadlineChanged0 json.Text:{json.Text} json.DataContext:{json.DataContext}");

        SaveSubeheadline(json.Text, json.DataContext, 0);
    }

    private void SubeheadlineLanguageChanged1(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;
        SaveSubeheadline(json.Text, json.DataContext, 1, true);
    }

    private void SubeheadlineChanged1(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;
        SaveSubeheadline(json.Text, json.DataContext, 1);
    }

    private void SubeheadlineLanguageChanged2(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;
        SaveSubeheadline(json.Text, json.DataContext, 2, true);
    }

    private void SubeheadlineChanged2(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;
        SaveSubeheadline(json.Text, json.DataContext, 2);
    }

    private void SubeheadlineLanguageChanged3(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;
        SaveSubeheadline(json.Text, json.DataContext, 3, true);
    }

    private void SubeheadlineChanged3(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;
        SaveSubeheadline(json.Text, json.DataContext, 3);
    }

    private void SubeheadlineLanguageChanged4(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;
        SaveSubeheadline(json.Text, json.DataContext, 4, true);
    }

    private void SubeheadlineChanged4(object sender, TextChangedEventArgs e)
    {
        TextBox json = null;
        try
        {
            json = (TextBox)sender;
        }
        catch (Exception es)
        {
            return;
        }

        if (json == null) return;
        SaveSubeheadline(json.Text, json.DataContext, 4);
    }

    private void SaveName(string name, object data, bool isSecondLanguage = false)
    {
        var path = "";

        try
        {
            var segmentjson = (SegmentJson)data;
            path = segmentjson.path;
        }
        catch (Exception e)
        {
            try
            {
                path = data.ToString();
            }
            catch (Exception exception)
            {
                return;
            }
        }

        var segment = Segments.FirstOrDefault(x => x.path == path);
        if (segment == null) return;

        if (isSecondLanguage)
            segment.namelanguage = name;
        else
            segment.name = name;
        var index = Segments.IndexOf(segment);
        Segments[index] = segment;
        if (DataSave.Option.AutoPublish)
        {
            JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH, "segments.json"), Segments.ToList());
            //JsonHandler.WriteJson(Path.Combine(FixedStrings.APPLICATIONSEGMENTPATH, DataSave.CurrentProject.Option.StartScreenMode.ToString(), "segments.json"), Segments.ToList());
        }
        else
        {
            IsDirtyApplication = true;
        }
    }

    private void SaveSubeheadline(string subheadline, object data, int id, bool isLanguage = false)
    {
        var path = "";
        try
        {
            var segmentjson = (SegmentJson)data;
            path = segmentjson.path;
        }
        catch (Exception e)
        {
            try
            {
                path = data.ToString();
            }
            catch (Exception exception)
            {
                return;
            }
        }

        var segment = Segments.FirstOrDefault(x => x.path == path);
        if (segment == null) return;
        
        isLanguage = !isLanguage;
        if (!isLanguage)
            segment.subheadlinelanguage[id] = subheadline;
        else
            segment.subheadline[id] = subheadline;
        var index = Segments.IndexOf(segment);
        Segments[index] = segment;
        
        // DataSave.CurrentProject.Segments[index] = segment;
        if (true)
        {
            JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH, "segments.json"), Segments.ToList());
            //JsonHandler.WriteJson(Path.Combine(FixedStrings.APPLICATIONSEGMENTPATH, DataSave.CurrentProject.Option.StartScreenMode.ToString(), "segments.json"), Segments.ToList());
        }
        else
        {
            IsDirty = true;
            IsDirtyApplication = true;
        }
    }

    #endregion

    private void MenuExit()
    {
        if (SaveReminderProject())
        {
            if (SaveReminderApplication())
            {
                if (SaveReminder(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString(),lastSegmentIndex))
                {
                    Application.Current.Shutdown();
                }       

            }
        }
    }

    private void MenuExit(object sender, RoutedEventArgs e)
    {
        MenuExit();
    }

    private void MenuSave(bool force = false)
    {
        try
        {
            if (File.Exists(DataSave.Option.LastProjectFile) || force)
            {
                DataSave.SaveProject(DataSave.Option.LastProjectFile);
                /*DataSave.RefreshTitle();
                this.Title = TitleStart + DataSave.ProjectName;*/
            }
            else
            {
                //Console.WriteLine("NONO:");
                DataSave.SaveProject(FixedStrings.STORAGEPATH);
                /*DataSave.RefreshTitle();
                this.Title = TitleStart + DataSave.ProjectName;*/
            }
            

            CreateMenus();
        }
        catch (Exception exception)
        {
            //Console.WriteLine("Tried to save project:"+exception);
        }
    
    }

    private void MenuSave(object sender, RoutedEventArgs e)
    {
        MenuSave(false);
    }

    private void MenuSaveAs(object sender, RoutedEventArgs e)
    {
        MenuSaveAs(out var path);
    }

    private bool MenuSaveAs(out string path)
    {
        
        var openfile = new SaveFileDialog();
        openfile.DefaultExt = ".zfpimi";
        openfile.Filter = "(.zfpimi)|*.zfpimi";
        openfile.Title = "Speichern des Projektes unter";
        if (!Directory.Exists(FixedStrings.SAVEDPROJECTS))
            Directory.CreateDirectory(FixedStrings.SAVEDPROJECTS);

        openfile.InitialDirectory = FixedStrings.SAVEDPROJECTS;

        var browsefile = openfile.ShowDialog();
        path = "";
        if (browsefile == true)
        {
            DataSave.Option.LastProjectFile = openfile.FileName;
            path = openfile.FileName;
            MenuSave(true);
            return true;

        }
        return false;

    }

    
    private void DeleteAllExcels()
    {
            
        var di = new DirectoryInfo(FixedStrings.EXCELSTORAGE);

        var index = 0;
        foreach (var file in di.GetFiles("*.xlsx"))
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }


      
        CreateExcelMenu();
        
    }

    private void DeleteExcel(object sender, RoutedEventArgs e)
    {
        string pathToFile = GetPathDataContextMenuItem(sender);
        FileInfo info = new FileInfo(pathToFile);
        if (info.Exists)
        {
            info.Delete();
        }
      
        CreateExcelMenu();
        
    }
    
    
    private void DeleteProject(object sender, RoutedEventArgs e)
    {
        string pathToFile = GetPathDataContextMenuItem(sender);
        FileInfo info = new FileInfo(pathToFile);
        if (info.Exists)
        {
            info.Delete();
        }
        DataSave.RefreshTitle();
        StarterTab();
        CreateMenus();
        
    }




    private string GetPathDataContextMenuItem(object sender)
    {
        var menuItem = (MenuItem)sender;
        return menuItem.DataContext.ToString();
    }

    private void SavePublishProject(object sender, RoutedEventArgs e)
    {
        
    }

    private void OpenPublishProject(object sender, RoutedEventArgs e)
    {
        string pathTo = GetPathDataContextMenuItem(sender);
        //Console.WriteLine("sender:"+pathTo);
        if (SaveReminderProject())
        {
            
            if (SaveReminder(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString(),lastSegmentIndex))
            {
                OpenProject(pathTo, () =>
                {
                               
                    try
                    {
                        var lastselect = Segments[DataSave.CurrentProject.OptionImporter.SegmentIndex];
                        IsDirty = false;
                        IsDirtyProject = false;
                        IsDirtyApplication = false;
                
                        Publish(
                            DataSave.CurrentProject.OptionImporter.SegmentsID.ToString(),
                            DataSave.CurrentProject.OptionImporter.SegmentsID,
                            DataSave.CurrentProject.OptionImporter.SegmentIndex);

                        //Publish(lastselect.path,DataSave.CurrentProject.OptionImporter.SegmentID);
                    }
                    catch (Exception exception)
                    {
                        //Console.WriteLine("EXCEPT:" + exception);
                    } 
                });

            }
            
           
        }
   

        //Publish();
        


    }

    private void OpenSingleProject(object sender, RoutedEventArgs e)
    {
        string pathTo = GetPathDataContextMenuItem(sender);
        //Console.WriteLine("TRY TO OPEN PROJKECT:"+pathTo);
        if (File.Exists(pathTo))
        {
            if (SaveReminderProject())
            {
                if (SaveReminder(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString(),lastSegmentIndex))
                {
                    OpenProject(pathTo);
                }
            }

        }
    }

    private void NewProject(object sender, RoutedEventArgs e)
    {
        
        if (SaveReminderProject())
        {
            
            if(SaveReminder(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString(),lastSegmentIndex))
            {
                DataSave.CurrentProject = new ProjectData();
                DataSave.CheckProject(DataSave.CurrentProject);

                DataSave.CurrentProject.OptionImporter.SegmentsID = 0;
                DataSave.CurrentProject.OptionImporter.SegmentIndex = 0;

                DataSave.CurrentProject.Option.StartScreenMode = 3;
                if (MenuSaveAs(out string path))
                {
                    OpenProject(path);
                }
                
            }
        }
        
     
    }

    private void OpenProject(string path,Action onComplete = null)
    {
        if (path == DataSave.Option.LastProjectFile)
        {
            //Console.WriteLine("ALREADY PATEHD:"+path);
            //return;
        }
        
        if (SaveReminderProject())
        {
            
            if (SaveReminder(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString(),lastSegmentIndex))
            {
                DataSave.Option.LastProjectFile = path;
                //Console.WriteLine("LASTPATH:"+DataSave.Option.LastProjectFile);
                Serializer.WriteBin(FixedStrings.PIMOPTIONSPATH,DataSave.Option);
                DataSave.RefreshTitle();

        
                StarterTab(onComplete);
            }
  
        }

        
        return;

        
                    
        var lastselect = Segments[lastSegmentIndex];

        RefreshSolutions(lastSegmentIndex,lastselect.path);
        lastselect = Segments[lastSegmentIndex];
        ;

        if (SegementList.SelectedIndex != 0)
            SegementList.SelectedIndex = 0;
        else
            SegementList.SelectedIndex = 1;
        return;
    }

    private void OpenProject(object sender, RoutedEventArgs e)
    {
        
        if (SaveReminderProject())
        {
            if (SaveReminder(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString(),lastSegmentIndex))
            {
                var openfile = new OpenFileDialog();
                openfile.DefaultExt = ".zfpimi";
                openfile.Filter = "(.zfpimi)|*.zfpimi";
                if (!Directory.Exists(FixedStrings.SAVEDPROJECTS))
                    Directory.CreateDirectory(FixedStrings.SAVEDPROJECTS);

                openfile.InitialDirectory = FixedStrings.SAVEDPROJECTS;
        
                var browsefile = openfile.ShowDialog();

                if (browsefile == true)
                {
                    OpenProject(openfile.FileName);

                }
            }
  
        }
        
  
    }

    private string currentlyOpenendProject;

    private void MenuClear()
    {
        var segment = (SegmentJson)SegementList.SelectedItem;
        var path = segment.path;
        ModalHelper.StartModal($"Do you really want clear all Products from {segment.name}",
            $"Clear Products from {segment.name}", () => { ClearSolutionData(path); }, () => { });
    }

    private void MenuClear(object sender, RoutedEventArgs e)
    {
        MenuClear();
    }

    
    public void ToggleConfidential(bool active)
    {
        
        DataSave.CurrentProject.Option.AllConfidentialActive = active;
        //ConfidentialToggle.Icon = active ? new PackIcon { Kind = PackIconKind.BookLock } : new PackIcon { Kind = PackIconKind.BookLockOutline };

        PimDataList.Items.Refresh();
        SolutionsList.Items.Refresh();

        //ConfidentialToggle.IsChecked = DataSave.CurrentProject.Option.AllConfidentialActive;
    }
    
    private void PressedConfidentialToggle(object sender, RoutedEventArgs e)
    {
        ToggleConfidential(!DataSave.CurrentProject.Option.AllConfidentialActive);
        PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
    }

    private void MenuToggleOptions(object sender, RoutedEventArgs e)
    {
        try
        {
            var menuItem = (MenuItem)sender;
            var optionType = 0;
            int.TryParse(menuItem.DataContext.ToString(), out optionType);
            SwitchOptionMenu(optionType);
            PathGenerator.SaveOptions();
        }
        catch (Exception ex)
        {
            try
            {
                var check = (CheckBox)sender;
                var optionType = 0;
                int.TryParse(check.DataContext.ToString(), out optionType);
                SwitchOptionMenu(optionType);
                PathGenerator.SaveOptions();
            }
            catch (Exception ex2)
            {
            }
        }
    }
    
    private void SwitchConfidantial(object sender, RoutedEventArgs e)
    {
       
    }

    private async Task MenuSwitchStartScreen(int startscreen)
    {
       
        
        if (SaveReminderProject())
        {
            if (SaveReminderApplication())
            {
                 SwitchStartScreenMenu(startscreen,()=> {
                
                });
            }

        }
    }
    private void MenuSwitchStartTemplateScreen(object sender, RoutedEventArgs e)
    {
        try
        {
            TurnAllOf();
            var menuItem = (MenuItem)sender;
            var startscreen = 0;
            if (!int.TryParse(menuItem.DataContext.ToString(), out startscreen)) return;

   
      
            switch (startscreen)
            {
                case 0:
                    DataSave.CurrentProject.Option.Template = "white";
                    PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                    MenuItemStartscreen.Header = "_ZF White Mode";
                    break;
                case 1:
                    DataSave.CurrentProject.Option.Template = "white";
                    PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                    MenuItemStartscreen.Header = "_ZF White Mode";

                    break;
             
                case 2:
                    DataSave.CurrentProject.Option.Template = "blue";
                    PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                    MenuItemStartscreen.Header = "_ZF Blue Mode";

                    break;
                case 3:
                    DataSave.CurrentProject.Option.Template = "lifetec";
                    PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                    MenuItemStartscreen.Header = "LIFETEC";

                    break;
                
            }
            MenuItemStartscreen.Header = "_ZF White Mode";
            switch (DataSave.CurrentProject.Option.Template)
            {
               
                case "white":
                    MenuItemStartscreen.Header = "_ZF White Mode";
                    WhiteModeToggle.Icon = new PackIcon { Kind = PackIconKind.CardText };

                    break;
             
                case "blue":
                    MenuItemStartscreen.Header = "_ZF Blue Mode";
                    BlueModeToggle.Icon = new PackIcon { Kind = PackIconKind.CardText };

                    break;
                case "lifetec":
                    LifeTecToggle.Icon = new PackIcon { Kind = PackIconKind.CardText };
                    MenuItemStartscreen.Header = "LIFETEC";

                    break;
            }

                 
            if (DataSave.CurrentProject.Option.StartScreenMode != 3)
            {
                Application.Current.Dispatcher.InvokeAsync(async () =>  MenuSwitchStartScreen(3));
            }
            else
            {
                
            }
            // Task.Run(()=> MenuSwitchStartScreen(startscreen)).Start();
           
    
        }
        catch (Exception ex)
        {
        }
    }
    private void MenuSwitchStartScreen(object sender, RoutedEventArgs e)
    {
        try
        {
            var menuItem = (MenuItem)sender;
            var startscreen = 0;
            if (!int.TryParse(menuItem.DataContext.ToString(), out startscreen)) return;
            Application.Current.Dispatcher.InvokeAsync(async () =>  MenuSwitchStartScreen(startscreen));

           // Task.Run(()=> MenuSwitchStartScreen(startscreen)).Start();
           
    
        }
        catch (Exception ex)
        {
        }
    }

    private void SwitchOptionMenu(int index)
    {
        var toogle = false;
        switch (index)
        {
            case 0:
                toogle = !DataSave.CurrentProject.Option.AllDetailsActive;
                break;
            case 1:
                toogle = !DataSave.CurrentProject.Option.AllMoreActive;

                break;
            case 2:
                toogle = !DataSave.Option.AutoPublish;

                break;
            case 3:
                toogle = !DataSave.Option.DarkMode;
                break;
            case 4:
                toogle = !DataSave.Option.ShowSnackbar;
                break;
            case 5:
                toogle = !DataSave.CurrentProject.Option.UseConverter;
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                toogle = !DataSave.CurrentProject.Option.AllSpecsActive;
                break;
            case 9:
                toogle = !DataSave.CurrentProject.Option.AllImagesActive;
                break;
            case 10:
                toogle = !DataSave.CurrentProject.Option.AllVideosActive;
                break;
            case 11:
                toogle = !DataSave.CurrentProject.Option.AllPresentationsActive;
                break;
        }

        SwitchOptionMenu(index, toogle);
    }

    private void SwitchOptionMenu(int index, bool value)
    {
        switch (index)
        {
            case 0:
                DataSave.CurrentProject.Option.AllDetailsActive = value;
                PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                break;
            case 1:
                DataSave.CurrentProject.Option.AllMoreActive = value;
                PathGenerator.SaveOptions(DataSave.CurrentProject.Option);

                break;
            case 2:
                DataSave.Option.AutoPublish = value;
                //PathGenerator.SaveImprterOptions(DataSave.CurrentProject.OptionImporter);
                break;
            case 3:
                ToogleTheme(value);
                DataSave.Option.DarkMode = value;
                //PathGenerator.SaveImprterOptions(DataSave.CurrentProject.OptionImporter);
                break;
            case 4:

                DataSave.Option.ShowSnackbar = value;
                //PathGenerator.SaveImprterOptions(DataSave.CurrentProject.OptionImporter);
                break;
            case 5:
                DataSave.CurrentProject.Option.UseConverter = value;
                PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                break;
            case 6:

                PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                break;
            case 8:
                DataSave.CurrentProject.Option.AllSpecsActive = value;

                PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                break;
            case 9:
                DataSave.CurrentProject.Option.AllImagesActive = value;

                PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                break;
            case 10:
                DataSave.CurrentProject.Option.AllVideosActive = value;

                PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                break;
            case 11:
                DataSave.CurrentProject.Option.AllPresentationsActive = value;
                PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
                break;
        }

        SetOptionMenu(index);
    }

    private void SetOptionMenu(int index)
    {
        var msg = "";
        return;
        if (index <= 1)
        {
            if (index == 1)
            {
                if (!DataSave.CurrentProject.Option.AllMoreActive)
                {
                    option1.ToolTip = FixedStrings.ActivateMoreTooltip;
                    msg += "More is now active";
                }
                else
                {
                    option1.ToolTip = FixedStrings.DeactivateMoreTooltip;
                    msg += "More deactivated";
                }
            }

            if (index == 0)
            {
                if (!DataSave.CurrentProject.Option.AllDetailsActive)
                {
                    option0.ToolTip = FixedStrings.ActivateDetailsTooltip;

                    msg += "Details is now active";
                }
                else
                {
                    option0.ToolTip = FixedStrings.DeactivateDetailsTooltip;
                    msg += "Details deactivated";
                }
            }
        }

        return;
        if (index >= 8)
        {
            if (index == 8)
            {
                if (!DataSave.CurrentProject.Option.AllDetailsActive)
                {
                    option0.ToolTip = FixedStrings.ActivateDetailsTooltip;

                    msg += "Details is now active";
                }
                else
                {
                    option0.ToolTip = FixedStrings.DeactivateDetailsTooltip;
                    msg += "Details deactivated";
                }
            }

            if (index == 9)
            {
                if (!DataSave.CurrentProject.Option.AllDetailsActive)
                {
                    option0.ToolTip = FixedStrings.ActivateDetailsTooltip;

                    msg += "Details is now active";
                }
                else
                {
                    option0.ToolTip = FixedStrings.DeactivateDetailsTooltip;
                    msg += "Details deactivated";
                }
            }

            if (index == 10)
            {
                if (!DataSave.CurrentProject.Option.AllDetailsActive)
                {
                    option0.ToolTip = FixedStrings.ActivateDetailsTooltip;

                    msg += "Details is now active";
                }
                else
                {
                    option0.ToolTip = FixedStrings.DeactivateDetailsTooltip;
                    msg += "Details deactivated";
                }
            }

            if (index == 11)
            {
                if (!DataSave.CurrentProject.Option.AllDetailsActive)
                {
                    option0.ToolTip = FixedStrings.ActivateDetailsTooltip;

                    msg += "Details is now active";
                }
                else
                {
                    option0.ToolTip = FixedStrings.DeactivateDetailsTooltip;
                    msg += "Details deactivated";
                }
            }
        }
    }

    private string ToggleSkipStartScreen(string message)
    {
        message = "Skip SartScreen is now ";

        DataSave.CurrentProject.Option.SkipStartScreen = !DataSave.CurrentProject.Option.SkipStartScreen;
        if (DataSave.CurrentProject.Option.SkipStartScreen)
        {
            message += "Activated";
            SkipMenu.Icon = new PackIcon { Kind = PackIconKind.PageNext };
        }
        else
        {
            message += "Deactivated";

            SkipMenu.Icon = new PackIcon { Kind = PackIconKind.PageNextOutline };
        }

        return message;
    }

    private void SwitchStartScreenMenu(int index, Action onComplete)
    {
        Application.Current.Dispatcher.InvokeAsync(async () => await SwitchStartScreenMenuAsync(index,onComplete));
    }
    private void TurnAllOf()
    {
        Araiv.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
        ShuttleScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
        SdVScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
        SmartMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
        FocusScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
        FocusMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
        CommercialVehicleSolutions.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
        
        WhiteModeToggle.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
        BlueModeToggle.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
        LifeTecToggle.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };

    }
    private async Task SwitchStartScreenMenuAsync(int index,Action onComplete)
    {
        var header = "_";
        var icon = new PackIcon();

        var message = "";
        PackIcon icon2;
        switch (index)
        {
            case 0:
                if (DataSave.CurrentProject.Option.StartScreenMode == 4)
                {
                    SegementList.Visibility = Visibility.Hidden;

                    DataSave.CurrentProject.Option.SkipStartScreen = false;
                }

                message = ToggleSkipStartScreen(message);
                break;

            case 5:
                TurnAllOf();
                ShuttleScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardText };


                DataSave.CurrentProject.Option.SkipStartScreen = false;

                SegementList.Visibility = Visibility.Hidden;

                message += "Using Shuttle Panel";
                ToggleSkipStartScreen(message);
                break;
            case 4:
                TurnAllOf();
                SdVScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardText };


                message += "Using SdV Panel";

                DataSave.CurrentProject.Option.SkipStartScreen = false;
                SegementList.Visibility = Visibility.Hidden;

                ToggleSkipStartScreen(message);
                break;
            case 7:
                MenuItemStartscreen.Header = "_Araiv";
                TurnAllOf();


                Araiv.Icon = new PackIcon { Kind = PackIconKind.CardText };

                DataSave.CurrentProject.Option.SkipStartScreen = false;
                SegementList.Visibility = Visibility.Hidden;

                ToggleSkipStartScreen(message);
                message += "Using Araiv";
                break;
            case 6:
                MenuItemStartscreen.Header = "_Commercial Vehicles";
                TurnAllOf();


                CommercialVehicleSolutions.Icon = new PackIcon { Kind = PackIconKind.CardText };
                DataSave.CurrentProject.Option.SkipStartScreen = true;
                SegementList.Visibility = Visibility.Visible;

                ToggleSkipStartScreen(message);
                message += "Using Commercial Vehicles";
                break;

            case 3:
                TurnAllOf();
                MenuItemStartscreen.Header = "_ZF White Mode";
                switch (DataSave.CurrentProject.Option.Template)
                {
               
                    case "white":
                        MenuItemStartscreen.Header = "_ZF White Mode";
                        WhiteModeToggle.Icon = new PackIcon { Kind = PackIconKind.CardText };

                        break;
             
                    case "blue":
                        MenuItemStartscreen.Header = "_ZF Blue Mode";
                        BlueModeToggle.Icon = new PackIcon { Kind = PackIconKind.CardText };

                        break;
                    case "lifetec":
                        LifeTecToggle.Icon = new PackIcon { Kind = PackIconKind.CardText };
                        MenuItemStartscreen.Header = "LIFETEC";

                        break;
                }
            

                DataSave.CurrentProject.Option.SkipStartScreen = true;
                SegementList.Visibility = Visibility.Visible;

                ToggleSkipStartScreen(message);
                message += "Using Templates";

                break;
            case 2:
                MenuItemStartscreen.Header = "_Truck and Trailer";
                Araiv.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
                ShuttleScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
                SdVScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
                SmartMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
                FocusScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
                CommercialVehicleSolutions.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };

                FocusMenu.Icon = new PackIcon { Kind = PackIconKind.CardText };
                DataSave.CurrentProject.Option.SkipStartScreen = true;
                SegementList.Visibility = Visibility.Visible;

                ToggleSkipStartScreen(message);
                message += "Using Truck and Trailer Panel";

                break;
            case 1:

                Araiv.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
                MenuItemStartscreen.Header = "_Smart Transportation";
                ShuttleScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
                SdVScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
                SmartMenu.Icon = new PackIcon { Kind = PackIconKind.CardText };
                FocusScreenMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
                FocusMenu.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };
                CommercialVehicleSolutions.Icon = new PackIcon { Kind = PackIconKind.CardTextOutline };

                DataSave.CurrentProject.Option.SkipStartScreen = true;
                SegementList.Visibility = Visibility.Visible;

                ToggleSkipStartScreen(message);
                message += "Using Smart Transportation";

                break;
        }

        //await Task.Delay(1);

        if (index is > 0 and <= 7)
        {
            DataSave.CurrentProject.Option.StartScreenMode = index;
            //Console.WriteLine("STARTSCREEN INDEX:"+index);
            //Console.WriteLine("SegmentsID:"+ DataSave.CurrentProject.OptionImporter.SegmentsID+ " SegmentIndex:"+ DataSave.CurrentProject.OptionImporter.SegmentIndex);
            PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
            DataSave.SwitchApplication(index);
            onComplete?.Invoke();
            ReloadSegments();
            SegementList_OnSelectionChanged(appID:index,segmentIndex:DataSave.CurrentProject.OptionImporter.SegmentsID);
            
            /*
            ReloadSegments();*/
        }
        else
        {
            PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
        }

        TriggerSnackbar(message, 1.2f);
    }

    private void MenuPublish(object sender, RoutedEventArgs e)
    {
        
        if (DataSave.CurrentProject.AllSegments.TryGetValue(DataSave.CurrentProject.OptionImporter.SegmentsID,
                out var test))
        {
            
            Publish(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString(),DataSave.CurrentProject.OptionImporter.SegmentIndex);

        }
        else
        {
            DataSave.CurrentProject.AllSegments[DataSave.CurrentProject.OptionImporter.SegmentsID] =
                new List<SegmentJson>();
            Publish(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString(),DataSave.CurrentProject.OptionImporter.SegmentIndex);

            //Console.WriteLine("PUBLISH NOT FOUND");
        }
    }

    private SnackbarMessageQueue currentQueue;

    private void TriggerSnackbar(string message, float time = 1.5f)
    {
        if (DataSave.Option.AutoPublish) return;
        if (Snackbar0.IsActive)
            Snackbar0.Message.Content = message;
        else
            Snackbar0.MessageQueue?.Enqueue(
                $"{message}",
                null,
                null,
                null,
                false,
                false,
                TimeSpan.FromSeconds(time));
    }

    private void Publish(string path, int id = -2, int index = -2)
    {
        PathGenerator.SaveOptions();
        var segments = Segments.ToList();
        JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH, "segments.json"), segments);
        if (!DataSave.Option.AutoPublish)
            TriggerSnackbar(FixedStrings.MessagePublished, 1.2f);
        IsDirty = false;
        IsDirtyApplication = false;
        IsDirtyProject = false;
        PublishSolutionData(path, id,index);

        
        //JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH, "products.json"), Products.ToList());
        // JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH, "options.json"),DataSave.CurrentProject.Option);
        
        //JsonHandler.WriteJson(Path.Combine(FixedStrings.APPLICATIONSEGMENTPATH, DataSave.CurrentProject.Option.StartScreenMode.ToString(), "segments.json"), segments);
    }

    private void MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        FileSystemDialog dialog = new MaterialDesignExtensions.Controls.SaveFileDialog();
    }

    private void MenuItem_OnChecked(object sender, RoutedEventArgs e)
    {
    }

    private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        Publish(DataSave.CurrentProject.OptionImporter.SegmentsID.ToString());
        MenuSave(sender, e); //Nico edit for saving
    }

    private void MenuItem_OnCheckeds(object sender, RoutedEventArgs e)
    {
    }

    private void SnackbarMessage_OnActionClick(object sender, RoutedEventArgs e)
    {
    }

    private void ActivateDevModus(object sender, ExecutedRoutedEventArgs e)
    {
        var projectPage = new ProjectPage();
        projectPage.ShowInTaskbar = true;
        projectPage.Show();
    }

    private void DevMenu_OnClick(object sender, RoutedEventArgs e)
    {
        var projectPage = new ProjectPage(this);
        projectPage.ShowInTaskbar = true;
        projectPage.Show();
    }

    private void MenuClearApplication()
    {
        var add = " in ";
        switch (DataSave.CurrentProject.Option.StartScreenMode)
        {
            case 1:
                add += "Smart Transportation";
                break;
            case 2:
                add += "Truck and Trailer";
                break;
            case 3:
                add = "Templates";
                break;
            case 4:
                add = "SdV Panel";
                break;
            case 5:
                add = "Shuttle Panel";
                break;
            case 6:
                add = "Commercial Vehicles";
                break;
            default:
                add = "";
                break;
        }

        ModalHelper.StartModal($"Do you really want to delete all data in {add}", $"Delete all data in {add}",
            () => { ClearApplication(); }, () => { });
    }

    private void MenuClearApplication(object sender, RoutedEventArgs e)
    {
        MenuClearApplication();
    }

    private void MenuClearAll(object sender, RoutedEventArgs e)
    {
        MenuClearApplication();
    }

    private static readonly Regex _regex = new("[^0-9]+");

    private static bool IsTextAllowed(string text)
    {
        return !_regex.IsMatch(text);
    }

    private static bool IsTextLengthAllowed(string text)
    {
        var maxLength = "Software / Digitalization".Length;
        if (maxLength <= text.Length)
            return false;
        return true;
    }

    private static readonly Regex _regexLanguage = new("[a-zA-Z]");

    private static bool IsLanguageTextAllowed(string text)
    {
        return _regexLanguage.IsMatch(text);
    }

    private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var json = (TextBox)sender;
        if (json == null) return;
        var txt = json.Text.Replace(" ", "").Replace("   ", "").Replace("\n", "");
        if (!int.TryParse(txt, out var index)) index = 0;
        json.Text = index.ToString();
        if (index <= 5)
            index = 5;

        DataSave.CurrentProject.OptionImporter.RefreshTimer = index;
        DataSave.CurrentProject.Option.RefreshTimer = index;
        PathGenerator.SaveOptions();
    }

    private void PreviewSegmentNameInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !IsTextLengthAllowed(e.Text);
    }

    private void TextBoxSegmentPasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(typeof(string)))
        {
            var text = (string)e.DataObject.GetData(typeof(string));
            if (!IsTextLengthAllowed(text)) e.CancelCommand();
        }
        else
        {
            e.CancelCommand();
        }
    }

    private void PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !IsTextAllowed(e.Text);
    }

    private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(typeof(string)))
        {
            var text = (string)e.DataObject.GetData(typeof(string));
            if (!IsTextAllowed(text)) e.CancelCommand();
        }
        else
        {
            e.CancelCommand();
        }
    }

    private void SolutionChecked(object sender, RoutedEventArgs e)
    {
        var checkBox = (ToggleButton)sender;
        if (checkBox?.IsChecked == null) return;

        if (DataSave.Option.AutoPublish)
            SaveSolutionData();
        else
        {
            IsDirty = true;
            IsDirtyProject = true;

        }
    }

    private void LanguageSegmentCheckedNormal(object sender, RoutedEventArgs e)
    {
        var checkBox = (ToggleButton)sender;
        if (checkBox?.IsChecked == null) return;

        //Console.WriteLine($"ISCHECK:{checkBox?.IsChecked}");
        Application.Current.Dispatcher.InvokeAsync(() => RefreshSegmentList(sender, e));
    }

    private async void RefreshSegmentList(object sender, RoutedEventArgs e)
    {
        var checkBox = (ToggleButton)sender;
        if (checkBox?.IsChecked == null) return;

        //await Task.Delay(200);

        if (SegementList != null)
            await SegementList.Dispatcher.InvokeAsync(() => { SegementList.Items.Refresh(); });

        if (DataSave.Option.AutoPublish)
        {
            JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH, "segments.json"), Segments.ToList());
            //JsonHandler.WriteJson(Path.Combine(FixedStrings.APPLICATIONSEGMENTPATH, DataSave.CurrentProject.Option.StartScreenMode.ToString(), "segments.json"), Segments.ToList());
        }
        else
        {
            IsDirtyApplication = true;
            /*IsDirtyProject = true;
            IsDirty = true;*/
        }
    }

    private void LanguageSegmentChecked(object sender, RoutedEventArgs e)
    {
        SegementList.Items.Refresh();
        var checkBox = (ToggleButton)sender;
        if (checkBox?.IsChecked == null) return;

        var parent = VisualTreeHelper.GetParent(checkBox);
        while (!(parent is ListViewItem)) parent = VisualTreeHelper.GetParent(parent);
        var container2 = parent as ListViewItem;
        if (container2 != null)
        {
            container2.ContentTemplateSelector = null;
            container2.ContentTemplateSelector = SegementList.ItemTemplateSelector;
        }

        if (false && SegementList != null)
        {
            foreach (var items in SegementList.Items)
            {
                var container = SegementList.ItemContainerGenerator.ContainerFromItem(items) as ListViewItem;
                if (container != null)
                {
                    container.ContentTemplateSelector = null;
                    container.ContentTemplateSelector = SegementList.ItemTemplateSelector;
                }
            }

            if (false)
            {
                try
                {
                    var data = (SegmentJson)SegementList.SelectedItem;

                    // if (data != null) DataSave.CurrentProject.OptionImporter.SegmentsID.ToString() = data.path;
                }
                catch (Exception es)
                {
                }

                var item = checkBox.DataContext;
                if (item != null)
                {
                    var index = SegementList.Items.IndexOf(item);
                    if (index >= 0)
                    {
                        SegementList.Items.RemoveAt(index);
                        SegementList.Items.Insert(index, item);
                    }
                }
            }
        }

        if (DataSave.Option.AutoPublish)
        {
            JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH, "segments.json"), Segments.ToList());
            // JsonHandler.WriteJson(Path.Combine(FixedStrings.APPLICATIONSEGMENTPATH, DataSave.CurrentProject.Option.StartScreenMode.ToString(), "segments.json"), Segments.ToList());
        }
        else
        {
            IsDirtyApplication = true;
        }
    }

    private void SegmentChecked(object sender, RoutedEventArgs e)
    {
        var checkBox = (ToggleButton)sender;
        if (checkBox?.IsChecked == null) return;

        if (DataSave.Option.AutoPublish)
        {
            JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH, "segments.json"), Segments.ToList());
            //JsonHandler.WriteJson(Path.Combine(FixedStrings.APPLICATIONSEGMENTPATH, DataSave.CurrentProject.Option.StartScreenMode.ToString(), "segments.json"), Segments.ToList());
        }
        else
        {
            IsDirtyApplication = true;
        }
    }

    private void RefreshChecked(object sender, RoutedEventArgs e)
    {
        var checkBox = (CheckBox)sender;
        if (checkBox == null) return;
        if (checkBox.IsChecked == null) return;

        checkBox.IsChecked = false;

        return;
        PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
    }

    private void MenuLanguageButton_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
    }

    private void MenuLanguageButton_OnSubmenuOpened(object sender, RoutedEventArgs e)
    {
    }

    private void LanguageTextbox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !IsLanguageTextAllowed(e.Text);
    }

    private void LanguageTextbox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var json = (TextBox)sender;
        if (json == null) return;
        var txt = json.Text.Replace(" ", "").Replace("   ", "").Replace("\n", "");
        if (string.IsNullOrEmpty(txt))
        {
            MenuLanguageButton.Header = " " + "EN" + " ";
            DataSave.CurrentProject.Option.SecondLanguage = "";

            PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
            return;
        }

        MenuLanguageButton.Header = " " + txt + " ";
        DataSave.CurrentProject.Option.SecondLanguage = txt;
        PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
    }

    private void NoSecondLanguage_OnClick(object sender, RoutedEventArgs e)
    {
        MenuLanguageButton.Header = " EN ";
        DataSave.CurrentProject.Option.SecondLanguage = "";
        PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
    }

    private void Help_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void LanguageTextbox_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var item = (MenuItem)sender;

        LanguageDataTools.LanguageMenuSwitch(item.Header.ToString());
    }

    private void LanguageTextbox_OnGotFocus(object sender, RoutedEventArgs e)
    {
    }

    private void MenuLanguageButton_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void UIElement_OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
        //Console.WriteLine("UIELEMENT ON MAN");
    }

    private void FrameworkElement_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        //Console.WriteLine("FrameworkElement_OnDataContextChanged ON MAN");
    }

    private void ScreenSaverModeUnChecked(object sender, RoutedEventArgs e)
    {
        //Console.WriteLine("UN CHECKED");
    }

    private void ScreenSaverModeChecked(object sender, RoutedEventArgs e)
    {
        //Console.WriteLine("CHECKED");
    }

    private void ScreenSaverValidate(object sender, RoutedEventArgs e)
    {
        
    }

    private void ScreenSaverClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            var radio = (RadioButton)sender;
            if (int.TryParse(radio.DataContext.ToString(), out var parsedint))
            {
                var mode = (ScreenSaverMode)parsedint;
                switch (mode)
                {
                    case ScreenSaverMode.Menu:

                        break;
                    case ScreenSaverMode.Product:

                        break;
                    case ScreenSaverMode.Video:

                        break;
                }

                if (mode == DataSave.CurrentProject.Option.ScreenSaverMode)
                {
                    radio.IsChecked = false;
                    ScreenSaverCountdown.Visibility = Visibility.Collapsed;
                    txtFilePath.Visibility = Visibility.Collapsed;
                    DataSave.CurrentProject.Option.ScreenSaverMode = ScreenSaverMode.Disabled;
                }
                else
                {
                    ScreenSaverCountdown.Visibility = Visibility.Visible;
                    txtFilePath.Visibility = Visibility.Visible;
                    DataSave.CurrentProject.Option.ScreenSaverMode = (ScreenSaverMode)parsedint;
                }

                PathGenerator.SaveOptions(DataSave.CurrentProject.Option);
            }
        }
        catch (Exception exception)
        {
            //Console.WriteLine("EXCEPT:" + exception);
        }
    }

    private void ButtonExcel_OnClick(object sender, RoutedEventArgs e)
    {
        ImportExcel(excelType: ExcelImportType.Normal);
        //Console.WriteLine("GOGOG BUTTON EXCEL");
    }
    private void PublishButton_OnClick(object sender, RoutedEventArgs e)
    {
        MenuPublish(sender,e);
    }

    private void PimDataList_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        try
        {
            
            var listView = sender as ListView;
            var listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
            if (listViewItem == null) return;
            
            var item = (PimJson)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
            if (item == null) return;
            item.is_confidential = !item.is_confidential;
            PimDataList.Items.Refresh();
            SolutionsList.Items.Refresh();

            //Console.WriteLine(item.is_confidential);

        }
        catch (Exception exception)
        {
            //Console.WriteLine($"ex:{exception}");
        }
  

        //Console.WriteLine("toggle confidential");
    }
}