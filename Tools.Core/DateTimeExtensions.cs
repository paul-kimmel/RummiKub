#region Copyright Notice
//Copyright 2002-2016 Software Conceptions, Inc 4103 Cornell Rd. 
//Okemos. MI 49964, U.S.A. All rights reserved.

//Software Conceptions, Inc has intellectual property rights relating to 
//technology embodied in this product. In particular, and without 
//limitation, these intellectual property rights may include one or more 
//of U.S. patents or pending patent applications in the U.S. and/or other countries.

//This product is distributed under licenses restricting its use, copying and
//distribution. No part of this product may be 
//reproduced in any form by any means without prior written authorization 
//of Software Conceptions.

//Software Conceptions is a trademarks of Software Conceptions, Inc
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
  public static class DateTimeExtensions
  {
    public static DateTime GetNextNonHolidayOrNonWeekendAfter(this DateTime o)
    {
      DateTime target = o;
      target = target.AddDays(1);

      while (target.IsHolidayOrWeekend() && (target - o).TotalDays <= 7)
        target = target.AddDays(1);

      return target;
    }

    public static bool IsHolidayOrWeekend(this DateTime o)
    {
      Func<DateTime, bool>[] tests = {
			IsSaturday,
			IsSunday,
			IsNewYears,
			IsPresidentsDay,
			IsMartinLutherKing,
			IsMemorialDay,
			IsIndependenceDay,
			IsLaborDay,
			IsGeneralElectionDay,
			IsThanksgivingDay,
			IsChristmasDay
		};

      foreach (var func in tests)
        if (func(o))
          return true;

      return false;
    }

    public static bool IsSaturday(this DateTime o)
    {

      return o.DayOfWeek == DayOfWeek.Saturday;
    }

    public static bool IsSunday(this DateTime o)
    {
      return o.DayOfWeek == DayOfWeek.Sunday;
    }

    public static bool IsWeekend(this DateTime o)
    {
      return IsSunday(o) || IsSaturday(o);
    }

    public static bool IsNewYears(this DateTime o)
    {
      //New Year's Day, January 1. Need to adjust for weekend
      DateTime test = new DateTime(o.Year, 1, 1);
      test = MakeWeekendFollowingMonday(test);
      return test == o || IsDayMonth(o, 1, 1);
    }

    public static DateTime GetNewYears(this DateTime o)
    {
      return MakeWeekendFollowingMonday(new DateTime(o.Year, 1, 1));
    }


    public static DateTime MakeWeekendFollowingMonday(this DateTime o)
    {
      switch (o.DayOfWeek)
      {
        case DayOfWeek.Sunday:
          return o.AddDays(1);
        case DayOfWeek.Saturday:
          return o.AddDays(2);
        default:
          return o;
      }
    }

    public static bool IsDayMonth(this DateTime o, int day, int month)
    {
      return o.Day == day && o.Month == month;
    }

    /// <summary>
    /// nthOccurs is which occurrence of the day of the week that is, first, second, third or fourth 
    /// {e.g. 4/28/2020 is the 4th Tuesday of April} 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="month"></param>
    /// <param name="nthOccurs"></param>
    /// <param name="dw"></param>
    /// <returns></returns>
    public static bool IsXDayOfMonth(this DateTime o, int month, int nthOccurs, DayOfWeek dw)
    {
      return o.DayOfWeek == dw && o.Month == month && GetOccurrenceOfDayInMonth(o) == nthOccurs;
    }

    public static int GetOccurrenceOfDayInMonth(this DateTime o)
    {
      return ((int)Math.Ceiling(o.Day / 7.0));
    }

    public static DateTime Next(this DateTime o, DayOfWeek dw)
    {
      return o.AddDays((dw < o.DayOfWeek ? 7 : 0) + dw - o.DayOfWeek);
    }

    public static DateTime GetNthDayOfMonth(this DateTime o, int week, DayOfWeek dw)
    {
      return o.Next(dw).AddDays((week - 1) * 7);
    }

    public static DateTime GetMartinLutherKing(this DateTime o)
    {
      return new DateTime(o.Year, 1, 1).GetNthDayOfMonth(3, DayOfWeek.Monday);
    }

    public static bool IsMartinLutherKing(this DateTime o)
    {
      //Martin Luther King, Jr. Day, Third Monday in January.
      return IsXDayOfMonth(o, 1, 3, DayOfWeek.Monday);
    }

    public static DateTime GetPresidentsDay(this DateTime o)
    {
      return new DateTime(o.Year, 2, 1).GetNthDayOfMonth(3, DayOfWeek.Monday);
    }

    public static bool IsPresidentsDay(this DateTime o)
    {
      //President's Day, Third Monday in February.
      return IsXDayOfMonth(o, 2, 3, DayOfWeek.Monday);
    }

    public static DateTime LastDayOfMonth(this DateTime o, DayOfWeek dw)
    {
      DateTime result = new DateTime(o.Year, o.Month, 1).AddMonths(1).AddDays(-1);
      while (result.DayOfWeek != dw)
        result = result.AddDays(-1);

      return result;
    }

    public static DateTime FirstDayOfMonth(this DateTime o, DayOfWeek dw)
    {
      DateTime result = new DateTime(o.Year, o.Month, 1);
      while (result.DayOfWeek != dw)
        result = result.AddDays(1);

      return result;
    }

    //BUGFIX 351
    public static DateTime GetMemorialDay(this DateTime o)
    {
      return new DateTime(o.Year, 5, 1).LastDayOfMonth(DayOfWeek.Monday);
    }

    public static bool IsMemorialDay(this DateTime o)
    {
      //Memorial Day, Last Monday in May.

      var timespan = o.GetLastDayOfMonth().Subtract(o);

      return o.DayOfWeek == DayOfWeek.Monday && o.Month == 5
        && (timespan.Days < 7);

    }

    public static DateTime GetLastDayOfMonth(this DateTime o)
    {
      return new DateTime(o.Year, o.Month, DateTime.DaysInMonth(o.Year, o.Month));

    }

    public static DateTime GetIndependenceDay(this DateTime o)
    {
      return new DateTime(o.Year, 7, 4);
    }

    public static bool IsIndependenceDay(this DateTime o)
    {
      //Independence Day, July 4.
      return IsDayMonth(o, 4, 7);
    }

    public static DateTime GetLaborDay(this DateTime o)
    {
      return new DateTime(o.Year, 9, 1).FirstDayOfMonth(DayOfWeek.Monday);
    }

    public static bool IsLaborDay(this DateTime o)
    {
      //Labor Day, First Monday in September.
      return o.DayOfWeek == DayOfWeek.Monday && GetOccurrenceOfDayInMonth(o) == 1
        && o.Month == 9;
    }

    public static DateTime GetGeneralElectionDay(this DateTime o)
    {
      int year = o.Year % 2 == 0 ? o.Year : o.Year + 1;

      return new DateTime(year, 11, 1).FirstDayOfMonth(DayOfWeek.Tuesday);
    }

    public static bool IsGeneralElectionDay(this DateTime o)
    {
      //General Election Day, First Tuesday in November, even numbered years
      return o.Year % 2 == 0 && o.Month == 11 && o.DayOfWeek == DayOfWeek.Tuesday
        && GetOccurrenceOfDayInMonth(o) == 1;
    }

    public static DateTime GetVeteransDay(this DateTime o)
    {
      return new DateTime(o.Year, 11, 11);
    }

    public static bool IsVeteransDay(this DateTime o)
    {
      //Veterans Day, November 11.
      return IsDayMonth(o, 11, 11);
    }

    public static DateTime GetThanksgivingDay(this DateTime o)
    {
      return new DateTime(o.Year, 11, 1).GetNthDayOfMonth(4, DayOfWeek.Thursday);
    }

    public static DateTime GetThanksgivingFridayDay(this DateTime o)
    {
      return o.GetThanksgivingDay().AddDays(1);
    }


    public static bool IsThanksgivingDay(this DateTime o)
    {
      //Thanksgiving Day and the day after, the fourth Thursday and Friday in November
      return o.Month == 11 && (o.DayOfWeek == DayOfWeek.Thursday || o.DayOfWeek == DayOfWeek.Friday)
        && GetOccurrenceOfDayInMonth(o) == 4;
    }

    public static DateTime GetChristmasDay(this DateTime o)
    {
      return new DateTime(o.Year, 12, 25);
    }

    public static bool IsChristmasDay(this DateTime o)
    {
      //Christmas Eve and Christmas Day, December 24 and 25.
      return o.Month == 12 && (o.Day == 24 || o.Day == 25);
    }

    public static bool IsThisWeek(this DateTime o)
    {
      var date = DateTime.Now.Date;

      while (date.DayOfWeek > DayOfWeek.Sunday)
        date = date.AddDays(-1);

      return date <= o  && o <= date.AddDays(6);
    }

    public static bool IsHistorical(this DateTime o)
    {
      return o.IsThisWeek() == false && o < DateTime.Today;
    }

    public static string GetDayOfWeek(this DateTime o, int index)
    {
      return GetDateOfWeek(o, index).DayOfWeek.ToString();
    }

    public static DateTime GetDateOfWeek(this DateTime o, int index)
    {
      return (o.AddDays(-1 * (int)o.DayOfWeek + index));
    }

    public static DateTime GetThisSunday(this DateTime o)
    {
      return o.GetDateOfWeek(0);
    }

    public static DateTime GetThisSaturday(this DateTime o)
    {
      return o.GetDateOfWeek(6);
    }

    public static DateTime GetThisMonday(this DateTime o)
    {
      return o.GetDateOfWeek(1);
    }

    public static DateTime GetThisFriday(this DateTime o)
    {
      return o.GetDateOfWeek(5);
    }

    public static IEnumerable<Tuple<DateTime, DateTime>> GetRange(int startMonth, int startYear, int monthIncrement, int endYear)
    {
      var start = new DateTime(startYear, startMonth, 1);
      var end = start.GetLastDayOfMonth();

      while (start.Year <= endYear)
      {
        yield return Tuple.Create(start.Date, end.Date);
        start = start.AddMonths(monthIncrement);
        end = start.GetLastDayOfMonth();
      }
    }


    public static int GetNextYear(int year, int month, int increment)
    {
      month += increment;
      return month > 12 ? year + (month / 12) : year;
    }

    public static int GetNextMonth(int month, int increment)
    {
      month += increment;

      if (month > 12)
      {
        month = month % 12;

        if (month == 0)
          month = 1;
      }

      return month;
    }

  }
}
