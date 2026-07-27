using TransLight.Utility.Enums;

namespace TransLight.DataAccess.ViewModels.Masters
{
    public class ProduceRawMaterialsVM
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }

        public Guid RawMaterialId { get; set; }
        public string? RawMaterialName { get; set; }

        public decimal Qty { get; set; }

        public ProductTypes Type { get; set; }

        public Guid? UnitId { get; set; }
        public string? UnitName { get; set; }

        public bool IsSelected { get; set; }
    }
}
