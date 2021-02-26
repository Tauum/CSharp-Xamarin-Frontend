using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using Entry = Microcharts.ChartEntry;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartTest : ContentPage
    {
        List<Entry> entries = new List<Entry>();
        public List<Entry> GenerateElements()
        {
            for (int i = 1; i < 15; i++)
            {
                Entry x = new Entry(5)
                {
                    Color = SKColor.Parse($"#{(i*3).ToString().PadLeft(2,'0')}25"),
                    //Color = SKColor.Parse("#328ba8"),
                    Label = i.ToString(),
                    ValueLabel = i.ToString()
                };

                entries.Add(x);
            }
            return entries;
        }

        public ChartTest()
        {
            entries = GenerateElements();
            InitializeComponent();
            // Chart1.Chart = new RadialGaugeChart { Entries = entries };
            Chart1.Chart = new BarChart { Entries = entries };
        }
    }
}