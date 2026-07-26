using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFShopApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        // Khóa ngoại
        public int CategoryId { get; set; }

        // Navigation Property: Mối quan hệ, 1 Sản phẩm thuộc 1 Danh mục
        public Category Category { get; set; }
    }
}
