using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ProductManagement.ASP.Utils
{
    public static class TempDataDictionaryExtension
    {
        public static void Info(this ITempDataDictionary tempData, string message)
        {
            tempData["info"] = message;
        }
        public static string Info(this ITempDataDictionary tempData)
        {
            return tempData["info"] as string;
        }
        public static void Success(this ITempDataDictionary tempData, string message)
        {
            tempData["success"] = message;
        }
        public static string Success(this ITempDataDictionary tempData)
        {
            return tempData["success"] as string;
        }
        public static void Error(this ITempDataDictionary tempData, string message)
        {
            tempData["error"] = message;
        }
        public static string Error(this ITempDataDictionary tempData)
        {
            return tempData["error"] as string;
        }
    }
}
