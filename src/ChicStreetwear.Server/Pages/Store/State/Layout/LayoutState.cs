using Fluxor;

namespace ChicStreetwear.Server.Pages.Store.State.Layout;

[FeatureState]
public record LayoutState(bool DefaultHeader, bool SideNavVisible)
{
    private LayoutState() : this(true, false) { }
}