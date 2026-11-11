using AvaloniaAuthApp.Models;
using System.Threading.Tasks;

namespace AvaloniaAuthApp.Services;

public interface IRegistrationDialogService
{
    Task<AuthResponse?> ShowAsync();
}
