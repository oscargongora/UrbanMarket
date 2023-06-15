using Fluxor;

namespace ChicStreetwear.Server.Pages.Store.State.Layout;

public static class LayoutReducers
{
    [ReducerMethod(typeof(SetDefaultHeaderAction))]
    public static LayoutState ReduceSetDefaultHeaderAction(LayoutState state) => state with { DefaultHeader = true };

    [ReducerMethod(typeof(UnsetDefaultHeaderAction))]
    public static LayoutState ReduceUnsetDefaultHeaderAction(LayoutState state) => state with { DefaultHeader = false };

    [ReducerMethod(typeof(ToggleSideNavVisibilityAction))]
    public static LayoutState ReduceToggleSideNavVisibilityAction(LayoutState state)
        => state with { SideNavVisible = !state.SideNavVisible };
}
