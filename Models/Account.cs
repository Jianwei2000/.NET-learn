using System.ComponentModel.DataAnnotations;

namespace MyEmployee.Models;

public class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
  
}