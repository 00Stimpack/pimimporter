using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ZFPimImporter;
using ZFPimImporter.DataTypes;
using ZFPimImporter.IO;

namespace ZFPimImporter.Helpes
{
    public static class ModalHelper
    {


        public static void StartModal(string text, string title,Action onSuccess, Action onAbort,string okayButton = "",string abortButton = "",string secondOption = "")
        {

           
            ModalWindow.Text = text;
            ModalWindow.inputActive = false;
            ModalWindow modalWindow = new ModalWindow();
            modalWindow.Title = title;
            
            if (string.IsNullOrWhiteSpace(secondOption))
            {
                modalWindow.btnTryColumn.Visibility = Visibility.Collapsed;
            }
            else
            {
                modalWindow.btnTryColumn.Content = secondOption;

            }
            if (string.IsNullOrWhiteSpace(abortButton))
            {
                modalWindow.btnAbortData.Content = "Cancel";
            }
            else
            {
                modalWindow.btnAbortData.Content = abortButton;

            }
            
            if (string.IsNullOrWhiteSpace(okayButton))
            {
                modalWindow.btnSaveData.Content = "Okay";
            }
            else
            {
                modalWindow.btnSaveData.Content = okayButton;

            }
            
            modalWindow.ShowDialog();
            
            if (ModalWindow.success)
            {
                Console.WriteLine("SUCCES OKAY");
                onSuccess?.Invoke();
            }
            else
            {
                Console.WriteLine("SUCCES ABOIRT INVOKE");
                onAbort?.Invoke();
            }
          
            
        }
    }
}