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
    New,
    Sale,
    BestSeller,
    Featured,
    TopRated,

    MostPopular,
    MostReviewed,
}
