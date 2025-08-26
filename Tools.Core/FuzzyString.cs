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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tools;

namespace Tools
{
  //TODO 3178: Add additional algorithms
  public class FuzzyString
  {
    /// <summary>
    /// J(A,B) = (A ∩ B)/(A U B)
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static double JaccardCoefficient(string first, string second, params string[] ignore)
    {
      if (BothAreNull(first, second)) return 1;
      if (OnlyOneIsNull(first, second)) return 0;

      try
      {
        var set1 = first.ToUpper().Split(ignore, StringSplitOptions.RemoveEmptyEntries);
        var set2 = second.ToUpper().Split(ignore, StringSplitOptions.RemoveEmptyEntries);

        return (double)set1.Intersect(set2).Count() / set1.Union(set2).Count();
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return 0;
      }
    }

    private static List<string> PerformPhraseAlgebra(string first, string second, Func<string[], string[], IEnumerable<string>> algebra)
    {
      first.Guard("phrase cannot be empty");
      second.Guard("phrase cannot be empty");

      string pattern = "\\.|\"|,| ";
      
      try
      {
        var set1 = Regex.Split(first.ToUpper().ToString(), pattern).Select(s => s = s.Replace("\"", "")).Where(s => s != String.Empty).ToArray();
        var set2 = Regex.Split(second.ToUpper().ToString(), pattern).Select(s => s = s.Replace("\"", "")).Where(s => s != String.Empty).ToArray();

        return algebra(set1, set2).ToList<string>(); 
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return new List<string>();
      }
    }

    public static List<string> GetPhraseDifferences(string first, string second)
    {
      //Debugger.Break();
      return PerformPhraseAlgebra(first, second, (set1, set2) => set1.Except(set2).Union(set2.Except(set1)));
    }

    public static List<string> GetPhraseMatches(string first, string second)
    {
      return PerformPhraseAlgebra(first, second, (set1, set2) => set1.Intersect(set2));
    }


    public static double JaccardCoefficient(string first, string second)
    {
      return JaccardCoefficient(first, second, new string[] { " ", ",", ".", "USA" });
    }


    public static bool BothAreNull(string first, string second)
    {
      return (String.IsNullOrEmpty(first) && String.IsNullOrEmpty(second));
    }

    public static bool OnlyOneIsNull(string first, string second)
    {
      return (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second) == false) ||
        ((string.IsNullOrEmpty(second) && string.IsNullOrEmpty(first) == false));
    }

    private static int GetNumberOfMatches(string first, string second)
    {
        return GetPhraseMatches(first, second).Count;
    }

    [Description("Double the matching number of characters divided by the total number of characters of the two strings")]
    public static double RatcliffObershelpCoefficient(string first, string second)
    {
      first = ImproveNoise(first);
      second = ImproveNoise(second);

      if (IsExactMatch(first, second)) return 1;
      if (BothAreNull(first, second)) return 0;

      return CalculateRatcliffObershelpCoefficient(first, second);
    }

    //TODO 3179: GetSubStringCoefficient Boundary Errors
    private static double CalculateRatcliffObershelpCoefficient(string first, string second)
    {
#if true
      return 2 * Convert.ToDouble(first.Intersect(second).Count()) / (Convert.ToDouble(first.Length + second.Length));
#else
      return (double)GetSubstringCoefficient(Encoding.ASCII.GetBytes(first), Encoding.ASCII.GetBytes(second), 0, first.Length, 0, second.Length) 
        / (first.Length + second.Length) * 2;
#endif
    }

    private static bool IsExactMatch(string first, string second)
    {
      return string.Compare(first, second, true) == 0;
    }

    private static string ImproveNoise(string s)
    {
      try
      {
        var o = s.Split(new char[] { ',', '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", o).ToUpper();
      }
      catch(Exception ex)
      {
        Debug.WriteLine("ImproveNoise Error: {0}", ex.Message);
        return s;
      }
    }

    private static int GetSubstringCoefficient(byte[] b1, byte[] b2, int start1, int end1, int start2, int end2)
    {
      int newStart1 = 0;
      int newStart2 = 0;
      int max = 0;

      if (start1 >= end1 || start2 >= end2 || start1 < 0 || start2 < 0) return 0;

      for (int c1 = start1; c1 < end1; c1++)
      {
        for (int c2 = start2; c2 < end2; c2++)
        {
          int i = 0;

          while (b1[c1 + i] == b2[c2 + i])
          {
            i++;

            if (i > max)
            {
              newStart1 = c1;
              newStart2 = c2;
              max = i;
            }
            if (c1 + i >= end1 || c2 + i >= end2) break;
          }
        }
      }

      max = max + GetSubstringCoefficient(b1, b2, newStart1 + max, end1, newStart2 + max, end2);
      max = max + GetSubstringCoefficient(b1, b2, start1, newStart1 - 1, start2, newStart2 - 1);

      return max;
    }

    public static string Soundex(string word)
    {
      if (string.IsNullOrEmpty(word)) return "0000";

      string value = "";
      int size = word.Length;
      if (size > 1)
      {
        word = word.ToUpper();
        char[] chars = word.ToCharArray();
        StringBuilder buffer = new StringBuilder();
        buffer.Length = 0;
        int previousCode = 0;
        int currentCode = 0;
        buffer.Append(chars[0]);

        for (int i = 1; i < size; i++)
        {
          switch (chars[i])
          {
            case 'A':
            case 'E':
            case 'I':
            case 'O':
            case 'U':
            case 'H':
            case 'W':
            case 'Y':
              currentCode = 0;
              break;
            case 'B':
            case 'F':
            case 'P':
            case 'V':
              currentCode = 1;
              break;
            case 'C':
            case 'G':
            case 'J':
            case 'K':
            case 'Q':
            case 'S':
            case 'X':
            case 'Z':
              currentCode = 2;
              break;
            case 'D':
            case 'T':
              currentCode = 3;
              break;
            case 'L':
              currentCode = 4;
              break;
            case 'M':
            case 'N':
              currentCode = 5;
              break;
            case 'R':
              currentCode = 6;
              break;
          }

          if (currentCode != previousCode)
          {
            if (currentCode != 0)
              buffer.Append(currentCode);
          }

          previousCode = currentCode;
          if (buffer.Length == word.Length)
            break;
        }

        size = buffer.Length;
        if (size < word.Length)
          buffer.Append('0', (word.Length - size));
        value = buffer.ToString();
      }

      return value.PadRight(4, '0').Substring(0, 4);
    }
  }
}
