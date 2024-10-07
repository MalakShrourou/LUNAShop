using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Enums
{
    public enum Permission
    {
        ViewProducts = 1,        
        AddProduct,          
        EditProduct,         
        DeleteProduct,       

        ViewCategories,
        AddCategory,   
        EditCategory,  
        DeleteCategory,

        ViewBrands,    
        AddBrand,      
        EditBrand,     
        DeleteBrand,   

        ViewOrders,    
        EditOrder,     
        CancelOrder,   

        ViewUsers,     
        AddUser,       
        EditUser,      
        DeleteUser,    

        ViewReviews,   
        AddReview,     
        DeleteReview,  

        ViewRoles,     
        AddRole,       
        EditRole,      
        DeleteRole,    
    }

}
