using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Models;
using DataLayer.Repository;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Estore.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Estore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly PRN211_DB_ASMContext _context;
        IOrderRepository _orderRepository;
        IMemberRepository _memberRepository;
        IOrderDetailRepository _orderDetailRepository;
        IProductRepository _productRepository;
        IMapper mapper;
        public OrdersController(PRN211_DB_ASMContext context, IOrderRepository order, IMemberRepository member
            ,IProductRepository productRepository,IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _memberRepository = member;
            _orderRepository = order;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _context = context;
            this.mapper = mapper;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var pRN211_DB_ASMContext = _orderRepository.GetAll(x => x.Status == true);
            return View(await pRN211_DB_ASMContext);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.Get(x=>x.OrderId == id); 
            if (order == null)
            {
                return NotFound();
            }
            var orderDetails = _orderDetailRepository.GetAll(x => x.OrderId == id).Result;
            ViewData["details"] = orderDetails;

            return View(order);
        }
        //Get:ORders/CreateCart
        public async Task<IActionResult> CreateCart()
        {
            return View(await _productRepository.GetAll(x => x.Status == true));
        }
        
        //Get:Orders/AddToCart/?
        public IActionResult AddToCart(int ProductId)
        {
            //Account user = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("USER"));
            Dictionary<int, ProductDTO> cart = SessionExtensions.
                    GetObjectFromJson<Dictionary<int, ProductDTO>>(HttpContext.Session, "Cart");
            if (cart == null) cart = new Dictionary<int, ProductDTO>();
            if (!cart.ContainsKey(ProductId))
            {
                Product product = _productRepository.GetById(x => x.ProductId == ProductId).Result;
                ProductDTO productDTO = mapper.Map<ProductDTO>(product);
                productDTO.UnitsInStock = 1;
                cart.Add(ProductId, productDTO);
            }
            else
            {
                ProductDTO product = cart[ProductId];
                product.UnitsInStock++;
            }
            HttpContext.Session.SetString("Cart",JsonConvert.SerializeObject(cart));
            return RedirectToAction("CreateCart");
        }
        //GET:Orders/ChangeCart
        public IActionResult ChangeCart(int ProductId,int quantity)
        {
            Dictionary<int, ProductDTO> cart = SessionExtensions.
                    GetObjectFromJson<Dictionary<int, ProductDTO>>(HttpContext.Session, "Cart");
            ProductDTO product = cart[ProductId];
            product.UnitsInStock = quantity;
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart)); 
            return RedirectToAction("Create");
        }
        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_memberRepository.GetAll(x=> true).Result, "MemberId", "Email");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        {
            Dictionary<int, ProductDTO> cart = SessionExtensions.
                    GetObjectFromJson<Dictionary<int, ProductDTO>>(HttpContext.Session, "Cart");
            foreach (int productId in cart.Keys)
            {
                ProductDTO product = cart[productId];
                Product p = _productRepository.GetById(x => x.ProductId == productId).Result;
                if (p.UnitsInStock < product.UnitsInStock)
                {
                    ViewData["MemberId"] = new SelectList(_memberRepository.GetAll(x => x.Status == true).Result, "MemberId", "Email", order.MemberId);
                    ViewData["Mess"] = $"Product {p.ProductName} is out of stock, limit {p.UnitsInStock}";
                    return View(order);
                }
            }
            if (ModelState.IsValid)
            {
                order.Status = true;
                await _orderRepository.Add(order);
                List<Order> orders =await _orderRepository.GetAll(x => true);
                int OrderId = orders.Max(x => x.OrderId);
                foreach(int productId in cart.Keys)
                {
                    ProductDTO product = cart[productId];
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        Discount = 0,
                        OrderId = OrderId,
                        ProductId = productId,
                        Quantity = product.UnitsInStock,
                        Status = true,
                        UnitPrice = product.UnitPrice,

                    };
                    await _orderDetailRepository.Add(orderDetail);
                    Product p =await _productRepository.GetById(x => x.ProductId == productId);
                    p.UnitsInStock -= product.UnitsInStock;
                    await _productRepository.Update(p);
                }


                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_memberRepository.GetAll(x => x.Status == true).Result, "MemberId", "Email", order.MemberId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.Get(x=> x.MemberId == id);   
            if (order == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_memberRepository.GetAll(x => x.Status == true).Result, "MemberId", "Email", order.MemberId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Order order1 = await _orderRepository.Get(x=> x.OrderId == order.OrderId);
                    order1.OrderDate = order.OrderDate;
                    order1.MemberId = order.MemberId;   
                    order1.Freight = order.Freight;
                    order1.RequiredDate = order.RequiredDate;
                    order1.ShippedDate = order.ShippedDate;
                    await _orderRepository.Update(order1);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_memberRepository.GetAll(x => x.Status == true).Result, "MemberId", "Email", order.MemberId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.Get(x=> x.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var order = await _context.Orders.FindAsync(id);
            //_context.Orders.Remove(order);
            //await _context.SaveChangesAsync();
            var order = await _orderRepository.Get(x=> x.OrderId == id);
            order.Status = false;
            await _orderRepository.Update(order);
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _orderRepository.Get(x => x.OrderId == id) != null;
        }
    }
}
