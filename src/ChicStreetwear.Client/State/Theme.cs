using Fluxor;

namespace ChicStreetwear.Client.State;
public sealed record ThemeState(bool IsDarkMode);

public sealed record ThemeSetModeAction(bool isDarkMode);

public static class ThemeReducers
{
    [ReducerMethod]
    public static AppState OnThemeSetMode(AppState state, ThemeSetModeAction action)
    {
        return state with
        {
            Theme = state.Theme with
            {
                IsDarkMode = action.isDarkMode
            }
        };
    }
}