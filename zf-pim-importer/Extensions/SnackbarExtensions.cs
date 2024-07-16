using System;
using MaterialDesignThemes.Wpf;


namespace ZFPimImporter.Extensions
{

    
    public static class SnackBarExtensions 
    {
        public static  void SnackBarEnqueue(this SnackbarMessageQueue snackbarMsgQueue,
            string msg, string btnContent = "", Action btnAction = null, double duration = 1) =>
            snackbarMsgQueue.Enqueue(msg,
                btnContent,
                _ => btnAction?.Invoke(), actionArgument:null, 
                promote:false, neverConsiderToBeDuplicate:false, 
                durationOverride:TimeSpan.FromSeconds(duration));

        
    } 
}