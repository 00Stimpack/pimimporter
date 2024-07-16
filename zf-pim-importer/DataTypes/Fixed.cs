using System;
using System.IO;
using System.Windows;

namespace ZFPimImporter.DataTypes
{

    public static class FixedStrings
    {
        public static readonly string VERSION = "18.0.9";
        public static readonly string BACKGROUND_PATH = "background";

        public static readonly int[] ApplicationIDs = new int[] { 1, 2, 3, 4, 5, 6 , 7  };
        public static readonly string StandardConfidentialName = "Confidential";
        public static readonly string[] StandardConfidentialTriggers = new String[] { "" };
        
        public static readonly string APPPATH = Directory.GetCurrentDirectory().ToString();
        public static readonly string FOCUS_APPPATH = Directory.GetParent(Directory.GetCurrentDirectory().ToString())?.ToString();
        public static readonly string FOCUS_APPPATH_BACKGROUND = Path.Combine(FOCUS_APPPATH,BACKGROUND_PATH);
        
        public static readonly string TEMPPATH = Path.GetTempPath();
        private static readonly string _appPatth;
        public static  string APPPATHS
        {
            get
            {
                if (string.IsNullOrEmpty(_appPatth))
                {
                    DirectoryInfo[] needToCheck = new[]
                    {
                        new DirectoryInfo(Path.Combine( "PimImporter")),
                        new DirectoryInfo(Path.Combine("app","PimImporter")),
                        new DirectoryInfo(Path.Combine("..\\","PimImporter")),
                        new DirectoryInfo(Path.Combine("..\\","app","PimImporter")),
                    };
                    DirectoryInfo checkDir = new DirectoryInfo(Path.Combine("PimImporter",""));
                }
                
                return _appPatth;
            }
            
        }

        
        public static Guid CURRENTID = new Guid();
        public static bool Temp = false; 
        public static readonly string APPLICATIONSEGMENTPATH = Path.Combine(Directory.GetCurrentDirectory(), "projects","applications");
        public static readonly string PROJECTPATH = Path.Combine(Directory.GetCurrentDirectory(), "projects");
        public static readonly string DATA_TEMPLATE = Path.Combine(APPPATH,"bin", "data");
        public static readonly string PROJECT_TEMPLATE = Path.Combine(APPPATH,"bin", "projects");
        public static readonly string SAVEDPROJECTS = Path.Combine(APPPATH,"saved") ;
        public static readonly string ALLSEGMENTSPATH = Path.Combine(PROJECTPATH,"all.json") ;
       
       
        public static string DATAPATH
        {
            get
            {
                if(Temp)
                {
                    if (!Directory.Exists(TEMPDATAPATH))
                    {
                        TEMPDATAPATH = Path.Combine(TEMPPATH, Guid.NewGuid().ToString(), "data");
                        Directory.CreateDirectory(TEMPDATAPATH);
                    }

                    return TEMPDATAPATH;
                }
                return PUBLISHDATAPATH;
            }
        }
       
        public static string PRODUCTPATH
        {
            get
            {
                if(Temp)
                {
                    if (Directory.Exists(TEMPPRODUCTPATH))
                    {
                       
                        TEMPPRODUCTPATH = Path.Combine(TEMPPATH, Guid.NewGuid().ToString(), "products");
                        Directory.CreateDirectory(TEMPPRODUCTPATH);

                       
                    }  
                    return TEMPPRODUCTPATH;
                }
                return PUBLISHPRODUCTPATH;
            }
        }

      
        public static  string TEMPDATAPATH = Path.Combine(TEMPPATH,Guid.NewGuid().ToString(), "data");
        //public static  string TEMPPRODUCTPATH =Path.Combine(TEMPPATH,Guid.NewGuid().ToString(), "products");
        public static  string TEMPPRODUCTPATH =Path.Combine(TEMPPATH,Guid.NewGuid().ToString(), "data");
      
        public static readonly string PUBLISHDATAPATH = Path.Combine(APPPATH, "data");
        public static readonly string PUBLISHPRODUCTPATH =Path.Combine(APPPATH, "products");
        public static readonly string LOGSDATAPATH = Path.Combine(APPPATH, "data","logs");

        public static readonly string MAINDATAPATH = Path.Combine(PUBLISHDATAPATH, "data.json");

       
        public static readonly string PIMOPTIONSPATH = Path.Combine(PROJECTPATH,".cache");
        public static readonly string STORAGEPATH = Path.Combine(SAVEDPROJECTS,"Standard.zfpimi");
       
        public static readonly string EXCELSTORAGE = Path.Combine(PROJECTPATH,"raw");

        
        public static readonly string ImportExcelTooltip = "Import product data from Excel spreadsheet";
        public static readonly string AddTooltip = "Add selected products to selected segment";
        public static readonly string RemoveTooltip = "Remove selected products from selected segment";
        public static readonly string EmptyTileToolTip = "Add blank tile to segment page";
        public static readonly string PublishTooltip = "Publish current project";
        public static readonly string ClearProjectTooltip = "Delete all products from selected segment";

        public static readonly string ClearTooltip = "Clear all products from the all segments for a fresh start";
        public static readonly string ClearSubheadlinesTooltip = "Clear all subheadlines from the selected segment";
        public static readonly string ClearAllTooltip = "Clear all products, solutions, and subheadlines from the entire project";
        public static readonly string DarkModeTooltip = "Toggle to Dark Mode UI color scheme";
        public static readonly string ConfidentialEditTooltip = "Toggle to Dark Mode UI color scheme";

        public static readonly string AutoPublishTooltip = "Toggle automatic publishing on/off (all changes get published immediately)";

        public static readonly string ActivateMoreTooltip = "Include 'More' pages in product tiles in all segments";
        public static readonly string DeactivateMoreTooltip = "Hide 'More' pages in product tiles in all segments";
        public static readonly string ActivateDetailsTooltip = "Include 'Details' pages in product tiles in all segments";
        public static readonly string DeactivateDetailsTooltip = "Hide 'Details' pages in product tiles in all segments";
        public static readonly string ActivateSpecsTooltip = "Include 'Specs' pages in product tiles in all segments";
        public static readonly string ActivateImagesTooltip = "Include 'Image' pages in product tiles in all segments";
        public static readonly string ActivateVideosTooltip = "Include 'Video' pages in product tiles in all segments";
        public static readonly string ActivatePresentationsTooltip = "Include 'Presentation' pages in product tiles in all segments";
        public static readonly string SkipHomeScreenTooltip = "Skip home screen and start with the segment page currently selected";
        public static readonly string SmartTransportationTooltip = "Use Smart Transportation Focus App template";
        public static readonly string FocusApplicationTooltip = "Use the Focus Application background and icon set for the home screen";
        public static readonly string SdvApplicationTooltip = "Use SDV Focus App template";
        public static readonly string CommercialVehiclesTooltip = "Use Commercial Vehicle Focus App template";
        public static readonly string TruckAndTrailerTooltip = "Truck and Trailer Focus App template";
        public static readonly string ShuttleTooltip = "Use Shuttle Focus App template";
        public static readonly string TechDomainsTooltip = "Use Templates Focus App template";
        public static readonly string AraivTooltip = "Use Araiv Focus App template";
        
        
        public static readonly string RefreshInputTooltip = "Set idle time in seconds until screensaver activates";
        public static readonly string AddRefreshTooltip = "Select screensaver product page";
        public static readonly string ScreenSaverProductOnOffTooltip = "Toggle product screensaver on/off";
        public static readonly string ScreenSaverVideoOnOffTooltip = "Toggle ZF video screensaver on/off";
        public static readonly string ScreenSaverHomeScreenOnOffTooltip = "Toggle the homescreen screensaver on/off";

        public static readonly string MessagePublished = "Current changes successfully published";
        public static readonly string EnableProductsTooltip = "Toggle product / Segment visibility ";
        public static readonly string EnableSegmentsTooltip = "Toggle product / Segment visibility ";


        public static readonly string EnableSecondLanguageTooltip = "Display segment name and subheadlines for the chosen second language";
        public static readonly string SwitchToPresentation = "Toggle DirectToPresentation Mode for selected product";

        public static readonly string SelectedSegmentTooltip = "Currently Selected Segment which will be used when skipping the homescreen";


        public static readonly string ClearExcelDataModalText = "Do you really want clear all Products and with that all Solutions?";
        public static readonly string ClearExcelDataModalTitle = "Clear Products";
        public static readonly string ClearExcelDataModalOkayText = "Delete";
        public static readonly string ClearExcelDataModalCancelText = "Cancel";

        public static readonly string ClearSolutionDataModalText = $"Do you really want clear all Products in SEGMENTNAME";
        public static readonly string ClearSolutionDataModalTitle = "Clear Products";
        public static readonly string ClearSolutionDataModalOkayText = "Delete";
        public static readonly string ClearSolutionDataModalCancelText = "Cancel";


        public static readonly string ConfidentialButton = "Toogle confidential files shown/not shown";
        public static readonly string BackgroundsMenu = "Chose main navigation screen Background";
        public static readonly string EditConfidentialMenu = "Change/define excel field terms to be seen as confidential";
        public static readonly string EditConfidentialMenuInfo = "Enter the tablename and confidential categories which should be used for filtering confidential products.";
        public static readonly string EditConfidentialMenuMoreInfo = "Please enter the name for the confidential table in the excel file (case sensitive, no typos please).";
        public static readonly string EditConfidentialMenuMoreInfoTextBox = "Please enter the possible confidential categories that are filtered as confidential (case sensitive, no typos please).";

        public static readonly string ClearSolutionDataModalMenuClear = "";

    }
}