using System;
using System.Globalization;
using System.Linq;

namespace SM.Utilities
{
    public class NumberToEnglish
    {
        public static String ChangeNumericToWords(double numb)
        {
            var num = numb.ToString(CultureInfo.InvariantCulture);
            return ChangeToWords(num, false);
        }
        public static String ChangeCurrencyToWords(String numb)
        {
            return ChangeToWords(numb, true);
        }
        public static String ChangeNumericToWords(String numb)
        {
            return ChangeToWords(numb, false);
        }
        public static String ChangeCurrencyToWords(double numb)
        {
            return ChangeToWords(numb.ToString(CultureInfo.InvariantCulture), true);
        }
        private static String ChangeToWords(String numb, bool isCurrency)
        {
            String val = "", wholeNo = numb;
            String andStr = "", pointStr = "";
            var endStr = (isCurrency) ? ("Only") : ("");
            try
            {
                var decimalPlace = numb.IndexOf(".", System.StringComparison.Ordinal);
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    var points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = (isCurrency) ? ("and") : ("point");// just to separate whole numbers from points/cents
                        endStr = (isCurrency) ? ("Cents " + endStr) : ("");
                        pointStr = TranslateCents(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", TranslateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { ;}
            return val;
        }

        private static String TranslateWholeNumber(String number)
        {
            var word = "";
            try
            {
                var isDone = false;//test if already translated
                var curenAmt = (Convert.ToDouble(number));
                var dblAmt = Math.Abs(curenAmt);
                //if ((dblAmt > 0) && number.StartsWith("0"))
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    var beginsZero = number.StartsWith("0");//tests for 0XX
                    var numDigits = number.Length;
                    var pos = 0;//store digit grouping
                    var place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://ones' range
                            word = Ones(number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = Tens(number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range
                        case 11:
                        case 12:
                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)
                        word = TranslateWholeNumber(number.Substring(0, pos)) + place + TranslateWholeNumber(number.Substring(pos));
                        //check for trailing zeros
                        if (beginsZero) word = " and " + word.Trim();
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
                if (curenAmt < 0) word = "Negative " + word;
            }
            catch { }
            return word.Trim();
        }
        private static String Tens(String digit)
        {
            var digt = Convert.ToInt32(digit);
            String name = null;
            switch (digt)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (digt > 0)
                    {
                        name = Tens(digit.Substring(0, 1) + "0") + " " + Ones(digit.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String Ones(String digit)
        {
            var digt = Convert.ToInt32(digit);
            var name = "";
            switch (digt)
            {
                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        private static String TranslateCents(String cents)
        {
            return (from t in cents select t.ToString(CultureInfo.InvariantCulture) into digit let engOne = "" select digit.Equals("0") ? "Zero" : Ones(digit)).Aggregate("", (current, engOne) => current + (" " + engOne));
        }
    }
}
