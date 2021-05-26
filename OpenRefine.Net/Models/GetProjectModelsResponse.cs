using System.Collections.Generic;

namespace OpenRefine.Net.Models
{
    public class GetProjectModelsResponse : BaseResponse
    {
        public ColumnModel ColumnModel { get; set; }
        public RecordModel RecordModel { get; set; }
        public OverlayModels OverlayModels { get; set; }
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Scripting { get; set; }
    }

    public class ColumnModel
    {
        public ICollection<Column> Columns { get; set; }
        public int KeyCellIndex { get; set; }
        public string KeyColumnName { get; set; }
        public ICollection<string> ColumnGroups { get; set; }
    }

    public class Column
    {
        public int CellIndex { get; set; }
        public string OriginalName { get; set; }
        public string Name { get; set; }
    }

    public class RecordModel {
        public bool HasRecords { get; set; }
    }

    public class OverlayModels {
    }
}
