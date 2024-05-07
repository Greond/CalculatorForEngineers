using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Calculator
{
    public partial class MainPage 
    {
        private double _oldwidth, _oldhight;
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != _oldwidth || height != _oldhight)
            {
                _oldwidth = width;
                _oldhight = height;
                if (width > height)
                { // landshaft
                    mainlabel.Margin = new Thickness(0, 10, 10, 0);
                    secondlabel.Margin = new Thickness(0, 0, 10, 0);
                    GridForGrids.Margin = new Thickness(0, 0, 0, 0);
                    GridForGrids.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
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
                    GridForGrids.ColumnDefinitions[0].Width = new GridLength(3, GridUnitType.Star);
                    Grid.SetColumn(Grid2, 0);
                    Grid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    Grid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    Grid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    Grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
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
                    label.FontAttributes = FontAttributes.Bold;
                    label.Margin = new Thickness(0, 6, 0, 0);

                    Switch @switch = new Switch();
                    @switch.OnColor = Color.FromHex("#318ef7");
                    @switch.ThumbColor = Color.White;
                    inv = false;
                    @switch.Toggled += (sender, e) =>
                    {
                        inv = !inv;
                        if (inv == true)
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
                    RadioButton deg = new RadioButton();
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
                    addRadioButton("Bin₂", "systems", 4, 0, Bin);
                    addRadioButton("Oct₈", "systems", 4, 1, Oct);
                    addRadioButton("Dec₁₀", "systems", 4, 2, Dec);


                    int n = 0;
                    int c = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (enginersnums.Length > n)
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

                    void addbutton(string text, bool ischar, int row, int column)
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
                    void addRadioButton(string text, string groupname, int row, int column, bool startposition)
                    {
                        RadioButton radioButton = new RadioButton();
                        radioButton.GroupName = groupname;
                        radioButton.Content = text;
                        radioButton.BackgroundColor = Color.FromHex("#19155c");
                        radioButton.CornerRadius = 30;
                        radioButton.FontSize = 15;
                        radioButton.FontAttributes = FontAttributes.Bold;
                        radioButton.TextColor = Color.FromHex("#bdbdc7fc");
                        radioButton.IsChecked = startposition;
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
                else
                { //  Portriet
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
                    foreach (Button btn in MainGrid.Children)
                    {
                        btn.FontSize = 40;
                        btn.FontAttributes = FontAttributes.None;
                    }
                    mainlabel.FontSize = 40;
                    secondlabel.FontSize = 30;

                }
            }

        }
    }
}
