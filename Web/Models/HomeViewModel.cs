using Business.ViewModels.CategoryViewModels;
using Business.ViewModels.ProductViewModels;

namespace Web.Models;

public class HomeViewModel
{

    public IEnumerable<ProductViewModel> HotDeals { get; set; } = new List<ProductViewModel>();

    public IEnumerable<ProductViewModel> NewArrivals { get; set; } = new List<ProductViewModel>();
    public IEnumerable<ProductViewModel> BestSelling { get; set; } = new List<ProductViewModel>();
    public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

     public HomeViewModel(
        IEnumerable<ProductViewModel> NewArrivals,
        IEnumerable<ProductViewModel> HotDeals,
        IEnumerable<CategoryViewModel> Categories,
        IEnumerable<ProductViewModel>BestSelling 
    )
    {
        this.NewArrivals = NewArrivals;
        this.HotDeals = HotDeals;
        this.Categories = Categories;
        this.BestSelling = BestSelling;
    }




}
