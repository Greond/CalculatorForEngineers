using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
                    secondlabel.Margin = new Thickness(0, 0, 0, 0);
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
                    @switch.Toggled += (sender, e) =>
                    {
                        inv = !inv;
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
                    if (Deg==true)
                    { deg.IsChecked = true; }
                    else { deg.IsChecked = false; }
                    deg.CheckedChanged += (sender, e) =>
                    {
                        Deg = true;
                        Rad = false;
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
                    else { rad.IsChecked = false; }
                    rad.CheckedChanged += (sender, e) =>
                    {
                        Rad = true;
                        Deg= false;
                    };
                    rad.CheckedChanged += (sender, e) =>
                    {
                        if (rad.IsChecked == true) { Rad = true; Deg = false; }
                        else { Rad = false; }
                    };
                    Grid2.Children.Add(rad);
                    Grid.SetColumn(rad, 2);
                    Grid.SetRow(rad, 5);
                    // rad/>


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
        string[] enginerschars = {"sin", "cos", "x³","tan", "log", "ln", "!" };
        string[] enginersnums = { "e", "φ"};
        
        private bool Rad = false;
        private bool Deg = true;
        private bool inv = false;
        private bool haverror = false;

        public void minusreverse(object sender,EventArgs e)
        {
            if (mainlabel.Text == "" | mainlabel.Text == string.Empty) { return; }
            if (mainlabel.Text.StartsWith("-")) { mainlabel.Text = mainlabel.Text.Substring(1);}
            else if  (!mainlabel.Text.StartsWith("-")) {mainlabel.Text = "-" + mainlabel.Text;}

            char temp = mainlabel.Text[mainlabel.Text.Length - 1];

            if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9'| temp == '%' | temp == 'π' |temp == 'e' | temp == 'φ')  
            {
                Getthecurrentexample();
                _Equals();
            }
        }
         public void numsbuttonclick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            
            if (mainlabel.Text.EndsWith("%") | mainlabel.Text.EndsWith("²") | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ"))
            {
                return ;
                
            }
            if  (btn.Text == "π")
            {
                if (mainlabel.Text == string.Empty | mainlabel.Text == "")
                {
                    mainlabel.Text += btn.Text;
                    Getthecurrentexample();
                    _Equals();
                    return;
                }

                char temp  = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9' | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ")) 
                { return ; }
            }
            mainlabel.Text += btn.Text;
            Getthecurrentexample();
            _Equals();
            }
        public void charbuttonclick(object sender, EventArgs e)  //  доделать чтобы знак ⅟ₓ можно было использовать после корня
        {
            Button btn = (Button)sender;
            if (btn.Text == "sin" | btn.Text == "cos" | btn.Text == "tan" | btn.Text == "ln")
            {
                if (mainlabel.Text.EndsWith("+") | mainlabel.Text.EndsWith("-") | mainlabel.Text.EndsWith("÷")
                  | mainlabel.Text.EndsWith("×") | mainlabel.Text == string.Empty | mainlabel.Text == "" | mainlabel.Text == " ")
                {
                    mainlabel.Text += btn.Text;  //  ⅟
                    return;
                }

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

            if (btn.Text == "X²")// квадрат
            {
                char  temp = mainlabel.Text[mainlabel.Text.Length - 1];
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9' | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ"))
                {
                    mainlabel.Text += "²";
                    Getthecurrentexample();
                    _Equals();
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
                if (temp == '0' | temp == '1' | temp == '2' | temp == '3' | temp == '4' | temp == '5' | temp == '6' | temp == '7' | temp == '8' | temp == '9' | mainlabel.Text.EndsWith("π") | mainlabel.Text.EndsWith("e") | mainlabel.Text.EndsWith("φ"))
                {
                    mainlabel.Text += btn.Text;
                    Getthecurrentexample();
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
                    char[] delimiterChars = { ' ', '÷', '+', '-', '×', '%', '\t', '√', '²', '⅟' }; 
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
            if (str.Contains("sin") |  str.Contains("cos")| str.Contains("tan"))
            {
                int temp = str.Length - 1;
                for (int i = 0; i < temp;i++)
                {
                    if (str[i] == 's' | str[i] == 'c' | str[i] == 't')
                    {

                    }
                }
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
            char[] delimiterChars = { ' ', '÷', '+', '-', '×', '%', '\t', '√', '²', '⅟', 's', 'i', 'n', 'c', 'o', 's', 't', 'a','n' }; // ÷ + - × % √ ² ⅟
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

            char[] delimetrnums = { ',', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' ,' ' ,'E','Е','e','е', '#', 'π', 'φ' };
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
        private string[] scobkisort()
        {
            string[] mas = new string[1];
            return mas;
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
                    if (i!=0 )
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
                else if (chararr[i] == "⅟")
                {
                        numsarr[i] = Math.Pow(numsarr[i],-1);
                        chararr.RemoveAt(i);
                        i--;
                }
                else if (chararr[i] == "sin")
                {
                    numsarr[i] = Math.Sin(numsarr[i]);
                    chararr.RemoveAt(i);
                    i--;
                }
                else if (chararr[i] == "cos")
                {
                    numsarr[i] = Math.Cos(numsarr[i]);
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

                    if (temp == '0'|temp == '1'|temp == '2'|temp == '3'|temp == '4'|temp == '5'|temp=='6'|temp == '7'|temp=='8'|temp=='9' | temp == '%' | temp == 'π' | temp == 'e' | temp == 'φ')  // вычислять если возможно после удаления последнего элемента
                    {
                        Getthecurrentexample();
                            _Equals();
                    }
                    if (mainlabel.Text == "-") { secondlabel.Text = string.Empty; return; }
                }
                catch { ClearAll(sender, e); }

        }
    }
}
