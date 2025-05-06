using Microsoft.AspNetCore.Mvc;
using MyEmployee.Data;
namespace MyEmployee.Controllers;


public class AccountController : Controller
{
    private readonly  AppDbContext _context;

    public AccountController(AppDbContext context){
        _context = context;
    }
    
     // 顯示登入頁面
    public IActionResult Login()
    {
        return View();
    }
    // 處理登入
    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // 驗證帳密（此為簡單版本，實際應用請加密處理）
        var user = _context.Accounts.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user != null)
        {
            // 使用 Session 記錄登入者資訊
            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("Index", "Employee"); // 登入成功導向首頁
        }

        ViewBag.Error = "帳號或密碼錯誤";
        return View();
    }

    //處理登出
    public IActionResult Logout(){
            // 清除所有 Session 資料
        HttpContext.Session.Clear();

        // 導向登入頁
        return RedirectToAction("Login", "Account");
    }
}