using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
namespace Calculator
{//test
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private Calculator calculator = new Calculator();
        private readonly string[] enginerschars = { "log", "ln","x!", "x³", "sin", "cos", "tan","sinh","cosh","tanh" }; //"sin", "cos", "x³","tan", "log", "ln", "!" 
        private readonly string[] enginersnums = { "e", "φ"};
        private readonly char[] CanEqualsChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '%', 'π', 'e', 'φ', '³', '²', '!' };

        private bool Dec = true;
        private bool Oct = false;
        private bool Bin = false;

        private protected bool Rad = true;
        private protected bool Deg = false;

        private bool inv = false;

        private void CanEquals()
        {
            if  (mainlabel.Text == string.Empty | mainlabel.Text == "")
            {
                return;
            }
            char temp = mainlabel.Text[mainlabel.Text.Length - 1];
            for (int i = 0; i < CanEqualsChars.Length;i++)
            {
                if (CanEqualsChars[i] == temp)
                {
                    Main();
                    break;
                }
            }
        }
        private void Minusreverse(object sender,EventArgs e)
        {
            if (mainlabel.Text == "" | mainlabel.Text == string.Empty) { mainlabel.Text = "-"; return; }
            if (mainlabel.Text == "-") { mainlabel.Text = string.Empty;  return; }
            if (mainlabel.Text.StartsWith("-")) { mainlabel.Text = mainlabel.Text.Substring(1);}
            else if  (!mainlabel.Text.StartsWith("-")) {mainlabel.Text = "-" + mainlabel.Text;}

            CanEquals();
        }
        private void numsbuttonclick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            
            if (mainlabel.Text.EndsWith("%") | mainlabel.Text.EndsWith("²")|mainlabel.Text.EndsWith("³") | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ") | mainlabel.Text.EndsWith("!"))
            {
                return;
                
            }
            if  (btn.Text == "π")
            {
                if (mainlabel.Text == string.Empty | mainlabel.Text == "")
                {
                    mainlabel.Text += btn.Text;
                    Main();
                    return;
                }

                char temp  = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9' | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ")) 
                { return ; }
            }
            else if (btn.Text == "e")
            {
                if (mainlabel.Text == string.Empty | mainlabel.Text == "")
                {
                    mainlabel.Text += btn.Text;
                    Main();
                    return;
                }

                char temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9' | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ"))
                { return; }
            }
            else if (btn.Text == "φ")
            {
                if (mainlabel.Text == string.Empty | mainlabel.Text == "")
                {
                    mainlabel.Text += btn.Text;
                    Main();
                    return;
                }

                char temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9' | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ"))
                { return; }
            }
            mainlabel.Text += btn.Text;
            Main();
         }
        private void charbuttonclick(object sender, EventArgs e)  //  доделать чтобы знак ⅟ₓ можно было использовать после корня
        {
            Button btn = (Button)sender;
            if (btn.Text == "x!")// куб
            {
                if (mainlabel.Text == string.Empty | mainlabel.Text == "") { return; }
                char temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9' | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ"))
                {
                    mainlabel.Text += "!";
                    Main();
                    return;
                }
                else return;
            }
            if (btn.Text == "sin" | btn.Text == "cos" | btn.Text == "tan" | btn.Text == "sinh" | btn.Text == "cosh" | btn.Text == "tanh" | btn.Text == "log" |  btn.Text ==  "ln" 
                | btn.Text == "Asin" | btn.Text == "Acos" | btn.Text == "Atan" | btn.Text == "Asinh" | btn.Text == "Acosh" | btn.Text == "Atanh")  
            {
                if (mainlabel.Text.EndsWith("+") | mainlabel.Text.EndsWith("-") | mainlabel.Text.EndsWith("÷")
                  | mainlabel.Text.EndsWith("×") | mainlabel.Text == string.Empty | mainlabel.Text == "" | mainlabel.Text == " ")
                {
                    mainlabel.Text += btn.Text;  
                    return;
                }
                else return;

            }
            if  (btn.Text == "⅟ₓ")  // единицу от числа ⁻¹  
            {
                if (mainlabel.Text.EndsWith("+") | mainlabel.Text.EndsWith("-") | mainlabel.Text.EndsWith("÷")
                  | mainlabel.Text.EndsWith("×") | mainlabel.Text == string.Empty | mainlabel.Text == "" | mainlabel.Text == " ")
                {
                    mainlabel.Text += "⅟";  //  ⅟
                    return;
                }
                else return;
            }

            if (btn.Text == "x³")// куб
            {
                if (mainlabel.Text == string.Empty | mainlabel.Text == "") { return; }
                char temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9' 
                    | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ")| temp == '!')
                {
                    mainlabel.Text += "³";
                    Main();
                    return;
                }
                else return;
            }
            if (btn.Text == "x²")// квадрат
            {
                if (mainlabel.Text == string.Empty |  mainlabel.Text == "") { return; }
                char  temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9'
                    | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ") | temp == '!' )
                {
                    mainlabel.Text += "²";
                    Main();
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
                if (mainlabel.Text == string.Empty | mainlabel.Text == "")
                { return; }
                char temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9'
                    | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ") |  temp == '²' | temp == '³'|temp == '!')
                {
                    mainlabel.Text += btn.Text;
                    Main();
                    return;
                }
                else { return ; }
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
                if (mainlabel.Text.EndsWith("+") | mainlabel.Text.EndsWith(",") | mainlabel.Text.EndsWith("-") | mainlabel.Text.EndsWith("√") | mainlabel.Text.EndsWith("ln") | mainlabel.Text.EndsWith("log"))
                {
                    return;
                }
                else if (mainlabel.Text.EndsWith("÷") | mainlabel.Text.EndsWith("×") | mainlabel.Text.EndsWith("n") | mainlabel.Text.EndsWith("s") | mainlabel.Text.EndsWith("h") | mainlabel.Text.EndsWith("⅟", StringComparison.OrdinalIgnoreCase))
                {
                    if (btn.Text == "-")
                    {
                        mainlabel.Text += btn.Text;
                    }
                }
                else  if (btn.Text == ",")
                {

                    string inputString = mainlabel.Text;
                    if (inputString.EndsWith("!")) { return; }
                    char[] delimiterChars = { ' ', '÷', '+', '-', '×', '%', '\t', '√', '²', '³', '⅟','s','i','n','c','o','t','a','n','!','l','o','n','g','A' }; 
                    string[] nums = inputString.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                    bool containsdot = nums[nums.Length - 1].Contains(","); 
                    if (inputString.EndsWith("e") | inputString.EndsWith("φ") | inputString.EndsWith("t") | inputString.EndsWith("π")) //"e","φ","t","π"
                    {  return; }
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
         
        private void Main() // like a  ÷ × + -  % √ ² 
        {
            NumberSystems system = NumberSystems.Dec;
            if (Bin == true) {system = NumberSystems.Bin;}
            else if (Oct == true) { system = NumberSystems.Oct; }
            secondlabel.Text = calculator.MainEquals(system,mainlabel.Text,Deg);
        }
        private void ClearAll(object sender, EventArgs e)
        {
            calculator.ClearData();
            mainlabel.Text = string.Empty;
            secondlabel.Text = string.Empty;
        }
        private void EqualsButtonClick(object sender, EventArgs e)
       {
            string output = secondlabel.Text;
            if (output == string.Empty | output == "" | output == " " | calculator.Haveerror == true| output == "Eror"
                | output == "-бесконечность" | output == "бесконечность" | output == "Не число" | output == "Syntax Erorr" | output == "не число") 
            {
                return;
            }
            if (secondlabel.Text.EndsWith("₂"))
            {
                //  if (secondlabel.Text.Length > 1) { output = secondlabel.Text.Substring(0, secondlabel.Text.Length - 2); }
                return;
            }
            if (secondlabel.Text.EndsWith("₈"))
            {
                return;
            }
            mainlabel.Text = output;
            secondlabel.Text = string.Empty;
            calculator.ClearData();
       }
        private void BackSpaceButtonClick(object sender, EventArgs e)
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
                else if (inputlabel[lastindex] == 'n' & inputlabel[lastindex-2] == 's' | inputlabel[lastindex] == 'n' & inputlabel[lastindex - 2] == 't' | inputlabel[lastindex] == 's' | inputlabel[lastindex] == 'g') //  sin cos tan log
                {
                    if(lastindex >= 3) 
                    {
                        if (inputlabel[lastindex-3] == 'A')
                        {
                            inputlabel = inputlabel.Substring(0, lastindex - 3);  // Asin  Acos  Atan
                        }
                        else
                        {
                            inputlabel = inputlabel.Substring(0, lastindex - 2); // sinh  cosh  tanh log
                        }
                    }
                    else
                    {
                        inputlabel = inputlabel.Substring(0, lastindex - 2); // sinh  cosh  tanh log
                    }
                }
                else if (inputlabel[lastindex] == 'h') // если  в кноце h 
                {
                    if (lastindex >= 4) // если размер больше 4 индексов
                    {
                        if (inputlabel[lastindex-4] == 'A') //   если с кноце -4 char  равен A 
                        {
                            inputlabel = inputlabel.Remove(lastindex-4);  //  Asinh  Acosh  Atanh
                        }
                        else
                        { inputlabel = inputlabel.Substring(0, lastindex - 3); }// sinh  cosh  tanh
                    }
                    else
                    { inputlabel = inputlabel.Substring(0, lastindex - 3); } // sinh  cosh  tanh
                }
                else if (inputlabel[lastindex] == 'n' & inputlabel[lastindex - 1] == 'l') // если удаление посреди строки пример: 2*sin0,3+ln34/2
                {
                    inputlabel = inputlabel.Remove(lastindex - 1);
                }
                else
                {
                    inputlabel = inputlabel.Remove(lastindex);
                }
            }
            else if (lastindex == 1 & inputlabel[lastindex] == 'n') //  ln
            {
                if (inputlabel[lastindex - 1] == 'l')
                {
                    inputlabel = inputlabel.Remove(lastindex - 1);
                }
            }
            else
            { 
              inputlabel = inputlabel.Remove(lastindex);
            }
            mainlabel.Text = inputlabel;
                try
                {
                    char temp = mainlabel.Text[mainlabel.Text.Length-1];

                    if (temp == '0'|temp == '1'|temp == '2'|temp == '3'|temp == '4'|temp == '5'|temp=='6'|temp == '7'|temp=='8'|temp=='9' 
                        | temp == '%' | temp == 'π' | temp == 'e' | temp == 'φ' | temp == '³' | temp == '²' | temp  == '!')  // вычислять если возможно после удаления последнего элемента
                    {
                            Main();
                    }
                    if (mainlabel.Text == "-") { secondlabel.Text = string.Empty; return; }
                }
                catch { ClearAll(sender, e); }

        }

    }
}
