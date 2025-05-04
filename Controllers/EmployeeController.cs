using Microsoft.AspNetCore.Mvc;
using MyEmployee.Data;
using MyEmployee.Models;

namespace MyEmployee.Controllers;

public class EmployeeController : Controller
{
    private readonly  AppDbContext _context;

    public EmployeeController(AppDbContext context){
        _context = context;
    }

    //主頁讀取功能
    public IActionResult Index(){
        var employees = _context.Employees.ToList();
        return View(employees);
    }
  
    //新增頁面+功能
    public IActionResult Create()
    {
        return View(); 
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee, [FromForm(Name = "Photo")] IFormFile? Photo){
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        foreach (var error in errors)
        {
            Console.WriteLine(error.ErrorMessage);
        }
        if(ModelState.IsValid){

            if (Photo != null && Photo.Length > 0){
                // 建立 uploads 路徑
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder); // 確保資料夾存在

                // 建立唯一檔名
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                 // 寫入檔案
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Photo.CopyToAsync(stream);
                }

                 // 儲存檔名到資料庫欄位
                employee.PhotoUrl = uniqueFileName;
            }

            _context.Employees.Add(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(employee);
    }

    //刪除功能
    [HttpPost]
    public async Task<IActionResult> Delete(int id){
        // 使用 FindAsync 查找指定 ID 的員工資料，這是一個異步操作
        var i = await _context.Employees.FindAsync(id);
        if(i == null){
            return NotFound();
        }
         // 如果找到了員工資料，則將其從資料庫中刪除
        _context.Employees.Remove(i);

        // 異步地保存對資料庫所做的變更
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }

    //編輯頁面+功能
    [HttpGet]
    public IActionResult Edit(int id){
        var employee = _context.Employees.FirstOrDefault(e=>e.Id == id);
        if (employee == null)
        {
            return NotFound(); // 如果找不到該員工，返回 404
        }
        return View(employee); 
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id,Employee employee,[FromForm(Name = "Photo")] IFormFile? Photo){ 

        if (id != employee.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (Photo != null && Photo.Length > 0){
                // 建立 uploads 路徑
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder); // 確保資料夾存在

                // 建立唯一檔名
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                 // 寫入檔案
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Photo.CopyToAsync(stream);
                }

                 // 儲存檔名到資料庫欄位
                employee.PhotoUrl = uniqueFileName;
            }

            _context.Update(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(employee);
    }
}