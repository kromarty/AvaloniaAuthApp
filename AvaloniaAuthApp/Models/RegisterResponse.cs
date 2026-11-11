using System;
using System.Text.Json.Serialization;

namespace AvaloniaAuthApp.Models;

public class RegisterResponse
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("onboarding_checklist")]
    public OnboardingChecklist? OnboardingChecklist { get; set; }
}

public class OnboardingChecklist
{
    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; set; }

    [JsonPropertyName("profile_completed")]
    public bool ProfileCompleted { get; set; }

    [JsonPropertyName("mfa_enabled")]
    public bool MfaEnabled { get; set; }
}
