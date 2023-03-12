using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.Helpers
{
    public class Parse
    {
        public static string ToString(object value)
        {
            if (value is decimal || value is float || value is double)
            {
                return ((decimal)value).ToString("N2");
            }
            else if (value is int)
            {
                return ((int)value).ToString("N");
            }
            else if (value is DateTime)
            {
                return ((DateTime)value).ToString("dd-MM-yyyy");
            }
            return value.ToString();

        }
        public static int ToInt(string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static double ToDouble(string value)
        {
            try
            {
                return double.Parse(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static decimal ToDecimal(string value)
        {
            try
            {
                return decimal.Parse(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static DateTime ToDateTime(string value)
        {
            try
            {
                return DateTime.Parse(value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool ToBoolean(string value)
        {
            if (value == null)
            {
                return false;
            }
            value = value.Replace(" ", "").ToLower();
            return value == "true" || value == "1" || value == "on" || value == "yes";
        }

        public static string ToString(decimal value, string format = "N2")
        {
            return value.ToString(format);
        }

        public static string ToString(decimal value, int currencyId)
        {
            return value.ToString(value.ToString());
        }
        public static string ToString(int value, string format = "N2")
        {
            return value.ToString(format);
        }

        public static string ToString(int value, int currencyId)
        {
            return value.ToString(value.ToString());
        }

        public static string ToWord(string value)
        {
            double val = ToDouble(value);
            return NumToWord.ToWord(val.ToString(), true, "");
        }

        private class NumToWord
        {
            /// <summary>
            /// Read Number to String
            /// </summary>
            /// <param name="numb">Number To Read</param>
            /// <param name="kh">True to get Khmer Language</param>
            /// <returns>String of Read Number</returns>
            static public String ToWord(String numb, bool kh, string Currency)
            {
                String number = numb, decimalVal = "", splitRange = "", dotStr = "";
                if (numb.StartsWith("0"))
                    numb = numb.Remove(0, 1);
                try
                {
                    int dot = numb.IndexOf(".");
                    if (dot > 0)
                    {
                        number = numb.Substring(0, dot);
                        decimalVal = numb.Substring(dot + 1);
                        if (Convert.ToInt32(decimalVal) > 0)
                        {

                            if (kh)
                            {
                                dotStr = readDecimal(decimalVal, true);
                                splitRange = "ចុច";
                            }
                            else
                            {
                                dotStr = readDecimal(decimalVal, false);
                                splitRange = "Dot";

                            }
                        }
                    }
                    if (kh)
                        return String.Format("{0} {1}{2} {3} {4}", number2WordKH(number).Trim(), splitRange, dotStr, Currency, " ");
                    else
                        return String.Format("{0} {1}{2} {3} {4}", number2Word(number).Trim(), splitRange, dotStr, Currency, "Only");

                }
                catch
                {
                    return "";
                }
            }

            /// <summary>
            /// Read Each One Digit of Number
            /// </summary>
            /// <param name="digit">Number Digit</param>
            /// <param name="kh">True to get Khmer Language</param>
            /// <returns>String of Read Digit</returns>
            static private String fOneDigt(String digit, bool kh)
            {
                String[] oneDigit = new String[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
                String[] oneDigitKH = new String[] { "មួយ", "ពីរ", "បី", "បួន", "ប្រាំ", "ប្រាំមួយ", "ប្រាំពីរ", "ប្រាំបី", "ប្រាំបួន" };
                int digt = Convert.ToInt32(digit);
                if (kh)
                    return oneDigitKH[digt - 1];
                else
                    return oneDigit[digt - 1];

            }

            /// <summary>
            /// Read Fret to Digit Number
            /// </summary>
            /// <param name="digit">Fret Two Digit</param>
            /// <param name="kh">True to get Khmer Language</param>
            /// <returns>String of Read Fret Two Digit</returns>
            static private String fTwoDigit(String digit, bool kh)
            {
                String[] twoDigit = new String[] { "Ten", "Eleven", "Twelve", "Thirteen", "FourTeen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                String[] twoDigitKH = new String[] { "ដប់", "ដប់មួយ", "ដប់ពីរ", "ដប់បី", "ដប់បួន", "ដប់ប្រាំ", "ដប់ប្រាំមួយ", "ដប់ប្រាំពីរ", "ដប់ប្រាំបី", "ដប់ប្រាំបួន", "ម្ភៃ", "សាមសិប", "សែសិប", "ហាសិប", "ហុកសិប", "ចិតសិប", "ប៉ែតសិប", "កៅសិប" };
                int digt = Convert.ToInt32(digit);
                String name = null, nameKH = null;
                if ((digt >= 10) && (digt / 10.0) < 2)
                {
                    name = twoDigit[digt % 10];
                    nameKH = twoDigitKH[digt % 10];
                }
                else if ((digt >= 20) && (digt % 10) == 0)
                {
                    name = twoDigit[8 + (digt / 10)];
                    nameKH = twoDigitKH[8 + (digt / 10)];
                }
                else if (digt > 0)
                {
                    name = fTwoDigit(digit.Substring(0, 1) + "0", false) + " " + fOneDigt(digit.Substring(1), false);
                    nameKH = fTwoDigit(digit.Substring(0, 1) + "0", true) + " " + fOneDigt(digit.Substring(1), true);
                }
                if (kh)
                    return nameKH;
                else
                    return name;



            }

            /// <summary>
            /// Read Fret Number
            /// </summary>
            /// <param name="number">Number to Read</param>
            /// <returns>String of Read Fret Number English</returns>
            static private String number2Word(String number)
            {
                string word = "";
                try
                {
                    bool zeroStart = false; //ពិនិត្យមើល សូន្យនៅខាងមុខ 0XX
                    bool isChange = false;  //ពិនិត្យមើល ខ្ទង់ដែលបានបំលែងទៅជា word
                    double dblAmt = (Parse.ToDouble(number));
                    if (dblAmt > 0)
                    {
                        zeroStart = number.StartsWith("0");

                        int numDigits = number.Length;
                        int pos = 0; //រក្សាទុកទីតាំងនៃខ្ទង់នីមួយៗ (រាយ, ដប់​, រយ​...)
                        String place = "";//digit grouping name:hundres,thousand,etc...
                        switch (numDigits)
                        {
                            case 1:  //ខ្ទង់រាយ
                                word = fOneDigt(number, false);
                                isChange = true;
                                break;
                            case 2:  //ខ្ទង់ដប់
                                word = fTwoDigit(number, false);
                                isChange = true;
                                break;
                            case 3:  //ខ្ទង់រយ
                                pos = (numDigits % 3) + 1;
                                place = " Hundred ";
                                break;
                            case 4:  //ខ្ទង់ពាន់
                            case 5:
                            case 6:
                                pos = (numDigits % 4) + 1;
                                place = " Thousand ";
                                break;
                            case 7:  //ខ្ទង់លាន
                            case 8:
                            case 9:
                                pos = (numDigits % 7) + 1;
                                place = " Million ";
                                break;
                            case 10:  //ខ្ទង់កោដ៏
                                pos = (numDigits % 10) + 1;
                                place = " Billion ";
                                break;
                            default:
                                isChange = true;
                                break;
                        }
                        if (!isChange)
                        {
                            //បើសិនជាមានខ្ទង់មិនទាន់បំលែងជាពាក្យ បន្តរដោយការហៅ
                            //function នេះឡើងវិញដោយខ្លួនវា (នេះជាការអនុវត្តន៏ Recursiv)
                            word = number2Word(number.Substring(0, pos)) + place + number2Word(number.Substring(pos));

                            if ((numDigits <= 5) && (numDigits % 3 == 0))
                                if (zeroStart)
                                {
                                    word = " Zero " + word.Trim();
                                }
                        }
                        if (word.Trim().Equals(place.Trim()))
                        {
                            word = "";
                        }
                        //MessageBox.Show(isChange.ToString());
                    }
                }
                catch {; }
                return word.Trim();
            }

            /// <summary>
            /// Read Fret Number
            /// </summary>
            /// <param name="number">Number to Read</param>
            /// <returns>String of Read Fret Number English</returns>
            static private String number2WordKH(String number)
            {
                string wordKH = "";
                try
                {
                    bool zeroStart = false; //ពិនិត្យមើល សូន្យនៅខាងមុខ 0XX
                    bool isChange = false;  //ពិនិត្យមើល ខ្ទង់ដែលបានបំលែងទៅជា word
                    double dblAmt = (Parse.ToDouble(number));
                    if (dblAmt > 0)
                    {
                        zeroStart = number.StartsWith("0");

                        int numDigits = number.Length;
                        int pos = 0; //រក្សាទុកទីតាំងនៃខ្ទង់នីមួយៗ (រាយ, ដប់​, រយ​...)
                        String placeKH = "";
                        switch (numDigits)
                        {
                            case 1:  //ខ្ទង់រាយ
                                wordKH = fOneDigt(number, true);
                                isChange = true;
                                break;
                            case 2:  //ខ្ទង់ដប់
                                wordKH = fTwoDigit(number, true);
                                isChange = true;
                                break;
                            case 3:  //ខ្ទង់រយ
                                pos = (numDigits % 3) + 1;
                                placeKH = " រយ ";
                                break;
                            case 4:  //ខ្ទង់ពាន់
                                pos = (numDigits % 3);
                                placeKH = " ពាន់ ";
                                break;
                            case 5:
                                pos = (numDigits % 3) - 1;
                                placeKH = " ម៉ឺន ";
                                break;
                            case 6:
                                pos = (numDigits % 4) - 1;
                                placeKH = " សែន ";
                                break;
                            case 7:  //ខ្ទង់លាន
                            case 8:
                            case 9:
                                pos = (numDigits % 7) + 1;
                                placeKH = " លាន ";
                                break;
                            case 10:  //ខ្ទង់កោដ៏
                                pos = (numDigits % 10) + 1;
                                placeKH = " កោដ៏ ";
                                break;
                            default:
                                isChange = true;
                                break;
                        }
                        if (!isChange)
                        {
                            //បើសិនជាមានខ្ទង់មិនទាន់បំលែងជាពាក្យ បន្តរដោយការហៅ
                            //function នេះឡើងវិញដោយខ្លួនវា (នេះជាការអនុវត្តន៏ Recursiv)
                            wordKH = number2WordKH(number.Substring(0, pos)) + placeKH + number2WordKH(number.Substring(pos));

                            if ((numDigits <= 5) && (numDigits % 3 == 0))
                                if (zeroStart)
                                {
                                    wordKH = " និង សូន្យ​" + wordKH.Trim();
                                }
                        }
                        if (wordKH.Trim().Equals(placeKH.Trim()))
                        {
                            wordKH = "";
                        }
                        //MessageBox.Show(isChange.ToString());
                    }
                }
                catch {; }
                return wordKH.Trim();
            }

            /// <summary>
            /// Read Number After Decimal
            /// </summary>
            /// <param name="tailVal">String To Read</param>
            /// <param name="kh">True to get Khmer Language</param>
            /// <returns>String of Read Number</returns>
            static private String readDecimal(String tailVal, bool kh)
            {
                String tailToWord = "", digit = "", inWord = "";
                for (int i = 0; i < tailVal.Length; i++)
                {
                    digit = tailVal[i].ToString();
                    if (digit.Equals("0"))
                    {
                        if (kh)
                            inWord = "សូន្យ";
                        else
                            inWord = "Zero";

                    }
                    else
                    {
                        if (kh)
                            inWord = fOneDigt(digit, true);
                        else
                            inWord = fOneDigt(digit, false);

                    }
                    tailToWord += " " + inWord;
                }
                return tailToWord;
            }
        }

    }
}
