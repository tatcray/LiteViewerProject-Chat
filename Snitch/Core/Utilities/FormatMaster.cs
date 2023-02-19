using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Snitch.Core.Utilities
{
    public class FormatMaster
    {
        //public static int PhoneLength { get; set; }

        public static string Region { get; set; } = "RU";


        public static bool CheckFormatPublicId(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            else
            {
                if (str.Length > 6 && str.Length < 10 && IsDigitsOnly(str))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


        }
        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static bool MailFormatCorrect(string MailStr)
        {
            Regex Mailreg = new Regex(@".+@.+\..+");
            if (Mailreg.IsMatch(MailStr))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string PhoneMask(string Phone)
        {
            var newVal = Regex.Replace(Phone, @"[^0-9]", "");
            // if (PhoneLength != newVal.Length && !string.IsNullOrEmpty(newVal))
            if (!string.IsNullOrEmpty(newVal))
            {
                //PhoneLength = newVal.Length;
                Phone = string.Empty;
                if (Region == "BY")
                {
                    if (newVal.Length <= 3)
                    {
                        Phone = Regex.Replace(newVal, @"(\d{3})", "+$1");
                    }
                    else if (newVal.Length <= 5)
                    {
                        Phone = Regex.Replace(newVal, @"(\d{3})(\d{0,2})", "+$1($2)");
                    }
                    else if (newVal.Length <= 8)
                    {
                        Phone = Regex.Replace(newVal, @"(\d{3})(\d{2})(\d{0,3})", "+$1($2)$3");
                    }
                    else if (newVal.Length <= 10)
                    {
                        Phone = Regex.Replace(newVal, @"(\d{3})(\d{2})(\d{0,3})(\d{0,2})", "+$1($2)$3-$4");
                    }
                    else if (newVal.Length > 10)
                    {
                        Phone = Regex.Replace(newVal, @"(\d{3})(\d{2})(\d{0,3})(\d{0,2})(\d{0,2})", "+$1($2)$3-$4-$5");
                    }
                }
                else if (Region == "RU")
                {
                    if (newVal.Length <= 1)
                    {
                        Phone = Regex.Replace(newVal, @"(\d{1})", "+$1");
                    }
                    else if (newVal.Length <= 4)
                    {
                        Phone = Regex.Replace(newVal, @"(\d{1})(\d{0,3})", "+$1($2)");
                    }
                    else if (newVal.Length <= 7)
                    {
                        Phone = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})", "+$1($2)$3");
                    }
                    else if (newVal.Length <= 9)
                    {
                        Phone = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})", "+$1($2)$3-$4");
                    }
                    else if (newVal.Length > 9)
                    {
                        Phone = Regex.Replace(newVal, @"(\d{1})(\d{3})(\d{0,3})(\d{0,2})(\d{0,2})", "+$1($2)$3-$4-$5");
                    }
                }
            }

            return Phone;
        }
    }
}
