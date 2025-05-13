namespace Web.Models;

/// <summary>
/// Configuration model to control which filters are displayed in the product views
/// </summary>
public class FilterConfigViewModel
{
    public bool ShowPriceFilter { get; set; } = true;
    public bool ShowCategoryFilter { get; set; } = true;
    public bool ShowStatusFilter { get; set; } = true;

    public FilterConfigViewModel()
    {
    }

    public FilterConfigViewModel(bool showPriceFilter, bool showCategoryFilter, bool showStatusFilter)
    {
        ShowPriceFilter = showPriceFilter;
        ShowCategoryFilter = showCategoryFilter;
        ShowStatusFilter = showStatusFilter;
    }

    /// <summary>
    /// Creates a configuration where all filters are shown
    /// </summary>
    public static FilterConfigViewModel ShowAllFilters() => new(true, true, true);

    /// <summary>
    /// Creates a configuration where only price filter is shown
    /// </summary>
    public static FilterConfigViewModel PriceFilterOnly() => new(true, false, false);

    /// <summary>
    /// Creates a configuration where only category filter is shown
    /// </summary>
    public static FilterConfigViewModel CategoryFilterOnly() => new(false, true, false);
}
