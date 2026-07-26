using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp
{
     class ProductNotFoundException :Exception
    {
        public ProductNotFoundException(int id)
            : base($"Không tìm thấy sản phẩm với Id = {id}")
        {
            // base(...) = gọi constructor của class cha (Exception)
            // truyền message vào để ex.Message có giá trị
        }
    }
}
