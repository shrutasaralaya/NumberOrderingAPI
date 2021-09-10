using NumberOrderingAPI.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NumberOrderingAPI.Services
{
    public class NumberOrderService: INumberOrderService
    {
        private readonly string fileNamePrefix ="result_";

        public NumberOrderService()
        {
        }

        public  List<int> SortUnOrderedList(List<int> unorderedNumbers)
        {
            QuickSort.Sort(unorderedNumbers, 0, unorderedNumbers.Count -1);
            return unorderedNumbers;
        }

        public async Task<bool> StoreListIntoFile(List<int> orderedList)
        {
            TimeSpan time = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)time.TotalSeconds;
            var fileName = fileNamePrefix + secondsSinceEpoch + ".txt"; //Create a new Name for the file .
            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "OrderedFiles");
            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "OrderedFiles", fileName);
            bool isFileSaveSuccessfull;
            try
            {
                // Check if file already exists. If yes, delete it.
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                // Create a new file  
                var csv = String.Join("\n", orderedList);

                await File.WriteAllTextAsync(path, csv);
                isFileSaveSuccessfull = true;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
                throw;
            }
            return isFileSaveSuccessfull;
        }

        public  List<string> GetLatestFileData()
        {
            try
            {
                // get the directory path and find latest file
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "OrderedFiles");
                var lastestFilePath = Directory.GetFiles(directoryPath).Select(x => Convert.ToInt64(x.Substring(directoryPath.Length + 1 + fileNamePrefix.Length, 10))).Max(); //get the latest file by comparing the epoc time in filename
                var latestPath = Path.Combine(directoryPath, fileNamePrefix + lastestFilePath + ".txt");
                var fileContent = File.ReadAllText(latestPath);
                var orderedList = fileContent.Split("\n").ToList();
                return orderedList;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
            

        }

        
    }

    public interface INumberOrderService
    {
        List<int> SortUnOrderedList(List<int> unorderedNumbers);
        Task<bool> StoreListIntoFile(List<int> orderedList);
        List<string> GetLatestFileData();
    }
}
