using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Calculator
{
    internal class Calculator 
    {

        List<double> numsarr = new List<double>();
        List<string> chararr = new List<string>();

        private bool haveerror = false;
        internal bool _haveerror {get { return haveerror; } }
         internal string MainEquals(bool Bin,bool Oct,string inputstring,bool Deg) // like a  ÷ × + -  % √ ² 
        {
            try
            {
                Getthecurrentexample(inputstring);
                firstcharsprocessing(Deg); // обработка доп  знаков
            }
            catch {return "Eror"; }
            string output;
            int firstpriority = 0;
            int secondpriority = 0;
            foreach (string _char in chararr)
            {
                if (_char == "÷" | _char == "×")
                {
                    firstpriority++;
                }
            }
            foreach (string _char in chararr)
            {
                if (_char == "+" | _char == "-")
                {
                    secondpriority++;
                }
            }
            while (firstpriority > 0)
            {
                DivideAndMultiplyEquals();
                firstpriority--;
            }
            while (secondpriority > 0)
            {
                PlusAndMinusEquals();
                secondpriority--;
            }
            if (haveerror == true) { output = "Syntax Erorr"; }
            else
            {
                Convector convector = new Convector();
                if (Bin == true)
                { output = convector.ConvertToBin(numsarr[0].ToString()) + "₂"; }
                else if (Oct == true)
                {
                   output = convector.ConvertToOct(numsarr[0].ToString()) + "₈";
                }
                else
                {
                    output = numsarr[0].ToString();
                }
            }
            return output;
        }
        private void charsort(string str) // str это строка с знаками или знаком, а pos это номер в цикле for 
        {
            int checkchar()// крч чтобы выбрать нужное  число  нада  найти значимые  знаки после которых  точно идут новые числа типа как + - "÷" "×"
            {
                int result = 0;
                for (int i = 0; i < chararr.Count; i++)
                {
                    if (chararr[i] == "×" | chararr[i] == "÷" | chararr[i] == "+" | chararr[i] == "-")
                    { result++; }
                }
                return result;
            }
            void skipminus(int i) // i is a number of char position in  string 
            {
                int pos = checkchar();
                if (str[i] == '-' & str[i - 1] == '÷' | str[i] == '-' & str[i - 1] == '×')
                {
                    numsarr[pos] = numsarr[pos] * -1;
                }
                else if (str[i] == '-' & str[i - 1] == 'n' | str[i] == '-' & str[i - 1] == 's' | str[i] == '-' & str[i - 1] == 'h')
                {
                    numsarr[pos] = numsarr[pos] * -1;
                }
                else if (str[i] == '-' & str[i - 1] == '⅟')
                {
                    numsarr[pos] = numsarr[pos] * -1;
                }
                else { chararr.Add(str[i].ToString()); }
            }

            if (str.Contains("log") | str.Contains("ln"))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == 'l' & str[i + 1] == 'n')
                    {
                        chararr.Add("ln");
                        i++;
                    }
                    else if (str[i] == 'l' & str[i + 1] == 'o' & str[i + 2] == 'g')
                    {
                        chararr.Add("log");
                        i = i + 2;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            chararr.Add(str[i].ToString());
                        }
                        else
                        {
                            skipminus(i);
                        }
                    }
                }
                return;
            }
            if (str.Contains("Asinh") | str.Contains("Acosh") | str.Contains("Atanh"))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == 'A')
                    {
                        chararr.Add(str.Substring(i, 5));
                        i = i + 4;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            chararr.Add(str[i].ToString());
                        }
                        else
                        {
                            skipminus(i);
                        }
                    }
                }
                return;

            }
            else if (str.Contains("Asin") | str.Contains("Acos") | str.Contains("Atan"))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == 'A')
                    {
                        chararr.Add(str.Substring(i, 4));
                        i = i + 3;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            chararr.Add(str[i].ToString());
                        }
                        else
                        {
                            skipminus(i);
                        }
                    }
                }
                return;
            }
            else if (str.Contains("sinh") | str.Contains("cosh") | str.Contains("tanh"))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == 's' | str[i] == 'c' | str[i] == 't')
                    {
                        if (str[i + 3] == 'h')
                        {
                            chararr.Add(str[i] + "h");
                            i = i + 3;
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            chararr.Add(str[i].ToString());
                        }
                        else
                        {
                            skipminus(i);
                        }
                    }
                }
                return;
            }
            else if (str.Contains("sin") | str.Contains("cos") | str.Contains("tan"))
            {
                int temp = str.Length - 1;
                for (int i = 0; i < temp + 1; i++)
                {
                    if (str[i] == 's' | str[i] == 'c' | str[i] == 't')
                    {
                        if (i != 0)
                        {
                            skipminus(i);
                            i = i + 2;
                        }
                        else if (temp == 2 | i == 0)
                        {
                            chararr.Add(str[i].ToString());
                            i = i + 2;
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            chararr.Add(str[i].ToString());
                        }
                        else
                        {
                            skipminus(i);
                        }
                    }
                }
                return;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (i == 0)
                {
                    chararr.Add(str[0].ToString());
                }
                else
                {
                    skipminus(i);
                }
            }
        }
        private void Getthecurrentexample(string inputString) // get currect  string in label and split is to a chars  and numbers 
        { //    ÷   ×   -  +   

            numsarr.Clear();
            chararr.Clear();
            haveerror = false;

           
            char[] delimiterChars = { ' ', '÷', '+', '-', '×', '%', '\t', '√', '²', '³', '⅟', 's', 'i', 'n', 'c', 'o', 's', 't', 'a', 'n', 'h', '!', 'l', 'o', 'g', 'A' }; // ÷ + - × % √ ² ⅟
            string[] nums = inputString.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nums.Length; i++)
            {
                try
                {
                    bool E_nums = nums[i].Contains('E'); // проверка на Е числа(степени)
                    if (E_nums)
                    {
                        string tempstr = nums[i] + inputString[inputString.IndexOf('E') + 1] + nums[i + 1];
                        double temp = double.Parse(tempstr);
                        numsarr.Add(temp);
                        removeat(ref nums, i + 1);
                        string str = Convert.ToString(inputString[inputString.IndexOf('E') + 1]);

                        inputString = inputString.Replace(inputString[inputString.IndexOf('E') + 1], '#');
                        inputString = inputString.Replace(inputString[inputString.IndexOf('E')], '#');
                    }
                    else if (nums[i] == "π")
                    {
                        numsarr.Add(Convert.ToDouble(Math.PI));
                    }
                    else if (nums[i] == "e")
                    {
                        numsarr.Add(Convert.ToDouble(Math.E));
                    }
                    else if (nums[i] == "φ")
                    {
                        numsarr.Add(Convert.ToDouble(1.618034));
                    }
                    else
                    {
                        double temp = double.Parse(nums[i]);

                        numsarr.Add(temp);
                        Console.WriteLine(nums[i]);
                    }
                }
                catch 
                {
                    haveerror = true;
                }
            }


            void removeat(ref string[] array, int index)
            {
                string[] newarray = new string[array.Length - 1];

                for (int i = 0; i < index; i++)
                {
                    newarray[i] = array[i];
                }
                for (int i = index + 1; i < array.Length; i++)
                {
                    newarray[i - 1] = array[i];
                }
                array = newarray;
            }
            if (inputString.StartsWith("-", true, null)) // первое число минусовое
            {
                numsarr[0] = numsarr[0] * -1;
                inputString = inputString.Substring(1);
            }
            char[] delimetrnums = { ',', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', 'E', 'Е', 'e', 'е', '#', 'π', 'φ' };
            string[] chars = inputString.Split(delimetrnums, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < chars.Length; i++)
            {
                try
                {
                    charsort(chars[i]);
                }
                catch
                {
                    haveerror = true;
                }
            }
        }
        private void firstcharsprocessing(bool Deg)
        {
            for (int i = 0; i < chararr.Count; i++)
            {
                if (chararr[i] == "√") //корень
                {
                    if (numsarr[i] > 0)
                    {
                        numsarr[i] = Math.Sqrt(numsarr[i]);
                        chararr.RemoveAt(i);
                        i--;
                    }
                    else if (numsarr[i] < 0)
                    {
                        numsarr[i] = Math.Sqrt(numsarr[i] * -1) * -1;
                        chararr.RemoveAt(i);
                        i--;
                    }
                }
                else if (chararr[i] == "%")
                {
                    if (i != 0)
                    {
                        if (chararr[i - 1] == "+" | chararr[i - 1] == "-")
                        {
                            numsarr[i] = numsarr[i] / 100 * numsarr[i - 1];
                            chararr.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            numsarr[i] = numsarr[i] / 100;
                            chararr.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        numsarr[i] = numsarr[i] / 100;
                        chararr.RemoveAt(i);
                        i--;
                    }
                }
                else if (chararr[i] == "²")
                {
                    numsarr[i] = Math.Pow(numsarr[i], 2);
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "³")
                {
                    numsarr[i] = Math.Pow(numsarr[i], 3);
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "⅟")
                {
                    numsarr[i] = Math.Pow(numsarr[i], -1);
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "s")
                {
                    if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                    numsarr[i] = Math.Sin(numsarr[i]);
                    chararr.RemoveAt(i);
                    i--;


                }
                else if (chararr[i] == "c")
                {
                    if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                    numsarr[i] = Math.Cos(numsarr[i]);
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "t")
                {
                    if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                    numsarr[i] = Math.Tan(numsarr[i]);
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "sh")
                {
                    if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                    numsarr[i] = Math.Sinh(numsarr[i]);
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "ch")
                {
                    if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                    numsarr[i] = Math.Cosh(numsarr[i]);
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "th")
                {
                    if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                    numsarr[i] = Math.Tanh(numsarr[i]);
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "!")
                {
                    if (numsarr[i].ToString().Contains(",")) { haveerror = true; }
                    else
                    {
                        try
                        {
                            int temp = Convert.ToInt32(numsarr[i]);
                            long result = 1;
                            for (int j = 1; j < temp + 1; j++)
                            {
                                result = result * j;
                            }
                            numsarr[i] = Convert.ToDouble(result);
                        }
                        catch
                        {
                            haveerror = true;
                        }
                    }
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "log")
                {
                    try
                    {
                        numsarr[i] = Math.Log10(numsarr[i]);
                    }
                    catch
                    { haveerror = true; }
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "ln")
                {
                    try
                    {
                        numsarr[i] = Math.Log(numsarr[i]);
                    }
                    catch
                    { haveerror = true; }
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "Asin")
                {
                    try
                    {
                        if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                        numsarr[i] = Math.Asin(numsarr[i]);
                    }
                    catch
                    { haveerror = true; }
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "Acos")
                {
                    try
                    {
                        if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                        numsarr[i] = Math.Acos(numsarr[i]);
                    }
                    catch
                    { haveerror = true; }
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "Atan")
                {
                    try
                    {
                        if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                        numsarr[i] = Math.Atan(numsarr[i]);
                    }
                    catch
                    { haveerror = true; }
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "Asinh")
                {
                    try
                    {
                        if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                        numsarr[i] = Math.Asinh(numsarr[i]);
                    }
                    catch
                    { haveerror = true; }
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "Acosh")
                {
                    try
                    {
                        if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                        numsarr[i] = Math.Acosh(numsarr[i]);
                    }
                    catch
                    { haveerror = true; }
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "Atanh")
                {
                    try
                    {
                        if (Deg == true) { numsarr[i] = numsarr[i] * Math.PI / 180; }
                        numsarr[i] = Math.Atanh(numsarr[i]);
                    }
                    catch
                    { haveerror = true; }
                    chararr.RemoveAt(i);
                    i--;
                }
            }

        }

        private void DivideAndMultiplyEquals()// like a  ÷ × + -  % √ ² 
        {
            try
            {
                if (haveerror == true)
                {
                    return;
                }

                for (int i = 0; i < chararr.Count; i++)



                    if (chararr[i] == "×")
                    {

                        numsarr[i] = (numsarr[i] * numsarr[i + 1]);
                        chararr.RemoveAt(i);
                        numsarr.RemoveAt(i + 1);

                        break;
                    }
                    else if (chararr[i] == "÷")
                    {

                        if (numsarr[i + 1] == 0) { haveerror = true; break; }  //divide on null
                        numsarr[i] = (numsarr[i] / numsarr[i + 1]);
                        chararr.RemoveAt(i);
                        numsarr.RemoveAt(i + 1);


                        break;
                    }
            }
            catch
            {
                haveerror = true;
            }
        }
        private void PlusAndMinusEquals()// like a  ÷ × + -  % √ ² 
        {
            try
            {
                for (int i = 0; i < chararr.Count; i++)
                    if (chararr[i] == "+")
                    {
                        numsarr[i] = (numsarr[i] + numsarr[i + 1]);
                        chararr.RemoveAt(i);
                        numsarr.RemoveAt(i + 1);

                        break;
                    }
                    else if (chararr[i] == "-")
                    {
                        numsarr[i] = (numsarr[i] - numsarr[i + 1]);
                        chararr.RemoveAt(i);
                        numsarr.RemoveAt(i + 1);

                        break;
                    }
            }
            catch
            {
                haveerror=true;
            }

        }
        internal void ClearData()
        {
            numsarr.Clear();
            chararr.Clear();
            haveerror = false;
        }

    }
}
