using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Shop.Common.DTOs.ProductDTO
{
  public class ProductListDTO : ProductCreateUpdateDTO
  {

    public int ProductId { get; set; }

    public string CategoryName { get; set; } = null!;

  }
}
