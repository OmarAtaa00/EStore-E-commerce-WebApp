using System;

namespace Core.Specifications
{
    //Class to hold all the spec parameters cuz it is so big it can't fit in anymore :D in GetProducts function of course
    public class ProductSpecParams
    {
        private const int MaxPageSize = 30;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 5; //user can override this value 

        public int PageSize
        {
            get => _pageSize;

            set => _pageSize = (value > MaxPageSize) ? value : MaxPageSize;

        }

        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Sort { get; set; }

        private string _search;

        public string Search
        {
            get => _search;

            set => _search= value.ToLower();
        }


    }

}
