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
{
    public class Convector
    {
        public string ConvertTo2(string num, int round = 5)
        {
            try { 
            bool firstminus = false;
                if (num.StartsWith("-"))  // если число отриательное
                {firstminus = true; num = num.Substring(1);}
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
            catch { return "large number"; }
        }
        public string ConvertTo8(string num)
        {
            try { 
            long left  =  0;// целая часть
            long right  = 0;// после запятой часть
            string result = string.Empty;  // куда  будем записывать результат
            bool firstminus = false;
            if (num.StartsWith("-"))  // если число отриательное
            { firstminus = true; num = num.Substring(1); }

            if (num.Contains(".") | num.Contains(",")) // проверяем есть ли дробная часть
            {
                left = Convert.ToInt64(num.Split('.',',')[0]); // присваиваем целую а затем и дробную часть
                right = Convert.ToInt64(num.Split('.', ',')[1]);
            }
            else { left = Convert.ToInt64(num);} // если нету дробной части 
            
            while (true) 
            {
                result = Convert.ToString(left % 8) + result;
                left = left / 8;
                if (left == 0) { break; }
            }
            if (num.Contains(".") | num.Contains(",")) // проверяем есть ли дробная часть
            {
                result +=",";
                int limit =  right.ToString().Count(); // количесвто цифр за превешением которых  добавляем 

                for (int i = 0; i < 5; i++)
                {
                    right = right * 8;
                    if  (right.ToString().Count() > limit) 
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
            catch { return "large number"; }

        }
    }
    public partial class MainPage : ContentPage
    {
      
        public MainPage()
        {
            InitializeComponent();
        }
        private double _oldwidth, _oldhight;
        protected override void OnSizeAllocated(double width, double height)
        {
         base.OnSizeAllocated(width, height);
            if(width != _oldwidth  || height != _oldhight)
            {
                _oldwidth = width;
                _oldhight = height;
                if (width>height)
                { // landshaft
                    mainlabel.Margin = new Thickness(0, 10, 10, 0);
                    secondlabel.Margin = new Thickness(0, 0, 10, 0);
                    GridForGrids.Margin = new Thickness(0, 0, 0, 0);
                    GridForGrids.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star)}) ;
                    Grid.SetColumn(MainGrid, 1);

                    foreach (Button btn in MainGrid.Children)
                    {
                        
                        btn.FontSize = 14;
                        btn.FontAttributes = FontAttributes.Bold;
                    }
                    mainlabel.FontSize = 30;
                    secondlabel.FontSize = 20;

                    Grid Grid2 = new Grid();
                    
                    GridForGrids.Children.Add(Grid2);
                     // GridForGrids.ColumnDefinitions.Contains(new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) });
                    GridForGrids.ColumnDefinitions[0].Width = new GridLength(3,GridUnitType.Star);
                    Grid.SetColumn(Grid2, 0);
                    Grid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    Grid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    Grid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    Grid2.RowDefinitions.Add(new RowDefinition { Height =  new GridLength (1, GridUnitType.Star) });
                    Grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    Grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    Grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    Grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    Grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    
                    // inv switch
                    Label label = new Label();
                    label.FontSize = 17;
                    label.Text = "Inv";
                    label.TextColor = Color.FromHex("#bdbdc7fc");
                    label.FontAttributes= FontAttributes.Bold;
                    label.Margin = new Thickness(0,6, 0, 0);

                    Switch @switch = new Switch();
                    @switch.OnColor = Color.FromHex("#318ef7");
                    @switch.ThumbColor = Color.White;
                    inv = false;
                    @switch.Toggled += (sender, e) =>
                    {
                        inv = !inv;
                        if (inv ==  true)
                        { // 14 = это кнопка с tanh
                            for (int i = 12; i < 18; i++)
                            {
                                Button Abutton = Grid2.Children[i] as Button;
                                Abutton.TextTransform = TextTransform.None;
                                Abutton.Text = "A" + Abutton.Text;
                            }
                        }
                        else
                        {
                            for (int i = 12; i < 18; i++)
                            {
                                Button Abutton = Grid2.Children[i] as Button;
                                Abutton.Text = Abutton.Text.Substring(1);
                            }
                        }
                    };

                    StackLayout stackLayout = new StackLayout();
                    stackLayout.Orientation = StackOrientation.Horizontal;
                    stackLayout.Children.Add(@switch);
                    stackLayout.Children.Add(label);
                    stackLayout.BackgroundColor = Color.FromHex("#333233");

                    Frame frame = new Frame();
                    frame.Content = stackLayout;
                    frame.CornerRadius = 30;
                    frame.Padding = 0;
                    Grid2.Children.Add(frame);
                    Grid.SetColumn(frame, 0); 
                    Grid.SetRow(frame, 5);
                    // inv switch/>
                    // deg 
                    RadioButton deg =  new RadioButton ();
                    deg.GroupName = "corner";
                    deg.Content = "Deg";
                    deg.BackgroundColor = Color.FromHex("#333233");
                    deg.CornerRadius = 30;
                    deg.FontSize = 15;
                    deg.FontAttributes = FontAttributes.Bold;
                    deg.TextColor = Color.FromHex("#bdbdc7fc");
                    deg.CheckedChanged += (sender, e) =>
                    {
                        Deg = true;
                        Rad = false;
                        CanEquals();
                    };
                    Grid2.Children.Add(deg);
                    Grid.SetColumn(deg, 1);
                    Grid.SetRow(deg, 5);
                    //  deg/>
                    // rad  
                    RadioButton rad = new RadioButton();
                    rad.GroupName = "corner";
                    rad.Content = "Rad";
                    rad.BackgroundColor = Color.FromHex("#333233");
                    rad.CornerRadius = 30;
                    rad.FontSize = 15;
                    rad.FontAttributes = FontAttributes.Bold;
                    rad.TextColor = Color.FromHex("#bdbdc7fc");
                    if (Rad == true)
                    { rad.IsChecked = true; }
                    else
                    { deg.IsChecked = true; }
                    rad.CheckedChanged += (sender, e) =>
                    {
                        Rad = true;
                        Deg = false;
                        CanEquals();
                    };
                    Grid2.Children.Add(rad);
                    Grid.SetColumn(rad, 2);
                    Grid.SetRow(rad, 5);
                    // rad/>
                    addRadioButton("Bin₂", "systems", 4, 0);
                    addRadioButton("Oct₈", "systems", 4, 1);
                    addRadioButton("Dec₁₀", "systems", 4, 2);


                       int n =0;
                      int c = 0;
                     for (int i = 0;i < 5; i++)
                      {
                          for (int j = 0;j < 3;j++)
                          {
                              if(enginersnums.Length > n)
                              {
                                  addbutton(enginersnums[n], false, i, j);
                                  n++;
                              }
                              else
                              {
                                 if (enginerschars.Length > c)
                                  {
                                      addbutton(enginerschars[c], true, i, j);
                                      c++;
                                  }

                              }

                          }
                      }

                    void addbutton(string text,bool ischar, int row,int column)
                    {
                        if (ischar) 
                        {
                            Button button = new Button();
                            button.Text = text;
                            button.TextTransform = TextTransform.Lowercase;
                            button.Style = InfoStyle2;
                            button.FontAttributes = FontAttributes.Bold;
                            button.Clicked += charbuttonclick;
                            Grid2.Children.Add(button);
                            Grid.SetColumn(button, column);
                            Grid.SetRow(button, row);
                        }
                        else
                        {
                            Button button = new Button();
                            button.Text = text;
                            button.TextTransform = TextTransform.Lowercase;
                            
                            button.Style = InfoStyle2;
                            button.FontAttributes = FontAttributes.Bold;
                            button.Clicked += numsbuttonclick;
                            Grid2.Children.Add(button);
                            Grid.SetColumn(button, column);
                            Grid.SetRow(button, row);
                        } 
                            
                    }
                    void addRadioButton(string text,string groupname,int row,int column)
                    {
                        RadioButton radioButton = new RadioButton();
                        radioButton.GroupName = groupname;
                        radioButton.Content = text;
                        radioButton.BackgroundColor = Color.FromHex("#19155c");
                        radioButton.CornerRadius = 30;
                        radioButton.FontSize = 15;
                        radioButton.FontAttributes = FontAttributes.Bold;
                        radioButton.TextColor = Color.FromHex("#bdbdc7fc");
                        radioButton.CheckedChanged += (sender, e) =>
                        {
                            if (radioButton.IsChecked == false) { return; }
                            switch (radioButton.Content.ToString())
                            {
                                case "Dec₁₀":
                                    Dec = true;
                                    Bin = false;
                                    Oct = false;
                                    break;
                                case "Bin₂":
                                    Bin = true;
                                    Oct = false;
                                    Dec = false;
                                    break;
                                case "Oct₈":
                                    Oct = true;
                                    Bin = false;
                                    Dec = false; 
                                    break;
                            }
                            CanEquals();
                        };
                        Grid2.Children.Add(radioButton);
                        Grid.SetColumn(radioButton, column);
                        Grid.SetRow(radioButton, row);

                    }
                }
                else { //  Portriet
                    mainlabel.Margin = new Thickness(0, 50, 10, 0);
                    secondlabel.Margin = new Thickness(0, 0, 20, 0);
                    GridForGrids.Margin = new Thickness(0, 11, 0, 0);
                    if (GridForGrids.ColumnDefinitions.Count > 1)
                    {
                        GridForGrids.Children.RemoveAt(1);

                    }
                    if (GridForGrids.ColumnDefinitions.Count > 1) 
                    {

                        GridForGrids.ColumnDefinitions.RemoveAt(1);
                        Grid.SetColumn(MainGrid, 0);

                    }
                    foreach(Button btn in MainGrid.Children)
                    {
                        btn.FontSize =  40;
                        btn.FontAttributes = FontAttributes.None;
                    }
                    mainlabel.FontSize = 40;
                    secondlabel.FontSize= 30;
                    
                }
            }
            
        }
        

        List<double> numsarr = new List<double>();
        List<string> chararr = new List<string>();
        readonly string[] enginerschars = { "log", "ln","x!", "x³", "sin", "cos", "tan","sinh","cosh","tanh" }; //"sin", "cos", "x³","tan", "log", "ln", "!" 
        readonly string[] enginersnums = { "e", "φ"};
        
        private bool Dec = true;
        private bool Oct = false;
        private bool Bin = false;

        private bool Rad = true;
        private bool Deg = false;

        private bool inv = false;

        private bool haverror = false;

        
        private void CanEquals()
        {
            if  (mainlabel.Text == string.Empty | mainlabel.Text == "")
            {
                return;
            }
            char temp = mainlabel.Text[mainlabel.Text.Length - 1];

            if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9'
                | temp == '%' | temp == 'π' | temp == 'e' | temp == 'φ' | temp == '³' | temp == '²'| temp ==  '!') 
            {
                Getthecurrentexample();
                MainEquals();
            }
        }
        public void Minusreverse(object sender,EventArgs e)
        {
            if (mainlabel.Text == "" | mainlabel.Text == string.Empty) { mainlabel.Text = "-"; return; }
            if (mainlabel.Text == "-") { mainlabel.Text = string.Empty;  return; }
            if (mainlabel.Text.StartsWith("-")) { mainlabel.Text = mainlabel.Text.Substring(1);}
            else if  (!mainlabel.Text.StartsWith("-")) {mainlabel.Text = "-" + mainlabel.Text;}

            char temp = mainlabel.Text[mainlabel.Text.Length - 1];

            if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9'| temp == '%' | temp == 'π' |temp == 'e' | temp == 'φ')  
            {
                Getthecurrentexample();
                MainEquals();
            }
        }
         public void numsbuttonclick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            
            if (mainlabel.Text.EndsWith("%") | mainlabel.Text.EndsWith("²")|mainlabel.Text.EndsWith("³") | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ") | mainlabel.Text.EndsWith("!"))
            {
                return ;
                
            }
            if  (btn.Text == "π")
            {
                if (mainlabel.Text == string.Empty | mainlabel.Text == "")
                {
                    mainlabel.Text += btn.Text;
                    Getthecurrentexample();
                    MainEquals();
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
                    Getthecurrentexample();
                    MainEquals();
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
                    Getthecurrentexample();
                    MainEquals();
                    return;
                }

                char temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9' | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ"))
                { return; }
            }
            mainlabel.Text += btn.Text;
            Getthecurrentexample();
            MainEquals();
         }
        public void charbuttonclick(object sender, EventArgs e)  //  доделать чтобы знак ⅟ₓ можно было использовать после корня
        {
            Button btn = (Button)sender;
            if (btn.Text == "x!")// куб
            {
                if (mainlabel.Text == string.Empty | mainlabel.Text == "") { return; }
                char temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9' | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ"))
                {
                    mainlabel.Text += "!";
                    Getthecurrentexample();
                    MainEquals();
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
                    Getthecurrentexample();
                    MainEquals();
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
                    Getthecurrentexample();
                    MainEquals();
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
                char temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9'
                    | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ") |  temp == '²' | temp == '³'|temp == '!')
                {
                    mainlabel.Text += btn.Text;
                    Getthecurrentexample();
                    MainEquals();
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
        // почему я нимогу создать обьект класса  канкулятоra
         
        private void MainEquals() // like a  ÷ × + -  % √ ² 
        {
            firstcharsprocessing(); // обработка доп  знаков

            int firstpriority  = 0;
            int secondpriority  = 0;
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
            if (haverror == true) { secondlabel.Text = "Syntax Erorr"; }
            else
            {
                Convector convector = new Convector();
                if (Bin == true)
                { secondlabel.Text = convector.ConvertTo2(numsarr[0].ToString()) + "₂"; }
                else if (Oct == true)
                {
                    secondlabel.Text = convector.ConvertTo8(numsarr[0].ToString()) + "₈";
                }
                else
                {
                    secondlabel.Text = numsarr[0].ToString();
                }
            }
        }

         public void charsort(string str) // str это строка с знаками или знаком, а pos это номер в цикле for 
        {
            int checkchar()// крч чтобы выбрать нужное  число  нада  найти значимые  знаки после которых  точно идут новые числа типа как + - "÷" "×"
            {
                int  result = 0;
                for (int i = 0; i < chararr.Count; i++)
                {
                    if (chararr[i] == "×" | chararr[i] == "÷" | chararr[i] == "+" | chararr[i] == "-")
                        { result++;}
                }
                return result;
            }
            void  skipminus(int i) // i is a number of char position in  string 
            {
                int pos = checkchar();
                if (str[i] == '-' & str[i - 1] == '÷' | str[i] == '-' & str[i - 1] == '×')
                {
                    numsarr[pos ] = numsarr[pos ] * -1;
                }
                else if (str[i] == '-' & str[i-1] == 'n' | str[i] == '-' & str[i - 1] == 's' | str[i] == '-' & str[i - 1] == 'h')
                {
                    numsarr[pos]  = numsarr[pos] * -1;
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
                    if (str[i]== 'A')
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
            else if (str.Contains("sinh") | str.Contains("cosh")|str.Contains("tanh"))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == 's' | str[i] == 'c' | str[i] == 't')
                    {
                        if (str[i+3] == 'h')
                        {
                                chararr.Add(str[i]+"h");
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
            else if (str.Contains("sin") |  str.Contains("cos")| str.Contains("tan"))
            {
                int temp = str.Length - 1;
                for (int i = 0; i < temp+1;i++)
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
                            chararr.Add(str[i].ToString() );
                            i = i + 2;
                        }
                    }
                    else
                    {
                        if (i ==  0)
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

                for (int i = 0; i < str.Length;i++)
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
        private void Getthecurrentexample() // get currect  string in label and split is to a chars  and numbers 
        { //    ÷   ×   -  +   

            numsarr.Clear();
            chararr.Clear();
            haverror = false;

            string inputString = mainlabel.Text;
            char[] delimiterChars = { ' ', '÷', '+', '-', '×', '%', '\t', '√', '²', '³', '⅟', 's', 'i', 'n', 'c', 'o', 's', 't', 'a','n','h','!','l','o','g','A' }; // ÷ + - × % √ ² ⅟
            string[] nums = inputString.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nums.Length; i++)
            {
                try
                {
                    bool E_nums = nums[i].Contains('E'); // проверка на Е числа(степени)
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
                    else if (nums[i]  == "π")
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
            if (mainlabel.Text.StartsWith("-", true, null)) // первое число минусовое
            {
                numsarr[0] = numsarr[0] * -1;
                inputString = inputString.Substring(1);
            }
            char[] delimetrnums = { ',', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' ,' ' ,'E','Е','e','е', '#', 'π', 'φ' };
            string[] chars = inputString.Split(delimetrnums, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < chars.Length; i++) 
            {
                try
                {
                    charsort(chars[i]);
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
                    if (numsarr[i].ToString().Contains(",")) { haverror = true; }
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
                            haverror = true;
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
                    { haverror = true; }
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
                    { haverror = true; }
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
                    { haverror = true; }
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
                    { haverror = true; }
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
                    { haverror = true; }
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
                    { haverror = true; }
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
                    { haverror = true; }
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
                    { haverror = true; }
                    chararr.RemoveAt(i);
                    i--;
                }
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
            string output = secondlabel.Text;
            if (secondlabel.Text == string.Empty | secondlabel.Text == "" | secondlabel.Text == " " | haverror == true | secondlabel.Text == "-бесконечность" | secondlabel.Text == "бесконечность" | secondlabel.Text == "Не число")
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



            if  (mainlabel.Text.EndsWith("%"))  // вычислять если возможно после удаления последнего элемента, если последний знак %
            {
                Getthecurrentexample();
                if (numsarr.Count == 1)
                {
                    MainEquals();
                    return;
                }
                MainEquals();
                return;
            }
            
            else 
                try
                {
                    char temp = mainlabel.Text[mainlabel.Text.Length-1];

                    if (temp == '0'|temp == '1'|temp == '2'|temp == '3'|temp == '4'|temp == '5'|temp=='6'|temp == '7'|temp=='8'|temp=='9' 
                        | temp == '%' | temp == 'π' | temp == 'e' | temp == 'φ' | temp == '³' | temp == '²' | temp  == '!')  // вычислять если возможно после удаления последнего элемента
                    {
                        Getthecurrentexample();
                            MainEquals();
                    }
                    if (mainlabel.Text == "-") { secondlabel.Text = string.Empty; return; }
                }
                catch { ClearAll(sender, e); }

        }

    }
}
