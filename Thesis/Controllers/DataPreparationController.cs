using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Thesis.Controllers
{
    [Route("[controller]")]
    public class DataPreparationController : ControllerBase
    {
        public DataPreparationController(){}

        [HttpGet, Route("processData")]
        public async Task<IActionResult> Rename()
        {
            try
            {
                string sourceLocation = "E:\\thesis\\dataset\\data_recreated_raw";
                string trainLocation = "E:\\thesis\\dataset\\data_80_20\\train";
                string validationLocation = "E:\\thesis\\dataset\\data_80_20\\validation";

                string[] directories = Directory.GetDirectories(sourceLocation);

                foreach (var dir in directories)
                {
                    string[] dir_splits = dir.Split('\\');
                    var _trainLocation = trainLocation + "\\" + dir_splits[dir_splits.Length - 1];
                    var _validationLocation = validationLocation + "\\" + dir_splits[dir_splits.Length - 1];

                    //create folder(s) unless non-existant
                    if (!System.IO.Directory.Exists(_trainLocation))
                        System.IO.Directory.CreateDirectory(_trainLocation);
                    if (!System.IO.Directory.Exists(_validationLocation))
                        System.IO.Directory.CreateDirectory(_validationLocation);

                    string[] files = Directory.GetFiles(dir);
                    var files_count = files.Length - 1;

                    for (int i = 0; i < files.Length; i++)
                    {
                        var sourceFile = files[i];
                        var extension = Path.GetExtension(sourceFile);
                        double normalize = (double)i / files_count;

                        //90% train, 10% validation
                        var filename = (i + 1).ToString() + extension;
                        if (normalize < 0.8)
                        {
                            var destFile = System.IO.Path.Combine(_trainLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                        else
                        {
                            var destFile = System.IO.Path.Combine(_validationLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                    }
                }
                return Ok(new { Success = true, Message = "Data Processing Done Successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = "Something went wrong." });
            }
        }

        [HttpGet, Route("process_for_k_fold")]
        public async Task<IActionResult> Process_for_k_fold()
        {
            try
            {
                string sourceLocation = "E:\\thesis\\dataset\\data_recreated_raw";
                string trainLocation = "E:\\thesis\\dataset\\fold9\\train";
                string validationLocation = "E:\\thesis\\dataset\\fold9\\validation";

                string[] directories = Directory.GetDirectories(sourceLocation);

                foreach (var dir in directories)
                {
                    string[] dir_splits = dir.Split('\\');
                    var _trainLocation = trainLocation + "\\" + dir_splits[dir_splits.Length - 1];
                    var _validationLocation = validationLocation + "\\" + dir_splits[dir_splits.Length - 1];

                    //create folder(s) unless non-existant
                    if (!System.IO.Directory.Exists(_trainLocation))
                        System.IO.Directory.CreateDirectory(_trainLocation);
                    if (!System.IO.Directory.Exists(_validationLocation))
                        System.IO.Directory.CreateDirectory(_validationLocation);

                    string[] files = Directory.GetFiles(dir);
                    var files_count = files.Length - 1;

                    for (int i = 0; i < files.Length; i++)
                    {
                        var sourceFile = files[i];
                        var extension = Path.GetExtension(sourceFile);
                        var file_splits = sourceFile.Split('\\');
                        string filename = file_splits[file_splits.Length - 1];
                        string filename_ = filename.Split('.')[0];
                        var fileNum = Convert.ToInt32(filename_);
                        if (fileNum > 176 && fileNum<=198)
                        {
                            var destFile = System.IO.Path.Combine(_validationLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                        else 
                        {
                            filename = filename + extension;
                            var destFile = System.IO.Path.Combine(_trainLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                    }
                }
                return Ok(new { Success = true, Message = "Data Processing Done Successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = "Something went wrong." });
            }
        }
        [HttpGet, Route("processTestData")]
        public async Task<IActionResult> RenameTestData()
        {
            try
            {
                string sourceLocation = "C:\\Users\\Dell\\thesis\\data\\test_raw";
                string testLocation = "C:\\Users\\Dell\\thesis\\data\\test";
                string[] directories = Directory.GetDirectories(sourceLocation);

                foreach (var dir in directories)
                {
                    string[] dir_splits = dir.Split('\\');
                    var _testLocation = testLocation + "\\" + dir_splits[dir_splits.Length - 1];

                    //create folder(s) unless non-existant
                    if (!System.IO.Directory.Exists(_testLocation))
                        System.IO.Directory.CreateDirectory(_testLocation);

                    string[] files = Directory.GetFiles(dir);
                    var files_count = files.Length - 1;

                    for (int i = 0; i < files.Length; i++)
                    {
                        var sourceFile = files[i];
                        var extension = Path.GetExtension(sourceFile);
                        var filename = (i + 1).ToString() + extension;
                        var destFile = System.IO.Path.Combine(_testLocation, filename);
                        System.IO.File.Copy(sourceFile, destFile);
                    }
                }
                return Ok(new { Success = true, Message = "Data Processing Done Successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = "Something went wrong." });
            }
        }
        [HttpGet, Route("recreateRawData")]
        public async Task<IActionResult> RecreateRawData()
        {
            try
            {
                string sourceTrainLocation = "E:\\thesis\\dataset\\data\\train";
                string sourceValLocation = "E:\\thesis\\dataset\\data\\validation";
                string destLocation = "E:\\thesis\\dataset\\data_recreated_raw";
                string[] directories = Directory.GetDirectories(sourceTrainLocation);

                foreach (var dir in directories)
                {
                    string[] dir_splits = dir.Split('\\');
                    var _destLocation = destLocation + "\\" + dir_splits[dir_splits.Length - 1];

                    //create folder(s) unless non-existant
                    if (!System.IO.Directory.Exists(_destLocation))
                        System.IO.Directory.CreateDirectory(_destLocation);
                    string[] files = Directory.GetFiles(dir);
                    for (int i = 0; i < files.Length; i++)
                    {
                        var sourceFile = files[i];
                        //var extension = Path.GetExtension(sourceFile);

                        string[] dir_file_splits_ = sourceFile.Split('\\');
                        var filename = dir_file_splits_[dir_file_splits_.Length - 1];

                        var _destLocation_ = System.IO.Path.Combine(_destLocation, filename);
                        System.IO.File.Copy(sourceFile, _destLocation_);
                    }
                }
                return Ok(new { Success = true, Message = "Data Processing Done Successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = "Something went wrong." });
            }
        }

        [HttpGet, Route("insertToWordFile")]
        public async Task<IActionResult> InsertToWordFile()
        {
            try
            {
                string sourceLocation = "C:\\Users\\Dell\\thesis\\data\\test";
                string destLocation = "C:\\Users\\Dell\\thesis\\word_files2";
                string[] directories = Directory.GetDirectories(sourceLocation);

                foreach (var dir in directories)
                {
                    string[] dir_splits = dir.Split('\\');
                    var _destLocation = destLocation + "\\" + dir_splits[dir_splits.Length - 1];

                    //create folder(s) unless non-existant
                    if (!System.IO.Directory.Exists(_destLocation))
                        System.IO.Directory.CreateDirectory(_destLocation);

                    string[] files = Directory.GetFiles(dir);
                    var files_count = files.Length - 1;

                    for (int i = 0; i < files.Length; i++)
                    {
                        var sourceFile = files[i];
                        
                    }
                }
                return Ok(new { Success = true, Message = "Data Insertion to word file Done Successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = "Something went wrong." });
            }
        }
        [HttpGet, Route("consonants")]
        public async Task<IActionResult> PrepareConsonants()
        {
            try
            {
                string sourceLocation = "E:\\thesis\\dataset\\consonants";
                string trainLocation = "E:\\thesis\\dataset\\data\\train";
                string testLocation = "E:\\thesis\\dataset\\data\\test";
                string validationLocation = "E:\\thesis\\dataset\\data\\validation";

                string[] directories = Directory.GetDirectories(sourceLocation);

                foreach (var dir in directories)
                {
                    //create a renamed destination folder
                    string[] dir_splits = dir.Split('\\');
                    var _trainLocation = trainLocation + "\\c" + dir_splits[dir_splits.Length - 1];
                    var _testLocation = testLocation + "\\c" + dir_splits[dir_splits.Length - 1];
                    var _validationLocation = validationLocation + "\\c" + dir_splits[dir_splits.Length - 1];

                    //create folder(s) unless non-existant
                    if (!System.IO.Directory.Exists(_trainLocation))
                        System.IO.Directory.CreateDirectory(_trainLocation);
                    if (!System.IO.Directory.Exists(_testLocation))
                        System.IO.Directory.CreateDirectory(_testLocation);
                    if (!System.IO.Directory.Exists(_validationLocation))
                        System.IO.Directory.CreateDirectory(_validationLocation);

                    string[] files = Directory.GetFiles(dir);
                    var files_count = files.Length - 1;

                    for (int i = 0; i < files.Length; i++)
                    {
                        var sourceFile = files[i];
                        double normalize = (double) i / files_count;
                        //80% train, 10% test, 10% validation
                        if (normalize <= 0.8)
                        {
                            var filename = System.IO.Path.GetFileName(sourceFile);
                            var destFile = System.IO.Path.Combine(_trainLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                        else if (normalize > 0.8 && normalize <= 0.9)
                        {
                            var filename = System.IO.Path.GetFileName(sourceFile);
                            var destFile = System.IO.Path.Combine(_testLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                        else if (normalize > 0.9)
                        {
                            var filename = System.IO.Path.GetFileName(sourceFile);
                            var destFile = System.IO.Path.Combine(_validationLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                    }
                }
                return Ok(new { Success = true, Message = "Consonants transferred successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = "Consonants transfer Failed." });
            }
        }

        [HttpGet, Route("vowels")]
        public async Task<IActionResult> PrepareVowels()
        {
            try
            {
                string sourceLocation = "E:\\thesis\\dataset\\vowels";
                string trainLocation = "E:\\thesis\\dataset\\data\\train";
                string testLocation = "E:\\thesis\\dataset\\data\\test";
                string validationLocation = "E:\\thesis\\dataset\\data\\validation";

                string[] directories = Directory.GetDirectories(sourceLocation);

                foreach (var dir in directories)
                {
                    //create a renamed destination folder
                    string[] dir_splits = dir.Split('\\');
                    var _trainLocation = trainLocation + "\\v" + dir_splits[dir_splits.Length - 1];
                    var _testLocation = testLocation + "\\v" + dir_splits[dir_splits.Length - 1];
                    var _validationLocation = validationLocation + "\\v" + dir_splits[dir_splits.Length - 1];

                    //create folder(s) unless non-existant
                    if (!System.IO.Directory.Exists(_trainLocation))
                        System.IO.Directory.CreateDirectory(_trainLocation);
                    if (!System.IO.Directory.Exists(_testLocation))
                        System.IO.Directory.CreateDirectory(_testLocation);
                    if (!System.IO.Directory.Exists(_validationLocation))
                        System.IO.Directory.CreateDirectory(_validationLocation);

                    string[] files = Directory.GetFiles(dir);
                    var files_count = files.Length - 1;

                    for (int i = 0; i < files.Length; i++)
                    {
                        var sourceFile = files[i];
                        double normalize = (double)i / files_count;
                        //80% train, 10% test, 10% validation
                        if (normalize <= 0.8)
                        {
                            var filename = System.IO.Path.GetFileName(sourceFile);
                            var destFile = System.IO.Path.Combine(_trainLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                        else if (normalize > 0.8 && normalize <= 0.9)
                        {
                            var filename = System.IO.Path.GetFileName(sourceFile);
                            var destFile = System.IO.Path.Combine(_testLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                        else if (normalize > 0.9)
                        {
                            var filename = System.IO.Path.GetFileName(sourceFile);
                            var destFile = System.IO.Path.Combine(_validationLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                    }
                }
                return Ok(new { Success = true, Message = "Vowels transferred successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = "Vowels transfer Failed." });
            }
        }

        [HttpGet, Route("numerals")]
        public async Task<IActionResult> PrepareNumerals()
        {
            try
            {
                string sourceLocation = "E:\\thesis\\dataset\\numerals";
                string trainLocation = "E:\\thesis\\dataset\\data\\train";
                string testLocation = "E:\\thesis\\dataset\\data\\test";
                string validationLocation = "E:\\thesis\\dataset\\data\\validation";

                string[] directories = Directory.GetDirectories(sourceLocation);

                foreach (var dir in directories)
                {
                    //create a renamed destination folder
                    string[] dir_splits = dir.Split('\\');
                    var _trainLocation = trainLocation + "\\n" + dir_splits[dir_splits.Length - 1];
                    var _testLocation = testLocation + "\\n" + dir_splits[dir_splits.Length - 1];
                    var _validationLocation = validationLocation + "\\n" + dir_splits[dir_splits.Length - 1];

                    //create folder(s) unless non-existant
                    if (!System.IO.Directory.Exists(_trainLocation))
                        System.IO.Directory.CreateDirectory(_trainLocation);
                    if (!System.IO.Directory.Exists(_testLocation))
                        System.IO.Directory.CreateDirectory(_testLocation);
                    if (!System.IO.Directory.Exists(_validationLocation))
                        System.IO.Directory.CreateDirectory(_validationLocation);

                    string[] files = Directory.GetFiles(dir);
                    var files_count = files.Length - 1;

                    for (int i = 0; i < files.Length; i++)
                    {
                        var sourceFile = files[i];
                        double normalize = (double)i / files_count;
                        //80% train, 10% test, 10% validation
                        if (normalize <= 0.8)
                        {
                            var filename = System.IO.Path.GetFileName(sourceFile);
                            var destFile = System.IO.Path.Combine(_trainLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                        else if (normalize > 0.8 && normalize <= 0.9)
                        {
                            var filename = System.IO.Path.GetFileName(sourceFile);
                            var destFile = System.IO.Path.Combine(_testLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                        else if (normalize > 0.9)
                        {
                            var filename = System.IO.Path.GetFileName(sourceFile);
                            var destFile = System.IO.Path.Combine(_validationLocation, filename);
                            System.IO.File.Copy(sourceFile, destFile);
                        }
                    }
                }
                return Ok(new { Success = true, Message = "Numerals transferred successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = "Numerals transfer Failed." });
            }
        }

    }
    
}
