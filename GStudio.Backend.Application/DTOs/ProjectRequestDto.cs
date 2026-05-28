using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GStudio.Backend.Application.DTOs
{
    public class ProjectRequestDto
    {
        [JsonPropertyName("clientName")]      public string ClientName { get; set; } = string.Empty;
        [JsonPropertyName("phone")]           public string Phone { get; set; } = string.Empty;
        [JsonPropertyName("company")]         public string Company { get; set; } = string.Empty;
        [JsonPropertyName("location")]        public string Location { get; set; } = string.Empty;
        [JsonPropertyName("tagline")]         public string Tagline { get; set; } = string.Empty;
        [JsonPropertyName("category")]        public string Category { get; set; } = string.Empty;
        [JsonPropertyName("brandName")]       public string BrandName { get; set; } = string.Empty;
        [JsonPropertyName("nameMeaning")]     public string NameMeaning { get; set; } = string.Empty;
        [JsonPropertyName("logoName")]        public string LogoName { get; set; } = string.Empty;
        [JsonPropertyName("decisionMakers")]  public string DecisionMakers { get; set; } = string.Empty;
        [JsonPropertyName("industryTraits")]  public string IndustryTraits { get; set; } = string.Empty;
        [JsonPropertyName("deadline")]        public string Deadline { get; set; } = string.Empty;
        [JsonPropertyName("vision")]          public string Vision { get; set; } = string.Empty;
        [JsonPropertyName("foundingDate")]    public string FoundingDate { get; set; } = string.Empty;
        [JsonPropertyName("launchDate")]      public string LaunchDate { get; set; } = string.Empty;
        [JsonPropertyName("scope")]           public string Scope { get; set; } = string.Empty;
        [JsonPropertyName("targetAudience")]  public string TargetAudience { get; set; } = string.Empty;
        [JsonPropertyName("competitors")]     public string Competitors { get; set; } = string.Empty;
        [JsonPropertyName("additionalInfo")]  public string AdditionalInfo { get; set; } = string.Empty;
        
        [JsonPropertyName("logoTypes")]       public string[] LogoTypes { get; set; } = Array.Empty<string>();
        [JsonPropertyName("colors")]          public string[] Colors { get; set; } = Array.Empty<string>();
        [JsonPropertyName("customColor")]     public string CustomColor { get; set; } = string.Empty;
        [JsonPropertyName("likedLogosReason")] public string LikedLogosReason { get; set; } = string.Empty;
        [JsonPropertyName("dislikes")]         public string Dislikes { get; set; } = string.Empty;
        [JsonPropertyName("selectedTraits")]  public string[] SelectedTraits { get; set; } = Array.Empty<string>();
        [JsonPropertyName("images")]          public List<string>? Images { get; set; } = new();
    }
}
