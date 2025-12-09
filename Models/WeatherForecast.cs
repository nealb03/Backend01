<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;

namespace MyWebApi.Models;

public partial class WeatherForecast
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string Summary { get; set; } = null!;
=======
using System;
using System.ComponentModel.DataAnnotations;

namespace MyWebApi.Models
{
    public class WeatherForecast
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
>>>>>>> 36c0305 (Save local changes before switching branch)
}
