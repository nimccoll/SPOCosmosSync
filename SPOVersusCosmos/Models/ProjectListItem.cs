//===============================================================================
// Microsoft FastTrack for Azure
// SharePoint Online versus CosmosDB Samples
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
using Newtonsoft.Json;
using System;

namespace SPOVersusCosmos.Models
{
    public class Type
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class PostedBy
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }
        public string Claims { get; set; }
        public string DisplayName { get; set; }
        public object Email { get; set; }
        public object Picture { get; set; }
        public object Department { get; set; }
        public object JobTitle { get; set; }
    }

    public class Status
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class Author
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }
        public string Claims { get; set; }
        public string DisplayName { get; set; }
        public object Email { get; set; }
        public object Picture { get; set; }
        public object Department { get; set; }
        public object JobTitle { get; set; }
    }

    public class Editor
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }
        public string Claims { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public object Department { get; set; }
        public object JobTitle { get; set; }
    }

    public class Thumbnail
    {
        public object Large { get; set; }
        public object Medium { get; set; }
        public object Small { get; set; }
    }

    public class ProjectListItem
    {
        public string id { get; set; }

        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }
        public string ItemInternalId { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Type Type { get; set; }

        [JsonProperty("Type#Id")]
        public int TypeId { get; set; }
        public decimal EffortHours { get; set; }
        public decimal EffortMinutes { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ExpirationDate { get; set; }
        public string Location { get; set; }
        public string Skill1 { get; set; }
        public string Skill2 { get; set; }
        public string Skill3 { get; set; }
        public string Skill4 { get; set; }
        public string Skill5 { get; set; }
        public string Skill6 { get; set; }
        public string Skill7 { get; set; }
        public string Skill8 { get; set; }
        public string Skill9 { get; set; }
        public string Skill10 { get; set; }
        public PostedBy PostedBy { get; set; }

        [JsonProperty("PostedBy#Claims")]
        public string PostedByClaims { get; set; }
        public Status Status { get; set; }

        [JsonProperty("Status#Id")]
        public int StatusId { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
        public Author Author { get; set; }

        [JsonProperty("Author#Claims")]
        public string AuthorClaims { get; set; }
        public Editor Editor { get; set; }

        [JsonProperty("Editor#Claims")]
        public string EditorClaims { get; set; }

        [JsonProperty("{Identifier}")]
        public string Identifier { get; set; }

        [JsonProperty("{IsFolder}")]
        public bool IsFolder { get; set; }

        [JsonProperty("{Thumbnail}")]
        public Thumbnail Thumbnail { get; set; }

        [JsonProperty("{Link}")]
        public string Link { get; set; }

        [JsonProperty("{Name}")]
        public string Name { get; set; }

        [JsonProperty("{FilenameWithExtension}")]
        public string FilenameWithExtension { get; set; }

        [JsonProperty("{Path}")]
        public string Path { get; set; }

        [JsonProperty("{FullPath}")]
        public string FullPath { get; set; }

        [JsonProperty("{HasAttachments}")]
        public bool HasAttachments { get; set; }

        [JsonProperty("{VersionNumber}")]
        public string VersionNumber { get; set; }
    }
}
