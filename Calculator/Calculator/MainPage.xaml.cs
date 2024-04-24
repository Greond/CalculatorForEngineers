using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
namespace Calculator
{
    public partial class MainPage : ContentPage
    {
      
        public MainPage()
        {
            InitializeComponent();

        }
        List<double> numsarr = new List<double>();
        List<string> chararr = new List<string>(); // like a "plus,minus,multi,divide"
        
        private bool haverror = false;

         public void numsbuttonclick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (mainlabel.Text.EndsWith("%"))
            {
                return ;
            }
            mainlabel.Text += btn.Text;
            try
            {
                Getthecurrentexample();
                if (numsarr.Count > 1)
                {

                    _Equals();
                }
                else
                {
                    if (mainlabel.Text.StartsWith("√") & numsarr.Count == 1)
                    {
                        _Equals();
                    }
                }
            }
            catch { }
            
            }
        public void charbuttonclick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Text == "X²")// квадрат
            {
                char  temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9')
                {
                    mainlabel.Text += "²";
                    Getthecurrentexample();
                    return;
                }
                else return;
            }

            if (btn.Text == "√") // корень
            {
                if (mainlabel.Text.EndsWith("+") | mainlabel.Text.EndsWith("-") | mainlabel.Text.EndsWith("÷")
                   | mainlabel.Text.EndsWith("×") | mainlabel.Text == string.Empty | mainlabel.Text == "" | mainlabel.Text == " ")
                {
                    mainlabel.Text += btn.Text;
                }
                else { return; }
            }

            if  (btn.Text == "%")
            {
                
                if (mainlabel.Text.EndsWith("+")  | mainlabel.Text.EndsWith(",") | mainlabel.Text.EndsWith("√")
                    | mainlabel.Text.EndsWith("-") | mainlabel.Text.EndsWith("÷") | mainlabel.Text.EndsWith("%")
                    | mainlabel.Text.EndsWith("×") | mainlabel.Text == string.Empty | mainlabel.Text == "" | mainlabel.Text == " " )
                {
                    return;
                }
                else
                {
                    mainlabel.Text += btn.Text;
                    
                Getthecurrentexample();
                if (numsarr.Count == 1)
                {
                    int f = 0;
                    percentages(ref f,false);
                    _Equals();
                        return;
                }
                _Equals();
                    return;
                }
            }


            if (mainlabel.Text == "" | mainlabel.Text == null | mainlabel.Text == string.Empty)
            {
                if (btn.Text == "-")
                {
                    mainlabel.Text += btn.Text;
 
                }
                
            }
            else
            {
                if (mainlabel.Text.EndsWith("+")  | mainlabel.Text.EndsWith(",") | mainlabel.Text.EndsWith("-") | mainlabel.Text.EndsWith("√"))
                {
                    return;
                }
                else if (mainlabel.Text.EndsWith("÷") | mainlabel.Text.EndsWith("×") )
                {
                    if (btn.Text == "-")
                    {
                        mainlabel.Text += btn.Text;
                    }
                }
                else  if (btn.Text == ",")
                {

                    string inputString = mainlabel.Text;
                    char[] delimiterChars = { ' ', '÷', '+', '-', '×', '\t' , };
                    string[] nums = inputString.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                    bool containsdot = nums[nums.Length - 1].Contains(","); 
                    if (containsdot == true)
                    {
                        return ;
                    }
                    else
                    {
                        mainlabel.Text += btn.Text;
                    }
                }
                else
                {
                    mainlabel.Text += btn.Text;
                }
            }



    }
        // почему я нимогу создать обьект класса  канкулятоra
         
        private void _Equals() // like a  ÷ × + -  % √ ² 
        {
            firstcharsprocessing(); // обработка доп  знаков

            int firstpriority  = 0;
            int secondpriority  = 0;
                foreach (string _char in chararr)
                {
                    if (_char == "÷" | _char == "×")
                    {
                        firstpriority = firstpriority + 1;
                    }
                }
                foreach (string _char in chararr)
                {
                    if (_char == "+" | _char == "-")
                    {
                        secondpriority = secondpriority + 1;
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
          
           
            secondlabel.Text = numsarr[0].ToString();
            if  (haverror ==  true) { secondlabel.Text = "Erorr"; }
        }

         public void charsort(string str, int pos) // str это строка с знаками или знаком, а pos это номер в цикле for 
        {
            void  skipminus(int i) // i is a number of char position in  string 
            {

                if (str[i] == '-' & str[i-1] == '÷' | str[i-1] == '×')
                {
                    numsarr[pos + 1] = numsarr[pos + 1] * -1;
                }
                else { chararr.Add(str[i].ToString()); }
            }

            if (str.Length == 1)
            {
                chararr.Add(str);
            }
            else if (str.Length == 2)// обрабатываем исключение если 2 знака
            {
                chararr.Add(str[0].ToString());
                skipminus(1);
            }
            else if (str.Length  == 3)// обрабатываем исключение если 3 знака
            {
                chararr.Add(str[0].ToString());
                skipminus(1);
                skipminus(2);
            }
            else if (str.Length  == 4)// обрабатываем исключение если 4 знака
            {
                chararr.Add(str[0].ToString());
                skipminus(1); 
                skipminus(2);
                skipminus(3);
            }
            else if (str.Length == 5) // if chars count is 5
            {
                chararr.Add(str[0].ToString());
                skipminus(1);
                skipminus(2);
                skipminus(3);
                skipminus(4);
            }
        }
        private void Getthecurrentexample() // get currect  string in label and split is to a chars  and numbers 
        { //    ÷   ×   -  +   

            numsarr.Clear();
            chararr.Clear();
            haverror = false;

            string inputString = mainlabel.Text;
            char[] delimiterChars = {' ', '÷', '+', '-', '×', '%', '\t', '√', '²'}; // ÷ + - × % √ ²
            string[] nums = inputString.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nums.Length; i++)
            {
                try
                {
                    bool E_nums = nums[i].Contains('E'); // проверка на Е числа
                    if (E_nums)
                    {
                        string tempstr = nums[i] + inputString[inputString.IndexOf('E')+1] + nums[i+1]; 
                        double temp = double.Parse(tempstr);
                        numsarr.Add(temp);
                        removeat(ref nums, i+1);
                        string str = Convert.ToString(inputString[inputString.IndexOf('E') + 1]);

                        inputString = inputString.Replace(inputString[inputString.IndexOf('E')+1], '#');
                        inputString = inputString.Replace(inputString[inputString.IndexOf('E')], '#');
                    }
                    else {
                        double temp = double.Parse(nums[i]);

                        numsarr.Add(temp);
                        Console.WriteLine(nums[i]);
                    }
                }
                catch (Exception ex)
                {
                    warn(ex);
                }
            }
            

            void removeat(ref string[] array,int index)
            {
                string[] newarray = new string[array.Length -1];

                for (int i = 0; i < index; i++)
                {
                    newarray[i] = array[i];
                }
                for (int i = index +1;i < array.Length;i++)
                {
                    newarray[i-1] = array[i];
                }
                array = newarray;
            }

            char[] delimetrnums = { ',', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' ,' ' ,'E','Е','e','е', '#' };
            string[] chars = inputString.Split(delimetrnums, System.StringSplitOptions.RemoveEmptyEntries);
            if (mainlabel.Text.StartsWith("-", true, null)) // первое число минусовое
            {
                numsarr[0] = numsarr[0] * -1;
                removeat(ref chars, 0);
            }
            for (int i = 0; i < chars.Length; i++) 
            {
                try
                {
                    charsort(chars[i],i);
                }
                catch (Exception ex)
                {
                    warn(ex);

                }
            }
        }
        private async void warn(Exception exception) // warn  method
        {
            string temp = "Source: " + exception.Source.ToString()
                + "\nDesroption: " + exception.Message 
                + "\nMethod: " + exception.TargetSite.ToString()
                + "\nТрасировка стека: " + exception.StackTrace.ToString();
                
            
            await DisplayAlert("Ошибка", temp, "Cancel");
            
        }

        private  void firstcharsprocessing()
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
                    if (i!=0 & chararr[i - 1] == "+" | chararr[i - 1] == "-")
                    {
                        numsarr[i] = numsarr[i] / 100 * numsarr[i-1];
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
                else if (chararr[i] == "²")
                {
                    numsarr[i] = Math.Pow(numsarr[i], 2);
                    chararr.RemoveAt(i);
                    i--;
                }
            }

        }
        private void square()
        {
            try// like a  ÷ × + -  % √ ² 
            {
                int temp = 0;
                for (int i = 0; i < chararr.Count; i++)
                {
                    if (chararr[i] == "%") { temp++; }
                    if (chararr[i] == "√") { temp++; }
                    if (chararr[i] == "²")
                    {

                            numsarr[i - temp] = Math.Pow(numsarr[i - temp],2);
                            chararr.RemoveAt(i);
                            i--;


                    }
                }
            }
            catch (Exception ex) { warn(ex); }
        }
       private void rootof()
        {
            try// like a  ÷ × + -  % √ ² 
            {
                int temp = 0;
                for (int i = 0; i < chararr.Count; i++)
                {
                    if (chararr[i] == "%") { temp++; }
                    if (chararr[i] == "√")
                    {
                        if (numsarr[i - temp] > 0)
                        {
                            numsarr[i - temp] = Math.Sqrt(numsarr[i - temp]);
                            chararr.RemoveAt(i);
                            i--;
                        }
                        else if (numsarr[i - temp] < 0)
                        {
                            numsarr[i - temp] = Math.Sqrt(numsarr[i - temp] * -1) * -1;
                            chararr.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            catch (Exception ex) { warn(ex); }
        }

        private void percentages()
        {

        }
        private void percentages(ref int nowchar,bool IsMultiOrDivide ) // обработка процентов 
        {
            if (numsarr.Count == 1)
            {
                if (chararr[0] == "%")
                {

                    numsarr[0] = numsarr[0] / 100;
                }

                return;
            }
            if (IsMultiOrDivide == true) //для умножения и деления
            {
                int temp =  nowchar;
                if (temp != 0) 
                {
                    if (chararr[temp - 1] == "%")
                    {
                        numsarr[temp - 1] = numsarr[temp - 1] / 100;
                        chararr.RemoveAt(temp - 1);
                        temp--;
                    }
                }
                if (chararr.Count > temp +1)
                {
                    if (chararr[temp + 1] == "%")
                    {
                        numsarr[temp + 1] = numsarr[temp + 1] / 100;
                        chararr.RemoveAt(temp + 1);
                    }
                }
                nowchar = temp;

            }
            else if (IsMultiOrDivide == false) // для плюса и  минуса
            {
                int temp = nowchar;

                if (temp != 0)
                {
                    if (chararr[temp - 1] == "%")
                    {
                        numsarr[temp - 1] = numsarr[temp - 1] / 100;
                        chararr.RemoveAt(temp - 1);
                        temp--;
                    }
                }
                if (chararr.Count > temp +1)
                {
                    if (chararr[temp + 1] == "%")
                    {
                        numsarr[temp + 1] = numsarr[temp + 1] / 100 * numsarr[temp];
                        chararr.RemoveAt(temp + 1);
                    }
                }
                nowchar = temp;
            }
            
        }

        public void DivideAndMultiplyEquals()// like a  ÷ × + -  % √ ² 
        {
            try
            {
           if (haverror == true )
            {
                return;
            }
           
            for(int i = 0; i < chararr.Count; i++)
                    
                    
                    
                if (chararr[i] == "×")
                {

                        numsarr[i] = (numsarr[i] * numsarr[i + 1]);
                        chararr.RemoveAt(i);
                        numsarr.RemoveAt(i+1);
                    
                    break;
                }
                else if (chararr[i] == "÷")
                {

                            if (numsarr[i+1] == 0) { haverror= true; break; }  //divide on null
                        numsarr[i] = (numsarr[i] / numsarr[i + 1]);
                        chararr.RemoveAt(i);
                        numsarr.RemoveAt(i + 1);
                       
                    
                    break;
                }
            }
            catch (Exception e)
            {
                warn(e);
            }
        }
        public void PlusAndMinusEquals()// like a  ÷ × + -  % √ ² 
        {
            try { 
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
            catch (Exception e)
            {
                warn(e);
            }

        }

        private void ClearAll(object sender, EventArgs e)
        {
            
            numsarr.Clear();
            chararr.Clear();
            mainlabel.Text = string.Empty;
            secondlabel.Text = string.Empty;

        }

       public void EqualsButtonClick(object sender, EventArgs e)
        {
            if (secondlabel.Text == string.Empty | secondlabel.Text == "" | secondlabel.Text == " " | haverror == true | secondlabel.Text == "-бесконечность" | secondlabel.Text == "бесконечность")
            {
                return;
            }


            mainlabel.Text = secondlabel.Text;
            secondlabel.Text = string.Empty;

           

            numsarr.Clear();
            chararr.Clear();
        }
        private void  BackSpaceButtonClick(object sender, EventArgs e)
        {
            
            string inputlabel = mainlabel.Text;
            int lastindex = mainlabel.Text.Length-1;

            if (lastindex == -1) { return; }
            if  (lastindex >= 2)
            {
                if (inputlabel[lastindex - 2] == 'E')
                {
                    inputlabel = inputlabel.Remove(lastindex - 2);
                }
                else
                {
                    inputlabel = inputlabel.Remove(lastindex);
                }
            }
            else
            { 
              inputlabel = inputlabel.Remove(lastindex);
            }
            mainlabel.Text = inputlabel;



            if  (mainlabel.Text.EndsWith("%"))  // вычислять если возможно после удаления последнего элемента, если последний знак %
            {
                Getthecurrentexample();
                if (numsarr.Count == 1)
                {
                    int f = 0;
                    percentages(ref f, false);
                    _Equals();
                    return;
                }
                _Equals();
                return;
            }
            
            else 
                try
                {
                    char temp = mainlabel.Text[lastindex-1];

                    if (temp == '0'|temp == '1'|temp == '2'|temp == '3'|temp == '4'|temp == '5'|temp=='6'|temp == '7'|temp=='8'|temp=='9')  // вычислять если возможно после удаления последнего элемента
                    {

                        Getthecurrentexample();
                        if (numsarr.Count > 1)
                        {
                            _Equals();
                        }
                    }
                }
                catch { }

        }
    }
}
