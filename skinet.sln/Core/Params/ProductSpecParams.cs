namespace Core.Params;

public class ProductSpecParams
{
    private string _search;
    private int _pageSize = 6;
    private const int _maxPageSize = 50;

    public int PageIndex { get; set; } = 1;
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public string Sort { get; set; }

    public int PageSize
	{
		get { return _pageSize; }
		set { _pageSize = value > _maxPageSize ? _maxPageSize : value; }
	}

	public string Search
	{
		get { return _search; }
		set { _search = value.ToLower(); }
	}

}
