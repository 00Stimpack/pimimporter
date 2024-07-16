using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ZFPimImporter.DataTypes;
using ZFPimImporter.IO;

namespace ZFPimImporter
{
    public partial class ModalWindow : Window
    {
        public static bool success = false;
        public static bool inputActive = false;

        public static string Segment = "";
        public static string Text = "";
        public static string TextInput = "";
        
        public static string DataPath = "";
        
        
        
        public  List<string> TextGroupsInput = new  List<string>{};

        public static List<PimRawData> PimJson = new List<PimRawData>();

        public ModalWindow()
        {
            ModalWindow.success = false;
            InitializeComponent();
            txtSomeBox.Text = Text;

            if (inputActive)
            {
                MinHeight = 500;
                Height = 500;
                txtTableInput.TextChanged -= TxtSomeBox_TextChanged;
                txtTableInput.Text = DataSave.Option.TableName;
                txtTableInput.TextChanged += TxtSomeBox_TextChanged;
                txtTableInput.Focusable = true;
                txtTableInput.Focus();
                
                //txtConfidentialInput.TextChanged -= TxtGroupdSomeBox_TextChanged;
                //txtConfidentialInput.Text = string.Join("\n", DataSave.Option.ConfidentialGroups);
                //txtConfidentialInput.TextChanged += TxtGroupdSomeBox_TextChanged;
     
                
                TextInput = DataSave.Option.TableName;
                StringBuilder group = new StringBuilder();
                foreach (var set in DataSave.Option.ConfidentialGroups)
                {
                    if(TextGroupsInput.Contains(set))continue;
                    TextGroupsInput.Add(set);
                    group.AppendLine(set);
                }
                txtConfidentialInput.Focusable = true;
                txtConfidentialInput.Focus();
                txtConfidentialInput.Visibility = Visibility.Visible;
                txtConfidentialInput.Text = group.ToString();

                
                TextGroupsInput = DataSave.Option.ConfidentialGroups.ToList();

                txtTableInput.Visibility = Visibility.Visible;
                txtInfo.Visibility = Visibility.Visible;
                txtInfoConfidential.Visibility = Visibility.Visible;
                txtInfoConfidential.Text = FixedStrings.EditConfidentialMenuMoreInfoTextBox;
                txtInfo.Text = FixedStrings.EditConfidentialMenuMoreInfo;

                btnTryColumn.Content = "Save Changes";
                btnTryColumn.Visibility = Visibility.Visible;
                btnSaveData.Visibility = Visibility.Collapsed;
                btnAbortData.Visibility = Visibility.Visible;

            }
            else
            {
                MinHeight = 200;
                Height = 200;
                btnTryColumn.Visibility = Visibility.Collapsed;
                txtTableInput.Visibility = Visibility.Collapsed;
                txtInfo.Visibility= Visibility.Collapsed;
                txtInfoConfidential.Visibility = Visibility.Collapsed;
                txtConfidentialInput.Visibility = Visibility.Collapsed;

                btnSaveData.Visibility  = Visibility.Visible;
                btnAbortData.Visibility = Visibility.Visible;

            }
    
        }
        
        private void TxtSomeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            // Reflect changes to static Text variable
            if(string.IsNullOrWhiteSpace(txtTableInput.Text))return;
            TextInput = txtTableInput.Text;
        }
        
        private void TxtGroupdSomeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveGroup();
        }

        public List<string> SaveGroup( )
        {
            string temp ="\n" + txtConfidentialInput.Text;
            List<string> textGroupsInput = new  List<string>{};

            var splittet = temp.Split('\n');
            List<string> testing = new List<string>();
            foreach (var test in splittet)
            {
                string current = test.Trim();
                if(string.IsNullOrWhiteSpace(current))continue;
                if(testing.Contains(current))continue;
                testing.Add(current);
            }
            Console.WriteLine($"testing Count:{testing.Count}");
            if (!testing.Any())
            {
                return textGroupsInput;
            }

            foreach (var test in testing)
            {
                if(string.IsNullOrWhiteSpace(test))continue;
                if(textGroupsInput.Contains(test))continue;
                textGroupsInput.Add(test);
            }
            Console.WriteLine($"testing Count:{textGroupsInput.Count}");
            return textGroupsInput;

        }
        public ModalWindow(Tabs from)
        {
           
        }

       private void btnTryColumn_Click(object sender, RoutedEventArgs e)
       {
    try
    {
        if (txtConfidentialInput.Visibility == Visibility.Visible)
        {
            var groups =  SaveGroup();
            DataSave.Option.ConfidentialGroups = groups.ToArray();
            if(!string.IsNullOrWhiteSpace(txtTableInput.Text)) 
                DataSave.Option.TableName = txtTableInput.Text;
            Serializer.WriteBin(FixedStrings.PIMOPTIONSPATH,DataSave.Option);
            
        }
        
        success = true;
        this.Close();

        return;

        
        Console.WriteLine("try to check for!!");
        return;
        string tableName = TextInput;

        if (string.IsNullOrEmpty(tableName))
        {
            txtInfo.Text = "Table name cannot be empty.";
            return;
        }

        btnSaveData.Content = "Save";

        if (File.Exists(DataPath))
        {
            //InfoStack2.Visibility = Visibility.Visible;
            //InfoStack.Children.Clear();
            PimJson.Clear();
            PimJson = ExcelReader.LoadPimDataFromExcel(DataPath);
            
            if (PimJson == null)
            {
                txtInfo.Text = "Data could not be loaded.";
                return;
            }

            HashSet<string> uniques = new HashSet<string>();
            Dictionary<string, int> dataDict = new Dictionary<string, int>();
            
            foreach (var raw in PimJson)
            {
                /*Console.WriteLine("raw.Confidential:"+raw.Confidential+ " TextInput:"+TextInput);

                if (string.IsNullOrEmpty(raw?.Confidential?.Trim()))
                {
                    continue;
                }

                string testString = $"{raw.Confidential}{raw.ID}";
                if(uniques.Contains(testString))continue;
                uniques.Add(testString);
                if (dataDict.TryGetValue(raw.Confidential, out var counter))
                {
                    dataDict[raw.Confidential] += 1;
                }
                else
                {
                    dataDict[raw.Confidential] = 1;
                }*/
            }

            string foundtxt = $"Found: {dataDict.Count}\n will create the following Projects Files:\n \n";
            int maxValueLength = dataDict.Values.Max(v => v.ToString().Length);

            foreach (var dict in dataDict)
            {
                string temp = dict.Value.ToString().PadRight(maxValueLength, ' ');
                foundtxt += $"{temp}  {dict.Key}\n";
            }





            if (dataDict.Count > 0)
            {
                this.Width = 600;
                btnSaveData.Visibility = Visibility.Visible;
                DataSave.Option.TableName = tableName;
                
                //btnSaveData.Content = "Create";
            }
            else
            {
                this.Width = 400;
                btnSaveData.Visibility = Visibility.Collapsed;

            }
            txtInfo.Text = foundtxt;

            Console.WriteLine($"{foundtxt}");
            DataSave.Option.TableName = tableName;
        }
        else
        {
            txtInfo.Text = $"Data file does not exist. DataPath:{DataPath}";
        }
    }
    catch (Exception ex)
    {
        txtInfo.Text = "An error occurred.";
        // Log the exception for debugging purposes
        // Consider using a more specific catch block for different exception types
    }
}


        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button) sender;
            if ((string) button.Content == "Ok" || (string) button.Content == "Save Changes" || (string) button.Content == "Save"  || (string) button.Content == "Publish" || (string) button.Content == "Okay" || (string) button.Content == "Delete"  || (string) button.Content == "Yes")
                success = true;
            else
            {
                success = false;
            }

            if (success && txtConfidentialInput.Visibility == Visibility.Visible)
            {
                var groups =  SaveGroup();
                DataSave.Option.ConfidentialGroups = groups.ToArray();
                if(!string.IsNullOrWhiteSpace(txtTableInput.Text)) 
                    DataSave.Option.TableName = txtTableInput.Text;
                Console.WriteLine($"DataSave.Option.ConfidentialGroups:{DataSave.Option.ConfidentialGroups.Length}");
                Serializer.WriteBin(FixedStrings.PIMOPTIONSPATH,DataSave.Option);
                
            
            }
            
            //DataPath = "";
            this.Close();
        }

        private void SegementList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("SEGMENT CHANGED:");
        }
    }
}