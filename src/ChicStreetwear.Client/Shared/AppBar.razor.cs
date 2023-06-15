using Blazored.LocalStorage;
using ChicStreetwear.Client.State;
using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ChicStreetwear.Client.Shared;

public sealed partial class AppBar
{
    [Inject]
    private IState<AppState> State { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }
    private string ToggleThemeButtonColor => State.Value.Theme.IsDarkMode ? Colors.Amber.Darken1 : Colors.Grey.Lighten5; 

    private async Task ToggleThemeMode(bool toggled)
    {
        await LocalStorage.SetItemAsync(nameof(AppState.Theme.IsDarkMode), toggled);
        Dispatcher.Dispatch(new ThemeSetModeAction(toggled));
    }

}
