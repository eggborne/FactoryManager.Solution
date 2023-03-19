

using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models
{
public class EngineerAvailableDay
{
    public int EngineerAvailableDayId { get; set; }

    [Required(ErrorMessage = "Day of week is required.")]
    public DayOfWeek DayOfWeek { get; set; }

    [Required(ErrorMessage = "Start time is required.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "End time is required.")]
    public DateTime EndTime { get; set; }

    public int EngineerId { get; set; }
    public Engineer Engineer { get; set; }
}
}