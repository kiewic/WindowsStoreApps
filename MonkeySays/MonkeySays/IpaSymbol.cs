using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace MonkeySays
{
    public class IpaSymbol
    {
        //private static Color[] colors = new Color[] {
        //    Colors.Turquoise,
        //    Colors.DodgerBlue,
        //    Colors.Indigo,
        //    Colors.SlateBlue,
        //    Colors.Purple,
        //    Colors.RoyalBlue
        //};
        private static Color[] colors = new Color[] {
            Colors.BlueViolet,
            Colors.Crimson,
            Colors.DarkOrange,
            Colors.DarkOrchid,
            Colors.DarkViolet,
            Colors.DeepPink,
            Colors.DeepSkyBlue,
            Colors.Goldenrod,
            Colors.Green,
            Colors.Orange,
            Colors.OrangeRed,
            Colors.Purple,
            Colors.RoyalBlue,
            Colors.SlateBlue,
            Colors.SteelBlue,
            Colors.Tomato,
            Colors.YellowGreen
        };
        private static int colorIndex = 0;


        public IpaSymbol()
        {
            Background = colors[colorIndex++];
            if (colorIndex >= colors.Length)
            {
                colorIndex = 0;
            }
        }

        public string Value { get; set; }
        public string Example { get; set; }
        public Color Background { get; set; }
        public object[] ExamplePieces
        {
            get
            {
                List<object> list = new List<object>();
                bool isBold = false;
                foreach (string piece in Example.Split(new string[] { "**" }, StringSplitOptions.None))
                {
                    list.Add(new IpaExamplePiece(piece, isBold));
                    isBold = !isBold;
                }
                return list.ToArray();
            }
        }
        public SolidColorBrush BackgroundBrush
        {
            get
            {
                return new SolidColorBrush(Background);
            }
        }
        public SolidColorBrush DarkBackgroundBrush
        {
            get
            {
                const int delta = 0x20;
                Color backgroundCopy = Background;
                backgroundCopy.R = MakeDarker(backgroundCopy.R, delta);
                backgroundCopy.G = MakeDarker(backgroundCopy.G, delta);
                backgroundCopy.B = MakeDarker(backgroundCopy.B, delta);
                return new SolidColorBrush(backgroundCopy);
            }
        }
        public SolidColorBrush LightBackgroundBrush
        {
            get
            {
                const int delta = 0x20;
                Color backgroundCopy = Background;
                backgroundCopy.R = MakeLighter(backgroundCopy.R, delta);
                backgroundCopy.G = MakeLighter(backgroundCopy.G, delta);
                backgroundCopy.B = MakeLighter(backgroundCopy.B, delta);
                return new SolidColorBrush(backgroundCopy);
            }
        }
        private byte MakeDarker(byte value, int delta)
        {
            return (byte)(value - Math.Min(delta, value));
        }

        private byte MakeLighter(byte value, int delta)
        {
            return (byte)(value + Math.Min(delta, 255 - value));
        }

        public static async Task LoadSymbolsAsync(Action<IpaSymbol> doSomething)
        {
            Uri uri = new Uri("ms-appx:///ipa.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            string jsonString = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonString);

            foreach (var jsonValue in jsonObject.Values)
            {
                JsonObject subObject = jsonValue.GetObject();
                foreach (var subValue in subObject)
                {
                    doSomething(new IpaSymbol() { Value = subValue.Key, Example = subValue.Value.GetString() });
                }
            }
        }

        public override string ToString()
        {
            return Value;
        }

        static public void SetExampleBlock(TextBlock exampleBlock)
        {
            exampleBlock.Inlines.Clear();

            bool isBold = false;

            string example = exampleBlock.Text;
            string[] pieces = example.Split(new string[] { "**" }, StringSplitOptions.None);
            foreach (var piece in pieces)
            {
                // Bold text.
                var block = new Run();
                block.Text = piece;

                if (isBold)
                {
                    block.FontWeight = FontWeights.Bold;
                }
                isBold = !isBold;

                exampleBlock.Inlines.Add(block);
            }
        }
    }
}
