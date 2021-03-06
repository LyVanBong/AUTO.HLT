namespace AUTOHLT.WEB.API.Models.Version1
{
    public class ProductTypeModel
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int EndDate { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// số lương dịch vụ gói 300 like
        /// </summary>
        public int Number { get; set; }
    }
}