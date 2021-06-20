using Models.EntityFramework;
using ShopQuanAo.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopQuanAo.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        private ShopQuanAoDBContext context;
        // GET: Cart
        public CartController()
        {
            this.context = new ShopQuanAoDBContext();
        }
        public ActionResult Index()
        {
            var listCartItem = new List<CartItem>();
            var cart = Session[CartSession];
            if(cart != null)
            {
                listCartItem = (List<CartItem>)cart;
            }
            return View(listCartItem);
        }

        public ActionResult InsertCart(string id , FormCollection formCollection)
        {
            var quantity = int.Parse(formCollection["quantity"]);
            var idColor = formCollection["idColor"];
            var idSize = formCollection["idSize"];
            string sql = String.Format("SELECT * FROM SAN_PHAM_CHI_TIET WHERE ID_COLOR = {0} AND ID_SIZE = {1} AND MA_SP='{2}' ;", idColor , idSize , id);
            var productDetails = this.context.SAN_PHAM_CHI_TIET.SqlQuery(sql).FirstOrDefault();
            var cartItem = new CartItem();
            if (productDetails != null)
            {
                var product = this.context.SAN_PHAM.Find(productDetails.MA_SP);
                cartItem.productID = product.MA_SP;
                cartItem.quantity = quantity;
                cartItem.GIA_BAN = product.GIA_BAN;
                cartItem.SLUG = product.SLUG;
                cartItem.Color = this.context.COLORs.Find(productDetails.ID_COLOR).TEN_MAU;
                cartItem.Size = this.context.SIZEs.Find(productDetails.ID_SIZE).TEN_SIZE;   
                cartItem.TEN_SP = product.TEN_SP;
                cartItem.Image = product.LINK_ANH_CHINH;
                cartItem.DetailsProductID = productDetails.ID;
                int soluongcon = ( productDetails.SO_LUONG ?? 0);
                if(soluongcon > 1)
                {
                    this.AddCart(cartItem, soluongcon);
                }
                else
                {
                    ViewBag.ErrorCart = "Lỗi không đủ số lượng để bán !!!";
                }
            }
            return RedirectToRoute("giohang", new { action ="Index"  });
        }

        private void AddCart(CartItem cartItem , int soluong)
        {
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.DetailsProductID == cartItem.DetailsProductID))
                {
                    var item = list.Where(c => c.DetailsProductID == cartItem.DetailsProductID).FirstOrDefault();
                    int sl_mua =  item.quantity + cartItem.quantity;
                    if(sl_mua <= soluong)
                    {
                        item.quantity = sl_mua;
                    }
                    else
                    {
                        ViewBag.ErrorCart = "Lỗi không đủ số lượng để bán !!!";
                        return;
                    }
                }
                else
                {
                    list.Add(cartItem);
                }

                Session[CartSession] = list;
            }
            else
            {
                var listItem = new List<CartItem>();
                listItem.Add(cartItem);
                Session[CartSession] = listItem;
            }
        }

        public ActionResult DeleteCart()
        {
            var cart = Session[CartSession];
            if(cart != null)
            {
                Session[CartSession] = null;
            }
            return RedirectToRoute("giohang", new { action = "Index" });
        }

        public ActionResult ThanhToan()
        {

            

            return RedirectToRoute("giohang", new { action = "Index" });
        }

    }
}