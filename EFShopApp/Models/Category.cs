using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFShopApp.Models
{
    public class Category
    {
       public int Id { get; set; }
       public string Name { get; set; }

        // Mối quan hệ: 1 Danh mục có NHIỀU Sản phẩm
        public List<Product> Products { get; set; } = new();

    }
}
