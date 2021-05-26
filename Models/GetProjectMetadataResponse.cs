using System;
using System.Collections.Generic;

namespace OpenRefine.Net.Models
{
    public class GetProjectsMetadataResponse : BaseResponse
    {
        public IReadOnlyDictionary<string, ProjectMetadata> Projects { get; set; }
    }

    public class ProjectMetadata
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Name { get; set; }
        public string[] Tags { get; set; }
        public string Creator { get; set; }
        public string Contributors { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int RowCount { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string License { get; set; }
        public string Homepage { get; set; }
        public string Image { get; set; }
        public IReadOnlyCollection<ImportOptionMetadata> ImportOptionMetadata { get; set; }
        public IReadOnlyDictionary<string, object> CustomMetadata { get; set; }
    }

    public class ImportOptionMetadata
    {
        public bool IncludeFileSources { get; set; }
        public string Encoding { get; set; }
        public int IgnoreLines { get; set; }
        public int HeaderLines { get; set; }
        public int SkipDataLines { get; set; }
        public bool StoreBlankRows { get; set; }
        public bool StoreBlankCellsAsNulls { get; set; }
        public string Separator { get; set; }
        public bool GuessCellValueTypes { get; set; }
        public bool ProcessQuotes { get; set; }
        public string QuoteCharacter { get; set; }
        public bool TrimStrings { get; set; }
        public string ProjectName { get; set; }
        public string FileSource { get; set; }
        public string[] RecordPath { get; set; }
        public string[] ProjectTags { get; set; }
    }
}
