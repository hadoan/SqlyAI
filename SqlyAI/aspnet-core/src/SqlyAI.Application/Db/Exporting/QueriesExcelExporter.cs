//namespace SqlyAI.Application.Db.Exporting
//{
//    public class QueriesExcelExporter : NpoiExcelExporterBase, IQueriesExcelExporter
//    {

//        private readonly ITimeZoneConverter _timeZoneConverter;
//        private readonly IAbpSession _abpSession;

//        public QueriesExcelExporter(
//            ITimeZoneConverter timeZoneConverter,
//            IAbpSession abpSession,
//            ITempFileCacheManager tempFileCacheManager) :
//    base(tempFileCacheManager)
//        {
//            _timeZoneConverter = timeZoneConverter;
//            _abpSession = abpSession;
//        }

//        public FileDto ExportToFile(List<GetQueryForViewDto> queries)
//        {
//            return CreateExcelPackage(
//                "Queries.xlsx",
//                excelPackage =>
//                {

//                    var sheet = excelPackage.CreateSheet(L("Queries"));

//                    AddHeader(
//                        sheet,
//                        L("Text"),
//                        L("Sql"),
//                        L("MetaData")
//                        );

//                    AddObjects(
//                        sheet, queries,
//                        _ => _.Query.Text,
//                        _ => _.Query.Sql,
//                        _ => _.Query.MetaData
//                        );

//                });
//        }
//    }
//}