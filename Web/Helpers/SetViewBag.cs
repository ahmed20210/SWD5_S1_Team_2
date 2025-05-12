using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Web.Helpers;

public static class SetViewBag
{
    public static void SetViewBagData(
        Controller controller,
        string searchTerm,
        int? categoryId,
        OrderBy orderBy,
        decimal? minPrice,
        decimal? maxPrice,
        string status,
        int? totalCount,
        int? totalPages,
        int currentPage)
    {
        controller.ViewBag.SearchTerm = searchTerm;
        controller.ViewBag.CategoryId = categoryId;
        controller.ViewBag.OrderBy = orderBy;
        controller.ViewBag.MinPrice = minPrice;
        controller.ViewBag.MaxPrice = maxPrice;
        controller.ViewBag.Status = status;
        controller.ViewBag.TotalCount = totalCount;
        controller.ViewBag.TotalPages = totalPages;
        controller.ViewBag.CurrentPage = currentPage;
    }
}
