//using System.Net.Http;
//using System.IO;

//namespace SqlyAI.Application.Db
//{

//    public class GoogleSheetImporterJobAppService : SqlyAIAppService
//    {
//        private readonly IDistributedCache<DynamicTable, Guid> _cacheManager;
//        private readonly ICsvService _csvService;
//        private readonly CsvImporterJobAppService _csvImporterJobAppService;

//        public GoogleSheetImporterJobAppService(IDistributedCache<DynamicTable, Guid> cacheManager, ICsvService csvService, CsvImporterJobAppService csvImporterJobAppService)
//        {
//            _cacheManager = cacheManager;
//            _csvService = csvService;

//            _csvImporterJobAppService = csvImporterJobAppService;
//        }

//        public async Task<DynamicTable> Import(string url)
//        {
//            try
//            {
//                using var client = new HttpClient();
//                // Send a GET request to the Google Sheets URL
//                HttpResponseMessage response = await client.GetAsync(url);

//                // Ensure the request was successful
//                response.EnsureSuccessStatusCode();

//                // Read the content of the response
//                using var stream = await response.Content.ReadAsStreamAsync();

//                var table = await _csvService.ParseCsvFile(stream, ",");
//                table.Delimiter = ",";
//                table.FileName = Path.GetFileNameWithoutExtension(response.Content.Headers.ContentDisposition.FileNameStar).Replace(".", "_").Replace(" - ", "_") + ".csv";
//                table.FileType = "csv";
//                table.FileId = Guid.NewGuid();
//                table.FileSize = response.Content.Headers.ContentDisposition.Size ?? 0;
//                table.SqlTableName = Path.GetFileNameWithoutExtension(table.FileName);
//                table.FileUrl = url;
//                //cache data one hour
//                await _cacheManager
//                   .SetAsync(table.FileId, table, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) });

//                return table;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error occurred: " + ex.Message + " " + ex.ToString());
//                throw new UserFriendlyException("Error occurred: " + ex.Message);
//            }

//        }

//        public Task<int> ImportData(ImportCsvInput input)
//        {
//            //input.SourceType = SourceType.GoogleSheet;
//            //return _csvImporterJobAppService.ImportCsv(input);
//            return Task.FromResult(0);
//        }
//    }

//}