using Fluxor;

namespace ChicStreetwear.Client.State;

public sealed class AppFeature : Feature<AppState>
{
    public override string GetName() => "App";

    protected override AppState GetInitialState()
    {
        ThemeState theme = new(false);
        return new(theme);
    }
}