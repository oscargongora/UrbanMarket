using Blazored.LocalStorage;
using ChicStreetwear.Client.State;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace ChicStreetwear.Client.Shared;

public partial class ThemeProvider
{
    [Inject]
    private IState<AppState> State { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }

    private async Task SetThemeMode(bool darkMode)
    {
        await LocalStorage.SetItemAsync(nameof(AppState.Theme.IsDarkMode), darkMode);
        Dispatcher.Dispatch(new ThemeSetModeAction(darkMode));
        StateHasChanged();
    }

    private async Task<bool?> GetModeFromLocalStorage()
    {
        if (await LocalStorage.ContainKeyAsync(nameof(AppState.Theme.IsDarkMode)))
        {
            return await LocalStorage.GetItemAsync<bool>(nameof(AppState.Theme.IsDarkMode));
        }
        return null;
    }
}
