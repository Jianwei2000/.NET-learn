using System.ComponentModel.DataAnnotations;

namespace MyEmployee.Models;

public class Employee
{
    public int Id { get; set; }
    [Required]  //配合表單驗證
    public string Name { get; set; } = string.Empty;    //string.Empty 給預設值
    [Required]
    public string Position { get; set; } = string.Empty;
    [Required]
    public decimal Salary { get; set; } 
    [Required]
    public string Phone { get; set; } = string.Empty;
    [Required]
    public DateTime? HireDate { get; set; }
    //放大頭貼的欄位
    public string? PhotoUrl { get; set; }
}