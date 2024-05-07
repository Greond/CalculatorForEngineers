using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    internal class Convector
    {
        internal string ConvertToBin(string num, int round = 5)
        {
            try
            {
                bool firstminus = false;
                if (num.StartsWith("-"))  // если число отриательное
                { firstminus = true; num = num.Substring(1); }
                string result = ""; //Результат
                long left = 0; //Целая часть
                string right = "0"; //Дробная часть
                string[] temp1 = num.Split(new char[] { '.', ',' }); //Нужна для разделения целой и дробной частей
                left = Convert.ToInt64(temp1[0]);
                //Проверяем имеется ли у нас дробная часть
                if (temp1.Count() > 1)
                {
                    right = num.Split(new char[] { '.', ',' })[1]; //Дробная часть
                }
                //Алгоритм перевода целой части в двоичную систему
                while (true)
                {
                    result += left % 2; //В ответ помещаем остаток от деления. В конце программы мы перевернём строку, так как в обратном порядке записываются остатки
                    left = left / 2; //Так как Left целое число, то при делении например числа 2359 на 2, мы получим не 1179,5 а 1179
                    if (left == 0)
                        break;
                }
                result = new string(result.ToCharArray().Reverse().ToArray()); //Реверсирование строки

                //Прокеряем есть ли у нас дробная часть, можно было бы проверить и так if(temp1.count()>1)
                if (temp1.Count() == 1)
                {
                    if (firstminus) { return "-" + result; }
                    else { return result; }
                }

                //Добавляем разделить целой части от дробной
                result += ',';

                int count = right.ToString().Count(); // Нам нужно знать кол-во цифр, при превышении которого дописывается единичка
                long INTright = Convert.ToInt64(right);

                for (int i = 0; i < round; i++)
                {
                    /*Умножаем число на 2 и проверяем, стало ли оно больше по количеству цифр, если да,
                    то в результат идёт "1" и первая цифра у right удаляется */
                    INTright = INTright * 2;
                    if (INTright.ToString().Count() > count)
                    {
                        string buf = INTright.ToString();
                        buf = buf.Remove(0, 1);
                        INTright = Convert.ToInt64(buf);

                        result += '1';
                    }
                    else
                    {
                        result += '0';
                    }
                }

                if (firstminus) { return "-" + result; }
                else { return result; }
            }
            catch { return "Convert Eror"; }
        }
        internal string ConvertToOct(string num)
        {
            try
            {
                long left = 0;// целая часть
                long right = 0;// после запятой часть
                string result = string.Empty;  // куда  будем записывать результат
                bool firstminus = false;
                if (num.StartsWith("-"))  // если число отриательное
                { firstminus = true; num = num.Substring(1); }

                if (num.Contains(".") | num.Contains(",")) // проверяем есть ли дробная часть
                {
                    left = Convert.ToInt64(num.Split('.', ',')[0]); // присваиваем целую а затем и дробную часть
                    right = Convert.ToInt64(num.Split('.', ',')[1]);
                }
                else { left = Convert.ToInt64(num); } // если нету дробной части 

                while (true)
                {
                    result = Convert.ToString(left % 8) + result;
                    left = left / 8;
                    if (left == 0) { break; }
                }
                if (num.Contains(".") | num.Contains(",")) // проверяем есть ли дробная часть
                {
                    result += ",";
                    int limit = right.ToString().Count(); // количесвто цифр за превешением которых  добавляем 

                    for (int i = 0; i < 5; i++)
                    {
                        right = right * 8;
                        if (right.ToString().Count() > limit)
                        {
                            result += right.ToString()[0];
                            right = Convert.ToInt64(right.ToString().Substring(1));
                        }
                        else
                        {
                            result += "0";
                        }
                    }
                }
                if (firstminus) { return "-" + result; }
                else { return result; }
            }
            catch { return "Convert Eror"; }

        }
    }
}
