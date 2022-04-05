using System;
using Android.App;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;

namespace Lab2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private readonly List<char> letters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
        private readonly List<List<char>> data = new List<List<char>>();
        private readonly List<TextView> selected = new List<TextView>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            FetchData();
            UpdateTableData(data);
            SetPrintButton();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        private void SetPrintButton()
        {
            var btn = FindViewById<Button>(Resource.Id.print_btn);
            btn.Click += delegate
            {
                foreach (var row in data)
                {
                    foreach (var cell in row)
                    {
                        Console.Write(cell);
                    }
                    Console.Write("\n");
                }
            };
        }

        private void UpdateTableData(List<List<char>> data)
        {
            var dataTable = FindViewById<TableLayout>(Resource.Id.data_table);
            foreach (var row in data) {
                dataTable.AddView(CreateRow(row));
            }
        }

        private TableRow CreateRow(List<char> data)
        {
            var row = new TableRow(this);
            foreach (var cell in data) {
                var textView = new TextView(this)
                {
                    Text = cell.ToString(),
                    Gravity = GravityFlags.Center,
                    TextSize = 20
                };
                Highlight(textView, false);
                textView.Click += delegate
                {
                    selected.Add(textView);
                    ProcessSelected();
                };
                row.AddView(textView);
            }
            return row;
        }

        private void ProcessSelected()
        {
            var size = selected.Count;
            switch (size)
            {
                case 1:
                    Highlight(selected[0], true);
                    break;
                case 2:
                    Highlight(selected[1], true);
                    break;
                case 3:
                    var match = selected[0].Text == selected[1].Text;
                    if (match)
                    {
                        Highlight(selected[0], true, true);
                        Highlight(selected[1], true, true);
                    }
                    else
                    {
                        Highlight(selected[0], false);
                        Highlight(selected[1], false);
                    }
                    Highlight(selected[2], true);
                    selected.RemoveRange(0, 2);
                    break;
            }
        }

        private void Highlight(TextView textView, bool highlight, bool match = false)
        {
            if (highlight)
            {
                textView.SetBackgroundColor(Color.Yellow);
                textView.SetTextColor(Color.Black);
            }
            else
            {
                textView.SetBackgroundColor(Color.Gray);
                textView.SetTextColor(Color.Gray);
            }
            if (match)
            {
                textView.SetBackgroundColor(Color.White);
                textView.SetTextColor(Color.Black);
            }
        }

        private void FetchData()
        {
            letters.AddRange(letters);
            var rnd = new Random();
            var list = letters.OrderBy(item => rnd.Next()).ToList();
            for (var i = 0; i < 5; i++)
            {
                var row = new List<char>();
                for (var j = 0; j < 5; j++)
                {
                    var index = (i + 1) * (j + 1) - 1;
                    row.Add(list[index]);
                }
                data.Add(row);
            }
        }
    }	
}
