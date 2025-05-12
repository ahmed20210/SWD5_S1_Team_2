namespace Domain;

public enum OrderBy
{
    NameAsc,
    NameDesc,
    PriceAsc,
    PriceDesc,

    PopularityAsc,
    PopularityDesc,

    NewestAsc,
    NewestDesc
}

public enum FilterBy
{
    Featured,
    NewArrivals,
    BestSelling,
    Discounted,
    MostPopular,
    MostReviewed,
    TopRated
}
