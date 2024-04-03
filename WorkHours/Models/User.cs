using System;

namespace WorkHours.Models;

public class User
{
    public User()
    {
        CreatedTime = DateTime.Today;
    }

    public string Name { get; set; }

    public DateTime CreatedTime { get; set; }
}