using System;

namespace GStudio.Backend.Domain.Entities
{
    public class ProjectRequest
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Contact
        public string ClientName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        // Brand
        public string Tagline { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string NameMeaning { get; set; } = string.Empty;
        public string LogoName { get; set; } = string.Empty;
        public string DecisionMakers { get; set; } = string.Empty;
        public string IndustryTraits { get; set; } = string.Empty;
        public string Deadline { get; set; } = string.Empty;

        // Business
        public string Vision { get; set; } = string.Empty;
        public string FoundingDate { get; set; } = string.Empty;
        public string LaunchDate { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;

        // Audience
        public string TargetAudience { get; set; } = string.Empty;
        public string Competitors { get; set; } = string.Empty;

        // Identity
        public string LogoTypes { get; set; } = string.Empty;   // stored as comma-joined
        public string Colors { get; set; } = string.Empty;
        public string CustomColor { get; set; } = string.Empty;
        public string SelectedTraits { get; set; } = string.Empty;
        public string LikedLogosReason { get; set; } = string.Empty;
        public string Dislikes { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;

        // Files
        public string? Images { get; set; } // Comma separated URLs

        // Status
        public string Status { get; set; } = "New";  // New | Reviewed | InProgress | Done
    }
}
