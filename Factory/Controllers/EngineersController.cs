using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Factory.Controllers
{
  public class EngineersController : Controller
  {
    private readonly FactoryContext _db;

    public EngineersController(FactoryContext db)
    {
      _db = db;
    }

    // UTILITY

    // probably only necessary because EngineerAvailableDay lists are handled code-side(?)
    // I could not figure out how to use a Dictionary<string, Dictionary<string, string> as a firld type for 

    List<string> dayNames = new List<string>{"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
    private Dictionary<string, Dictionary<string, string>> GetWeekStartEndDictFromStringList(List<string> stringList) {
      // takes a list with 14 time strings: Mon start, Mon end, Tue start, Tue end, etc.
      
      Dictionary<string, Dictionary<string, string>> availableDays = new Dictionary<string, Dictionary<string, string>>{ };
      for (var i = 0; i < stringList.Count; i += 2) {
        availableDays.Add(key: dayNames[i / 2], value: new Dictionary<string, string> { { "StartTime", stringList[i] }, { "EndTime", stringList[(i + 1)] } });
      }

      return availableDays;
    }

    private void SaveAvailableDays(Engineer engineer, Dictionary<string, Dictionary<string, string>> availableDays) {
      // adds EngineerAvailableDay objects to db from formatted Dictionary availableDays
      // deletes existing if it already exists
      // TODO: check if new and old are same

      List<EngineerAvailableDay> daysToAdd = new List<EngineerAvailableDay>();
      foreach (var day in availableDays)
      {
        EngineerAvailableDay newAvailableDay = new EngineerAvailableDay() 
        { 
          EngineerId = engineer.EngineerId, 
          DayOfWeek = day.Key, 
          StartTime = DateTime.Parse(day.Value["StartTime"]), 
          EndTime = DateTime.Parse(day.Value["EndTime"])
        }; 
        #nullable enable
        EngineerAvailableDay? existingDayData = _db.EngineerAvailableDays.FirstOrDefault(join => (join.EngineerId == engineer.EngineerId && join.DayOfWeek == day.Key));
        #nullable disable
        if (existingDayData != null) {
          _db.EngineerAvailableDays.Remove(existingDayData);
          newAvailableDay.EngineerAvailableDayId = existingDayData.EngineerAvailableDayId;
        }
        daysToAdd.Add(newAvailableDay);
      }
      
      _db.EngineerAvailableDays.AddRange(daysToAdd);
      _db.SaveChanges();
    }

    // CREATE

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Engineer engineer, string MondayStartTime, string MondayEndTime, string TuesdayStartTime, string TuesdayEndTime, string WednesdayStartTime, string WednesdayEndTime, string ThursdayStartTime, string ThursdayEndTime, string FridayStartTime, string FridayEndTime, string SaturdayStartTime, string SaturdayEndTime, string SundayStartTime, string SundayEndTime)
    {
      if (!ModelState.IsValid)
      {
          return View();
      }
      else
      {
        _db.Engineers.Add(engineer);
        _db.SaveChanges();

        List<string> dayStringList = new List<string>{ MondayStartTime, MondayEndTime, TuesdayStartTime, TuesdayEndTime, WednesdayStartTime, WednesdayEndTime, ThursdayStartTime, ThursdayEndTime, FridayStartTime, FridayEndTime, SaturdayStartTime, SaturdayEndTime, SundayStartTime, SundayEndTime };
        Dictionary<string, Dictionary<string, string>> availableDays = GetWeekStartEndDictFromStringList(dayStringList);
        SaveAvailableDays(engineer, availableDays);

        return RedirectToAction("Index");
      }
    }

    // READ

    public ActionResult Index()
    {
      return View(_db.Engineers.ToList());
    }

    public ActionResult Details(int id)
    {
      Engineer thisEngineer = _db.Engineers
                              .Include(engineer => engineer.CertificationPartners)
                              .ThenInclude(join => join.Machine)
                              .Include(engineer => engineer.AvailableDays)
                              .FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    // UPDATE

    public ActionResult Edit(int id)
    {
      Engineer thisEngineer = _db.Engineers
                              .Include(engineer => engineer.CertificationPartners)
                              .ThenInclude(join => join.Machine)
                              .Include(engineer => engineer.AvailableDays)
                              .FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult Edit(Engineer engineer, string MondayStartTime, string MondayEndTime, string TuesdayStartTime, string TuesdayEndTime, string WednesdayStartTime, string WednesdayEndTime, string ThursdayStartTime, string ThursdayEndTime, string FridayStartTime, string FridayEndTime, string SaturdayStartTime, string SaturdayEndTime, string SundayStartTime, string SundayEndTime)
    {
      if (!ModelState.IsValid)
      {
        // this works without db call but does not display error messages
        // return RedirectToAction("Edit", engineer.EngineerId);

        Engineer thisEngineer = _db.Engineers
                                .Include(engineer => engineer.CertificationPartners)
                                .ThenInclude(join => join.Machine)
                                .Include(engineer => engineer.AvailableDays)
                                .FirstOrDefault(engineer => engineer.EngineerId == engineer.EngineerId);
        return View(thisEngineer);
      }
      else
      {
        _db.Engineers.Update(engineer);
        _db.SaveChanges();
        
        List<string> dayStringList = new List<string>{ MondayStartTime, MondayEndTime, TuesdayStartTime, TuesdayEndTime, WednesdayStartTime, WednesdayEndTime, ThursdayStartTime, ThursdayEndTime, FridayStartTime, FridayEndTime, SaturdayStartTime, SaturdayEndTime, SundayStartTime, SundayEndTime };
        Dictionary<string, Dictionary<string, string>> availableDays = GetWeekStartEndDictFromStringList(dayStringList);
        SaveAvailableDays(engineer, availableDays);
        
        return RedirectToAction(actionName: "Details", new { id = engineer.EngineerId });
      }
    }

    public ActionResult AddMachine(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Model");
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult AddMachine(Engineer engineer, int machineId)
    {
      #nullable enable
      EngineerMachine? joinEntity = _db.EngineerMachines.FirstOrDefault(join => (join.MachineId == machineId && join.EngineerId == engineer.EngineerId));
      #nullable disable
      if (joinEntity == null && machineId != 0)
      {
        _db.EngineerMachines.Add(new EngineerMachine() { MachineId = machineId, EngineerId = engineer.EngineerId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = engineer.EngineerId });
    }

    // DELETE

    public ActionResult Delete(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      _db.Engineers.Remove(thisEngineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId, int engineerId)
    {
      EngineerMachine joinEntry = _db.EngineerMachines.FirstOrDefault(entry => entry.EngineerMachineId == joinId);
      _db.EngineerMachines.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = engineerId });
    }
  }
}