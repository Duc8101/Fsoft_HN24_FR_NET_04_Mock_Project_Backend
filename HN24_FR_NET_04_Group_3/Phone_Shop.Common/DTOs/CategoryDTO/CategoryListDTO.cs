using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Shop.Common.DTOs.CategoryDTO
{
  public class CategoryListDTO : CategoryCreateUpdateDTO
  {
    public int CategoryId { get; set; }
  }
}
